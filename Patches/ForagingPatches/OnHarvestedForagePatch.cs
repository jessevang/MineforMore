using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MineForMore.Patches.ForagingPatches
{
    public class OnHarvestedForagePatch
    {
        public void Apply(Harmony harmony, IMonitor monitor)
        {
            var original = AccessTools.Method(typeof(GameLocation), nameof(GameLocation.OnHarvestedForage));
            if (original is null)
            {
                if (ModEntry.Instance.Config.DebugMode)
                    monitor.Log("Failed to find GameLocation.OnHarvestedForage", LogLevel.Error);
                return;
            }

            var postfix = new HarmonyMethod(typeof(OnHarvestedForagePatch), nameof(Postfix));
            harmony.Patch(original: original, postfix: postfix);
        }


        //When Foraging it forages more items into inventory
        public static void Postfix(GameLocation __instance, Farmer who, StardewValley.Object forage)
        {
            if (ModEntry.Instance.Config.DebugMode)
                ModEntry.Instance.Monitor.Log($"[Foraging] Check Forage Item: ID = {forage.ItemId}, Name = {forage.Name}, ForageType = {forage.GetType} ", LogLevel.Info);
            if (forage == null || forage.ItemId == null)
                return;
            //Logging to check item
            if (ModEntry.Instance.Config.DebugMode)
                ModEntry.Instance.Monitor.Log($"[Foraging] Picked: ID = {forage.ItemId}, Name = {forage.Name}", LogLevel.Info);





            // Parse numeric item ID from forage.ItemId (removes "(O)" prefix if present)
            string id = forage.ItemId;
            if (id.StartsWith("(O)") && int.TryParse(id.Substring(3), out int numericId))
                id = numericId.ToString();



            // Find matching drop rule
            var rule = ModEntry.Instance.GetAllRules()
                .FirstOrDefault(r => r.Type == "Forage" && r.DropsFromObjectIDs.Contains(id));



            if (rule == null)
            {
                //Logging to check item
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[Foraging] There is no rule found for this item meaning it's not listed rule is = {rule}", LogLevel.Info);
                return;
            }
                

            //Logging to check item
            if (ModEntry.Instance.Config.DebugMode)
                ModEntry.Instance.Monitor.Log($"[Foraging] rule = {rule}", LogLevel.Info);

            int baseAmount = rule.AddAmount;

            // Gatherer profession bonus
            int extraCount = 0;
            if (who.professions.Contains(13)) // Gatherer
            {
                int level = who.GetSkillLevel(Farmer.foragingSkill);
                extraCount = (int)(level * ModEntry.Instance.Config.GathererExtraDropPerLevel);
            }

            int totalCount = (baseAmount + extraCount);
            totalCount = (int)Math.Round(totalCount * rule.Multiplier);

            for (int i = 0; i < totalCount; i++)
            {
                Item extra = forage.getOne();
                who.addItemToInventoryBool(extra);
            }
        }
    }
}
