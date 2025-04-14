using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley.Extensions;
using StardewValley.Locations;
using Microsoft.Xna.Framework;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = StardewValley.Object;
using MineForMore;
using System.Xml.Linq;
using System.Security.Cryptography;

internal class UpdateOreGemDropsPatch
{
    private readonly Config _config;

    public UpdateOreGemDropsPatch(Config config)
    {
        _config = config;
    }

    public void Apply(Harmony harmony, IMonitor monitor)
    {
        // Patch all overloads of createMultipleObjectDebris
        //PatchCreateMultipleObjectDebris(harmony);

        // Patch stone destruction
        var stoneDestroyed = AccessTools.Method(typeof(GameLocation), nameof(GameLocation.OnStoneDestroyed));
        harmony.Patch(stoneDestroyed, prefix: new HarmonyMethod(typeof(UpdateOreGemDropsPatch), nameof(ModifiedOnStoneDestroyed)));
    }
    /*
    private void PatchCreateMultipleObjectDebris(Harmony harmony)
    {
        var methods = new (Type[] args, string name)[]
        {
            (new[] { typeof(string), typeof(int), typeof(int), typeof(int) }, nameof(Before_createMultipleObjectDebris1)),
            (new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(GameLocation) }, nameof(Before_createMultipleObjectDebris2)),
            (new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(float) }, nameof(Before_createMultipleObjectDebris3)),
            (new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(long) }, nameof(Before_createMultipleObjectDebris4)),
            (new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(long), typeof(GameLocation) }, nameof(Before_createMultipleObjectDebris5))
        };

        foreach (var (args, patchName) in methods)
        {
            var method = AccessTools.Method(typeof(Game1), nameof(Game1.createMultipleObjectDebris), args);
            var prefix = new HarmonyMethod(typeof(UpdateOreGemDropsPatch), patchName);
            harmony.Patch(method, prefix: prefix);
        }
    }


    
    // === Debris Interception Methods ===

    public static bool Before_createMultipleObjectDebris1(string id, int xTile, int yTile, int number)
    {
        number = getNumber(id, number);
        for (int i = 0; i < number; i++)
            Game1.createObjectDebris(id, xTile, yTile);
        return false;
    }

    public static bool Before_createMultipleObjectDebris2(string id, int xTile, int yTile, int number, GameLocation location)
    {
        number = getNumber(id, number);
        for (int i = 0; i < number; i++)
            Game1.createObjectDebris(id, xTile, yTile, -1, 0, 1f, location);
        return false;
    }

    public static bool Before_createMultipleObjectDebris3(string id, int xTile, int yTile, int number, float velocityMultiplier)
    {
        number = getNumber(id, number);
        for (int i = 0; i < number; i++)
            Game1.createObjectDebris(id, xTile, yTile, -1, 0, velocityMultiplier);
        return false;
    }

    public static bool Before_createMultipleObjectDebris4(string id, int xTile, int yTile, int number, long who)
    {
        number = getNumber(id, number, who);
        for (int i = 0; i < number; i++)
            Game1.createObjectDebris(id, xTile, yTile, who);
        return false;
    }

    public static bool Before_createMultipleObjectDebris5(string id, int xTile, int yTile, int number, long who, GameLocation location)
    {
        number = getNumber(id, number, who);
        for (int i = 0; i < number; i++)
            Game1.createObjectDebris(id, xTile, yTile, who, location);
        return false;
    }


    // === Helper: Drop Quantity Calculation ===

    public static int getNumber(string id, int number)
    {
        foreach (var entry in ModEntry.Instance.Config.MiningTypeDrop)
        {
            // Skip the special case: Ore entry that drops item (O)390 (Stone)
            if (entry.ObjectID == "(O)390" && entry.Type.Equals("Ore", StringComparison.OrdinalIgnoreCase))
                continue;

            if (entry.DropsFromObjectIDs?.Contains(id) == true)
            {
                number = (int)((number + entry.AddAmount) * entry.Multiplier);
                break;
            }
        }
        return number;
    }


    public static int getNumber(string id, int number, long who)
    {
        if (who != null)
        {
            Farmer farmer = Game1.getAllFarmers().FirstOrDefault(f => f.UniqueMultiplayerID == who);
            if (farmer != null)
            {
                foreach (var entry in ModEntry.Instance.Config.MiningTypeDrop)
                {
                    // Skip the "(O)390" stone drop entry from Ore type
                    if (entry.ObjectID == "(O)390" && entry.Type.Equals("Ore", StringComparison.OrdinalIgnoreCase))
                        continue;

                    bool matches = entry.DropsFromObjectIDs.Any(ID =>
                        (ID.StartsWith("(O)") && ID.Length > 3 && ID.Substring(3) == id) || ID == id);

                    if (!matches)
                        continue;

                    float bonus = 0f;

                    // Apply profession bonus based on entry type
                    if (entry.Type.Equals("Ore", StringComparison.OrdinalIgnoreCase) && farmer.professions.Contains(18))
                        bonus = ModEntry.Instance.Config.MinerProfessionBonusOrePerLevel * farmer.MiningLevel;
                    else if (entry.Type.Equals("Gem", StringComparison.OrdinalIgnoreCase) && farmer.professions.Contains(19))
                        bonus = ModEntry.Instance.Config.GeologistProfessionBonusGemsPerLevel * farmer.MiningLevel;
                    else if (entry.Type.Equals("Geode", StringComparison.OrdinalIgnoreCase) && farmer.professions.Contains(22))
                        bonus = ModEntry.Instance.Config.ExcavatorProfessionBonusGeodesPerLevel * farmer.MiningLevel;
                    else if (entry.Type.Equals("Ore", StringComparison.OrdinalIgnoreCase) &&
                             entry.ObjectID == "(O)382" && farmer.professions.Contains(21)) // Coal check
                        bonus = ModEntry.Instance.Config.ProspectorProfessionBonusCoalPerLevel * farmer.MiningLevel;

                    int num = (int)((entry.AddAmount + bonus) * entry.Multiplier);

                    if (num > 0)
                        return number;
                }
            }
        }

        return number;
    }


    
    // === Patch: Custom Drops on Stone Destruction ===

    public static void ModifiedOnStoneDestroyed(string stoneId, int x, int y, Farmer who)
    {
        // Only proceed if stoneId is associated with a "Stone"-type drop source
        bool isValidStone = ModEntry.Instance.Config.MiningTypeDrop.Any(entry =>
            entry.Type.Equals("Stone", StringComparison.OrdinalIgnoreCase) &&
            entry.DropsFromObjectIDs.Any(id =>
                (id.StartsWith("(O)") && id.Substring(3) == stoneId) || id == stoneId));

        if (!isValidStone)
            return;

        // Log destruction if setting enabled
        if (ModEntry.Instance.Config.listStoneDestroyedInConsole &&
            Game1.objectData.TryGetValue(stoneId, out StardewValley.GameData.Objects.ObjectData value))
        {
            ModEntry.Instance.Monitor.Log($"Stone destroyed: {value.Name ?? "Unknown"} (ID: {stoneId}) at ({x}, {y})", LogLevel.Info);
        }

        if (who == null)
            return;

        foreach (var entry in ModEntry.Instance.Config.MiningTypeDrop)
        {
            bool matches = entry.DropsFromObjectIDs.Any(id =>
                (id.StartsWith("(O)") && id.Substring(3) == stoneId) || id == stoneId);

            if (!matches)
                continue;

            float bonus = 0f;

            // Example profession logic
            if (entry.Type.Equals("Stone", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(18))
                bonus = ModEntry.Instance.Config.MinerProfessionBonusOrePerLevel * who.MiningLevel;

            int num = (int)((entry.AddAmount + bonus) * entry.Multiplier);

            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                    Game1.createObjectDebris(entry.ObjectID, x, y, who.UniqueMultiplayerID);
            }
        }
    }

    */




