using GenericModConfigMenu;
using HarmonyLib;
using LevelForMore.Classes;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Objects;
using System;
using System.Numerics;




namespace LevelForMore
{

    /// <summary>
    /// Defines a configurable drop rule used in the Mine For More mod to enhance Mining and Foraging.
    /// Each rule determines which item drops, under what conditions (such as skill type or object ID),
    /// and how many items are added or multiplied. It can also control the chance to spawn extra resource nodes.
    /// This model powers all drop-related behavior for ores, gems, logs, forageables, and more.
    /// </summary>
    public class ResourceDropRule
    {
        public string Name { get; set; } = "";
        public string SkillType { get; set; } = ""; 
        public string Type { get; set; } = "";
        public string ObjectID { get; set; } = "";
        public int AddAmount { get; set; } = 3;
        public double Multiplier { get; set; } = 1.0;
        public float ExtraNodeSpawnChancePercent { get; set; } = 0f;
        public List<string> DropsFromObjectIDs { get; set; } = new();
    }

    /// <summary>
    /// Defines all configurable options for the Mine For More mod, including global toggles,
    /// skill-specific multipliers, and embedded drop rules for mining and foraging resources.
    /// 
    /// This config class controls the behavior of:
    /// - Feature toggles such as exceeding level 10 or enabling extra node spawns
    /// - Mining profession bonuses (ore, gems, coal, geodes)
    /// - Foraging profession bonuses (wood, seeds, tree growth, hardwood, forage quantity)
    /// - Spawn chance bonuses for special nodes (gems, coal, geodes)
    /// - All ResourceDropRules for ores, gems, geodes, forage, logs, stumps, and seasonal wild items
    ///
    /// Each ResourceDropRule defines what item drops, from which object IDs, and under what conditions.
    /// These rules are embedded directly in the config and used by mining/foraging patch systems.
    /// </summary>
    public class Config
    {
        public bool TurnOnMineForMore { get; set; } = true;
        public bool CanMineOnDay1 { get; set; } = true;
        public bool AllowPlayerToExceedLevel10 { get; set; } = true;
        public bool TurnOnProfessionLevelUpDescription { get; set; } = true;
        public bool AllowExtraNodeSpawnsInMine { get; set; } = true;


        public bool DebugMode { get; set; } = false;


        //Mining
        public float MinerProfessionBonusOrePerLevel { get; set; } = 1.0f;
        public float GeologistProfessionBonusGemsPerLevel { get; set; } = 1.0f;
        public float ProspectorProfessionBonusCoalPerLevel { get; set; } = 1.0f;
        public float ExcavatorProfessionBonusGeodesPerLevel { get; set; } = 1.0f;

        //Foraging
        public float ForesterWoodPerLevelBonus { get; set; } = 3.0f;
        public float ForesterSeedPerLevelBonus { get; set; } = 1.0f;
        public float ForesterTreeGrowthPerLevelBonus { get; set; } = 0.10f;
        public float GathererExtraDropPerLevel { get; set; } = 1.5f;
        public float LumberjackHardwoodPerLevelBonus { get; set; } = 0.5f;
        public float LumberjackHardwordDropChancePerLevelBonus { get; set; } = 0.025f;
        public float TapperSpeedBonusPercentPerLevel { get; set; } = 0.25f;

        public float TapperExtraQuantityPerLevel { get; set; } = 1.0f;






        // Ores
        public ResourceDropRule Stone { get; set; } = new() { Name = "Stone", SkillType = "Mining", Type = "Ore", ObjectID = "(O)390", DropsFromObjectIDs = new() { "32", "34", "36", "38", "40", "48", "50", "52", "54", "56", "58", "343", "450", "668", "670", "760", "762", "845", "846", "847" } };
        public ResourceDropRule CopperOre { get; set; } = new() { Name = "Copper Ore", SkillType = "Mining", Type = "Ore", ObjectID = "(O)378", DropsFromObjectIDs = new() { "751" } };
        public ResourceDropRule IronOre { get; set; } = new() { Name = "Iron Ore", SkillType = "Mining", Type = "Ore", ObjectID = "(O)380", DropsFromObjectIDs = new() { "290" } };
        public ResourceDropRule GoldOre { get; set; } = new() { Name = "Gold Ore", SkillType = "Mining", Type = "Ore", ObjectID = "(O)384", DropsFromObjectIDs = new() { "764" } };
        public ResourceDropRule IridiumOre { get; set; } = new() { Name = "Iridium Ore", SkillType = "Mining", Type = "Ore", ObjectID = "(O)386", DropsFromObjectIDs = new() { "765" } };
        public ResourceDropRule RadioactiveOre { get; set; } = new() { Name = "Radioactive Ore", SkillType = "Mining", Type = "Ore", ObjectID = "(O)909", DropsFromObjectIDs = new() { "95" } };
        public ResourceDropRule CinderShard { get; set; } = new() { Name = "Cinder Shard", SkillType = "Mining", Type = "Ore", ObjectID = "(O)848", DropsFromObjectIDs = new() { "843", "844" } };

