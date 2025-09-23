using HarmonyLib;
using StardewModdingAPI;
using StardewValley;


namespace LevelForMore.Patches.MiningPatches
{

    internal sealed class ExtraMoreCustomGemsDrops_OnPickupAndManualDropPatch
    {
        private const int Profession_Gemologist = 19;
        private const string NoBonusKey = "LevelForMore/NoBonus";
        private const string FromBonusKey = "LevelForMore/FromBonus";

        [ThreadStatic] private static bool _inHook;

        // QualifiedItemId => rule
        private static readonly Dictionary<string, ResourceDropRule> _rulesByQid =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly Config _config;
        private readonly IMonitor _monitor;

        public ExtraMoreCustomGemsDrops_OnPickupAndManualDropPatch(Config config, IMonitor monitor)
        {
            _config = config;
            _monitor = monitor;
        }

        public void Apply(Harmony harmony)
        {
            BuildRuleCache();

            // Tag manual drops: patch each Game1.createItemDebris(Item, …)
            var createOverloads = AccessTools.GetDeclaredMethods(typeof(Game1))
                .Where(m => m.Name == nameof(Game1.createItemDebris))
                .Where(m => m.GetParameters().Length >= 1 && typeof(Item).IsAssignableFrom(m.GetParameters()[0].ParameterType))
                .ToList();

            if (!createOverloads.Any())
            {
                if (_config.DebugMode)
                    _monitor.Log("[MoreCustomGems] No Game1.createItemDebris(Item, …) overloads found to tag manual drops.", LogLevel.Warn);
            }
            else
            {
                var tagPostfix = new HarmonyMethod(typeof(ExtraMoreCustomGemsDrops_OnPickupAndManualDropPatch), nameof(Postfix_CreateItemDebris_TagNoBonus));
                foreach (var m in createOverloads)
                {
                    harmony.Patch(m, postfix: tagPostfix);
                    if (_config.DebugMode)
                        _monitor.Log($"[MoreCustomGems] Patched Game1.{m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})", LogLevel.Trace);
                }
            }

            // Apply bonus on pickup: patch each Farmer.addItemToInventoryBool(Item, …)
            var addOverloads = AccessTools.GetDeclaredMethods(typeof(Farmer))
                .Where(m => m.Name == "addItemToInventoryBool" && m.ReturnType == typeof(bool))
                .Where(m => m.GetParameters().Length >= 1 && typeof(Item).IsAssignableFrom(m.GetParameters()[0].ParameterType))
                .ToList();

            if (!addOverloads.Any())
            {
                if (_config.DebugMode)
                    _monitor.Log("[MoreCustomGems] Couldn’t find Farmer.addItemToInventoryBool(Item, …) overloads. Pickup bonus won’t run.", LogLevel.Error);
            }
            else
            {
                var addPostfix = new HarmonyMethod(typeof(ExtraMoreCustomGemsDrops_OnPickupAndManualDropPatch), nameof(Postfix_AddItemToInventoryBool_GrantBonus));
                foreach (var m in addOverloads)
                {
                    harmony.Patch(m, postfix: addPostfix);
                    if (_config.DebugMode)
                        _monitor.Log($"[MoreCustomGems] Patched Farmer.{m.Name}({string.Join(", ", m.GetParameters().Select(p => p.ParameterType.Name))})", LogLevel.Trace);
                }
            }
        }

        private void BuildRuleCache()
        {
            _rulesByQid.Clear();

            var all = ModEntry.Instance?.GetAllRules() ?? Enumerable.Empty<ResourceDropRule>();
            int added = 0;

            foreach (var r in all)
            {
                if (!"Mining".Equals(r.SkillType, StringComparison.OrdinalIgnoreCase)) continue;
                if (!"MoreCustomGemsConversion".Equals(r.Type, StringComparison.OrdinalIgnoreCase)) continue;
                if (string.IsNullOrWhiteSpace(r.ObjectID)) continue;

                var qid = NormalizeToQualifiedObjectId(r.ObjectID);
                if (string.IsNullOrWhiteSpace(qid)) continue;

                _rulesByQid[qid] = r;
                added++;
            }
            if (_config.DebugMode)
                _monitor.Log($"[MoreCustomGems] Cached {added} MoreCustomGemsConversion rules.", LogLevel.Info);
        }

