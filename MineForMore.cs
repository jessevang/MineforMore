using Spacechase.Shared.Patching;
using StardewModdingAPI;
using StardewValley;

namespace MineForMore
{
    public class Config
    {
        public int BonusOresWithMinerProfession { get; set; } = 5;
        public int AddStone { get; set; } = 5;
        public int AddCoal { get; set; } = 5;
        public int AddCooperOre { get; set; } = 5;
        public int AddGoldOre { get; set; } = 5;
        public int AddIronOre { get; set; } = 5;
        public int AddIridiumOre { get; set; } = 5;
        public int AddRadiactiveOre { get; set; } = 5;
        public int AddDiamond { get; set; } = 5;
        public int AddAmethyst { get; set; } = 5;
        public int AddAquamarine { get; set; } = 5;
        public int AddEarthCrystal { get; set; } = 5;
        public int AddEmerald { get; set; } = 5;
        public int AddFireQuartz { get; set; } = 5;
        public int AddFrozenTear { get; set; } = 5;
        public int AddQuartz { get; set; } = 5;
        public int AddRuby { get; set; } = 5;
        public int AddTopaz { get; set; } = 5;
        public int AddJade { get; set; } = 5;
        public double MultiplyStone { get; set; } = 2.0;
        public double MultiplyCoal { get; set; } = 2.0;
        public double MultiplyCooperOre { get; set; } = 2.0;
        public double MultiplyGoldOre { get; set; } = 2.0;
        public double MultiplyIronOre { get; set; } = 2.0;
        public double MultiplyIridiumOre { get; set; } = 2.0;
        public double MultiplyRadiactiveOre { get; set; } = 2.0;
        public double MultiplyDiamond { get; set; } = 2.0;
        public double MultiplyAmethyst { get; set; } = 2.0;
        public double MultiplyAquamarine { get; set; } = 2.0;
        public double MultiplyEarthCrystal { get; set; } = 2.0;
        public double MultiplyEmerald { get; set; } = 2.0;
        public double MultiplyFireQuartz { get; set; } = 2.0;
        public double MultiplyFrozenTear { get; set; } = 2.0;
        public double MultiplyQuartz { get; set; } = 2.0;
        public double MultiplyRuby { get; set; } = 2.0;
        public double MultiplyTopaz { get; set; } = 2.0;
        public double MultiplyJade { get; set; } = 2.0;
    }

    internal sealed class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; } // Singleton instance for global access
        public Config Config { get; private set; } // Store the config
        public uint NumberOfDrops;
        public uint NumberOfDropsWithRing;

        public override void Entry(IModHelper helper)
        {
            Instance = this; // Assign the instance for global access
            Config = helper.ReadConfig<Config>() ?? new Config();

            //This Harmony Patcher turns on more monster drops
            HarmonyPatcher.Apply(this,
            new ObjectPatches(Config)
            );
        }


        // Save config method for later use
        public void SaveConfig()
        {
            Helper.WriteConfig(Config);
        }





    }
}