using HarmonyLib;
using LevelForMore;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Objects;
using System.Collections.Generic;
using static LevelForMore.ModEntry;

internal class ProfessionLevelDescriptionPatch
{
    private readonly Config _config;

    public ProfessionLevelDescriptionPatch(Config config)
    {
        _config = config;
        
    }

    public void Apply(Harmony harmony, IMonitor monitor)
    {
        monitor.Log("Patching LevelUpMenu.getProfessionDescription", LogLevel.Debug);

        var original = AccessTools.Method(typeof(LevelUpMenu), nameof(LevelUpMenu.getProfessionDescription));
        var postfix = new HarmonyMethod(typeof(ProfessionLevelDescriptionPatch), nameof(ModifyProfessionDescription));

        harmony.Patch(original, postfix: postfix);
    }

    public static void ModifyProfessionDescription(int whichProfession, ref List<string> __result)
    {
        if (__result == null)
            return;
        var i18n = Instance.Helper.Translation;


        switch (whichProfession)
        {
            case 12: // Forester 
                __result = new List<string>
            {
                $"{i18n.Get("profession.12.name")}",
                i18n.Get("profession.12.detail", new { bonus = Instance.Config.ForesterWoodPerLevelBonus.ToString("0.0"), bonus1 = Instance.Config.ForesterSeedPerLevelBonus.ToString("0.0") })
            };
                break;
            case 13: // Gatherer 
                __result = new List<string>
            {
                $"{i18n.Get("profession.13.name")}",
                i18n.Get("profession.13.detail", new { bonus = Instance.Config.GathererExtraDropPerLevel.ToString("0.0") })
            };
                break;
            case 14: // Lumberjack 
                __result = new List<string>
            {
                $"{i18n.Get("profession.14.name")}",
                i18n.Get("profession.14.detail")
            };
                break;
            case 15: // Tapper 
                __result = new List<string>
            {
                $"{i18n.Get("profession.15.name")}",
                i18n.Get("profession.15.detail", new { bonus = Instance.Config.TapperSpeedBonusPercentPerLevel.ToString("0.0"), bonus1 = Instance.Config.TapperExtraQuantityPerLevel.ToString("0.0") })
            };
                break;
            case 16: // Botanist
                __result = new List<string>
            {
                $"{i18n.Get("profession.16.name")}",
                i18n.Get("profession.16.detail")
            };
                break;
            case 17: // Tracker 
                __result = new List<string>
            {
                $"{i18n.Get("profession.17.name")}",
                i18n.Get("profession.17.detail")
            };
                break;
            case 18: // Miner
                __result = new List<string>
            {
                $"{i18n.Get("profession.18.name")}\n{i18n.Get("profession.18.desc")}",
                i18n.Get("profession.18.detail", new { bonus = Instance.Config.MinerProfessionBonusOrePerLevel.ToString("0.0") })
            };
                break;

            case 19: // Geologist
                __result = new List<string>
        {
            $"{i18n.Get("profession.19.name")}\n{i18n.Get("profession.19.desc")}",
            i18n.Get("profession.19.detail", new { bonus = Instance.Config.GeologistProfessionBonusGemsPerLevel.ToString("0.0") })
        };
                break;

            case 20: // Blacksmith
                __result = new List<string>
        {
            i18n.Get("profession.20.name"),
            i18n.Get("profession.20.desc")
        };
                break;

            case 21: // Prospector
                __result = new List<string>
        {
            i18n.Get("profession.21.name"),
            i18n.Get("profession.21.desc", new { bonus = Instance.Config.ProspectorProfessionBonusCoalPerLevel.ToString("0.0") })
        };
                break;

            case 22: // Excavator
                __result = new List<string>
        {
            i18n.Get("profession.22.name"),
            i18n.Get("profession.22.desc", new { bonus = Instance.Config.ExcavatorProfessionBonusGeodesPerLevel.ToString("0.0") })
        };
                break;

            case 23: // Gemologist
                __result = new List<string>
        {
            i18n.Get("profession.23.name"),
            i18n.Get("profession.23.desc")
        };
                break;
        }




    }
}
