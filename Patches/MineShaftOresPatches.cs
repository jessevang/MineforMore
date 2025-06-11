using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using Microsoft.Xna.Framework;
using HarmonyLib;
using Object = StardewValley.Object;
using System;
using System.Linq;
using MineForMore;
using System.Collections.Generic;

internal class MineShaftOresPatches
{
    public static Config Config;

    public MineShaftOresPatches(Config _config)
    {
        Config = _config;
    }

    public void Apply(Harmony harmony, IMonitor monitor)
    {
        var original = AccessTools.Method(typeof(MineShaft), "createLitterObject");

        if (original == null)
        {
            monitor.Log("Failed to find method: MineShaft.createLitterObject", LogLevel.Error);
            return;
        }

        var postfix = new HarmonyMethod(typeof(MineShaftOresPatches), nameof(ModifiedCreateLitterObject));
        harmony.Patch(original, postfix: postfix);
    }

    private static void ModifiedCreateLitterObject(
        double chanceForPurpleStone,
        double chanceForMysticStone,
        double gemStoneChance,
        Vector2 tile,
        ref Object __result)
    {
        Random rand = new Random();
        var allDrops = ModEntry.Instance.GetAllDrops();
        if (allDrops == null)
            return;

        // Try each supported node type (Gem, Geode, Coal, Ore)
        TryReplaceLitterWithNode("Gem", allDrops, rand, __result: ref __result);
        TryReplaceLitterWithNode("Geode", allDrops, rand, __result: ref __result);
        TryReplaceLitterWithNode("Coal&Others", allDrops, rand, __result: ref __result);
        TryReplaceLitterWithNode("Ore", allDrops, rand, __result: ref __result); // Covers all ores
    }

    private static void TryReplaceLitterWithNode(string type, IEnumerable<ResourceDropRule> allDrops, Random rand, ref Object __result)
    {
        List<ResourceDropRule> candidates = allDrops.Where(m => m.Type == type).ToList();
        if (!candidates.Any())
            return;

        foreach (var drop in candidates)
        {
            float chance = drop.ExtraNodeSpawnChancePercent;

            // Always add profession bonus *if* player has profession
            if (type == "Gem" && Game1.player.professions.Contains(23)) // Gemologist
                chance += Config.GemNodeSpawnChanceBonusWithProfession;
            else if (type == "Geode" && Game1.player.professions.Contains(22)) // Excavator
                chance += Config.GeodeNodeSpawnChanceBonusWithProfession;
            else if (type == "Coal&Others" && Game1.player.professions.Contains(21)) // Prospector
                chance += Config.CoalNodeSpawnChanceBonusWithProfession;

            if (chance > 0f && rand.NextDouble() < chance / 100.0)
            {
                var nodeIDs = drop.DropsFromObjectIDs;
                if (nodeIDs.Count > 0)
                {
                    string chosenID = nodeIDs[rand.Next(nodeIDs.Count)];
                    __result = new Object(chosenID, 1);
                    return;
                }
            }
        }
    }

}