        // Coal&Others
        public ResourceDropRule Coal { get; set; } = new() { Name = "Coal", SkillType = "Mining", Type = "Coal&Others", ObjectID = "(O)382", DropsFromObjectIDs = new() { "VolcanoCoalNode0", "VolcanoCoalNode1", "BasicCoalNode0", "BasicCoalNode1" } };
        public ResourceDropRule Clay { get; set; } = new() { Name = "Clay", SkillType = "Mining", Type = "Coal&Others", ObjectID = "(O)330", DropsFromObjectIDs = new() { "818" } };

        // Gems
        public ResourceDropRule Diamond { get; set; } = new() { Name = "Diamond", SkillType = "Mining", Type = "Gem", ObjectID = "(O)72", DropsFromObjectIDs = new() { "2" } };
        public ResourceDropRule Amethyst { get; set; } = new() { Name = "Amethyst", SkillType = "Mining", Type = "Gem", ObjectID = "(O)66", DropsFromObjectIDs = new() { "8" } };
        public ResourceDropRule Aquamarine { get; set; } = new() { Name = "Aquamarine", SkillType = "Mining", Type = "Gem", ObjectID = "(O)62", DropsFromObjectIDs = new() { "14" } };
        public ResourceDropRule Emerald { get; set; } = new() { Name = "Emerald", SkillType = "Mining", Type = "Gem", ObjectID = "(O)60", DropsFromObjectIDs = new() { "12" } };
        public ResourceDropRule Topaz { get; set; } = new() { Name = "Topaz", SkillType = "Mining", Type = "Gem", ObjectID = "(O)68", DropsFromObjectIDs = new() { "10" } };
        public ResourceDropRule Ruby { get; set; } = new() { Name = "Ruby", SkillType = "Mining", Type = "Gem", ObjectID = "(O)64", DropsFromObjectIDs = new() { "4" } };
        public ResourceDropRule Jade { get; set; } = new() { Name = "Jade", SkillType = "Mining", Type = "Gem", ObjectID = "(O)70", DropsFromObjectIDs = new() { "6" } };



        // Geodes
        public ResourceDropRule Geode { get; set; } = new() { Name = "Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)535", DropsFromObjectIDs = new() { "75" } };
        public ResourceDropRule FrozenGeode { get; set; } = new() { Name = "Frozen Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)536", DropsFromObjectIDs = new() { "76" } };
        public ResourceDropRule MagmaGeode { get; set; } = new() { Name = "Magma Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)537", DropsFromObjectIDs = new() { "77" } };
        public ResourceDropRule OmniGeode { get; set; } = new() { Name = "Omni Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)749", DropsFromObjectIDs = new() { "819" } };

        // Additional node spawn chance if player has specific professions
        public float GemNodeSpawnChanceBonusWithProfession { get; set; } = 1f;
        public float GeodeNodeSpawnChanceBonusWithProfession { get; set; } = 1f;
        public float CoalNodeSpawnChanceBonusWithProfession { get; set; } = 1f;




        // Foraging: Tree & Stump drops
        public ResourceDropRule Oak_Wood { get; set; } = new() { Name = "Oak Wood", SkillType = "Foraging", Type = "Wood", ObjectID = "(O)388", DropsFromObjectIDs = new() { "TreeOak" } };
        public ResourceDropRule Oak_Seed { get; set; } = new() { Name = "Oak Seed", SkillType = "Foraging", Type = "Seed", ObjectID = "(O)309", AddAmount = 2, DropsFromObjectIDs = new() { "TreeOak" } };
        public ResourceDropRule Oak_Resin { get; set; } = new() { Name = "Oak Resin", SkillType = "Foraging", Type = "Tap", ObjectID = "(O)725", DropsFromObjectIDs = new() { "TreeOak" } };

