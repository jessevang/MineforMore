
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Spacechase.Shared.Patching;
using MineForMore;
using static MineForMore.ModEntry;
using StardewValley.Menus;
using System.Reflection.Emit;
using StardewValley.Locations;



/// <summary>Applies Harmony patches to <see cref="GameLocation"/>.</summary>

internal class MineProfessionLevelDescriptionPatch : BasePatcher
{

    private readonly Config _config;

    public MineProfessionLevelDescriptionPatch(Config config)
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




    }


    public static void ModifyProfessionDescription(int whichProfession, ref List<string> __result)
    {

        // Farmer.cs Miner Profession ID is 18 (Miner)
        if (whichProfession == 18)
        {
            __result.Clear(); // Clear the existing list first (optional, just to be safe)
            __result.Add("Miner" + "\n" + "More Ores per vein"); // Custom profession name
            __result.Add("Additional +" + Instance.Config.MinerProfessionBonusOrePerLevel + " ores per mining level\n" +
                "Stone, Copper, Iron, Gold, Iridium, Radiactive");  // Custom profession description
   
        }
        // Farmer.cs Miner Profession ID is 19 (Geologist) 
        else if (whichProfession == 19)
        {
            __result.Clear(); 
            __result.Add("Geologist" + "\n" + "More gems per vein"); 
            __result.Add("+(" + (int)Instance.Config.GeologistProfessionBonusGemsPerLevel + ") additional gems per mining level\n"+ "Diamond, Amethyst, Aquamarine, Emerald, Quartz, Ruby, Topaz, Jade");

 

}

        
        // Farmer.cs Miner Profession ID is 20 (Blacksmith) 
        else if (whichProfession == 20)
        {
            __result.Clear();
            __result.Add("Blacksmith");
            __result.Add("Metal bars worth 50% more.\n New crafting recipe: QuickSmelt \n Smelt ores into bars instantly but cost more resource");

        }
        // Miner Profession ID is 21 (burrower) probably renamed to "Prospector"
        else if (whichProfession == 21)
        {
            __result.Clear();
            __result.Add("Prospector");
            __result.Add("Chance to find Coal doubled. +" 
                + Instance.Config.ProspectorProfessionBonusCoalPerLevel + " extra coal drops per mining level. +1% nodes are coal Nodes"); // Custom profession description

        }
        // Miner Profession ID is 22 (Excavator) 
        else if (whichProfession == 22)
        {
            __result.Clear();
            __result.Add("Excavator");
            __result.Add("Chance to find geodes doubled. +" + Instance.Config.ExcavatorProfessionBonusGeodesPerLevel +" additional node drop per mining level.\n +1% nodes are Geode Nodes"); // Custom profession description

        }
        // Miner Profession ID is 23 (Gemologist) 
        else if (whichProfession == 23)
        {
            __result.Clear();
            __result.Add("Gemologist");
            __result.Add("Gems Worth 30% more. \n +1% nodes are gem nodes"); // Custom profession description

        }

        


    }


    private static IEnumerable<CodeInstruction> ModifyLevelUpMenu(IEnumerable<CodeInstruction> instructions)
    {
        var codes = new List<CodeInstruction>(instructions);
        for (int i = 0; i < codes.Count; i++)
        {
            // Look for the instruction setting the height
            if (codes[i].opcode == OpCodes.Ldc_I4 && codes[i].operand is int value && value == 512)
            {
                codes[i].operand = 612; // Increase the height (adjust as needed)
                break;
            }
        }
        return codes;
    }


    

}