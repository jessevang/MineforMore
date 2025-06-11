using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using Microsoft.Xna.Framework;
using HarmonyLib;
using Object = StardewValley.Object;
using System;
using System.Linq;
using MineForMore;

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
        int randomChance = rand.Next(1, 101);
        var allDrops = ModEntry.Instance.GetAllDrops();
        if (allDrops == null)
            return;

        // Excavator (ID 22): Randomly replace litter with geode node
        if (Game1.player.professions.Contains(22) && randomChance == 1)
        {
            var geodeNodes = allDrops
                .Where(m => m.Type == "Geode")
                .SelectMany(m => m.DropsFromObjectIDs)
                .ToList();

            if (geodeNodes.Count > 0)
            {
                string randomGeodeNode = geodeNodes[rand.Next(geodeNodes.Count)];
                __result = new Object(randomGeodeNode, 1);
                return;
            }
        }

        // Gemologist (ID 23): Randomly replace litter with gem node
        if (Game1.player.professions.Contains(23) && randomChance == 2)
        {
            var gemNodes = allDrops
                .Where(m => m.Type == "Gem")
                .SelectMany(m => m.DropsFromObjectIDs)
                .ToList();

            if (gemNodes.Count > 0)
            {
                string randomGemNode = gemNodes[rand.Next(gemNodes.Count)];
                __result = new Object(randomGemNode, 1);
                return;
            }
        }

        // Prospector (ID 21): Randomly replace litter with coal node
        if (Game1.player.professions.Contains(21) && randomChance == 3)
        {
            var coalNodes = allDrops
                .Where(m => m.Type == "Coal & Others")
                .SelectMany(m => m.DropsFromObjectIDs)
                .ToList();

            if (coalNodes.Count > 0)
            {
                string randomCoalNode = coalNodes[rand.Next(coalNodes.Count)];
                __result = new Object(randomCoalNode, 1);
                return;
            }
        }
    }
}