        public ResourceDropRule Maple_Wood { get; set; } = new() { Name = "Maple Wood", SkillType = "Foraging", Type = "Wood", ObjectID = "(O)388", DropsFromObjectIDs = new() { "TreeMaple" } };
        public ResourceDropRule Maple_Seed { get; set; } = new() { Name = "Maple Seed", SkillType = "Foraging", Type = "Seed", ObjectID = "(O)310", AddAmount = 2, DropsFromObjectIDs = new() { "TreeMaple" } };
        public ResourceDropRule Maple_Syrup { get; set; } = new() { Name = "Maple Syrup", SkillType = "Foraging", Type = "Tap", ObjectID = "(O)724", DropsFromObjectIDs = new() { "TreeMaple" } };

        public ResourceDropRule Pine_Wood { get; set; } = new() { Name = "Pine Wood", SkillType = "Foraging", Type = "Wood", ObjectID = "(O)388", DropsFromObjectIDs = new() { "TreePine" } };
        public ResourceDropRule Pine_Seed { get; set; } = new() { Name = "Pine Cone", SkillType = "Foraging", Type = "Seed", ObjectID = "(O)311", AddAmount = 2, DropsFromObjectIDs = new() { "TreePine" } };
        public ResourceDropRule Pine_Tar { get; set; } = new() { Name = "Pine Tar", SkillType = "Foraging", Type = "Tap", ObjectID = "(O)726", DropsFromObjectIDs = new() { "TreePine" } };

        public ResourceDropRule Mahogany_Hardwood { get; set; } = new() { Name = "Mahogany Hardwood", SkillType = "Foraging", Type = "Hardwood", ObjectID = "(O)709", DropsFromObjectIDs = new() { "TreeMahogany" } };
        public ResourceDropRule Mahogany_Seed { get; set; } = new() { Name = "Mahogany Seed", SkillType = "Foraging", Type = "Seed", ObjectID = "(O)292", AddAmount = 2, DropsFromObjectIDs = new() { "TreeMahogany" } };
        public ResourceDropRule Mahogany_Sap { get; set; } = new() { Name = "Sap", SkillType = "Foraging", Type = "Tap", ObjectID = "(O)92", DropsFromObjectIDs = new() { "TreeMahogany" } };

        public ResourceDropRule LargeStump { get; set; } = new() { Name = "Large Stump Hardwood", SkillType = "Foraging", Type = "Hardwood", ObjectID = "(O)709", DropsFromObjectIDs = new() { "LargeStump" }, AddAmount = 2, Multiplier = 1.0 };
        public ResourceDropRule LargeLog { get; set; } = new() { Name = "Large Log Hardwood", SkillType = "Foraging", Type = "Hardwood", ObjectID = "(O)709", DropsFromObjectIDs = new() { "LargeLog" }, AddAmount = 3, Multiplier = 1.0 };




        //Foraged items that are gathered in Any Season
        public ResourceDropRule Sap { get; set; } = new() { Name = "Sap", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)92", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "92" } };
        public ResourceDropRule SeaUrchin { get; set; } = new() { Name = "Sea Urchin", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)397", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "397" } };
        public ResourceDropRule Coral { get; set; } = new() { Name = "Coral", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)393", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "393" } };
        public ResourceDropRule Mussel { get; set; } = new() { Name = "Mussel", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)719", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "719" } };
        public ResourceDropRule Clam { get; set; } = new() { Name = "Clam", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)372", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "372" } };
        public ResourceDropRule Oyster { get; set; } = new() { Name = "Oyster", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)723", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "723" } };
        public ResourceDropRule Seaweed { get; set; } = new() { Name = "Seaweed", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)152", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "152" } };
        public ResourceDropRule CactusFruit { get; set; } = new() { Name = "Cactus Fruit", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)90", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "90" } };


        //Foraged items that are gathered in Spring
        public ResourceDropRule Daffodil { get; set; } = new() { Name = "Daffodil", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)18", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "18" } };
        public ResourceDropRule Dandelion { get; set; } = new() { Name = "Dandelion", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)22", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "22" } };
        public ResourceDropRule Leek { get; set; } = new() { Name = "Leek", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)20", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "20" } };
        public ResourceDropRule WildHorseradish { get; set; } = new() { Name = "Wild Horseradish", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)16", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "16" } };
        public ResourceDropRule SpringOnion { get; set; } = new() { Name = "Spring Onion", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)399", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "399" } };
        public ResourceDropRule Salmonberry { get; set; } = new() { Name = "Salmonberry", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)296", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "296" } };
        public ResourceDropRule Morel { get; set; } = new() { Name = "Morel", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)257", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "257" } };
        public ResourceDropRule CommonMushroom { get; set; } = new() { Name = "Common Mushroom", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)404", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "404" } };

