using Spacechase.Shared.Patching;
using StardewModdingAPI;
using StardewValley;

namespace MineForMore
{
    internal sealed class ModEntry : Mod
    {
        public uint NumberOfDrops;
        public uint NumberOfDropsWithRing;
        public const string ConfigFileName = "config.json";

        private class Config
        {
            public int BonusOresWithMinerProfession { get; set; } = 0;
            public int AddStone { get; set; } = 0;
            public int AddCoal { get; set; } = 0;
            public int AddCooperOre { get; set; } = 0;
            public int AddGoldOre { get; set; } = 0;
            public int AddIronOre { get; set; } = 0;
            public int AddIridiumOre { get; set; } = 0;
            public int AddRadiactiveOre { get; set; } = 0;
            public int AddDiamond { get; set; } = 0;
            public int AddAmethyst { get; set; } = 0;
            public int AddAquamarine { get; set; } = 0;
            public int AddEarthCrystal { get; set; } = 0;
            public int AddEmerald { get; set; } = 0;
            public int AddFireQuartz { get; set; } = 0;
            public int AddFrozenTear { get; set; } = 0;
            public int AddQuartz { get; set; } = 0;
            public int AddRuby { get; set; } = 0;
            public int AddTopaz { get; set; } = 0;
            public int AddJade { get; set; } = 0;
            public double MultiplyStone{ get; set; } = 1.0;
            public double MultiplyCoal { get; set; } = 1.0;
            public double MulitiplyCooperOre { get; set; } = 1.0;
            public double MulitiplyGoldOre { get; set; } = 1.0;
            public double MulitiplyIronOre { get; set; } = 1.0;
            public double MulitiplyIridiumOre { get; set; } = 1.0;
            public double MulitiplyRadiactiveOre { get; set; } = 1.0;
            public double MulitiplyDiamond { get; set; } = 1.0;
            public double MulitiplyAmethyst { get; set; } = 1.0;
            public double MulitiplyAquamarine { get; set; } = 1.0;
            public double MulitiplyEarthCrystal { get; set; } = 1.0;
            public double MulitiplyEmerald { get; set; } = 1.0;
            public double MulitiplyFireQuartz { get; set; } = 1.0;
            public double MulitiplyFrozenTear { get; set; } = 1.0;
            public double MulitiplyQuartz { get; set; } = 1.0;
            public double MulitiplyRuby { get; set; } = 1.0;
            public double MulitiplyTopaz { get; set; } = 1.0;
            public double MulitiplyJade { get; set; } = 1.0;



        }

        /*********
       ** Public methods
       *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            var config = helper.ReadConfig<Config>() ?? new Config();

       
            //This Harmony Patcher turns on more monster drops
            HarmonyPatcher.Apply(this,
            new ObjectPatches()
            );

        }






    }
}