        private static string NormalizeToQualifiedObjectId(string idOrQid)
        {
            if (string.IsNullOrWhiteSpace(idOrQid))
                return "";
            if (idOrQid.StartsWith("(O)", StringComparison.Ordinal))
                return idOrQid;
            if (int.TryParse(idOrQid, out _))
                return "(O)" + idOrQid;
            return idOrQid; // already QID or mod string id
        }

        // ---------- Patch #1: tag manual drops ----------
        private static void Postfix_CreateItemDebris_TagNoBonus(object __result)
        {
            try
            {
                // Only the Item-based overloads will produce a Debris with d.item != null.
                if (__result is Debris d && d.item != null)
                {
                    // Mark as a manual drop so pickup bonus won’t apply
                    if (!d.item.modData.ContainsKey(NoBonusKey))
                        d.item.modData[NoBonusKey] = "1";

                   
                    //ModEntry.Instance?.Monitor?.Log($"[MoreCustomGems] Tagged manual drop {d.item.QualifiedItemId} with {NoBonusKey}.", LogLevel.Trace);
                }
            }
            catch (Exception ex)
            {
       
                //ModEntry.Instance?.Monitor?.Log($"[MoreCustomGems] Error tagging manual drop: {ex}", LogLevel.Warn);
            }
        }


        private static void Postfix_AddItemToInventoryBool_GrantBonus(Farmer __instance, ref bool __result,[HarmonyArgument(0)] Item item)
        {
            try
            {
                // If nothing was added, or we’re adding our own bonus copies, bail.
                if (!__result) return;
                if (_inHook) return;
                if (item is null) return;

                // IMPORTANT: Never grant bonus while any game menu is open
                // (shops, shipping bin screen, chests, item grab menus, etc.).
                if (Game1.activeClickableMenu != null) return;

                // Ignore manually dropped items or our own bonus copies.
                var md = item.modData;
                if (md != null && (md.ContainsKey(NoBonusKey) || md.ContainsKey(FromBonusKey)))
                    return;

                var who = __instance ?? Game1.player;
                if (who is null) return;

                string qid;
                try { qid = item.QualifiedItemId; }
                catch { return; }

                if (!_rulesByQid.TryGetValue(qid, out var rule))
                    return;

                // Compute Total = (Count + (GeoBonus if has profession)) * Multiplier
                int miningLevel = who.MiningLevel;
                bool hasGeo = who.professions.Contains(Profession_Gemologist);
                float perLevel = ModEntry.Instance?.Config?.GeologistProfessionBonusGemsPerLevel ?? 0f;

                double baseCount = rule.AddAmount;
                double geoBonus = hasGeo ? (perLevel * miningLevel) : 0.0;// only if Gemologist
                double total = (baseCount + geoBonus) * rule.Multiplier;

                // How many were just granted by this pickup already?
                int already = Math.Max(1, item.Stack);
                int bonus = Math.Max(0, (int)Math.Floor(total) - already);
                if (bonus <= 0) return;

                var loc = who.currentLocation ?? Game1.currentLocation;
                var origin = who.Position;

                _inHook = true;

                /*
                ModEntry.Instance?.Monitor?.Log(
                    $"[MoreCustomGems] Pickup bonus for {qid}: base={baseCount}, geoBonus={geoBonus}, mult={rule.Multiplier}, " +
                    $"already={already} -> total={total}, extra={bonus}.",
                    LogLevel.Info
                );
                */

                for (int i = 0; i < bonus; i++)
                {
                    var copy = ItemRegistry.Create(qid);
                    if (copy == null) continue;

                    // Mark as our bonus so we don’t re-trigger on re-entry.
                    copy.modData[FromBonusKey] = "1";

                    // Try inventory; if full, drop near player (tag persists; harmless).
                    if (!who.addItemToInventoryBool(copy))
                        Game1.createItemDebris(copy, origin, -1, loc);
                }
            }
            catch (Exception ex)
            {
                ModEntry.Instance?.Monitor?.Log($"[MoreCustomGems] Error applying pickup bonus: {ex}", LogLevel.Error);
            }
            finally
            {
                _inHook = false;
            }
        }



    }
}
