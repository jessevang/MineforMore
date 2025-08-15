using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using SVObject = StardewValley.Object;

namespace LevelForMore.Patches.ForagingPatches
{
    public class UpdateTapperProductPatch
    {
        private const string QtyAdjustedFlag = "MineForMore/TapperQtyAdjusted";

        public void Apply(Harmony harmony, IMonitor monitor)
        {
            var original = AccessTools.Method(typeof(Tree), nameof(Tree.UpdateTapperProduct));
            if (original == null)
            {
                if (ModEntry.Instance.Config.DebugMode)
                    monitor.Log("Failed to find Tree.UpdateTapperProduct", LogLevel.Error);
                return;
            }

            harmony.Patch(original, postfix: new HarmonyMethod(typeof(UpdateTapperProductPatch), nameof(Postfix)));
        }

        public static void Postfix(Tree __instance, SVObject tapper, SVObject previousOutput, bool onlyPerformRemovals)
        {
            try
            {
                if (tapper == null || onlyPerformRemovals) return;
                if (tapper.heldObject?.Value == null) return;
                if (tapper.minutesUntilReady.Value <= 0) return;


                if (!ReferenceEquals(previousOutput, tapper.heldObject.Value))
                    tapper.modData.Remove(QtyAdjustedFlag);


                string qid = tapper.heldObject.Value.QualifiedItemId;
                bool isSyrupLike = qid == "(O)724" || qid == "(O)725" || qid == "(O)726"; // Maple / Oak / Pine
                if (!isSyrupLike) return;


                if (qid.StartsWith("(O)") && int.TryParse(qid.Substring(3), out int numericId))
                    qid = numericId.ToString();


                var rule = ModEntry.Instance.GetAllRules()
                    .FirstOrDefault(r => r.Type == "Forage" && r.DropsFromObjectIDs.Contains(qid));

                var who = Game1.player;
                const int TapperProfessionId = 14; // Require Tapper profession (id = 14)
                int level = Math.Max(0, who.GetSkillLevel(Farmer.foragingSkill));
                int ExtraCount = 0;
                if (who.professions.Contains(TapperProfessionId))
                {
                    // ---- SPEED BOOST (----
                    double perLevel = Math.Max(0.0, ModEntry.Instance.Config.TapperSpeedBonusPercentPerLevel);
                    double speedMult = 1.0 + perLevel * level;
                    if (speedMult <= 1.0) return;

                    int minutes = tapper.minutesUntilReady.Value;
                    int faster = Math.Max(1, (int)Math.Floor(minutes / speedMult));
                    tapper.minutesUntilReady.Value = faster;

                    if (ModEntry.Instance.Config.DebugMode)
                        ModEntry.Instance.Monitor.Log($"[Foraging][UpdateTapperProductPatch][Tapper] {qid}: {minutes} -> {faster} min (Lv{level}, +{perLevel}/lvl).", LogLevel.Trace);
                    ExtraCount = (int)(level * ModEntry.Instance.Config.TapperExtraQuantityPerLevel);
                }




                // ---- QUANTITY BONUS ----
                if (!tapper.modData.ContainsKey(QtyAdjustedFlag))
                {
                    var output = tapper.heldObject.Value;
                    int baseStack = Math.Max(1, output.Stack);


                    if (rule == null)
                    {
                        output.Stack = (int)((baseStack + ExtraCount));
                        tapper.modData[QtyAdjustedFlag] = "1";
                        if (ModEntry.Instance.Config.DebugMode)
                            ModEntry.Instance.Monitor.Log($"[Foraging][UpdateTapperProductPatch]rule is null.", LogLevel.Trace);
                        return;
                    }

                    output.Stack = (int)((baseStack + rule.AddAmount + ExtraCount)*rule.Multiplier);
                    tapper.modData[QtyAdjustedFlag] = "1";


                    if (ModEntry.Instance.Config.DebugMode)
                        ModEntry.Instance.Monitor.Log($"[Foraging][UpdateTapperProductPatch]TapperQty {qid}: {baseStack} -> {output.Stack} (Lv{level}).", LogLevel.Trace);
                    
                }
            }
            catch (Exception ex)
            {
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[Foraging][UpdateTapperProductPatch] Error adjusting tapper time: {ex}", LogLevel.Error);
            }
        }
    }
}
