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
    public static void Before_createMultipleObjectDebris1(string id, int xTile, int yTile, int number)
    {
        number = getNumber(id, number);

            for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile);
        }
    }

    public static void Before_createMultipleObjectDebris2(string id, int xTile, int yTile, int number, GameLocation location)
    {
        number = getNumber(id, number);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, -1, 0, 1f, location);
        }
    }

    public static void Before_createMultipleObjectDebris3(string id, int xTile, int yTile, int number, float velocityMultiplier)
    {
        number = getNumber(id, number);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, -1, 0, velocityMultiplier);
        }
    }

    public static void Before_createMultipleObjectDebris4(string id, int xTile, int yTile, int number, long who)
    {
        number = getNumber(id, number, who);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, who);
        }
    }

    public static void Before_createMultipleObjectDebris5(string id, int xTile, int yTile, int number, long who, GameLocation location)
    {
        number = getNumber(id, number, who);
        for (int i = 0; i < number; i++)
        {
            Game1.createObjectDebris(id, xTile, yTile, who, location);
        }
    }


    //adds number for more ore drops
    public static int getNumber(string id, int number)
    {

        int AddStoneCount = 5;
        int AddCooperOreCount = 5;
        int AddGoldOreCount = 5;
        int AddIronOreCount = 5;
        int AddIridiumOreCount = 5;
        int AddRadiactiveOreCount = 5;
        int AddDiamondCount = 5;
        int AddAmethystCount = 5;
        int AddAquamarineCount = 5;
        int AddEarthCrystalCount = 5;
        int AddEmeraldCount = 5;
        int AddFireQuartzCount = 5;
        int AddFrozenTearCount = 5;
        int AddQuartzCount = 5;
        int AddRubyCount = 5;
        int AddTopazCount = 5;
        int AddJadeCount = 5;
        double MultiplyStoneCount = 1.5;
        double MultiplyCooperOreCount = 1.5;
        double MultiplyGoldOreCount = 1.5;
        double MultiplyIronOreCount = 1.5;
        double MultiplyIridiumOreCount = 1.5;
        double MultiplyRadiactiveOreCount = 1.5;
        double MultiplyDiamondCount = 1.5;
        double MultiplyAmethystCount = 1.5;
        double MultiplyAquamarineCount = 1.5;
        double MultiplyEarthCrystalCount = 1.5;
        double MultiplyEmeraldCount = 1.5;
        double MultiplyFireQuartzCount = 1.5;
        double MultiplyFrozenTearCount = 1.5;
        double MultiplyQuartzCount = 1.5;
        double MultiplyRubyCount = 1.5;
        double MultiplyTopazCount = 1.5;
        double MultiplyJadeCount = 1.5;

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
                if (propertyName == "AddStoneCount")
                {
                    AddStoneCount = (int)((JProperty)item).Value;

                }

                else if (propertyName == "AddStoneCount") { AddStoneCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddCooperOreCount") { AddCooperOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddGoldOreCount") { AddGoldOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIronOreCount") { AddIronOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIridiumOreCount") { AddIridiumOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRadiactiveOreCount") { AddRadiactiveOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddDiamondCount") { AddDiamondCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAmethystCount") { AddAmethystCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAquamarineCount") { AddAquamarineCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEarthCrystalCount") { AddEarthCrystalCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEmeraldCount") { AddEmeraldCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFireQuartzCount") { AddFireQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFrozenTearCount") { AddFrozenTearCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddQuartzCount") { AddQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRubyCount") { AddRubyCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddTopazCount") { AddTopazCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddJadeCount") { AddJadeCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyStoneCount") { MultiplyStoneCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyCooperOreCount") { MultiplyCooperOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyGoldOreCount") { MultiplyGoldOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIronOreCount") { MultiplyIronOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIridiumOreCount") { MultiplyIridiumOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRadiactiveOreCount") { MultiplyRadiactiveOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyDiamondCount") { MultiplyDiamondCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAmethystCount") { MultiplyAmethystCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAquamarineCount") { MultiplyAquamarineCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEarthCrystalCount") { MultiplyEarthCrystalCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEmeraldCount") { MultiplyEmeraldCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFireQuartzCount") { MultiplyFireQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFrozenTearCount") { MultiplyFrozenTearCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyQuartzCount") { MultiplyQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRubyCount") { MultiplyRubyCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyTopazCount") { MultiplyTopazCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyJadeCount") { MultiplyJadeCount = (int)((JProperty)item).Value; }


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

   

        //Adds and Mulitple Number if item is part of one of these items
        if (id == ("(O)390")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)343")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)450")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)760")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)762")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)845")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)846")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)847")) { number = (int)((number + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)378")) { number = (int)((number + AddCooperOreCount) * MultiplyCooperOreCount); }
        if (id == ("(O)384")) { number = (int)((number + AddGoldOreCount) * MultiplyGoldOreCount); }
        if (id == ("(O)380")) { number = (int)((number + AddIronOreCount) * MultiplyIronOreCount); }
        if (id == ("(O)386")) { number = (int)((number + AddIridiumOreCount) * MultiplyIridiumOreCount); }
        if (id == ("(O)909")) { number = (int)((number + AddRadiactiveOreCount) * MultiplyRadiactiveOreCount); }
        if (id == ("(O)72")) { number = (int)((number + AddDiamondCount) * MultiplyDiamondCount); }
        if (id == ("(O)66")) { number = (int)((number + AddAmethystCount) * MultiplyAmethystCount); }
        if (id == ("(O)62")) { number = (int)((number + AddAquamarineCount) * MultiplyAquamarineCount); }
        if (id == ("(O)86")) { number = (int)((number + AddEarthCrystalCount) * MultiplyEarthCrystalCount); }
        if (id == ("(O)60")) { number = (int)((number + AddEmeraldCount) * MultiplyEmeraldCount); }
        if (id == ("(O)82")) { number = (int)((number + AddFireQuartzCount) * MultiplyFireQuartzCount); }
        if (id == ("(O)84")) { number = (int)((number + AddFrozenTearCount) * MultiplyFrozenTearCount); }
        if (id == ("(O)80")) { number = (int)((number + AddQuartzCount) * MultiplyQuartzCount); }
        if (id == ("(O)64")) { number = (int)((number + AddRubyCount) * MultiplyRubyCount); }
        if (id == ("(O)68")) { number = (int)((number + AddTopazCount) * MultiplyTopazCount); }
        if (id == ("(O)70")) { number = (int)((number + AddJadeCount) * MultiplyJadeCount); }


        return number;
    }

    //Added a 2nd method to add double Add due to professional to get gets double ores
    public static int getNumber(string id, int number, long who)
    {
        int BonusOresWithMinerProfession = 1;
        int AddStoneCount = 5;
        int AddCooperOreCount = 5;
        int AddGoldOreCount = 5;
        int AddIronOreCount = 5;
        int AddIridiumOreCount = 5;
        int AddRadiactiveOreCount = 5;
        int AddDiamondCount = 5;
        int AddAmethystCount = 5;
        int AddAquamarineCount = 5;
        int AddEarthCrystalCount = 5;
        int AddEmeraldCount = 5;
        int AddFireQuartzCount = 5;
        int AddFrozenTearCount = 5;
        int AddQuartzCount = 5;
        int AddRubyCount = 5;
        int AddTopazCount = 5;
        int AddJadeCount = 5;
        double MultiplyStoneCount = 1.5;
        double MultiplyCooperOreCount = 1.5;
        double MultiplyGoldOreCount = 1.5;
        double MultiplyIronOreCount = 1.5;
        double MultiplyIridiumOreCount = 1.5;
        double MultiplyRadiactiveOreCount = 1.5;
        double MultiplyDiamondCount = 1.5;
        double MultiplyAmethystCount = 1.5;
        double MultiplyAquamarineCount = 1.5;
        double MultiplyEarthCrystalCount = 1.5;
        double MultiplyEmeraldCount = 1.5;
        double MultiplyFireQuartzCount = 1.5;
        double MultiplyFrozenTearCount = 1.5;
        double MultiplyQuartzCount = 1.5;
        double MultiplyRubyCount = 1.5;
        double MultiplyTopazCount = 1.5;
        double MultiplyJadeCount = 1.5;

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
                if (propertyName == "AddStoneCount")
                {
                    AddStoneCount = (int)((JProperty)item).Value;

                }

                else if (propertyName == "AddStoneCount") { AddStoneCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddCooperOreCount") { AddCooperOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddGoldOreCount") { AddGoldOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIronOreCount") { AddIronOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddIridiumOreCount") { AddIridiumOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRadiactiveOreCount") { AddRadiactiveOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddDiamondCount") { AddDiamondCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAmethystCount") { AddAmethystCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddAquamarineCount") { AddAquamarineCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEarthCrystalCount") { AddEarthCrystalCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddEmeraldCount") { AddEmeraldCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFireQuartzCount") { AddFireQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddFrozenTearCount") { AddFrozenTearCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddQuartzCount") { AddQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddRubyCount") { AddRubyCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddTopazCount") { AddTopazCount = (int)((JProperty)item).Value; }
                else if (propertyName == "AddJadeCount") { AddJadeCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyStoneCount") { MultiplyStoneCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyCooperOreCount") { MultiplyCooperOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyGoldOreCount") { MultiplyGoldOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIronOreCount") { MultiplyIronOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyIridiumOreCount") { MultiplyIridiumOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRadiactiveOreCount") { MultiplyRadiactiveOreCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyDiamondCount") { MultiplyDiamondCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAmethystCount") { MultiplyAmethystCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyAquamarineCount") { MultiplyAquamarineCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEarthCrystalCount") { MultiplyEarthCrystalCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyEmeraldCount") { MultiplyEmeraldCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFireQuartzCount") { MultiplyFireQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyFrozenTearCount") { MultiplyFrozenTearCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyQuartzCount") { MultiplyQuartzCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyRubyCount") { MultiplyRubyCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyTopazCount") { MultiplyTopazCount = (int)((JProperty)item).Value; }
                else if (propertyName == "MultiplyJadeCount") { MultiplyJadeCount = (int)((JProperty)item).Value; }
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

        //Adds and Mulitple Number if item is part of one of these items
        if (id == ("(O)390")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)343")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)450")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)760")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)762")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)845")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)846")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)847")) { number = (int)((number + addedOres + AddStoneCount) * MultiplyStoneCount); }
        if (id == ("(O)378")) { number = (int)((number + addedOres + AddCooperOreCount) * MultiplyCooperOreCount); }
        if (id == ("(O)384")) { number = (int)((number + addedOres + AddGoldOreCount) * MultiplyGoldOreCount); }
        if (id == ("(O)380")) { number = (int)((number + addedOres + AddIronOreCount) * MultiplyIronOreCount); }
        if (id == ("(O)386")) { number = (int)((number + addedOres + AddIridiumOreCount) * MultiplyIridiumOreCount); }
        if (id == ("(O)909")) { number = (int)((number + addedOres + AddRadiactiveOreCount) * MultiplyRadiactiveOreCount); }
        if (id == ("(O)72")) { number = (int)((number + addedOres + AddDiamondCount) * MultiplyDiamondCount); }
        if (id == ("(O)66")) { number = (int)((number + addedOres + AddAmethystCount) * MultiplyAmethystCount); }
        if (id == ("(O)62")) { number = (int)((number + addedOres + AddAquamarineCount) * MultiplyAquamarineCount); }
        if (id == ("(O)86")) { number = (int)((number + addedOres + AddEarthCrystalCount) * MultiplyEarthCrystalCount); }
        if (id == ("(O)60")) { number = (int)((number + addedOres + AddEmeraldCount) * MultiplyEmeraldCount); }
        if (id == ("(O)82")) { number = (int)((number + addedOres + AddFireQuartzCount) * MultiplyFireQuartzCount); }
        if (id == ("(O)84")) { number = (int)((number + addedOres + AddFrozenTearCount) * MultiplyFrozenTearCount); }
        if (id == ("(O)80")) { number = (int)((number + addedOres + AddQuartzCount) * MultiplyQuartzCount); }
        if (id == ("(O)64")) { number = (int)((number + addedOres + AddRubyCount) * MultiplyRubyCount); }
        if (id == ("(O)68")) { number = (int)((number + addedOres + AddTopazCount) * MultiplyTopazCount); }
        if (id == ("(O)70")) { number = (int)((number + addedOres + AddJadeCount) * MultiplyJadeCount); }


        return number;
    }







}