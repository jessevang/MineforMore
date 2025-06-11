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
        public string Type { get; set; } = "";
        public string ObjectID { get; set; } = "";
        public int AddAmount { get; set; } = 3;
        public double Multiplier { get; set; } = 1.0;
        public List<string> DropsFromObjectIDs { get; set; } = new();
    }

    public class Config
    {
        public bool CanMineOnDay1 { get; set; } = true;
        public bool TurnOnMineForMore { get; set; } = true;
        public bool AllowPlayerToExceedLevel10 { get; set; } = true;
        public bool listStoneDestroyedInConsole { get; set; } = true;

        public float MinerProfessionBonusOrePerLevel { get; set; } = 1.0f;
        public float GeologistProfessionBonusGemsPerLevel { get; set; } = 1.0f;
        public float ProspectorProfessionBonusCoalPerLevel { get; set; } = 1.0f;
        public float ExcavatorProfessionBonusGeodesPerLevel { get; set; } = 1.0f;

        //Ores
        public ResourceDropRule Stone { get; set; } = new() { Name = "Stone", Type = "Ore", ObjectID = "(O)390", DropsFromObjectIDs = new() { "32", "34", "36", "38", "40", "48", "50", "52", "54", "56", "58", "343", "450", "668", "670", "760", "762", "845", "846", "847" } };
        public ResourceDropRule CopperOre { get; set; } = new() { Name = "Copper Ore", Type = "Ore", ObjectID = "(O)378", DropsFromObjectIDs = new() { "751" } };
        public ResourceDropRule IronOre { get; set; } = new() { Name = "Iron Ore", Type = "Ore", ObjectID = "(O)380", DropsFromObjectIDs = new() { "290" } };
        public ResourceDropRule GoldOre { get; set; } = new() { Name = "Gold Ore", Type = "Ore", ObjectID = "(O)384", DropsFromObjectIDs = new() { "764" } };
        public ResourceDropRule IridiumOre { get; set; } = new() { Name = "Iridium Ore", Type = "Ore", ObjectID = "(O)386", DropsFromObjectIDs = new() { "765" } };
        public ResourceDropRule RadioactiveOre { get; set; } = new() { Name = "Radioactive Ore", Type = "Ore", ObjectID = "(O)909", DropsFromObjectIDs = new() { "95" } };
        public ResourceDropRule CinderShard { get; set; } = new() { Name = "Cinder Shard", Type = "Ore", ObjectID = "(O)848", DropsFromObjectIDs = new() { "843", "844" } };


        //Coal & Others
        public ResourceDropRule Coal { get; set; } = new() { Name = "Coal", Type = "Coal & Others", ObjectID = "(O)382", DropsFromObjectIDs = new() { "VolcanoCoalNode0", "VolcanoCoalNode1", "BasicCoalNode0", "BasicCoalNode1" } };
        public ResourceDropRule Clay { get; set; } = new() { Name = "Clay", Type = "Coal & Others", ObjectID = "(O)330", DropsFromObjectIDs = new() { "818" } };


        //Gems
        public ResourceDropRule Diamond { get; set; } = new() { Name = "Diamond", Type = "Gem", ObjectID = "(O)72", DropsFromObjectIDs = new() { "2", "44" } };
        public ResourceDropRule Amethyst { get; set; } = new() { Name = "Amethyst", Type = "Gem", ObjectID = "(O)66", DropsFromObjectIDs = new() { "8", "44" } };
        public ResourceDropRule Aquamarine { get; set; } = new() { Name = "Aquamarine", Type = "Gem", ObjectID = "(O)62", DropsFromObjectIDs = new() { "14", "44" } };
        public ResourceDropRule Emerald { get; set; } = new() { Name = "Emerald", Type = "Gem", ObjectID = "(O)60", DropsFromObjectIDs = new() { "12", "44" } };
        public ResourceDropRule Topaz { get; set; } = new() { Name = "Topaz", Type = "Gem", ObjectID = "(O)68", DropsFromObjectIDs = new() { "10", "44" } };
        public ResourceDropRule Ruby { get; set; } = new() { Name = "Ruby", Type = "Gem", ObjectID = "(O)64", DropsFromObjectIDs = new() { "4", "44" } };
        public ResourceDropRule Jade { get; set; } = new() { Name = "Jade", Type = "Gem", ObjectID = "(O)70", DropsFromObjectIDs = new() { "6", "44" } };


        //Geodes
        public ResourceDropRule Geode { get; set; } = new() { Name = "Geode", Type = "Geode", ObjectID = "(O)535", DropsFromObjectIDs = new() { "75" } };
        public ResourceDropRule FrozenGeode { get; set; } = new() { Name = "Frozen Geode", Type = "Geode", ObjectID = "(O)536", DropsFromObjectIDs = new() { "76" } };
        public ResourceDropRule MagmaGeode { get; set; } = new() { Name = "Magma Geode", Type = "Geode", ObjectID = "(O)537", DropsFromObjectIDs = new() { "77" } };
        public ResourceDropRule OmniGeode { get; set; } = new() { Name = "Omni Geode", Type = "Geode", ObjectID = "(O)749", DropsFromObjectIDs = new() { "819" } };
    
        


    
    }

    public class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; }
        public Config Config { get; set; }
        public uint realCurrentDay; //Used for MineOnDay1
        public bool AddedAddQuickSmeltRecipes = false;
        public bool AddedGemologistRecipes = false;

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Config = helper.ReadConfig<Config>();
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;

            //turning off MineForMore will prevent any function from working except GMCM to allow reenabling this mod.
            if (Config.TurnOnMineForMore)
            {
                var harmony = new Harmony(ModManifest.UniqueID);

                new UpdateOreGemDropsPatch(Config).Apply(harmony, Monitor);
                new MineProfessionLevelDescriptionPatch(Config).Apply(harmony, Monitor);
                new MineShaftOresPatches(Config).Apply(harmony, Monitor);
                new UnlimitedMiningLevel(Config).Apply(harmony, Monitor);

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
                recipe.AddQuickSmeltRecipes();  //AddsQuickSmeltRecipes
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
            var gmcm = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (gmcm is null) return;

            gmcm.Register(
                mod: ModManifest,
                reset: () => Config = new Config(),
                save: () => Helper.WriteConfig(Config)
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "Mine For More Enabled",
                tooltip: () => "Enable enhanced drop logic.",
                getValue: () => Config.TurnOnMineForMore,
                setValue: value => Config.TurnOnMineForMore = value
            );

            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "General Settings"
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "Mine For More Mod is Enabled",
                tooltip: () => "Turning this off will prevent all functions in this mod from working with the exception the config menus to be able to re-enable. Will need reload game once setting is set",
                getValue: () => Config.TurnOnMineForMore,
                setValue: value => Config.TurnOnMineForMore = value
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "User Can Mine on Day 1",
                tooltip: () => "Allow mine access on day 1",
                getValue: () => Config.CanMineOnDay1,
                setValue: value => Config.CanMineOnDay1 = value
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "Allow Skill Level to exceed level 10",
                tooltip: () => "Allows all skill levels to exceed 10, saved changes on this option requires restarting the game.",
                getValue: () => Config.AllowPlayerToExceedLevel10,
                setValue: value => Config.AllowPlayerToExceedLevel10 = value
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "Log Stone Destruction",
                tooltip: () => "Print stone destruction to console.",
                getValue: () => Config.listStoneDestroyedInConsole,
                setValue: value => Config.listStoneDestroyedInConsole = value
            );

            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "Profession Bonuses"
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Miner Bonus (Ore)",
                tooltip: () => "Bonus ore per level with Miner profession.",
                getValue: () => Config.MinerProfessionBonusOrePerLevel,
                setValue: v => Config.MinerProfessionBonusOrePerLevel = v,
                min: 0f,
                max: 10f,
                interval: 0.1f
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Geologist Bonus (Gem)",
                tooltip: () => "Bonus gems per level with Geologist profession.",
                getValue: () => Config.GeologistProfessionBonusGemsPerLevel,
                setValue: v => Config.GeologistProfessionBonusGemsPerLevel = v,
                min: 0f,
                max: 10f,
                interval: 0.1f
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Prospector Bonus (Coal)",
                tooltip: () => "Bonus coal per level with Prospector profession.",
                getValue: () => Config.ProspectorProfessionBonusCoalPerLevel,
                setValue: v => Config.ProspectorProfessionBonusCoalPerLevel = v,
                min: 0f,
                max: 10f,
                interval: 0.1f
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Excavator Bonus (Geode)",
                tooltip: () => "Bonus geodes per level with Excavator profession.",
                getValue: () => Config.ExcavatorProfessionBonusGeodesPerLevel,
                setValue: v => Config.ExcavatorProfessionBonusGeodesPerLevel = v,
                min: 0f,
                max: 10f,
                interval: 0.1f
            );

            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "Drop Configurations"
            );


            foreach (var drop in GetAllDrops())
            {
                AddDropToGMCM(gmcm, drop.Name, () => drop);
            }
        }

        private void AddDropToGMCM(IGenericModConfigMenuApi gmcm, string label, Func<ResourceDropRule> getDrop)
        {
            var drop = getDrop();

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => $"{label} - AddAmount",
                tooltip: () => $"Extra {label} drop count to add when it spawns.",
                getValue: () => drop.AddAmount,
                setValue: v => drop.AddAmount = v,
                min: 0,
                max: 50
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => $"{label} - Multiplier",
                tooltip: () => $"Multiplier to apply to {label}'s base drop count.",
                getValue: () => (float)drop.Multiplier,
                setValue: v => drop.Multiplier = v,
                min: 0f,
                max: 10f,
                interval: 0.1f
            );
        }

    }
}
