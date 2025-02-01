using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley.Monsters;
using StardewValley;
using System.Collections.Generic;
using System;
using HarmonyLib;
using StardewValley.Locations;
using Spacechase.Shared.Patching;
using Netcode;
using static HarmonyLib.Code;
using System.Reflection;
using System.Text.Json.Nodes;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;
using StardewValley.Network;
using Newtonsoft.Json.Linq;
using StardewValley.Constants;
using StardewValley.Extensions;
using StardewValley.TerrainFeatures;
using System.Threading;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using MineForMore;
using static MineForMore.ModEntry;
using StardewValley.Menus;
using MineForMore.Classes; // To access CustomProfessions

/// <summary>Applies Harmony patches to <see cref="GameLocation"/>.</summary>



internal class ObjectPatches : BasePatcher
{

    private readonly Config _config;

    public ObjectPatches(Config config)
    {
        _config = config;
    }

    public override void Apply(Harmony harmony, IMonitor monitor)
    {
        //1.patch profession descriptions
        harmony.Patch(
            original: this.RequireMethod<LevelUpMenu>(
            nameof(LevelUpMenu.getProfessionDescription) // Correct parameter types
        ),
        postfix: this.GetHarmonyMethod(nameof(ModifyProfessionDescription)) // prefix method
);

        //2. 4 sets of code to patch what item drops
        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createMultipleObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(int) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createMultipleObjectDebris1)) // prefix method
        );

        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createMultipleObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(GameLocation) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createMultipleObjectDebris2)) // prefix method
        );

        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createMultipleObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(float) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createMultipleObjectDebris3)) // prefix method
        );

        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createMultipleObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(long) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createMultipleObjectDebris4)) // prefix method
        );

        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createMultipleObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(long), typeof(GameLocation) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createMultipleObjectDebris5)) // prefix method
        );
 

    }

    //1. Updates profession profession Text Display to align with Config file's BonusOresWithMinerProfession amount.
    public static void ModifyProfessionDescription(int whichProfession, ref List<string> __result)
    {
        // Miner Profession ID is 18 (Miner)
        if (whichProfession == 18)
        {
            __result.Clear(); // Clear the existing list first (optional, just to be safe)
            __result.Add("Master Miner"); // Custom profession name
            __result.Add("Gain +" + Instance.Config.BonusOresWithMinerProfession + " ore per mined rock instead of +1!"); // Custom profession description
            
        }
    }



    //2. multiple debris
    public static bool Before_createMultipleObjectDebris1(string id, int xTile, int yTile, int number)
    {
        number = getNumber(id, number);

            for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile);
        }
        return false;  //adding a return false makes it so original code don't run
    }

    public static bool Before_createMultipleObjectDebris2(string id, int xTile, int yTile, int number, GameLocation location)
    {
        number = getNumber(id, number);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, -1, 0, 1f, location);
        }
        return false;  //adding a return false makes it so original code don't run
    }

    public static bool Before_createMultipleObjectDebris3(string id, int xTile, int yTile, int number, float velocityMultiplier)
    {
        number = getNumber(id, number);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, -1, 0, velocityMultiplier);
        }
        return false;  //adding a return false makes it so original code don't run
    }

    public static bool Before_createMultipleObjectDebris4(string id, int xTile, int yTile, int number, long who)
    {
        number = getNumber(id, number, who);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, who);
        }
        return false;  //adding a return false makes it so original code don't run
    }

    public static bool Before_createMultipleObjectDebris5(string id, int xTile, int yTile, int number, long who, GameLocation location)
    {
        number = getNumber(id, number, who);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, who, location);
        }
        return false;  //adding a return false makes it so original code don't run
    }


    //2. adds number for more ore drops used for multiple debris function
    public static int getNumber(string id, int number)
    {

        //Console.WriteLine("Item is: " + id + " - Number Count Before adding value is: " + number + " StoneCount is  "+ Instance.Config.AddStone+ " StoneMultiplyValue is: " + Instance.Config.MultiplyStone);
        if (id == ("(O)390")) { number = (int)((number + Instance.Config.AddStone) * Instance.Config.MultiplyStone);
            //Console.WriteLine("Item is: " + id + " - Number Count after adding value is: " + number);
        }

        else if (id == ("(O)382")) { number = (int)((number + Instance.Config.AddCoal) * Instance.Config.MultiplyCoal); }
        else if (id == ("(O)378")) { number = (int)((number + Instance.Config.AddCooperOre) * Instance.Config.MultiplyCooperOre); }
        else if (id == ("(O)384")) { number = (int)((number + Instance.Config.AddGoldOre) * Instance.Config.MultiplyGoldOre); }
        else if (id == ("(O)380")) { number = (int)((number + Instance.Config.AddIronOre) * Instance.Config.MultiplyIronOre); }
        else if (id == ("(O)386")) { number = (int)((number + Instance.Config.AddIridiumOre) * Instance.Config.MultiplyIridiumOre); }
        else if (id == ("(O)909")) { number = (int)((number + Instance.Config.AddRadiactiveOre) * Instance.Config.MultiplyRadiactiveOre); }
        else if (id == ("(O)72")) { number = (int)((number + Instance.Config.AddDiamond) * Instance.Config.MultiplyDiamond); }
        else if (id == ("(O)66")) { number = (int)((number + Instance.Config.AddAmethyst) * Instance.Config.MultiplyAmethyst); }
        else if (id == ("(O)62")) { number = (int)((number + Instance.Config.AddAquamarine) * Instance.Config.MultiplyAquamarine); }
        else if (id == ("(O)86")) { number = (int)((number + Instance.Config.AddEarthCrystal) * Instance.Config.MultiplyEarthCrystal); }
        else if (id == ("(O)60")) { number = (int)((number + Instance.Config.AddEmerald) * Instance.Config.MultiplyEmerald); }
        else if (id == ("(O)82")) { number = (int)((number + Instance.Config.AddFireQuartz) * Instance.Config.MultiplyFireQuartz); }
        else if (id == ("(O)84")) { number = (int)((number + Instance.Config.AddFrozenTear) * Instance.Config.MultiplyFrozenTear); }
        else if (id == ("(O)80")) { number = (int)((number + Instance.Config.AddQuartz) * Instance.Config.MultiplyQuartz); }
        else if (id == ("(O)64")) { number = (int)((number + Instance.Config.AddRuby) * Instance.Config.MultiplyRuby); }
        else if (id == ("(O)68")) { number = (int)((number + Instance.Config.AddTopaz) * Instance.Config.MultiplyTopaz); }
        else if (id == ("(O)70")) { number = (int)((number + Instance.Config.AddJade) * Instance.Config.MultiplyJade); }
        
        return number;
    }

    //2. Added a 2nd method to add double Add due to professional to get gets double ores. used for multiple debris function
    public static int getNumber(string id, int number, long who)
    {

       Farmer forPlayer = Game1.GetPlayer(who) ?? Game1.player;
       int addedOres = ((forPlayer != null && forPlayer.professions.Contains(18)) ? Instance.Config.BonusOresWithMinerProfession : 0);
       //Console.WriteLine("Item is: " + id + " - Number Count Before adding value is: " + number + " StoneCount is " + Instance.Config.AddStone + " professionAddedOres: "+ Instance.Config.BonusOresWithMinerProfession+  " StoneMultiplyValue is: " + Instance.Config.MultiplyStone);

        if (id == ("(O)390")) { number = (int)((number + addedOres+  Instance.Config.AddStone) * Instance.Config.MultiplyStone);
            //Console.WriteLine("Item is: " + id + " - Number Count after adding value is: " + number);
        }

        else if (id == ("(O)382")) { number = (int)((number + addedOres + Instance.Config.AddCoal) * Instance.Config.MultiplyCoal); }
        else if (id == ("(O)378")) { number = (int)((number + addedOres + Instance.Config.AddCooperOre) * Instance.Config.MultiplyCooperOre); }
        else if (id == ("(O)384")) { number = (int)((number + addedOres + Instance.Config.AddGoldOre) * Instance.Config.MultiplyGoldOre); }
        else if (id == ("(O)380")) { number = (int)((number + addedOres + Instance.Config.AddIronOre) * Instance.Config.MultiplyIronOre); }
        else if (id == ("(O)386")) { number = (int)((number + addedOres + Instance.Config.AddIridiumOre) * Instance.Config.MultiplyIridiumOre); }
        else if (id == ("(O)909")) { number = (int)((number + addedOres + Instance.Config.AddRadiactiveOre) * Instance.Config.MultiplyRadiactiveOre); }
        else if (id == ("(O)72")) { number = (int)((number + addedOres + Instance.Config.AddDiamond) * Instance.Config.MultiplyDiamond); }
        else if (id == ("(O)66")) { number = (int)((number + addedOres + Instance.Config.AddAmethyst) * Instance.Config.MultiplyAmethyst); }
        else if (id == ("(O)62")) { number = (int)((number + addedOres + Instance.Config.AddAquamarine) * Instance.Config.MultiplyAquamarine); }
        else if (id == ("(O)86")) { number = (int)((number + addedOres + Instance.Config.AddEarthCrystal) * Instance.Config.MultiplyEarthCrystal); }
        else if (id == ("(O)60")) { number = (int)((number + addedOres + Instance.Config.AddEmerald) * Instance.Config.MultiplyEmerald); }
        else if (id == ("(O)82")) { number = (int)((number + addedOres + Instance.Config.AddFireQuartz) * Instance.Config.MultiplyFireQuartz); }
        else if (id == ("(O)84")) { number = (int)((number + addedOres + Instance.Config.AddFrozenTear) * Instance.Config.MultiplyFrozenTear); }
        else if (id == ("(O)80")) { number = (int)((number + addedOres + Instance.Config.AddQuartz) * Instance.Config.MultiplyQuartz); }
        else if (id == ("(O)64")) { number = (int)((number + addedOres + Instance.Config.AddRuby) * Instance.Config.MultiplyRuby); }
        else if (id == ("(O)68")) { number = (int)((number + addedOres + Instance.Config.AddTopaz) * Instance.Config.MultiplyTopaz); }
        else if (id == ("(O)70")) { number = (int)((number + addedOres + Instance.Config.AddJade) * Instance.Config.MultiplyJade); }

        return number;
    }



}