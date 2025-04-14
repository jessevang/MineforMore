using GenericModConfigMenu;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineForMore.Classes;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Enchantments;
using StardewValley.GameData.Weapons;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Locations;
using StardewValley.Minigames;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;



namespace MineForMore
{
    public class Config
    {
        public bool TurnOnMineOnDay1 { get; set; } = true;
        public bool TurnOnMineForMore { get; set; } = true;
        public bool AllowPlayerToExceedLevel10 { get; set; } = true;
        public bool listStoneDestroyedInConsole { get; set; } = true;

        public float MinerProfessionBonusOrePerLevel { get; set; } = 1.0f;
        public float GeologistProfessionBonusGemsPerLevel { get; set; } = 1.0f;
        public float ProspectorProfessionBonusCoalPerLevel { get; set; } = 1.0f;
        public float ExcavatorProfessionBonusGeodesPerLevel { get; set; } = 1.0f;

        public List<MiningTypeDrop> MiningTypeDrop { get; set; } = new()
        {
            // === ORES ===
            new MiningTypeDrop { Name = "Stone", Type = "Ore", ObjectID = "(O)390",  AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "32", "34", "36", "38", "40", "48", "50", "52", "54", "56", "58", "343", "450", "668", "670", "760", "762", "845", "846", "847" } },
            new MiningTypeDrop { Name = "Coal", Type = "Ore", ObjectID = "(O)382",  AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "670" } },
            new MiningTypeDrop { Name = "Copper Ore", Type = "Ore", ObjectID = "(O)378",   AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "751" } },
            new MiningTypeDrop { Name = "Iron Ore", Type = "Ore", ObjectID = "(O)380",   AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "290" } },
            new MiningTypeDrop { Name = "Gold Ore", Type = "Ore", ObjectID = "(O)384",   AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "764" } },
            new MiningTypeDrop { Name = "Iridium Ore", Type = "Ore", ObjectID = "(O)386",   AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "765" } },
            new MiningTypeDrop { Name = "Radioactive Ore", Type = "Ore", ObjectID = "(O)909",   AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "843" } },
            new MiningTypeDrop { Name = "Clay", Type = "Ore", ObjectID = "(O)330",   AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "668" } },
            new MiningTypeDrop { Name = "Cinder Shard", Type = "Ore", ObjectID = "(O)848",   AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "849" } },

            // === GEMS ===
            new MiningTypeDrop { Name = "Diamond", Type = "Gem", ObjectID = "(O)72", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "2" } },
            new MiningTypeDrop { Name = "Amethyst", Type = "Gem", ObjectID = "(O)66", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "8" } },
            new MiningTypeDrop { Name = "Aquamarine", Type = "Gem", ObjectID = "(O)62", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "14" } },
            new MiningTypeDrop { Name = "Emerald", Type = "Gem", ObjectID = "(O)60", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "12" } },
            new MiningTypeDrop { Name = "Topaz", Type = "Gem", ObjectID = "(O)68", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "10" } },
            new MiningTypeDrop { Name = "Ruby", Type = "Gem", ObjectID = "(O)64", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "4" } },
            new MiningTypeDrop { Name = "Jade", Type = "Gem", ObjectID = "(O)70", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "6" } },


            // === GEODES ===
            new MiningTypeDrop { Name = "Geode", Type = "Geode", ObjectID = "(O)535", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "75" } },
            new MiningTypeDrop { Name = "Frozen Geode", Type = "Geode", ObjectID = "(O)536", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "76" } },
            new MiningTypeDrop { Name = "Magma Geode", Type = "Geode", ObjectID = "(O)537", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "77" } },
            new MiningTypeDrop { Name = "Omni Geode", Type = "Geode", ObjectID = "(O)749", AddAmount = 3, Multiplier = 1.0, DropsFromObjectIDs = new() { "819" } }
        
        
            
        
        
        };


    }

    public class MiningTypeDrop
    {
        public string Name { get; set; }                 // e.g. "Diamond"
        public string Type { get; set; }                 // e.g. "Ore" or "Gem"
        public string ObjectID { get; set; }             // e.g. "(O)72"
        public int AddAmount { get; set; } = 3;          // How many extra to add
        public double Multiplier { get; set; } = 1.0;    // Bonus multiplier
        public List<string> DropsFromObjectIDs { get; set; } = new(); // Object IDs this item can drop from
    }




    internal sealed class ModEntry : Mod
    {

        public static ModEntry Instance { get; private set; }
        public Config Config { get; set; }
        public uint realCurrentDay; //Used for MineOnDay1
        public bool AddedAddQuickSmeltRecipes = false;
        public bool AddedGemologistRecipes = false;
        private Harmony _harmony;

        public override void Entry(IModHelper helper)
        {

            _harmony = new Harmony(ModManifest.UniqueID);

            Instance = this;
            Config = helper.ReadConfig<Config>();
            Farmer self = Game1.player;

            
            //TurnOnMineForMore
            if (Instance.Config.TurnOnMineForMore)
            {

                new UpdateOreGemDropsPatch(Config).Apply(_harmony, Monitor);
                new MineProfessionLevelDescriptionPatch(Config).Apply(_harmony, Monitor);
                new MineShaftOresPatches(Config).Apply(_harmony, Monitor);


                // Apply Harmony Patches for private functions
                var MineShaftpatches = new MineShaftOresPatches(Config);
                MineShaftpatches.Apply(_harmony, Monitor);


            }

        
            if (Instance.Config.AllowPlayerToExceedLevel10)
            {
                new UnlimitedMiningLevel(Config).Apply(_harmony, Monitor);
            }

            

            helper.Events.GameLoop.DayStarted += onStartOfDay;
            helper.Events.GameLoop.DayEnding += onEndOfDay;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.Input.ButtonPressed += OnButtonPressed;

        }

        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // Only run while a menu is open
            if (Game1.activeClickableMenu != null && e.Button == SButton.F7)
            {
                Item heldItem = Game1.player.CursorSlotItem;

                if (heldItem != null)
                {
                    this.Monitor.Log($"Held item: {heldItem.Name} (ID: {heldItem.ParentSheetIndex}, Type: {heldItem.GetType().Name})", LogLevel.Info);
                }
                else
                {
                    this.Monitor.Log("No item is currently held.", LogLevel.Info);
                }
            }
        }


        private void onStartOfDay(object sender, EventArgs e)
        {
            //Monitor.Log("DayStarted event triggered!", LogLevel.Info);
            this.realCurrentDay = (uint)Game1.stats.DaysPlayed;
            // Check the number of days played

            if (Game1.stats.DaysPlayed <= 5 && Instance.Config.TurnOnMineOnDay1)
            {
                Game1.stats.DaysPlayed = 5; // Set the days played to 5
            }


            if (Config.TurnOnMineForMore)
            {
                Game1.delayedActions.Add(new DelayedAction(0, () =>
                {
                    //ModEntry.Instance.Monitor.Log("Adding recipes after slight delay...", LogLevel.Info);
                    if (Game1.player.professions.Contains(20) && !AddedAddQuickSmeltRecipes)
                    {

                        Recipe recipe = new Recipe(Instance);
                        recipe.AddQuickSmeltRecipes();  //AddsQuickSmeltRecipes
                        AddedAddQuickSmeltRecipes = true;

                    }

                }));

            }




        }

        //resets the day back to the original current day at the end of the day so calendar
        private void onEndOfDay(object sender, EventArgs e)
        {
            if (!Instance.Config.TurnOnMineOnDay1)
                return;
            Game1.stats.DaysPlayed = this.realCurrentDay;

        }




        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var gmcm = Helper.ModRegistry
                .GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (gmcm is null)
                return;


            var i18n = Instance.Helper.Translation;

            gmcm.Register(
                mod: ModManifest,
                reset: () => Config = new Config(),
                save: () => Helper.WriteConfig(Config)
            );

            // === Section: General Settings ===
            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "General Settings"
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "Mine For More Enabled",
                tooltip: () => "Toggle the enhanced ore/gem drops.",
                getValue: () => Config.TurnOnMineForMore,
                setValue: value => Config.TurnOnMineForMore = value
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "Start Mine on Day 1",
                tooltip: () => "Allows the mine to be accessible on the first day of a new save.",
                getValue: () => Config.TurnOnMineOnDay1,
                setValue: value => Config.TurnOnMineOnDay1 = value
            );

            gmcm.AddBoolOption(
                mod: ModManifest,
                name: () => "Allow Player To Exceed Level 10",
                tooltip: () => "Turning this on will allyer player to exceed level 10 skills by continuing to gain experiences",
                getValue: () => Config.AllowPlayerToExceedLevel10,
                setValue: value => Config.AllowPlayerToExceedLevel10 = value
            );

            // === Section: Profession Bonuses ===
            gmcm.AddSectionTitle(
                mod: ModManifest,
                text: () => "Profession Bonuses"
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Miner (Ore per Level)",
                tooltip: () => "Bonus ore drops per mining level when using Miner profession.",
                getValue: () => Config.MinerProfessionBonusOrePerLevel,
                setValue: value => Config.MinerProfessionBonusOrePerLevel = value,
                min: 0.0f,
                max: 10.0f,
                interval: 0.1f
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Geologist (Gems per Level)",
                tooltip: () => "Bonus gem drops per mining level when using Geologist profession.",
                getValue: () => Config.GeologistProfessionBonusGemsPerLevel,
                setValue: value => Config.GeologistProfessionBonusGemsPerLevel = value,
                min: 0.0f,
                max: 10.0f,
                interval: 0.1f
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Prospector (coal per Level)",
                tooltip: () => "Bonus coal drops per mining level when using Prospector profession.",
                getValue: () => Config.ProspectorProfessionBonusCoalPerLevel,
                setValue: value => Config.ProspectorProfessionBonusCoalPerLevel = value,
                min: 0.0f,
                max: 10f,
                interval: 0.1f
            );

            gmcm.AddNumberOption(
                mod: ModManifest,
                name: () => "Excavator (Geodes per Level)",
                tooltip: () => "Bonus geode drops per mining level when using Excavator profession.",
                getValue: () => Config.ExcavatorProfessionBonusGeodesPerLevel,
                setValue: value => Config.ExcavatorProfessionBonusGeodesPerLevel = value,
                min: 0,
                max: 10,
                interval: 0.1f
            );

            // === Section: Note for Custom Entries ===
            gmcm.AddParagraph(
                mod: ModManifest,
                text: () => "To customize ores, gems, and geodes (including modded ones), open 'config.json' manually. Add or edit each entry"
                           
            );



            foreach (var entry in Config.MiningTypeDrop)
            {
                string dropsFrom = entry.DropsFromObjectIDs != null && entry.DropsFromObjectIDs.Any()
                    ? string.Join(", ", entry.DropsFromObjectIDs)
                    : "None";

                gmcm.AddParagraph(
                    mod: ModManifest,
                    text: () =>
                        $"Name: {entry.Name}\n" +
                        $"Type: {entry.Type}\n" +
                        $"ID: {entry.ObjectID}\n" +
                        $"Add: {entry.AddAmount}\n" +
                        $"Multiply: {entry.Multiplier}\n" +
                        $"Drops From: {dropsFrom}\n" +
                        $"------------------------"
                );
            }


        }








    }


}