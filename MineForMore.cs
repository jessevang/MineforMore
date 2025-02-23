using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineForMore.Classes;
using Spacechase.Shared.Patching;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Enchantments;
using StardewValley.GameData.Weapons;
using StardewValley.ItemTypeDefinitions;
using StardewValley.Locations;
using StardewValley.Minigames;
using StardewValley.Objects;
using StardewValley.Tools;
using System.Runtime.InteropServices;



namespace MineForMore
{
    public class Config
    {

        public string Comment1 { get; set; } = "======================================Turn On/Off Certain Features ======================================";
        public bool TurnOnMineOnDay1{ get; set; } = true;
        public bool TurnOnMineForMore {  get; set; } = true;
        public bool TurnOnMaxLevelFrom10toUnlimited { get; set; } = true;

        public string Comment { get; set; } = "======================================Mine for More Settings ============================================";
        public double MinerProfessionBonusOrePerLevel { get; set; } = 1;
        public double GeologistProfessionBonusGemsPerLevel { get; set; } = 1;
        public double ProspectorProfessionBonusCoalPerLevel { get; set; } = 1;
        public double ExcavatorProfessionBonusGeodesPerLevel { get; set; } = 1;
        public string Comment2 { get; set; } = "==================Flat values added to ore/gems without professions=======================================";
        public int AddStone { get; set; } =100;
        public int AddCoal { get; set; } =100;
        public int AddCooperOre { get; set; } =100;
        public int AddGoldOre { get; set; } =100;
        public int AddIronOre { get; set; } =100;
        public int AddIridiumOre { get; set; } =100;
        public int AddRadiactiveOre { get; set; } =100;
        public int AddDiamond { get; set; } =100;
        public int AddAmethyst { get; set; } =100;
        public int AddAquamarine { get; set; } =100;
        public int AddEarthCrystal { get; set; } =100;
        public int AddEmerald { get; set; } =100;
        public int AddRuby { get; set; } =100;
        public int AddTopaz { get; set; } =100;
        public int AddJade { get; set; } =100;
        public int AddCinderShard { get; set; } =100;
        public int AddGeode { get; set; } =100;
        public int AddFrozenGeode { get; set; } =100;
        public int AddMagmaGeode { get; set; } =100;
        public int AddOmniGeode { get; set; } =100;
        public int AddClay {  get; set; } =100;
        public double MultiplyStone { get; set; } = 1.0;
        public double MultiplyCoal { get; set; } = 1.0;
        public double MultiplyCooperOre { get; set; } = 1.0;
        public double MultiplyGoldOre { get; set; } = 1.0;
        public double MultiplyIronOre { get; set; } = 1.0;
        public double MultiplyIridiumOre { get; set; } = 1.0;
        public double MultiplyRadiactiveOre { get; set; } = 1.0;
        public double MultiplyDiamond { get; set; } = 1.0;
        public double MultiplyAmethyst { get; set; } = 1.0;
        public double MultiplyAquamarine { get; set; } = 1.0;
        public double MultiplyEarthCrystal { get; set; } = 1.0;
        public double MultiplyEmerald { get; set; } = 1.0;
        public double MultiplyRuby { get; set; } = 1.0;
        public double MultiplyTopaz { get; set; } = 1.0;
        public double MultiplyJade { get; set; } = 1.0;
        public double MultiplyCinderShard { get; set; } = 1.0;
        public double MultiplyGeode { get; set; } = 1.0;
        public double MultiplyFrozenGeode { get; set; } = 1.0;
        public double MultiplyMagmaGeode { get; set; } = 1.0;
        public double MultiplyOmniGeode { get; set; } = 1.0;
        public double MultiplyClay { get; set; } = 1.0;

    }

    internal sealed class ModEntry : Mod
    {

        public static ModEntry Instance { get; private set; }
        public Config Config { get; private set; }
        public uint realCurrentDay; //Used for MineOnDay1
        public bool AddedAddQuickSmeltRecipes = false;
        public bool AddedGemologistRecipes = false;
        private Harmony _harmony;

        public override void Entry(IModHelper helper)
        {
            _harmony = new Harmony(ModManifest.UniqueID);
   
            Instance = this; 
            Config = helper.ReadConfig<Config>() ?? new Config();
            Farmer self = Game1.player;

            //Handles Content Patching
            if (!helper.ModRegistry.IsLoaded("Pathoschild.ContentPatcher"))
            {
                Monitor.Log("Content Patcher is not installed. Skipping patches.", LogLevel.Warn);
            }



               // RegisterContentPatcherPack();




            //TurnOnMineForMore
            if (Instance.Config.TurnOnMineForMore)
            {
                HarmonyPatcher.Apply(this,
                 new UpdateOreGemDropsPatch(Config)
                 );
                HarmonyPatcher.Apply(this,
                new MineProfessionLevelDescriptionPatch(Config)
                );
                HarmonyPatcher.Apply(this,
                new MineShaftOresPatches(Config)
                );
                HarmonyPatcher.Apply(this,
                new MineShaftOresPatches(Config)
                );

                // Apply Harmony Patches for private functions
                var MineShaftpatches = new MineShaftOresPatches(Config);
                MineShaftpatches.Apply(_harmony, Monitor);


 
            }

            //TurnOnMaxLevelFrom10toUnlimited
            if (Instance.Config.TurnOnMaxLevelFrom10toUnlimited)
            {
                HarmonyPatcher.Apply(this,
                new UnlimitedMiningLevel(Config)
                );
            }
            

            //Event handlers
            helper.Events.GameLoop.DayStarted += onStartOfDay;
            helper.Events.GameLoop.DayEnding += onEndOfDay;
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;


            // Unpatch on game exit



        }





        private void onStartOfDay(object sender, EventArgs e)
        {
            Monitor.Log("DayStarted event triggered!", LogLevel.Info);
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
                        //Monitor.Log("Quick Smelt Recipes added!", LogLevel.Info);
                    }

                    /*
                    if (Game1.player.professions.Contains(23) && !AddedGemologistRecipes)
                    {

                        Recipe recipe = new Recipe(Instance);
                        recipe.AddGemologistRecipes();  //AddsQuickSmeltRecipes
                        AddedGemologistRecipes = true;
                        //Monitor.Log("Gemologist Recipes added!", LogLevel.Info);

                    }
                    */

                    
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

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            // Only run this logic for the local player
            if (!Context.IsWorldReady || !Game1.player.IsLocalPlayer)
                return;


            /*
            //TurnOnMineForMore
            if (Config.TurnOnIncreasePickAxeDamagePerMiningLevel)
            {
                double damage = Config.IncreasePickAxeDamagePerMiningLevel;
                IncreasePickAxeDamage pickaxe1 = new IncreasePickAxeDamage(damage);
                pickaxe1.ApplyIncreasedDamage();
            }

            */


        }








    }
}