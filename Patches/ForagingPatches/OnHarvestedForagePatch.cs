using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace MineForMore.Patches.ForagingPatches
{
    public class OnHarvestedForagePatch
    {
        public void Apply(Harmony harmony, IMonitor monitor)
        {
            var original = AccessTools.Method(typeof(GameLocation), nameof(GameLocation.OnHarvestedForage));
            if (original is null)
            {
                monitor.Log("Failed to find GameLocation.OnHarvestedForage", LogLevel.Error);
                return;
            }

            var postfix = new HarmonyMethod(typeof(OnHarvestedForagePatch), nameof(Postfix));
            harmony.Patch(original: original, postfix: postfix);
        }

        public static void Postfix(GameLocation __instance, Farmer who, StardewValley.Object forage)
        {
            if (!who.professions.Contains(13)) // Gatherer profession
                return;

            int level = who.GetSkillLevel(Farmer.foragingSkill);
            int extraCount = level * ModEntry.Instance.Config.GathererExtraDropPerLevel;

            for (int i = 0; i < extraCount; i++)
            {
                Item extra = forage.getOne();
                who.addItemToInventoryBool(extra);
            }
        }
    }
}
