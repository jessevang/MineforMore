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

/// <summary>Applies Harmony patches to <see cref="GameLocation"/>.</summary>



internal class ObjectPatches : BasePatcher
{



    public override void Apply(Harmony harmony, IMonitor monitor)
    {
        // Debug log to confirm patching process starts
        //Console.WriteLine("Attempting to patch GameLocation.OnStoneDestroyed", LogLevel.Debug);


        /*
        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(long) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createObjectDebris1)) // prefix method
        );

        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(long), typeof(GameLocation) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createObjectDebris2)) // prefix method
        );

        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(GameLocation) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createObjectDebris3)) // prefix method
        );

        harmony.Patch(
            original: this.RequireMethod<Game1>(
                nameof(Game1.createObjectDebris),
                new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(GameLocation) } // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(Before_createObjectDebris4)) // prefix method
        );
        */

        
        //Multiple Objects
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



    public class RootObject
    {
        public string url_short { get; set; }
        public string url_long { get; set; }
        public int type { get; set; }
    }




    //single debris
    /*
    public static void Before_createObjectDebris1(string id, int xTile, int yTile, long whichPlayer)
    {
        int number = getNumber(id, 0);
        for (int i = 0; i < number; i++)
        {


            Farmer forPlayer = Game1.GetPlayer(whichPlayer) ?? Game1.player;
            Game1.currentLocation.debris.Add(new Debris(id, new Vector2(xTile * 64 + 32, yTile * 64 + 32), forPlayer.getStandingPosition()));

        }
    }

    public static void Before_createObjectDebris2(string id, int xTile, int yTile, long whichPlayer, GameLocation location)
    {
        int number = getNumber(id, 0);
        for (int i = 0; i < number; i++)
        {

            Farmer forPlayer = Game1.GetPlayer(whichPlayer) ?? Game1.player;
            location.debris.Add(new Debris(id, new Vector2(xTile * 64 + 32, yTile * 64 + 32), forPlayer.getStandingPosition()));
        }
    }

    public static void Before_createObjectDebris3(string id, int xTile, int yTile, GameLocation location)
    {
        int number = getNumber(id, 0);
        for (int i = 0; i < number; i++)
        {

            Game1.createObjectDebris(id, xTile, yTile, -1, 0, 1f, location);
        }
    }

    public static void Before_createObjectDebris4(string id, int xTile, int yTile, int groundLevel = -1, int itemQuality = 0, float velocityMultiplyer = 1f, GameLocation location = null)
    {
        int number = getNumber(id, 0);
        for (int i = 0; i < number; i++)
        {

            if (location == null)
            {
                location = Game1.currentLocation;
            }
            Debris d = new Debris(id, new Vector2(xTile * 64 + 32, yTile * 64 + 32), Game1.player.getStandingPosition())
            {
                itemQuality = itemQuality
            };
            foreach (Chunk chunk in d.Chunks)
            {
                chunk.xVelocity.Value *= velocityMultiplyer;
                chunk.yVelocity.Value *= velocityMultiplyer;
            }
            if (groundLevel != -1)
            {
                d.chunkFinalYLevel = groundLevel;
            }
            location.debris.Add(d);
        }
    }

    */


    //multiple debris
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


    //adds number for more ore drops
    public static int getNumber(string id, int number)
    {

        int AddStone = 0;
        int AddCoal = 0;
        int AddCooperOre = 0;
        int AddGoldOre = 0;
        int AddIronOre = 0;
        int AddIridiumOre = 0;
        int AddRadiactiveOre = 0;
        int AddDiamond = 0;
        int AddAmethyst = 0;
        int AddAquamarine = 0;
        int AddEarthCrystal = 0;
        int AddEmerald = 0;
        int AddFireQuartz = 0;
        int AddFrozenTear = 0;
        int AddQuartz = 0;
        int AddRuby = 0;
        int AddTopaz = 0;
        int AddJade = 0;
        double MultiplyStone = 1.0;
        double MultiplyCoal = 1.0;
        double MultiplyCooperOre = 1.0;
        double MultiplyGoldOre = 1.0;
        double MultiplyIronOre = 1.0;
        double MultiplyIridiumOre = 1.0;
        double MultiplyRadiactiveOre = 1.0;
        double MultiplyDiamond = 1.0;
        double MultiplyAmethyst = 1.0;
        double MultiplyAquamarine = 1.0;
        double MultiplyEarthCrystal = 1.0;
        double MultiplyEmerald = 1.0;
        double MultiplyFireQuartz = 1.0;
        double MultiplyFrozenTear = 1.0;
        double MultiplyQuartz = 1.0;
        double MultiplyRuby = 1.0;
        double MultiplyTopaz = 1.0;
        double MultiplyJade = 1.0;

        // Get the current directory
        string filePath = Directory.GetCurrentDirectory() + "\\Mods\\MineForMore\\config.json";

        //assign value to from config.json
        try
        {
            // Read the JSON file
            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string into a dynamic object
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);

            // Access the properties of the JSON object
            foreach (var item in jsonObj)
            {
                //Console.WriteLine($"{((JProperty)item).Name}: {((JProperty)item).Value}");
                string propertyName = ((JProperty)item).Name.ToString();
                if(propertyName == "AddStone"){AddStone = (int)((JProperty)item).Value;}
                else if (propertyName == "AddCoal") { AddCoal = (int)((JProperty)item).Value; }
                else if (propertyName == "AddCooperOre") { AddCooperOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddGoldOre") { AddGoldOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIronOre") { AddIronOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIridiumOre") { AddIridiumOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRadiactiveOre") { AddRadiactiveOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddDiamond") { AddDiamond = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAmethyst") { AddAmethyst = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAquamarine") { AddAquamarine = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEarthCrystal") { AddEarthCrystal = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEmerald") { AddEmerald = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFireQuartz") { AddFireQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFrozenTear") { AddFrozenTear = (int)((JProperty)item).Value; }
                else if (propertyName == "AddQuartz") { AddQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRuby") { AddRuby = (int)((JProperty)item).Value; }
                else if (propertyName == "AddTopaz") { AddTopaz = (int)((JProperty)item).Value; }
                else if (propertyName == "AddJade") { AddJade = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyStone") { MultiplyStone = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyCoal") { MultiplyCoal = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyCooperOre") { MultiplyCooperOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyGoldOre") { MultiplyGoldOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIronOre") { MultiplyIronOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIridiumOre") { MultiplyIridiumOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRadiactiveOre") { MultiplyRadiactiveOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyDiamond") { MultiplyDiamond = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAmethyst") { MultiplyAmethyst = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAquamarine") { MultiplyAquamarine = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEarthCrystal") { MultiplyEarthCrystal = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEmerald") { MultiplyEmerald = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFireQuartz") { MultiplyFireQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFrozenTear") { MultiplyFrozenTear = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyQuartz") { MultiplyQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRuby") { MultiplyRuby = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyTopaz") { MultiplyTopaz = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyJade") { MultiplyJade = (int)((JProperty)item).Value; }


            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File '{filePath}' not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine($"Invalid JSON format in '{filePath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }


        //Console.WriteLine("Item is: " + id + " - Number Count Before adding value is: " + number + " StoneMultiplyValue is: " + MultiplyStone);
        //Adds and Mulitple Number if item is part of one of these items
        if (id == ("(O)390")) { number = (int)((number + AddStone) * MultiplyStone); }
        else if (id == ("(O)382")) { number = (int)((number + AddCoal) * MultiplyCoal); }
        else if (id == ("(O)378")) { number = (int)((number + AddCooperOre) * MultiplyCooperOre); }
        else if (id == ("(O)384")) { number = (int)((number + AddGoldOre) * MultiplyGoldOre); }
        else if (id == ("(O)380")) { number = (int)((number + AddIronOre) * MultiplyIronOre); }
        else if (id == ("(O)386")) { number = (int)((number + AddIridiumOre) * MultiplyIridiumOre); }
        else if (id == ("(O)909")) { number = (int)((number + AddRadiactiveOre) * MultiplyRadiactiveOre); }
        else if (id == ("(O)72")) { number = (int)((number + AddDiamond) * MultiplyDiamond); }
        else if (id == ("(O)66")) { number = (int)((number + AddAmethyst) * MultiplyAmethyst); }
        else if (id == ("(O)62")) { number = (int)((number + AddAquamarine) * MultiplyAquamarine); }
        else if (id == ("(O)86")) { number = (int)((number + AddEarthCrystal) * MultiplyEarthCrystal); }
        else if (id == ("(O)60")) { number = (int)((number + AddEmerald) * MultiplyEmerald); }
        else if (id == ("(O)82")) { number = (int)((number + AddFireQuartz) * MultiplyFireQuartz); }
        else if (id == ("(O)84")) { number = (int)((number + AddFrozenTear) * MultiplyFrozenTear); }
        else if (id == ("(O)80")) { number = (int)((number + AddQuartz) * MultiplyQuartz); }
        else if (id == ("(O)64")) { number = (int)((number + AddRuby) * MultiplyRuby); }
        else if (id == ("(O)68")) { number = (int)((number + AddTopaz) * MultiplyTopaz); }
        else if (id == ("(O)70")) { number = (int)((number + AddJade) * MultiplyJade); }

        //Console.WriteLine("Item is: " + id + " - Number Count after addding/multiplying value is: " + number);
        return number;
    }

    //Added a 2nd method to add double Add due to professional to get gets double ores
    public static int getNumber(string id, int number, long who)
    {
        int BonusOresWithMinerProfession = 0;
        int AddStone = 0;
        int AddCoal = 0;
        int AddCooperOre = 0;
        int AddGoldOre = 0;
        int AddIronOre = 0;
        int AddIridiumOre = 0;
        int AddRadiactiveOre = 0;
        int AddDiamond = 0;
        int AddAmethyst = 0;
        int AddAquamarine = 0;
        int AddEarthCrystal = 0;
        int AddEmerald = 0;
        int AddFireQuartz = 0;
        int AddFrozenTear = 0;
        int AddQuartz = 0;
        int AddRuby = 0;
        int AddTopaz = 0;
        int AddJade = 0;
        double MultiplyStone = 1.0;
        double MultiplyCoal = 1.0;
        double MultiplyCooperOre = 1.0;
        double MultiplyGoldOre = 1.0;
        double MultiplyIronOre = 1.0;
        double MultiplyIridiumOre = 1.0;
        double MultiplyRadiactiveOre = 1.0;
        double MultiplyDiamond = 1.0;
        double MultiplyAmethyst = 1.0;
        double MultiplyAquamarine = 1.0;
        double MultiplyEarthCrystal = 1.0;
        double MultiplyEmerald = 1.0;
        double MultiplyFireQuartz = 1.0;
        double MultiplyFrozenTear = 1.0;
        double MultiplyQuartz = 1.0;
        double MultiplyRuby = 1.0;
        double MultiplyTopaz = 1.0;
        double MultiplyJade = 1.0;

        // Get the current directory
        string filePath = Directory.GetCurrentDirectory() + "\\Mods\\MineForMore\\config.json";

        //assign value to from config.json
        try
        {
            // Read the JSON file
            string jsonString = File.ReadAllText(filePath);

            // Deserialize the JSON string into a dynamic object
            dynamic jsonObj = JsonConvert.DeserializeObject(jsonString);

            // Access the properties of the JSON object
            foreach (var item in jsonObj)
            {
                //Console.WriteLine($"{((JProperty)item).Name}: {((JProperty)item).Value}");
                string propertyName = ((JProperty)item).Name.ToString();
                if (propertyName == "AddStone") {AddStone = (int)((JProperty)item).Value;}
                else if (propertyName == "AddCoal") { AddCoal = (int)((JProperty)item).Value; }
                else if (propertyName == "AddCooperOre") { AddCooperOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddGoldOre") { AddGoldOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIronOre") { AddIronOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIridiumOre") { AddIridiumOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRadiactiveOre") { AddRadiactiveOre = (int)((JProperty)item).Value; }
                else if (propertyName == "AddDiamond") { AddDiamond = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAmethyst") { AddAmethyst = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAquamarine") { AddAquamarine = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEarthCrystal") { AddEarthCrystal = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEmerald") { AddEmerald = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFireQuartz") { AddFireQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFrozenTear") { AddFrozenTear = (int)((JProperty)item).Value; }
                else if (propertyName == "AddQuartz") { AddQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRuby") { AddRuby = (int)((JProperty)item).Value; }
                else if (propertyName == "AddTopaz") { AddTopaz = (int)((JProperty)item).Value; }
                else if (propertyName == "AddJade") { AddJade = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyStone") { MultiplyStone = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyCoal") { MultiplyCoal = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyCooperOre") { MultiplyCooperOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyGoldOre") { MultiplyGoldOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIronOre") { MultiplyIronOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIridiumOre") { MultiplyIridiumOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRadiactiveOre") { MultiplyRadiactiveOre = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyDiamond") { MultiplyDiamond = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAmethyst") { MultiplyAmethyst = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAquamarine") { MultiplyAquamarine = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEarthCrystal") { MultiplyEarthCrystal = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEmerald") { MultiplyEmerald = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFireQuartz") { MultiplyFireQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFrozenTear") { MultiplyFrozenTear = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyQuartz") { MultiplyQuartz = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRuby") { MultiplyRuby = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyTopaz") { MultiplyTopaz = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyJade") { MultiplyJade = (int)((JProperty)item).Value; }
                else if (propertyName == "BonusOresWithMinerProfession") { BonusOresWithMinerProfession = (int)((JProperty)item).Value; }


            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File '{filePath}' not found.");
        }
        catch (JsonException)
        {
            Console.WriteLine($"Invalid JSON format in '{filePath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }


        Farmer forPlayer = Game1.GetPlayer(who) ?? Game1.player;
        int addedOres = ((forPlayer != null && forPlayer.professions.Contains(18)) ? BonusOresWithMinerProfession : 0);
       // Console.WriteLine("Item is: " + id + " - Number Count Before adding value is: " + number + " StoneMultiplyValue is: " + MultiplyStone);
        //Adds and Mulitple Number if item is part of one of these items
        if (id == ("(O)390")) { number = (int)((number + addedOres + AddStone) * MultiplyStone); }
        else if (id == ("(O)382")) { number = (int)((number + addedOres + AddCoal) * MultiplyCoal); }
        else if (id == ("(O)378")) { number = (int)((number + addedOres + AddCooperOre) * MultiplyCooperOre); }
        else if (id == ("(O)384")) { number = (int)((number + addedOres + AddGoldOre) * MultiplyGoldOre); }
        else if (id == ("(O)380")) { number = (int)((number + addedOres + AddIronOre) * MultiplyIronOre); }
        else if (id == ("(O)386")) { number = (int)((number + addedOres + AddIridiumOre) * MultiplyIridiumOre); }
        else if (id == ("(O)909")) { number = (int)((number + addedOres + AddRadiactiveOre) * MultiplyRadiactiveOre); }
        else if (id == ("(O)72")) { number = (int)((number + addedOres + AddDiamond) * MultiplyDiamond); }
        else if (id == ("(O)66")) { number = (int)((number + addedOres + AddAmethyst) * MultiplyAmethyst); }
        else if (id == ("(O)62")) { number = (int)((number + addedOres + AddAquamarine) * MultiplyAquamarine); }
        else if (id == ("(O)86")) { number = (int)((number + addedOres + AddEarthCrystal) * MultiplyEarthCrystal); }
        else if (id == ("(O)60")) { number = (int)((number + addedOres + AddEmerald) * MultiplyEmerald); }
        else if (id == ("(O)82")) { number = (int)((number + addedOres + AddFireQuartz) * MultiplyFireQuartz); }
        else if (id == ("(O)84")) { number = (int)((number + addedOres + AddFrozenTear) * MultiplyFrozenTear); }
        else if (id == ("(O)80")) { number = (int)((number + addedOres + AddQuartz) * MultiplyQuartz); }
        else if (id == ("(O)64")) { number = (int)((number + addedOres + AddRuby) * MultiplyRuby); }
        else if (id == ("(O)68")) { number = (int)((number + addedOres + AddTopaz) * MultiplyTopaz); }
        else if (id == ("(O)70")) { number = (int)((number + addedOres + AddJade) * MultiplyJade); }

        //Console.WriteLine("Item is: " + id + " - Number Count after addding/multiplying value is: " + number);
        return number;
    }







}