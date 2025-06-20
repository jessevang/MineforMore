using HarmonyLib;
using StardewValley.TerrainFeatures;
using StardewValley;
using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace MineForMore.Patches.ForagingPatches
{
    [HarmonyPatch(typeof(Tree), nameof(Tree.dayUpdate))]
    internal class TreeGrowthPatch
    {
        public void Apply(Harmony harmony, IMonitor monitor)
        {
            var method = AccessTools.Method(typeof(Tree), nameof(Tree.dayUpdate));
            if (method == null)
            {
                monitor.Log("Failed to find Tree.dayUpdate", LogLevel.Error);
                return;
            }

            var postfix = new HarmonyMethod(typeof(TreeGrowthPatch), nameof(Postfix));
            harmony.Patch(method, postfix: postfix);
        }

        public static void Postfix(Tree __instance)
        {
            if (!Game1.IsMasterGame)
                return;

            if (__instance.growthStage.Value >= __instance.GetMaxSizeHere())
                return;

            // Apply +1 for fertilizer
            if (__instance.fertilized.Value)
            {
                __instance.growthStage.Value++;
                if (__instance.growthStage.Value >= __instance.GetMaxSizeHere())
                    return;
            }

            //Get nearest farmer with professions
            Farmer farmer = GetNearestFarmer(__instance.Tile);
            if (farmer == null)
                return;

            int foragingLevel = farmer.GetSkillLevel(Farmer.foragingSkill);
            bool hasForester = farmer.professions.Contains(12);     // Forester
            bool hasLumberjack = farmer.professions.Contains(14);   // Lumberjack

            if (!hasForester)
                return;

            float baseChance = 0.25f;
            float bonusPerLevel = 0.10f;
            float growthChance = baseChance + (foragingLevel * bonusPerLevel);

            //  First roll
            if (Game1.random.NextDouble() < growthChance)
            {
                __instance.growthStage.Value++;
                if (__instance.growthStage.Value >= __instance.GetMaxSizeHere())
                    return;
            }

            // If user has LumberJack profession then , Recursively reduce growth chance by half and test to see if tree should grow again until tree fully grown or chance is less than 1%.
            if (hasLumberjack)
            {
                while (growthChance > 0.01f && __instance.growthStage.Value < __instance.GetMaxSizeHere())
                {
                    growthChance /= 2f;
                    if (Game1.random.NextDouble() < growthChance)
                    {
                        __instance.growthStage.Value++;
                    }
                    else break;
                }
            }


        }

        private static Farmer GetNearestFarmer(Vector2 tile)
        {
            Farmer nearest = null;
            double minDistance = double.MaxValue;

            foreach (Farmer farmer in Game1.getAllFarmers())
            {
                double dist = Vector2.Distance(farmer.Tile, tile);
                if (dist < minDistance)
                {
                    nearest = farmer;
                    minDistance = dist;
                }
            }

            return nearest;
        }



    }
}