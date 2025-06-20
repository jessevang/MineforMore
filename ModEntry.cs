using GenericModConfigMenu;
using HarmonyLib;
using MineForMore.Classes;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;


namespace MineForMore
{
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

    public class Config
    {
        public bool TurnOnMineForMore { get; set; } = true;
        public bool CanMineOnDay1 { get; set; } = true;
        public bool AllowPlayerToExceedLevel10 { get; set; } = true;
        public bool TurnOnProfessionLevelUpDescription { get; set; } = true;
        public bool AllowExtraNodeSpawnsInMine { get; set; } = true;


        public bool listStoneDestroyedInConsole { get; set; } = false;


        //Mining
        public float MinerProfessionBonusOrePerLevel { get; set; } = 1.0f;
        public float GeologistProfessionBonusGemsPerLevel { get; set; } = 1.0f;
        public float ProspectorProfessionBonusCoalPerLevel { get; set; } = 1.0f;
        public float ExcavatorProfessionBonusGeodesPerLevel { get; set; } = 1.0f;

        //Foraging
        public float ForesterWoodPerLevelBonus = 3.0f;
        public float ForesterSeedPerLevelBonus = 1.0f;
        public float ForesterTreeGrowthPerLevelBonus = 0.10f;

        public int GathererExtraDropPerLevel = 1;

        public float LumberjackHardwoodPerLevelBonus = 0.5f;
        public float LumberjackHardwordDropChancePerLevelBonus = 0.025f;





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


        // Additional node spawn chance if player has specific professions
        public float GemNodeSpawnChanceBonusWithProfession { get; set; } = 1f;
        public float GeodeNodeSpawnChanceBonusWithProfession { get; set; } = 1f;
        public float CoalNodeSpawnChanceBonusWithProfession { get; set; } = 1f;


        //Foraged items that are gathered in Any Season
        public ResourceDropRule Sap { get; set; } = new() { Name = "Sap", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)92", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "92" } };
        public ResourceDropRule SeaUrchin { get; set; } = new() { Name = "Sea Urchin", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)132", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "132" } };
        public ResourceDropRule Coral { get; set; } = new() { Name = "Coral", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)129", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "129" } };
        public ResourceDropRule Mussel { get; set; } = new() { Name = "Mussel", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)146", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "146" } };
        public ResourceDropRule Clam { get; set; } = new() { Name = "Clam", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)145", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "145" } };
        public ResourceDropRule Oyster { get; set; } = new() { Name = "Oyster", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)309", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "309" } };
        public ResourceDropRule Seaweed { get; set; } = new() { Name = "Seaweed", SkillType = "Foraging", Type = "Forage", ObjectID = "(O)142", AddAmount = 2, Multiplier = 1f, ExtraNodeSpawnChancePercent = 0f, DropsFromObjectIDs = new() { "142" } };
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

    }


