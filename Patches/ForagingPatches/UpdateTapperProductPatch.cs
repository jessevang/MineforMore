using System;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using SVObject = StardewValley.Object;

namespace MineForMore.Patches.ForagingPatches
{

    public class UpdateTapperProductPatch
    {
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

                // Only syrup-like products
                string qid = tapper.heldObject.Value.QualifiedItemId;
                bool isSyrupLike = qid == "(O)724" || qid == "(O)725" || qid == "(O)726"; // Maple / Oak / Pine
                if (!isSyrupLike) return;

                var who = Game1.player;
                const int TapperProfessionId = 14; // Require Tapper profession (id = 14)
            if (!who.professions.Contains(TapperProfessionId)) return;

                int level = who.GetSkillLevel(Farmer.foragingSkill);
                if (level <= 0) return;

                // Per-level bonus: 0.25 => +25% per level (Lv10 => 3.5x speed)
                double perLevel = Math.Max(0.0, ModEntry.Instance.Config.TapperSpeedBonusPercentPerLevel);
                double speedMult = 1.0 + perLevel * level;
                if (speedMult <= 1.0) return;

                int minutes = tapper.minutesUntilReady.Value;
                int faster = Math.Max(1, (int)Math.Floor(minutes / speedMult));
                tapper.minutesUntilReady.Value = faster;

                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[UpdateTapperProductPatch][Tapper] {qid}: {minutes} -> {faster} min (Lv{level}, +{perLevel}/lvl).", LogLevel.Trace);
            }
            catch (Exception ex)
            {
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[UpdateTapperProductPatch] Error adjusting tapper time: {ex}", LogLevel.Error);
            }
        }
    }
}