        //Foraged items that are gathered in Summer
        public ResourceDropRule SweetPea { get; set; } = new() { Name = "Sweet Pea", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)402", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "402" } };
        public ResourceDropRule SpiceBerry { get; set; } = new() { Name = "Spice Berry", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)396", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "396" } };
        public ResourceDropRule Grape { get; set; } = new() { Name = "Grape", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)398", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "398" } };
        public ResourceDropRule FiddleheadFern { get; set; } = new() { Name = "Fiddlehead Fern", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)490", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "490" } };
        public ResourceDropRule RedMushroom { get; set; } = new() { Name = "Red Mushroom", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)420", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "420" } };

        //Foraged items that are gathered in Fall
        public ResourceDropRule Blackberry { get; set; } = new() { Name = "Blackberry", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)410", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "410" } };
        public ResourceDropRule Hazelnut { get; set; } = new() { Name = "Hazelnut", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)408", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "408" } };
        public ResourceDropRule WildPlum { get; set; } = new() { Name = "Wild Plum", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)406", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "406" } };
        public ResourceDropRule Chanterelle { get; set; } = new() { Name = "Chanterelle", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)422", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "422" } };
        public ResourceDropRule PurpleMushroom { get; set; } = new() { Name = "Purple Mushroom", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)422", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "422" } };

        //Foraged items that are gathered in Winter
        public ResourceDropRule WinterRoot { get; set; } = new() { Name = "Winter Root", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)412", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "412" } };
        public ResourceDropRule SnowYam { get; set; } = new() { Name = "Snow Yam", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)416", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "416" } };
        public ResourceDropRule Crocus { get; set; } = new() { Name = "Crocus", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)418", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "418" } };
        public ResourceDropRule CrystalFruit { get; set; } = new() { Name = "Crystal Fruit", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)414", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "414" } };
        public ResourceDropRule Holly { get; set; } = new() { Name = "Holly", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)421", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "421" } };

        //Foraged items that are gathered in Ginger Island
        public ResourceDropRule Ginger { get; set; } = new() { Name = "Ginger", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)829", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "829" } };
        public ResourceDropRule MagmaCap { get; set; } = new() { Name = "Magma Cap", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)851", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "851" } };


        //Foraged items that are gathered in Desert
         public ResourceDropRule Coconut { get; set; } = new() { Name = "Coconut", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)88", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "88" } };

        //Cave Foraged items

        //Salmonberry and Blackberry, SpiceBerry, WildPlum already listed
        public ResourceDropRule Cherry { get; set; } = new() { Name = "Cherry", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)638", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "638" } };
        public ResourceDropRule Pomegranate { get; set; } = new() { Name = "Pomegranate", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)637", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "637" } };
        public ResourceDropRule Peach { get; set; } = new() { Name = "Peach", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)636", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "636" } };
        public ResourceDropRule Apple { get; set; } = new() { Name = "Apple", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)613", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "613" } };
        public ResourceDropRule Orange { get; set; } = new() { Name = "Orange", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)635", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "635" } };
  
        public ResourceDropRule Apricot { get; set; } = new() { Name = "Apricot", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)634", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "634" } };
 

    }