    public class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; }
        public Config Config { get; set; }
        public uint realCurrentDay; //Used for MineOnDay1
        public bool AddedAddQuickSmeltRecipes = false;  //Used for Mining-Level 10 Blacksmith Profession
        public bool AddedGemologistRecipes = false;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Config = helper.ReadConfig<Config>();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;


            //Gives Ability to turn off Harmony Patching to be able to work with other mods
            if (Config.TurnOnMineForMore)
            {
                var harmony = new Harmony(ModManifest.UniqueID);

                new MineForMore.Patches.ForagingPatches.PerformTreeFallPatch().Apply(harmony, Monitor);
                new MineForMore.Patches.ForagingPatches.TreeGrowthPatch().Apply(harmony, Monitor);
                new MineForMore.Patches.ForagingPatches.OnHarvestedForagePatch().Apply(harmony, Monitor);


                new MineForMore.Patches.ForagingPatches.ResourceClumpDestroyedPatch().Apply(harmony, Monitor);



                new MineforMore.Patches.MiningPatches.UpdateOreGemDropsPatch(Config).Apply(harmony, Monitor);
                if (Config.TurnOnProfessionLevelUpDescription)
                {
                    new ProfessionLevelDescriptionPatch(Config).Apply(harmony, Monitor);
                }
                
                if (Config.AllowExtraNodeSpawnsInMine)
                {
                    new MineforMore.Patches.MiningPatches.MineShaftOresPatches(Config).Apply(harmony, Monitor);
                }


                if (Config.AllowPlayerToExceedLevel10)
                {
                    new UnlimitedSkillLevel(Config).Apply(harmony, Monitor);
                }

                
                helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
                helper.Events.GameLoop.Saving += OnSaving;
                helper.Events.GameLoop.ReturnedToTitle += OnReturnedToTitle;
            }

        }


        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {

            this.realCurrentDay = (uint)Game1.stats.DaysPlayed;
            if (Game1.stats.DaysPlayed <= 5 && Instance.Config.CanMineOnDay1)
            {
                Game1.stats.DaysPlayed = 5; 
            }
            
            

            if (Game1.player.professions.Contains(20) && !AddedAddQuickSmeltRecipes)
            {
                Recipe recipe = new Recipe(Instance);
                recipe.AddQuickSmeltRecipes();  
                AddedAddQuickSmeltRecipes = true;

            }



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

        private void registerGMCM()
        {
            IGenericModConfigMenuApi gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm is null) return;

            gmcm.Register(mod: ModManifest, reset: () => Config = new Config(), save: () => Helper.WriteConfig(Config));
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "Edit Skill Settings");
            gmcm.AddParagraph(mod: ModManifest, text: () => "Click on one of the Skill pages below to modify the drops without professions and the drop bonuses from a professions based on the skill level.");
            gmcm.AddPageLink(mod: ModManifest, pageId: "Mining Settings", text: () => "     1. Mining Setting Page", tooltip: () => "");
            gmcm.AddPageLink(mod: ModManifest, pageId: "Foraging Settings", text: () => "     2. Foraging Setting Page", tooltip: () => "");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => " ");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "Other Settings");
            gmcm.AddParagraph(mod: ModManifest, text: () => "Click on one of the other pages below to turn off entire function or some functions in this mod so that it can continue to work with other mods");
            gmcm.AddPageLink(mod: ModManifest, pageId: "Other Settings", text: () => "     3. Other Settings Page", tooltip: () => "");

            MiningConfigPage(gmcm);
            ForagingConfigPage(gmcm);
            OtherSettingsPages(gmcm);
        }


        private void OtherSettingsPages(IGenericModConfigMenuApi gmcm)
        {
            gmcm.AddPage(mod: ModManifest, pageId: "Other Settings", pageTitle: () => "Other Settings");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "Other Settings");
            gmcm.AddParagraph(mod: ModManifest, text: () => "If your having issues with this mod working with other mods, feel free to adjust the general setting features below to turn off certain functions in this mod so that you may use other mods that may share the same code they are being overwritten (Harmony Patch).");
            gmcm.AddBoolOption(mod: ModManifest, name: () => "Mine For More Enabled", tooltip: () => "Disables all functions from this mod except the config menu, allowing you to re-enable it later. Requires restarting the game.", getValue: () => Config.TurnOnMineForMore, setValue: value => Config.TurnOnMineForMore = value);
            gmcm.AddBoolOption(mod: ModManifest, name: () => "User Can Mine on Day 1", tooltip: () => "Allows mine access on Day 1. Turning this off will prevent mine access before Day 5.", getValue: () => Config.CanMineOnDay1, setValue: value => Config.CanMineOnDay1 = value);
            gmcm.AddBoolOption(mod: ModManifest, name: () => "Skill Levels Can Exceed 10", tooltip: () => "Allows skills to exceed level 10. Requires restarting the game. Disabling this will remove the Harmony patch on Farmer.ExperienceGain. This setting doesn't affect current EXP, it prevents further EXP gain beyond level 10.", getValue: () => Config.AllowPlayerToExceedLevel10, setValue: value => Config.AllowPlayerToExceedLevel10 = value);
            gmcm.AddBoolOption(mod: ModManifest, name: () => "Allow Updated Level Up Descriptions", tooltip: () => "Enables a Harmony patch to show more accurate profession descriptions during level-up. Turning this off will revert to vanilla descriptions. Requires game restart to apply", getValue: () => Config.TurnOnProfessionLevelUpDescription, setValue: value => Config.TurnOnProfessionLevelUpDescription = value);
            gmcm.AddBoolOption(mod: ModManifest, name: () => "Allow Extra Node Spawns", tooltip: () => "Enables extra node spawns in the mines, based on both base chance and profession bonuses. Requires a game restart to take effect.", getValue: () => Config.AllowExtraNodeSpawnsInMine, setValue: value => Config.AllowExtraNodeSpawnsInMine = value);
            gmcm.AddBoolOption(mod: ModManifest, name: () => "Log Stone Destruction", tooltip: () => "Print stone destruction to console including information on bonus ore, multiplier, bonus from profession, and expected total from bonus.", getValue: () => Config.listStoneDestroyedInConsole, setValue: value => Config.listStoneDestroyedInConsole = value);
        }


        private void ForagingConfigPage(IGenericModConfigMenuApi gmcm)
        {
            gmcm.AddPage(mod: ModManifest, pageId: "Foraging Settings", pageTitle: () => "Foraging Settings");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "Foraging Profession Bonuses", tooltip: () => "These settings apply Foraging drop bonuses and multipliers based on your professions.\nFormula: Final Drop = (Base Drop + Bonus from Profession) × Multiplier");
            gmcm.AddParagraph(mod: ModManifest, text: () => "Update the bonus drops obtained from a profession. Adjust the drop per skill level below to adjust how much bonus drops you get if you have learned the listed profession.");
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.ForesterWoodPerLevelBonus, setValue: value => Config.ForesterWoodPerLevelBonus = value, name: () => "Extra Wood Per Foraging Level", tooltip: () => "How much extra wood to grant per Foraging level if the player has the Forester profession.", min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.ForesterTreeGrowthPerLevelBonus, setValue: value => Config.ForesterTreeGrowthPerLevelBonus = value, name: () => "Tree Growth Rate Bonus Per Level", tooltip: () => "How much faster trees grow per Foraging level with the Forester profession.", min: 0f, max: 1f, interval: 0.01f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.ForesterSeedPerLevelBonus, setValue: value => Config.ForesterSeedPerLevelBonus = value, name: () => "Tree Seed Bonus Per Level", tooltip: () => "How much Seed drops per Foraging level with the Forester profession.", min: 0f, max: 10f, interval: 0.01f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.LumberjackHardwoodPerLevelBonus, setValue: value => Config.LumberjackHardwoodPerLevelBonus = value, name: () => "Lumberjack Hardwood Chance Per Level", tooltip: () => "Chance to drop hardwood from any tree per Foraging level if you have the Lumberjack profession. Value is a percentage (e.g., 0.5 = 50%).", min: 0f, max: 1f, interval: 0.01f);
            gmcm.AddNumberOption(mod: ModManifest, getValue: () => Config.LumberjackHardwordDropChancePerLevelBonus, setValue: value => Config.LumberjackHardwordDropChancePerLevelBonus = value, name: () => "Lumberjack Drop Chance Per Level", tooltip: () => "Chance per Foraging level to trigger a hardwood drop when cutting down any tree (e.g., 0.025 = 2.5% per level).", min: 0f, max: 1f, interval: 0.001f);
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "");
            gmcm.AddSectionTitle(ModManifest, text: () => "Foraging - Configure Drops", tooltip: () => "Set Foraging drop bonuses");
            gmcm.AddParagraph(mod: ModManifest, text: () => "Update the drops obtained without requiring a profession. These adjust works right away and is added with the profession bonuses before the added values are multiplied with the multiplier below.");

            foreach (var drop in GetAllRules())
            {
                if (drop.SkillType.ToString().Equals("Foraging"))
                    AddDropToGMCM(gmcm, drop.Name, () => drop, drop.SkillType);
            }
        }


        private void MiningConfigPage(IGenericModConfigMenuApi gmcm)
        {
            gmcm.AddPage(mod: ModManifest, pageId: "Mining Settings", pageTitle: () => "Mining Settings");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "Mining Profession Bonuses", tooltip: () => "These settings apply mining drop bonuses and multipliers based on your professions.\nFormula: Final Drop = (Base Drop + Bonus from Profession) × Multiplier");
            gmcm.AddParagraph(mod: ModManifest, text: () => "Update the bonus drops obtained from a profession. Adjust the drop per skill level below to adjust how much bonus drops you get if you have learned the listed profession.");
            gmcm.AddNumberOption(mod: ModManifest, name: () => "Miner Bonus (Ore)", tooltip: () => "Bonus ore amount per each mining level if you have the Miner profession.", getValue: () => Config.MinerProfessionBonusOrePerLevel, setValue: v => Config.MinerProfessionBonusOrePerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => "Geologist Bonus (Gem)", tooltip: () => "Bonus number of gems dropped per mining level when you have the Geologist profession.", getValue: () => Config.GeologistProfessionBonusGemsPerLevel, setValue: v => Config.GeologistProfessionBonusGemsPerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => "Gemologist Bonus (Gem Node)", tooltip: () => "Bonus gem node spawn chance (%) if you have the Gemologist profession.", getValue: () => Config.GemNodeSpawnChanceBonusWithProfession, setValue: v => Config.GemNodeSpawnChanceBonusWithProfession = v, min: 0f, max: 100f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => "Prospector Bonus (Coal)", tooltip: () => "Bonus coal per each mining level if you have the Prospector profession.", getValue: () => Config.ProspectorProfessionBonusCoalPerLevel, setValue: v => Config.ProspectorProfessionBonusCoalPerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => "Prospector Bonus (Coal Node)", tooltip: () => "Bonus coal node spawn chance (%) if you have the Prospector profession.", getValue: () => Config.CoalNodeSpawnChanceBonusWithProfession, setValue: v => Config.CoalNodeSpawnChanceBonusWithProfession = v, min: 0f, max: 100f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => "Excavator Bonus (Geode)", tooltip: () => "Bonus geodes per each mining level if you have the Excavator profession.", getValue: () => Config.ExcavatorProfessionBonusGeodesPerLevel, setValue: v => Config.ExcavatorProfessionBonusGeodesPerLevel = v, min: 0f, max: 10f, interval: 0.1f);
            gmcm.AddNumberOption(mod: ModManifest, name: () => "Excavator Bonus (Geode Node)", tooltip: () => "Bonus geode node spawn chance (%) if you have the Excavator profession.", getValue: () => Config.GeodeNodeSpawnChanceBonusWithProfession, setValue: v => Config.GeodeNodeSpawnChanceBonusWithProfession = v, min: 0f, max: 100f, interval: 0.1f);
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "");
            gmcm.AddSectionTitle(mod: ModManifest, text: () => "Mining - Configure Drops", tooltip: () => "These settings applies mining drop and multiplier that works with the extra drops from profession. Formula is Ore Drop = (DropCountFromBelow + BonusFromProfession) * MultipierFromBelow");
            gmcm.AddParagraph(mod: ModManifest, text: () => "Update the drops obtained from without requiring a profession. These adjust works right away without profession, and is added with the profession bonuses.");

            foreach (var drop in GetAllRules())
            {
                if (drop.SkillType.ToString().Equals("Mining"))
                    AddDropToGMCM(gmcm, drop.Name, () => drop, drop.SkillType);
            }
        }


        private void AddDropToGMCM(IGenericModConfigMenuApi gmcm, string label, Func<ResourceDropRule> getDrop, string skilltype)
        {
            var drop = getDrop();

            gmcm.AddNumberOption(mod: ModManifest, name: () => $"{label} - AddAmount", tooltip: () => $"Extra {label} drop count added.", getValue: () => drop.AddAmount, setValue: v => drop.AddAmount = v, min: 0, max: 50);
            gmcm.AddNumberOption(mod: ModManifest, name: () => $"{label} - Multiplier", tooltip: () => $"Multiplier to apply to {label}'s total drop count.", getValue: () => (float)drop.Multiplier, setValue: v => drop.Multiplier = v, min: 0f, max: 10f, interval: 0.1f);

            if (drop.SkillType.ToString().Equals("Mining"))
                gmcm.AddNumberOption(mod: ModManifest, name: () => $"{label} - Extra Node Chance (%)", tooltip: () => $"Chance that an extra {label} node appears in the mine (no profession required).", getValue: () => drop.ExtraNodeSpawnChancePercent, setValue: v => drop.ExtraNodeSpawnChancePercent = v, min: 0f, max: 100f, interval: 0.1f);
        }



    }
}
