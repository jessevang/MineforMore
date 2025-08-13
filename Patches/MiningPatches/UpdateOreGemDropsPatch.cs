using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using MineForMore;
namespace MineforMore.Patches.MiningPatches
{
    internal class UpdateOreGemDropsPatch
    {
        private readonly Config _config;

        public UpdateOreGemDropsPatch(Config config)
        {
            _config = config;
        }

        public void Apply(Harmony harmony, IMonitor monitor)
        {
            if (ModEntry.Instance.Config.DebugMode)
                monitor.Log("Patching GameLocation.OnStoneDestroyed with ModifiedOnStoneDestroyed...", LogLevel.Info);
            var stoneDestroyed = AccessTools.Method(typeof(GameLocation), nameof(GameLocation.OnStoneDestroyed));
            if (stoneDestroyed == null)
            {
                if (ModEntry.Instance.Config.DebugMode)
                    monitor.Log("Failed to find GameLocation.OnStoneDestroyed method!", LogLevel.Error);
                return;
            }
            harmony.Patch(stoneDestroyed, prefix: new HarmonyMethod(typeof(UpdateOreGemDropsPatch), nameof(ModifiedOnStoneDestroyed)));
            if (ModEntry.Instance.Config.DebugMode)
                monitor.Log("Patch applied to OnStoneDestroyed", LogLevel.Info);
        }

        public static void ModifiedOnStoneDestroyed(string stoneId, int x, int y, Farmer who)
        {
            if (ModEntry.Instance.Config.DebugMode)
                ModEntry.Instance.Monitor.Log($"[HOOK] OnStoneDestroyed triggered with stoneId={stoneId}, x={x}, y={y}", LogLevel.Warn);
            if (ModEntry.Instance.Config.DebugMode &&
                Game1.objectData.TryGetValue(stoneId, out StardewValley.GameData.Objects.ObjectData value))
            {
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"Stone destroyed: {value.Name ?? "Unknown"} (ID: {stoneId}) at ({x}, {y})", LogLevel.Info);
            }

            if (who == null)
                return;

            IEnumerable<ResourceDropRule> drops = typeof(Config)
                .GetProperties()
                .Where(p => p.PropertyType == typeof(ResourceDropRule))
                .Select(p => p.GetValue(ModEntry.Instance.Config) as ResourceDropRule);

            foreach (var entry in drops)
            {
                if (entry == null)
                    continue;

                bool matches = entry.DropsFromObjectIDs.Any(id =>
                    (id.StartsWith("(O)") && id.Length > 3 && id.Substring(3) == stoneId) || id == stoneId);

                if (!matches)
                    continue;

                float bonus = 0f;

                if (entry.Type.Equals("Ore", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(18))
                    bonus = ModEntry.Instance.Config.MinerProfessionBonusOrePerLevel * who.MiningLevel;
                else if (entry.Type.Equals("Gem", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(19))
                    bonus = ModEntry.Instance.Config.GeologistProfessionBonusGemsPerLevel * who.MiningLevel;
                else if (entry.Type.Equals("Geode", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(22))
                    bonus = ModEntry.Instance.Config.ExcavatorProfessionBonusGeodesPerLevel * who.MiningLevel;
                else if (entry.Type.Equals("Coal&Others", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(21))
                    bonus = ModEntry.Instance.Config.ProspectorProfessionBonusCoalPerLevel * who.MiningLevel;


                int num = (int)((entry.AddAmount + bonus) * entry.Multiplier);
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"Matched entry: {entry.Name}, StoneID: {stoneId}, Base: {entry.AddAmount}, Bonus: {bonus}, Multiplier: {entry.Multiplier}, Final drop count: {num} + normal ore drops", LogLevel.Info);

                if (num > 0)
                {
                    for (int i = 0; i < num; i++)
                        Game1.createObjectDebris(entry.ObjectID, x, y, who.UniqueMultiplayerID);
                }
            }
        }
    }
}