    //for all
    public static void ModifiedOnStoneDestroyed(string stoneId, int x, int y, Farmer who)
    {


        // Log stone destruction if setting enabled
        if (ModEntry.Instance.Config.listStoneDestroyedInConsole &&
            Game1.objectData.TryGetValue(stoneId, out StardewValley.GameData.Objects.ObjectData value))
        {
            ModEntry.Instance.Monitor.Log($"Stone destroyed: {value.Name ?? "Unknown"} (ID: {stoneId}) at ({x}, {y})", LogLevel.Info);
        }

        if (who == null)
            return;

        foreach (var entry in ModEntry.Instance.Config.MiningTypeDrop)
        {
            // Check if stoneId matches any of the valid source nodes
            bool matches = entry.DropsFromObjectIDs.Any(id =>
                (id.StartsWith("(O)") && id.Length > 3 && id.Substring(3) == stoneId) || id == stoneId);

            if (!matches)
                continue;

            float bonus = 0f;

            // Apply profession bonus based on type
            if (entry.Type.Equals("Ore", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(18))
                bonus = ModEntry.Instance.Config.MinerProfessionBonusOrePerLevel * who.MiningLevel;
            else if (entry.Type.Equals("Gem", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(19))
                bonus = ModEntry.Instance.Config.GeologistProfessionBonusGemsPerLevel * who.MiningLevel;
            else if (entry.Type.Equals("Geode", StringComparison.OrdinalIgnoreCase) && who.professions.Contains(22))
                bonus = ModEntry.Instance.Config.ExcavatorProfessionBonusGeodesPerLevel * who.MiningLevel;
            else if (entry.Type.Equals("Ore", StringComparison.OrdinalIgnoreCase) &&
                     entry.ObjectID == "(O)382" && who.professions.Contains(21)) // Coal drop check
                bonus = ModEntry.Instance.Config.ProspectorProfessionBonusCoalPerLevel * who.MiningLevel;

            int num = (int)((entry.AddAmount + bonus) * entry.Multiplier);

            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                    Game1.createObjectDebris(entry.ObjectID, x, y, who.UniqueMultiplayerID);
            }

        }
    }




}
