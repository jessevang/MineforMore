using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Characters;
using StardewValley.TerrainFeatures;
using System;
using System.Linq;

namespace MineForMore.Patches.ForagingPatches
{
    public class CropHarvestPatch
    {
        public void Apply(Harmony harmony, IMonitor monitor)
        {
            var original = AccessTools.Method(typeof(Crop), nameof(Crop.harvest));
            if (original is null)
            {
                monitor.Log("Failed to find Crop.harvest", LogLevel.Error);
                return;
            }

            var postfix = new HarmonyMethod(typeof(CropHarvestPatch), nameof(Postfix));
            harmony.Patch(original: original, postfix: postfix);
        }

        public static void Postfix(Crop __instance, HoeDirt soil, bool __result)
        {
            if (!__result)
                return; // Crop was not harvested — skip so we don't add crop and glitch infinite item

            if (soil?.crop?.indexOfHarvest.Value == null)
                return;

            var crop = __instance;
            var location = crop.currentLocation; 

            if (soil?.crop?.indexOfHarvest.Value == null)
                return;

            string rawId = soil.crop.indexOfHarvest.Value.ToString();
            if (!int.TryParse(rawId, out int numericId))
                return;

            string id = numericId.ToString();

            var rule = ModEntry.Instance.GetAllRules()
                .FirstOrDefault(r => r.Type == "Forage" && r.DropsFromObjectIDs.Contains(id));

            if (rule == null)
                return;

            int baseAmount = rule.AddAmount;

            int extraCount = 0;
            Farmer who = Game1.player; 
            if (who.professions.Contains(13)) // Gatherer
            {
                int level = who.GetSkillLevel(Farmer.foragingSkill);
                extraCount = level * ModEntry.Instance.Config.GathererExtraDropPerLevel;
            }

            int totalCount = (int)Math.Round((baseAmount + extraCount) * rule.Multiplier);

            for (int i = 0; i < totalCount; i++)
            {
                Item extra = new StardewValley.Object(rule.ObjectID, 1);
                who.addItemToInventoryBool(extra);
            }
        }

    }
}
