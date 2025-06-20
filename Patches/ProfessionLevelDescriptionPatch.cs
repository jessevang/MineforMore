using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using MineForMore;
using static MineForMore.ModEntry;
using StardewValley.Menus;
using System.Collections.Generic;

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