    /// <summary>
    /// Mine For More is a Stardew Valley mod that expands the functionality of the Mining and Foraging skills.
    /// It adds configurable profession-based effects and drop enhancements based on a unified resource drop system.
    ///
    /// The mod's logic is structured into modular patch classes located in:
    /// - <c>Patches/Mining/</c>: for all mining-related patches
    /// - <c>Patches/Foraging/</c>: for all foraging-related patches
    /// - <c>Patches/</c>: for general patches that apply to all skills or shared game logic
    ///
    /// The mod reads from a central configuration and uses ResourceDropRules to determine bonus drops,
    /// profession effects, and custom spawn behavior.
    ///
    /// This class serves as the main entry point and registers all skill-specific logic via Harmony patches.
    /// </summary>
    public class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; }
        public Config Config { get; set; }
        public uint realCurrentDay; //Used for MineOnDay1
        public bool AddedAddQuickSmeltRecipes = false;  //Used for Mining-Level 10 Blacksmith Profession
        public bool AddedGemologistRecipes = false;
        public bool SaveGameIsLoaded = false;


        

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Config = helper.ReadConfig<Config>();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            
            //Gives Ability to turn off Harmony Patching to be able to work with other mods
            if (Config.TurnOnMineForMore)
            {
                var harmony = new Harmony(ModManifest.UniqueID);


                if (Config.TurnOnMineForMore)
                {


                    new LevelForMore.Patches.ForagingPatches.PerformTreeFallPatch().Apply(harmony, Monitor);
                    new LevelForMore.Patches.ForagingPatches.TreeGrowthPatch().Apply(harmony, Monitor);
                    new LevelForMore.Patches.ForagingPatches.OnHarvestedForagePatch().Apply(harmony, Monitor);
                    new LevelForMore.Patches.ForagingPatches.GetShakenOffItemPatch().Apply(harmony, Monitor);
                    new LevelForMore.Patches.ForagingPatches.UpdateTapperProductPatch().Apply(harmony, Monitor);
                    new LevelForMore.Patches.ForagingPatches.ResourceClumpDestroyedPatch().Apply(harmony, Monitor);
                    //new MineForMore.Patches.ForagingPatches.CropHarvestPatch().Apply(harmony, Monitor);

                    new LevelForMore.Patches.MiningPatches.UpdateOreGemDropsPatch(Config).Apply(harmony, Monitor);
                    if (Config.TurnOnProfessionLevelUpDescription)
                    {
                        new ProfessionLevelDescriptionPatch(Config).Apply(harmony, Monitor);
                    }

                    if (Config.AllowExtraNodeSpawnsInMine)
                    {
                        new LevelForMore.Patches.MiningPatches.MineShaftOresPatches(Config).Apply(harmony, Monitor);
                    }


                    if (Config.AllowPlayerToExceedLevel10)
                    {
                        new UnlimitedSkillLevel(Config).Apply(harmony, Monitor);
                   
                    }
                }


                helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
                helper.Events.GameLoop.Saving += OnSaving;
                helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;


            }

        }
       

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            SaveGameIsLoaded = true;
            //Handles mine on day 1 logic - entry
            this.realCurrentDay = (uint)Game1.stats.DaysPlayed;
            if (Game1.stats.DaysPlayed <= 5 && Instance.Config.CanMineOnDay1)
            {
                Game1.stats.DaysPlayed = 5; 
            }


            //checks for player's Blacksmith profession and add recipes for quick smelt
            if (Game1.player.professions.Contains(20) && !AddedAddQuickSmeltRecipes)
            {
                Recipe recipe = new Recipe(Instance);
                recipe.AddQuickSmeltRecipes();  
                AddedAddQuickSmeltRecipes = true;

            }

            //Rebuilgs GMCM so that it uses object item from localized langauges
            registerGMCM();



        }

        private void OnSaving(object sender, SavingEventArgs e)
        {
            if (Config.CanMineOnDay1)
                Game1.stats.DaysPlayed = realCurrentDay;
        }

        private void OnReturnedToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            if (Config.CanMineOnDay1)
                Game1.stats.DaysPlayed = realCurrentDay;

            SaveGameIsLoaded = false;
            //Rebuilgs GMCM so that it uses original config names instead of object names since object names are unloaded on title page.
            registerGMCM();
        }


      

        //gets all DropResourceRules dynamically instead of manually entering each
        public IEnumerable<ResourceDropRule> GetAllRules()
        {
            var config = this.Config;
            var props = typeof(Config).GetProperties()
                .Where(pi => pi.PropertyType == typeof(ResourceDropRule));

            foreach (var pi in props)
            {
                if (pi.GetValue(config) is ResourceDropRule rule)
                    yield return rule;
            }
        }





        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            registerGMCM();
        }

 


        // returns the localized item name for an object ID ("390" or "(O)390"); falls back to 'fallback' otherwise
        private static string GetObjectName(string idOrQualifiedId, string fallback = null)
        {
            if (string.IsNullOrWhiteSpace(idOrQualifiedId))
                return fallback ?? "";

            // normalize to qualified object id
            string qid;
            if (idOrQualifiedId.StartsWith("(O)", StringComparison.Ordinal))
                qid = idOrQualifiedId;
            else if (int.TryParse(idOrQualifiedId, out _))
                qid = $"(O){idOrQualifiedId}";
            else
                return fallback ?? idOrQualifiedId; // not an object ID — assume it's already a label

            try
            {
                return StardewValley.Object.GetObjectDisplayName(qid, null, null); // localized
            }
            catch
            {
                return fallback ?? idOrQualifiedId;
            }
        }


        /// <summary>
        /// Registers the Generic Mod Config Menu (GMCM) entries for the Mine For More mod.
        /// The configuration UI is modularized for clarity and ease of maintenance:
        /// 
        /// - <c>MiningConfigPage()</c>: Defines all GMCM options related to the Mining skill
        /// - <c>ForagingConfigPage()</c>: Defines all GMCM options related to the Foraging skill
        /// - <c>OtherSettingsPages()</c>: Defines general mod toggles and compatibility options
        ///
        /// Each section is implemented in its own method, so anyone modifying the Mining or Foraging UI
        /// can update just the corresponding method without needing to touch unrelated logic.
        /// </summary>

        private void registerGMCM()
        {
            IGenericModConfigMenuApi gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu"); if (gmcm is null) return;

            gmcm.Unregister(this.ModManifest);
            gmcm.Register(mod: ModManifest, reset: () => Config = new Config(), save: () => Helper.WriteConfig(Config));
            gmcm.AddSectionTitle(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.section.editSkills.title"));
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.section.editSkills.desc"));
            gmcm.AddPageLink(mod: ModManifest, pageId: "Mining Settings", text: () => Helper.Translation.Get("gmcm.pageLink.mining.text"), tooltip: () => "");
            gmcm.AddPageLink(mod: ModManifest, pageId: "Foraging Settings", text: () => Helper.Translation.Get("gmcm.pageLink.foraging.text"), tooltip: () => "");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => " ");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.section.other.title"));
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.section.other.desc"));
            gmcm.AddPageLink(mod: ModManifest, pageId: "Other Settings", text: () => Helper.Translation.Get("gmcm.pageLink.other.text"), tooltip: () => "");

            MiningConfigPage(gmcm);
            ForagingConfigPage(gmcm);
            OtherSettingsPages(gmcm);
        }


        private void OtherSettingsPages(IGenericModConfigMenuApi gmcm)
        {
            gmcm.AddPage(mod: ModManifest, pageId: "Other Settings", pageTitle: () => Helper.Translation.Get("gmcm.page.other.title"));
            gmcm.AddSectionTitle(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.other.sectionTitle"));
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.other.paragraph"));
            gmcm.AddBoolOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.option.enableMod.name"), tooltip: () => Helper.Translation.Get("gmcm.option.enableMod.tooltip"), getValue: () => Config.TurnOnMineForMore, setValue: v => Config.TurnOnMineForMore = v);
            gmcm.AddBoolOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.option.mineDay1.name"), tooltip: () => Helper.Translation.Get("gmcm.option.mineDay1.tooltip"), getValue: () => Config.CanMineOnDay1, setValue: v => Config.CanMineOnDay1 = v);
            gmcm.AddBoolOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.option.overLevel10.name"), tooltip: () => Helper.Translation.Get("gmcm.option.overLevel10.tooltip"), getValue: () => Config.AllowPlayerToExceedLevel10, setValue: v => Config.AllowPlayerToExceedLevel10 = v);
            gmcm.AddBoolOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.option.levelUpDesc.name"), tooltip: () => Helper.Translation.Get("gmcm.option.levelUpDesc.tooltip"), getValue: () => Config.TurnOnProfessionLevelUpDescription, setValue: v => Config.TurnOnProfessionLevelUpDescription = v);
            gmcm.AddBoolOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.option.extraNodes.name"), tooltip: () => Helper.Translation.Get("gmcm.option.extraNodes.tooltip"), getValue: () => Config.AllowExtraNodeSpawnsInMine, setValue: v => Config.AllowExtraNodeSpawnsInMine = v);
            gmcm.AddBoolOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.option.DebugMode.name"), tooltip: () => Helper.Translation.Get("gmcm.option.DebugMode.tooltip"), getValue: () => Config.DebugMode, setValue: v => Config.DebugMode = v);
        }


        
        private void ForagingConfigPage(IGenericModConfigMenuApi gmcm)
        {
            gmcm.AddPage(mod: ModManifest, pageId: "Foraging Settings", pageTitle: () => Helper.Translation.Get("gmcm.page.foraging.title"));
            gmcm.AddSectionTitle(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.foraging.prof.title"), tooltip: () => Helper.Translation.Get("gmcm.page.foraging.prof.tooltip"));
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.foraging.prof.paragraph"));

            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.ForesterWoodPerLevelBonus, setValue: v => Config.ForesterWoodPerLevelBonus = v, name: () => Helper.Translation.Get("gmcm.foraging.option.woodPerLevel.name"), tooltip: () => Helper.Translation.Get("gmcm.foraging.option.woodPerLevel.tooltip"), min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.ForesterTreeGrowthPerLevelBonus, setValue: v => Config.ForesterTreeGrowthPerLevelBonus = v, name: () => Helper.Translation.Get("gmcm.foraging.option.treeGrowthPerLevel.name"), tooltip: () => Helper.Translation.Get("gmcm.foraging.option.treeGrowthPerLevel.tooltip"), min: 0f, max: 1f, interval: 0.01f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.ForesterSeedPerLevelBonus, setValue: v => Config.ForesterSeedPerLevelBonus = v, name: () => Helper.Translation.Get("gmcm.foraging.option.seedPerLevel.name"), tooltip: () => Helper.Translation.Get("gmcm.foraging.option.seedPerLevel.tooltip"), min: 0f, max: 10f, interval: 0.01f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.LumberjackHardwoodPerLevelBonus, setValue: v => Config.LumberjackHardwoodPerLevelBonus = v, name: () => Helper.Translation.Get("gmcm.foraging.option.hardwoodChancePerLevel.name"), tooltip: () => Helper.Translation.Get("gmcm.foraging.option.hardwoodChancePerLevel.tooltip"), min: 0f, max: 1f, interval: 0.01f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.LumberjackHardwordDropChancePerLevelBonus, setValue: v => Config.LumberjackHardwordDropChancePerLevelBonus = v, name: () => Helper.Translation.Get("gmcm.foraging.option.hardwoodDropChancePerLevel.name"), tooltip: () => Helper.Translation.Get("gmcm.foraging.option.hardwoodDropChancePerLevel.tooltip"), min: 0f, max: 1f, interval: 0.001f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.foraging.option.gathererExtraPerLevel.name"), tooltip: () => Helper.Translation.Get("gmcm.foraging.option.gathererExtraPerLevel.tooltip"), getValue: () => (float)Config.GathererExtraDropPerLevel, setValue: v => Config.GathererExtraDropPerLevel = (int)v, min: 0f, max: 10f, interval: 0.01f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.foraging.option.tapperSpeedPerLevel.name"), tooltip: () => Helper.Translation.Get("gmcm.foraging.option.tapperSpeedPerLevel.tooltip"), getValue: () => Config.TapperSpeedBonusPercentPerLevel, setValue: v => Config.TapperSpeedBonusPercentPerLevel = MathF.Max(0f, v), min: 0f, max: 10f, interval: 0.05f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.TapperExtraQuantityPerLevel, v => Config.TapperExtraQuantityPerLevel = Math.Max(0f, v), name: () => Helper.Translation.Get("gmcm.foraging.option.tapperExtraQuantityPerLevel.name"), () => Helper.Translation.Get("gmcm.foraging.option.tapperExtraQuantityPerLevel.tooltip"), 0f, 20f, 0.25f);



            gmcm.AddSectionTitle(ModManifest, text: () => Helper.Translation.Get("gmcm.page.foraging.chop.sectionTitle"), tooltip: () => Helper.Translation.Get("gmcm.page.foraging.chop.tooltip")); // Foraging - Configure Chopped Tree/stomps Drops
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.foraging.chop.paragraph"));

            foreach (var drop in GetAllRules())
                if (drop.SkillType.ToString().Equals("Foraging"))
                    if (!drop.Type.ToString().Equals("Forage"))
                    {
                        string dropName = drop.Name;
                        if (SaveGameIsLoaded) //Reason for using original name until game load is because game localization for langues on object doesn't occur until game loads
                        {
                            dropName = GetObjectName(drop.ObjectID, drop.Name);
                        }

                        AddDropToGMCM(gmcm, dropName, () => drop, drop.SkillType);
                    }


            gmcm.AddSectionTitle(ModManifest, text: () => Helper.Translation.Get("gmcm.page.foraging.foraged.sectionTitle"), tooltip: () => Helper.Translation.Get("gmcm.page.foraging.foraged.tooltip")); // Foraging - Configure Foraged Drops
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.foraging.foraged.paragraph"));

            foreach (var drop in GetAllRules())
                if (drop.SkillType.ToString().Equals("Foraging"))
                    if (drop.Type.ToString().Equals("Forage"))
                    {
                        string dropName = drop.Name;
                        if (SaveGameIsLoaded) //Reason for using original name until game load is because game localization for langues on object doesn't occur until game loads
                        {
                            dropName = GetObjectName(drop.ObjectID, drop.Name);
                        }

                        AddDropToGMCM(gmcm, dropName, () => drop, drop.SkillType);
                    }
                        
        }


        private void MiningConfigPage(IGenericModConfigMenuApi gmcm)
        {
            gmcm.AddPage(mod: ModManifest, pageId: "Mining Settings", pageTitle: () => Helper.Translation.Get("gmcm.page.mining.title"));
            gmcm.AddSectionTitle(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.mining.prof.title"), tooltip: () => Helper.Translation.Get("gmcm.page.mining.prof.tooltip"));
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.mining.prof.paragraph"));

            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.mining.option.minerOre.name"), tooltip: () => Helper.Translation.Get("gmcm.mining.option.minerOre.tooltip"), getValue: () => Config.MinerProfessionBonusOrePerLevel, setValue: v => Config.MinerProfessionBonusOrePerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.mining.option.geologistGem.name"), tooltip: () => Helper.Translation.Get("gmcm.mining.option.geologistGem.tooltip"), getValue: () => Config.GeologistProfessionBonusGemsPerLevel, setValue: v => Config.GeologistProfessionBonusGemsPerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.mining.option.gemologistNode.name"), tooltip: () => Helper.Translation.Get("gmcm.mining.option.gemologistNode.tooltip"), getValue: () => Config.GemNodeSpawnChanceBonusWithProfession, setValue: v => Config.GemNodeSpawnChanceBonusWithProfession = v, min: 0f, max: 100f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.mining.option.prospectorCoal.name"), tooltip: () => Helper.Translation.Get("gmcm.mining.option.prospectorCoal.tooltip"), getValue: () => Config.ProspectorProfessionBonusCoalPerLevel, setValue: v => Config.ProspectorProfessionBonusCoalPerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.mining.option.prospectorCoalNode.name"), tooltip: () => Helper.Translation.Get("gmcm.mining.option.prospectorCoalNode.tooltip"), getValue: () => Config.CoalNodeSpawnChanceBonusWithProfession, setValue: v => Config.CoalNodeSpawnChanceBonusWithProfession = v, min: 0f, max: 100f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.mining.option.excavatorGeode.name"), tooltip: () => Helper.Translation.Get("gmcm.mining.option.excavatorGeode.tooltip"), getValue: () => Config.ExcavatorProfessionBonusGeodesPerLevel, setValue: v => Config.ExcavatorProfessionBonusGeodesPerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.mining.option.excavatorGeodeNode.name"), tooltip: () => Helper.Translation.Get("gmcm.mining.option.excavatorGeodeNode.tooltip"), getValue: () => Config.GeodeNodeSpawnChanceBonusWithProfession, setValue: v => Config.GeodeNodeSpawnChanceBonusWithProfession = v, min: 0f, max: 100f, interval: 0.1f);

            gmcm.AddSectionTitle(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.mining.drops.title"), tooltip: () => Helper.Translation.Get("gmcm.page.mining.drops.tooltip")); // Mining - Configure Drops
            gmcm.AddParagraph(mod: ModManifest, text: () => Helper.Translation.Get("gmcm.page.mining.drops.paragraph"));

            foreach (var drop in GetAllRules())
                if (drop.SkillType.ToString().Equals("Mining"))
                {
                    string dropName = drop.Name;
                    if (SaveGameIsLoaded) //Reason for using original name until game load is because game localization for langues on object doesn't occur until game loads
                    {
                        dropName = GetObjectName(drop.ObjectID, drop.Name);
                    }

                        AddDropToGMCM(gmcm, dropName, () => drop, drop.SkillType);
                }
                    

            
        }


        private void AddDropToGMCM(IGenericModConfigMenuApi gmcm, string label, Func<ResourceDropRule> getDrop, string skilltype)
        {
            var drop = getDrop();
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.drop.addAmount.name", new { label }), tooltip: () => Helper.Translation.Get("gmcm.drop.addAmount.tooltip", new { label }), getValue: () => drop.AddAmount, setValue: v => drop.AddAmount = v, min: 0, max: 50);
            gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.drop.multiplier.name", new { label }), tooltip: () => Helper.Translation.Get("gmcm.drop.multiplier.tooltip", new { label }), getValue: () => (float)drop.Multiplier, setValue: v => drop.Multiplier = v, min: 0f, max: 10f, interval: 0.1f);
            if (skilltype.ToString().Equals("Mining")) gmcm.AddNumberOption(mod: ModManifest, name: () => Helper.Translation.Get("gmcm.drop.extraNodeChance.name", new { label }), tooltip: () => Helper.Translation.Get("gmcm.drop.extraNodeChance.tooltip", new { label }), getValue: () => drop.ExtraNodeSpawnChancePercent, setValue: v => drop.ExtraNodeSpawnChancePercent = v, min: 0f, max: 100f, interval: 0.1f);
        }



    }
}
