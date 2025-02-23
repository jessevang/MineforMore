
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Spacechase.Shared.Patching;
using MineForMore;
using StardewValley.Locations;
using StardewValley.Extensions;
using StardewValley.Objects;
using Microsoft.Xna.Framework;
using Object = StardewValley.Object;
using Netcode;



/// <summary>Applies Harmony patches to <see cref="GameLocation"/>.</summary>

internal class MineShaftOresPatches : BasePatcher
{

    private readonly Config _config;

    public MineShaftOresPatches(Config config)
    {
        _config = config;
    }

    public override void Apply(Harmony harmony, IMonitor monitor)
    {
        var originalMethod = AccessTools.Method(typeof(MineShaft), "createLitterObject");

        if (originalMethod == null)
        {
            monitor.Log("Failed to find method: MineShaft.createLitterObject", LogLevel.Error);
            return;
        }

        harmony.Patch(
            original: originalMethod,
            postfix: new HarmonyMethod(typeof(MineShaftOresPatches), nameof(ModifiedcreateLitterObject))
        );



    }

    private static void ModifiedcreateLitterObject(double chanceForPurpleStone, double chanceForMysticStone, double gemStoneChance, Vector2 tile, ref Object __result)
    {
        Random rand = new Random();
        int randomChance = rand.Next(1, 101);
        int chosenNode = rand.Next(1, 5);
        int chosenGem = rand.Next(1, 8);
        // Change result to a random geode if user has profession node 5% chance to convert any stones to geode
        if (Game1.player.professions.Contains(22))
        {


            if (randomChance ==1)
            {  
                if (chosenNode == 1)
                {
                    __result = new Object("75", 1); // Geode Stone
                }
                else if (chosenNode ==2)
                {
                    __result = new Object("76", 1); //Frozen Geode Stone
                }
                else if (chosenNode == 3)
                {
                    __result = new Object("77", 1); //Magma Geode Stone
                }

                else if (chosenNode == 4)
                {
                    __result = new Object("819", 1); //Omni Geode Stone

                }
            }

        }

        //adds more gems for Gemologist
        if (Game1.player.professions.Contains(23))
        {

            if (randomChance == 2)
            {
                if (chosenGem == 1)
                {
                    __result = new Object("12", 1); // Emerald

                }
                else if (chosenGem == 2)
                {
                    __result = new Object("14", 1); //Aquamarine
                }
                else if (chosenGem == 3)
                {
                    __result = new Object("4", 1); //Ruby
                }

                else if (chosenGem == 4)
                {
                    __result = new Object("8", 1); //Amethyst

                }
                else if (chosenGem == 5)
                {
                    __result = new Object("10", 1); //Topaz


                }
                else if (chosenGem == 6)
                {
                    __result = new Object("6", 1); //Jade


                }
                else if (chosenGem == 7)
                {
                    __result = new Object("2", 1); //Diamond


                }
            }

        }



        if (Game1.player.professions.Contains(21))  //Prospector or burrower
        {

            if (randomChance == 3)
            {
                __result = new Object("VolcanoCoalNode0", 1); // Geode Stone

            }

        }


    }




}