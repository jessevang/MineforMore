
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Spacechase.Shared.Patching;
using MineForMore;
using static MineForMore.ModEntry;


/// <summary>Applies Harmony patches to <see cref="GameLocation"/>.</summary>

internal class UpdateOreGemDropsPatch : BasePatcher
{

    private readonly Config _config;

    public UpdateOreGemDropsPatch(Config config)
    {
        _config = config;
    }

    public override void Apply(Harmony harmony, IMonitor monitor)
    {
 

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

        harmony.Patch(
            original: this.RequireMethod<GameLocation>(
                nameof(GameLocation.OnStoneDestroyed) // Correct parameter types
            ),
            prefix: this.GetHarmonyMethod(nameof(ModifiedOnStoneDestroyed)) // prefix method
        );





    }



    


    //2. 4 sets of function that is called when stone breaks and a ore drops.
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


    //2. adds number for more ore drops used for multiple debris function without professions
    public static int getNumber(string id, int number)
    {

        if (id == ("(O)390")) { number = (int)((number + Instance.Config.AddStone) * Instance.Config.MultiplyStone); }
        if (id == ("(O)382")) { number = (int)((number + Instance.Config.AddCoal) * Instance.Config.MultiplyCoal); }
        if (id == ("(O)378")) { number = (int)((number + Instance.Config.AddCooperOre) * Instance.Config.MultiplyCooperOre); }
        if (id == ("(O)384")) { number = (int)((number + Instance.Config.AddGoldOre) * Instance.Config.MultiplyGoldOre); }
        if (id == ("(O)380")) { number = (int)((number + Instance.Config.AddIronOre) * Instance.Config.MultiplyIronOre); }
        if (id == ("(O)386")) { number = (int)((number + Instance.Config.AddIridiumOre) * Instance.Config.MultiplyIridiumOre); }
        if (id == ("(O)909")) { number = (int)((number + Instance.Config.AddRadiactiveOre) * Instance.Config.MultiplyRadiactiveOre); }
        if (id == ("(O)72")) { number = (int)((number + Instance.Config.AddDiamond) * Instance.Config.MultiplyDiamond); }
        if (id == ("(O)66")) { number = (int)((number + Instance.Config.AddAmethyst) * Instance.Config.MultiplyAmethyst); }
        if (id == ("(O)62")) { number = (int)((number + Instance.Config.AddAquamarine) * Instance.Config.MultiplyAquamarine); }
        if (id == ("(O)86")) { number = (int)((number + Instance.Config.AddEarthCrystal) * Instance.Config.MultiplyEarthCrystal); }
        if (id == ("(O)60")) { number = (int)((number + Instance.Config.AddEmerald) * Instance.Config.MultiplyEmerald); }
        if (id == ("(O)64")) { number = (int)((number + Instance.Config.AddRuby) * Instance.Config.MultiplyRuby); }
        if (id == ("(O)68")) { number = (int)((number + Instance.Config.AddTopaz) * Instance.Config.MultiplyTopaz); }
        if (id == ("(O)70")) { number = (int)((number + Instance.Config.AddJade) * Instance.Config.MultiplyJade); }

        
        return number;
    }

