using System;
using System.Linq;
using HarmonyLib;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace MineForMore.Patches.ForagingPatches
{
    public class GetShakenOffItemPatch
    {
        public void Apply(Harmony harmony, StardewModdingAPI.IMonitor monitor)
        {
            var original = AccessTools.Method(typeof(Bush), nameof(Bush.GetShakeOffItem));
            if (original == null)
            {
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
                if (who.professions.Contains(Farmer.gatherer)) // 13
                {
                    int level = who.GetSkillLevel(Farmer.foragingSkill);
                    gathererBonus = (int)(level * ModEntry.Instance.Config.GathererExtraDropPerLevel);
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
                ModEntry.Instance.Monitor.Log($"[GetShakenOffItemPatch] Error adding extra berries: {ex}", StardewModdingAPI.LogLevel.Error);
            }
        }
    }
}
