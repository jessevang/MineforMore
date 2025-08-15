using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Linq;

namespace LevelForMore.Patches.ForagingPatches
{
    public class GetShakenOffItemPatch
    {
        public void Apply(Harmony harmony, StardewModdingAPI.IMonitor monitor)
        {
            var original = AccessTools.Method(typeof(Bush), nameof(Bush.GetShakeOffItem));
            if (original == null)
            {
                if (ModEntry.Instance.Config.DebugMode)
                    monitor.Log("Failed to find Bush.GetShakeOffItem", StardewModdingAPI.LogLevel.Error);
                return;
            }

            harmony.Patch(original, postfix: new HarmonyMethod(typeof(GetShakenOffItemPatch), nameof(Postfix)));
        }

        public static void Postfix(Bush __instance, ref string __result)
        {
            try
            {
                string resultId = __result;
                if (string.IsNullOrEmpty(resultId))
                    return;

            
                if (resultId != "(O)296" && resultId != "(O)410") // Salmonberry / Blackberry
                    return;

                // Match rules on qualified "(O)###" or plain "###"
                string numericId = resultId.StartsWith("(O)", StringComparison.Ordinal) ? resultId.Substring(3) : resultId;

                var rule = ModEntry.Instance.GetAllRules()
                    .FirstOrDefault(r => r.Type == "Forage" &&
                                         (r.DropsFromObjectIDs.Contains(resultId) || r.DropsFromObjectIDs.Contains(numericId)));
                if (rule == null)
                    return;

                var who = Game1.player;
                int baseAmount = rule.AddAmount;

                int gathererBonus = 0;
                int level = who.GetSkillLevel(Farmer.foragingSkill);
                if (who.professions.Contains(Farmer.gatherer)) // 13
                {

                    gathererBonus = (int)(level * ModEntry.Instance.Config.GathererExtraDropPerLevel);
                    if (ModEntry.Instance.Config.DebugMode)
                        ModEntry.Instance.Monitor.Log($"[Foraging][GetShakenOffItemPatch] Farmer Has Gatherer Profession ExtraCount= {gathererBonus}", LogLevel.Info);
                }
                if (who.professions.Contains(Farmer.botanist)) // 16
                {

                    gathererBonus = (int)(gathererBonus * 1.25);
                    if (ModEntry.Instance.Config.DebugMode)
                        ModEntry.Instance.Monitor.Log($"[Foraging][GetShakenOffItemPatch] Farmer Has Botanist Profession ExtraCount= {gathererBonus}", LogLevel.Info);
                }
                if (who.professions.Contains(Farmer.tracker)) // 17
                {

                    gathererBonus = (int)(gathererBonus * 1.50);
                    if (ModEntry.Instance.Config.DebugMode)
                        ModEntry.Instance.Monitor.Log($"[Foraging][GetShakenOffItemPatch] Farmer Has Tracker Profession ExtraCount= {gathererBonus}", LogLevel.Info);
                }

                int extraCount = (int)Math.Round((baseAmount + gathererBonus) * rule.Multiplier);
                if (extraCount <= 0)
                    return;

                var location = Game1.player.currentLocation ?? __instance.Location ?? Game1.currentLocation;
                var spawnPos = Utility.PointToVector2(__instance.getBoundingBox().Center);

                for (int i = 0; i < extraCount; i++)
                {
                    var item = ItemRegistry.Create(resultId, 1);        // resultId is "(O)296" or "(O)410"
                    if (Game1.player.professions.Contains(Farmer.botanist)) // 16 → iridium quality
                        item.Quality = 4;

                    // Drop like vanilla berry shakes:
                    Game1.createItemDebris(item, spawnPos, Game1.random.Next(1, 4), location);
                }
            }
            catch (Exception ex)
            {
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[Foraging][GetShakenOffItemPatch] Error adding extra berries: {ex}", StardewModdingAPI.LogLevel.Error);
            }
        }
    }
}