    //2. Added a 2nd method to add double Add due to professional to get gets double ores. used for multiple debris function
    public static int getNumber(string id, int number, long who)
    {

       Farmer forPlayer = Game1.GetPlayer(who) ?? Game1.player;
       Double MinerProfessionBonusOrePerLevel = ((forPlayer != null && forPlayer.professions.Contains(18)) ? (Instance.Config.MinerProfessionBonusOrePerLevel * forPlayer.MiningLevel) : 0.0);

        double GeologistGemsPerLevel = ((forPlayer != null && forPlayer.professions.Contains(19)) ? (Instance.Config.GeologistProfessionBonusGemsPerLevel * forPlayer.MiningLevel) : 0.0);
        double ProspectorProfessionBonusCoalPerLevel = ((forPlayer != null && forPlayer.professions.Contains(21)) ? (Instance.Config.ProspectorProfessionBonusCoalPerLevel * forPlayer.MiningLevel) : 0.0);
    
        //Console.WriteLine("Item is: " + id + " - Number Count Before adding value is: " + number + " StoneCount is " + Instance.Config.AddStone + " professionAddedOres: "+ Instance.Config.BonusOresWithMinerProfession+  " StoneMultiplyValue is: " + Instance.Config.MultiplyStone);


        //handles ores
        if (id == ("(O)390")) { number = (int)((number + MinerProfessionBonusOrePerLevel + Instance.Config.AddStone) * Instance.Config.MultiplyStone);}
        if (id == ("(O)378")) { number = (int)((number + MinerProfessionBonusOrePerLevel + Instance.Config.AddCooperOre) * Instance.Config.MultiplyCooperOre); }
        if (id == ("(O)384")) { number = (int)((number + MinerProfessionBonusOrePerLevel + Instance.Config.AddGoldOre) * Instance.Config.MultiplyGoldOre); }
        if (id == ("(O)380")) { number = (int)((number + MinerProfessionBonusOrePerLevel + Instance.Config.AddIronOre) * Instance.Config.MultiplyIronOre); }
        if (id == ("(O)386")) { number = (int)((number + MinerProfessionBonusOrePerLevel + Instance.Config.AddIridiumOre) * Instance.Config.MultiplyIridiumOre); }
        if (id == ("(O)909")) { number = (int)((number + MinerProfessionBonusOrePerLevel + Instance.Config.AddRadiactiveOre) * Instance.Config.MultiplyRadiactiveOre); }

        //Hnadles Coal
        if (id == ("(O)382")) { number = (int)((number + ProspectorProfessionBonusCoalPerLevel + Instance.Config.AddCoal) * Instance.Config.MultiplyCoal); }

        //Handle Gems
        if (id == ("(O)72")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddDiamond) * Instance.Config.MultiplyDiamond); }
        if (id == ("(O)66")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddAmethyst)  * Instance.Config.MultiplyAmethyst); }
        if (id == ("(O)62")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddAquamarine)  * Instance.Config.MultiplyAquamarine); }
        if (id == ("(O)86")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddEarthCrystal)  * Instance.Config.MultiplyEarthCrystal); }
        if (id == ("(O)60")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddEmerald)  * Instance.Config.MultiplyEmerald); }
        if (id == ("(O)64")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddRuby)  * Instance.Config.MultiplyRuby); }
        if (id == ("(O)68")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddTopaz)  * Instance.Config.MultiplyTopaz); }
        if (id == ("(O)70")) { number = (int)((number + GeologistGemsPerLevel + Instance.Config.AddJade)  * Instance.Config.MultiplyJade); }





        return number;
    }



    public static bool ModifiedOnStoneDestroyed(string stoneId, int x, int y, Farmer who)
    {
        int number = 0;

        // Check if the Excavator profession and adds extra Geode drops
        if (who.professions.Contains(22))
        {
            if (stoneId.Equals("75")) // If stone is geo, drop extra geodes.
            {
                number = getNumberForGeodes("535", who);
                for (int i = 0; i < number; i++)
                {
                    Game1.createObjectDebris("535", x, y, who.UniqueMultiplayerID);
                }
            }
            else if (stoneId.Equals("76"))
            {
                number = getNumberForGeodes("536", who);
                for (int i = 0; i < number; i++)
                {
                    Game1.createObjectDebris("536", x, y, who.UniqueMultiplayerID);
                }
            }
            else if (stoneId.Equals("77"))
            {
                number = getNumberForGeodes("537", who);
                for (int i = 0; i < number; i++)
                {
                    Game1.createObjectDebris("537", x, y, who.UniqueMultiplayerID);
                }
            }
            else if (stoneId.Equals("819"))
            {
                number = getNumberForGeodes("749", who);
                for (int i = 0; i < number; i++)
                {
                    Game1.createObjectDebris("749", x, y, who.UniqueMultiplayerID);
                }
            }

        }

        // Check if the Miner Profession adds additional stone drops if the node is one of the stone versions.
        if (who.professions.Contains(18))
        {

            if (stoneId == "450"|| stoneId == "32" || stoneId == "34" || stoneId == "36" || stoneId == "38" 
                || stoneId == "40" || stoneId == "343" || stoneId == "760" || stoneId == "762" 
                || stoneId == "48" || stoneId == "50" || stoneId == "52" || stoneId == "54" || stoneId == "56" || stoneId == "58"
                || stoneId == "668" || stoneId == "670" || stoneId == "845" || stoneId == "846" || stoneId == "847")

            {

                Double MinerProfessionBonusOrePerLevel = Instance.Config.MinerProfessionBonusOrePerLevel * who.MiningLevel;
                number = (int)((number + MinerProfessionBonusOrePerLevel + Instance.Config.AddStone) * Instance.Config.MultiplyStone);
                for (int i = 0; i < number; i++)
                {
                    Game1.createObjectDebris("(O)390", x, y, Game1.player.UniqueMultiplayerID);
                }

            }

        }



            return true;
    }

    //handle geodes
    public static int getNumberForGeodes(string id, StardewValley.Farmer who)
    {

        int number = 0;
        double ExcavatorProfessionBonusGeodesPerLevel = Instance.Config.ExcavatorProfessionBonusGeodesPerLevel * who.MiningLevel;
        
        if (id == ("535")) { number = (int)((number + ExcavatorProfessionBonusGeodesPerLevel + Instance.Config.AddGeode) * Instance.Config.MultiplyGeode); }
        if (id == ("536")) { number = (int)((number + ExcavatorProfessionBonusGeodesPerLevel + Instance.Config.AddFrozenGeode) * Instance.Config.MultiplyFrozenGeode); }
        if (id == ("537")) { number = (int)((number + ExcavatorProfessionBonusGeodesPerLevel + Instance.Config.AddMagmaGeode) * Instance.Config.MultiplyMagmaGeode); }
        if (id == ("749")) { number = (int)((number + ExcavatorProfessionBonusGeodesPerLevel + Instance.Config.AddOmniGeode) * Instance.Config.MultiplyOmniGeode); }


        return number;
    }




}