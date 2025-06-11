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

        public bool listStoneDestroyedInConsole { get; set; } = true;

        public float MinerProfessionBonusOrePerLevel { get; set; } = 1.0f;
        public float GeologistProfessionBonusGemsPerLevel { get; set; } = 1.0f;
        public float ProspectorProfessionBonusCoalPerLevel { get; set; } = 1.0f;
        public float ExcavatorProfessionBonusGeodesPerLevel { get; set; } = 1.0f;

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
        public ResourceDropRule Diamond { get; set; } = new() { Name = "Diamond", SkillType = "Mining", Type = "Gem", ObjectID = "(O)72", DropsFromObjectIDs = new() { "2", "44" } };
        public ResourceDropRule Amethyst { get; set; } = new() { Name = "Amethyst", SkillType = "Mining", Type = "Gem", ObjectID = "(O)66", DropsFromObjectIDs = new() { "8", "44" } };
        public ResourceDropRule Aquamarine { get; set; } = new() { Name = "Aquamarine", SkillType = "Mining", Type = "Gem", ObjectID = "(O)62", DropsFromObjectIDs = new() { "14", "44" } };
        public ResourceDropRule Emerald { get; set; } = new() { Name = "Emerald", SkillType = "Mining", Type = "Gem", ObjectID = "(O)60", DropsFromObjectIDs = new() { "12", "44" } };
        public ResourceDropRule Topaz { get; set; } = new() { Name = "Topaz", SkillType = "Mining", Type = "Gem", ObjectID = "(O)68", DropsFromObjectIDs = new() { "10", "44" } };
        public ResourceDropRule Ruby { get; set; } = new() { Name = "Ruby", SkillType = "Mining", Type = "Gem", ObjectID = "(O)64", DropsFromObjectIDs = new() { "4", "44" } };
        public ResourceDropRule Jade { get; set; } = new() { Name = "Jade", SkillType = "Mining", Type = "Gem", ObjectID = "(O)70", DropsFromObjectIDs = new() { "6", "44" } };

        // Geodes
        public ResourceDropRule Geode { get; set; } = new() { Name = "Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)535", DropsFromObjectIDs = new() { "75" } };
        public ResourceDropRule FrozenGeode { get; set; } = new() { Name = "Frozen Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)536", DropsFromObjectIDs = new() { "76" } };
        public ResourceDropRule MagmaGeode { get; set; } = new() { Name = "Magma Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)537", DropsFromObjectIDs = new() { "77" } };
        public ResourceDropRule OmniGeode { get; set; } = new() { Name = "Omni Geode", SkillType = "Mining", Type = "Geode", ObjectID = "(O)749", DropsFromObjectIDs = new() { "819" } };


        // Additional node spawn chance if player has specific professions
        public float GemNodeSpawnChanceBonusWithProfession { get; set; } = 1f;
        public float GeodeNodeSpawnChanceBonusWithProfession { get; set; } = 1f;
        public float CoalNodeSpawnChanceBonusWithProfession { get; set; } = 1f;

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

                //Applies ore count bonus based on your mining level, profession, and general flat rate setting.
                new UpdateOreGemDropsPatch(Config).Apply(harmony, Monitor);

                //Applies additional description to level description
                new MineProfessionLevelDescriptionPatch(Config).Apply(harmony, Monitor);
                
                //
                new MineShaftOresPatches(Config).Apply(harmony, Monitor);

                
                //Applies patching Farmer.ExperienceGain to continuely gain exp even after level 10, also sets level 11-infinite level
                if (Config.AllowPlayerToExceedLevel10)
                {
                    new UnlimitedMiningLevel(Config).Apply(harmony, Monitor);
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


        public IEnumerable<ResourceDropRule> GetAllDrops()
        {
            return new List<ResourceDropRule>
            {
                Config.Stone,
                Config.CopperOre,
                Config.IronOre,
                Config.GoldOre,
                Config.IridiumOre,
                Config.RadioactiveOre,

                Config.Coal,
                Config.Clay,
                Config.CinderShard,

                Config.Diamond,
                Config.Amethyst,
                Config.Aquamarine,
                Config.Emerald,
                Config.Topaz,
                Config.Ruby,
                Config.Jade,

                Config.Geode,
                Config.FrozenGeode,
                Config.MagmaGeode,
                Config.OmniGeode
            };
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            registerGMCM();
        }

        private void registerGMCM()
        {
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm is null) return;

            gmcm.Register(
                mod: ModManifest,
                reset: () => Config = new Config(),
                save: () => Helper.WriteConfig(Config)
            );

            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "General Settings"
            );

                        gmcm.AddBoolOption(
                            mod: ModManifest,
                            name: () => "Mine For More Enabled",
                            tooltip: () => "Disabling this will disable all functions from this mod except for the generic mod config menus to allow user to re-enable this mod. Use this if you want to disable this mod without deleting it",
                            getValue: () => Config.TurnOnMineForMore,
                            setValue: value => Config.TurnOnMineForMore = value
                        );



                        gmcm.AddBoolOption(
                            mod: ModManifest,
                            name: () => "User Can Mine on Day 1",
                            tooltip: () => "Allow mine access on day 1. Turning this off will prevent users from going to the mines prior to day 5.",
                            getValue: () => Config.CanMineOnDay1,
                            setValue: value => Config.CanMineOnDay1 = value
                        );

                        gmcm.AddBoolOption(
                            mod: ModManifest,
                            name: () => "Allow Skill Level to exceed level 10",
                            tooltip: () => "Allows all skill levels to exceed 10, saved changes on this option requires restarting the game. Turning this off will disable the harmony patch on Farmer.ExperienceGain. Since this is a harmony patch will require game reset to take effect. You may want to turn this off because other mods may patch this same code. This option won't affect your current Experience amount, it only no longer patches the gain experience which means your skills can't gain exp after you get level 10 or (15,0000 EXP)",
                            getValue: () => Config.AllowPlayerToExceedLevel10,
                            setValue: value => Config.AllowPlayerToExceedLevel10 = value
                        );



            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "Profession Bonuses",
                tooltip: () => "These settings applies mining drop and multiplier that works with the extra drops from profession. " +
                "Formula is Ore Drop = (DropCountFromBelow + BonusFromProfession) * MultipierFromBelow"
            );


                        gmcm.AddNumberOption(
                            mod: ModManifest,
                            name: () => "Miner Bonus (Ore)",
                            tooltip: () => "Bonus ore amount per each mining level if you have the Miner profession.",
                            getValue: () => Config.MinerProfessionBonusOrePerLevel,
                            setValue: v => Config.MinerProfessionBonusOrePerLevel = v,
                            min: 0f,
                            max: 10f,
                            interval: 0.1f
                        );

                        gmcm.AddNumberOption(
                            mod: ModManifest,
                            name: () => "Geologist Bonus (Gem)",
                            tooltip: () => "Bonus gems amount that drops per each mining level if you have the Geologist profession.",
                            getValue: () => Config.GeologistProfessionBonusGemsPerLevel,
                            setValue: v => Config.GeologistProfessionBonusGemsPerLevel = v,
                            min: 0f,
                            max: 10f,
                            interval: 0.1f
                        );

                        gmcm.AddNumberOption(
                            mod: ModManifest,
                            name: () => "Gemologist Bonus (Gem Node)",
                            tooltip: () => "Bonus gem node spawn chance (%) if you have the Gemologist profession.",
                            getValue: () => Config.GemNodeSpawnChanceBonusWithProfession,
                            setValue: v => Config.GemNodeSpawnChanceBonusWithProfession = v,
                            min: 0f,
                            max: 100f,
                            interval: 0.1f
                        );


                        gmcm.AddNumberOption(
                            mod: ModManifest,
                            name: () => "Prospector Bonus (Coal)",
                            tooltip: () => "Bonus coal per each mining level if you have the Prospector profession.",
                            getValue: () => Config.ProspectorProfessionBonusCoalPerLevel,
                            setValue: v => Config.ProspectorProfessionBonusCoalPerLevel = v,
                            min: 0f,
                            max: 10f,
                            interval: 0.1f
                        );

                        gmcm.AddNumberOption(
                            mod: ModManifest,
                            name: () => "Prospector Bonus (Coal Node)",
                            tooltip: () => "Bonus coal node spawn chance (%) if you have the Prospector profession.",
                            getValue: () => Config.CoalNodeSpawnChanceBonusWithProfession,
                            setValue: v => Config.CoalNodeSpawnChanceBonusWithProfession = v,
                            min: 0f,
                            max: 100f,
                            interval: 0.1f
                        );

                        gmcm.AddNumberOption(
                            mod: ModManifest,
                            name: () => "Excavator Bonus (Geode)",
                            tooltip: () => "Bonus geodes per each mining level if you have the Excavator profession.",
                            getValue: () => Config.ExcavatorProfessionBonusGeodesPerLevel,
                            setValue: v => Config.ExcavatorProfessionBonusGeodesPerLevel = v,
                            min: 0f,
                            max: 10f,
                            interval: 0.1f
                        );

                        gmcm.AddNumberOption(
                            mod: ModManifest,
                            name: () => "Excavator Bonus (Geode Node)",
                            tooltip: () => "Bonus geode node spawn chance (%) if you have the Excavator profession.",
                            getValue: () => Config.GeodeNodeSpawnChanceBonusWithProfession,
                            setValue: v => Config.GeodeNodeSpawnChanceBonusWithProfession = v,
                            min: 0f,
                            max: 100f,
                            interval: 0.1f
                        );

            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "Mining - Configure Drops",
                tooltip: () => "These settings applies mining drop and multiplier that works with the extra drops from profession. " +
                "Formula is Ore Drop = (DropCountFromBelow + BonusFromProfession) * MultipierFromBelow"
            );


                        foreach (var drop in GetAllDrops())
                        {
                            AddDropToGMCM(gmcm, drop.Name, () => drop);
                        }


            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "Debug Options",
                tooltip: () => "Set this setting for additional logging to assist with troubleshooting"
            );

                        gmcm.AddBoolOption(
                            mod: ModManifest,
                            name: () => "Log Stone Destruction",
                            tooltip: () => "Print stone destruction to console including information on bonus ore, multiplier, bonus from profession, and expected total from bonus.",
                            getValue: () => Config.listStoneDestroyedInConsole,
                            setValue: value => Config.listStoneDestroyedInConsole = value
                        );

        }

        private void AddDropToGMCM(IGenericModConfigMenuApi gmcm, string label, Func<ResourceDropRule> getDrop)
        {
            var drop = getDrop();

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => $"{label} - AddAmount",
                tooltip: () => $"Extra {label} drop count added.",
                getValue: () => drop.AddAmount,
                setValue: v => drop.AddAmount = v,
                min: 0,
                max: 50
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => $"{label} - Multiplier",
                tooltip: () => $"Multiplier to apply to {label}'s total drop count.",
                getValue: () => (float)drop.Multiplier,
                setValue: v => drop.Multiplier = v,
                min: 0f,
                max: 10f,
                interval: 0.1f
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => $"{label} - Extra Node Chance (%)",
                tooltip: () => $"Chance that an extra {label} node appears in the mine (no profession required).",
                getValue: () => drop.ExtraNodeSpawnChancePercent,
                setValue: v => drop.ExtraNodeSpawnChancePercent = v,
                min: 0f,
                max: 100f,
                interval: 0.1f
            );
        }


    }
}
