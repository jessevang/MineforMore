using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelForMore.Patches.ForagingPatches
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
                ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] Check Forage Item: ID = {forage.ItemId}, Name = {forage.Name}, ForageType = {forage.GetType} ", LogLevel.Info);
            if (forage == null || forage.ItemId == null)
                return;
            //Logging to check item
            if (ModEntry.Instance.Config.DebugMode)
                ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] Picked: ID = {forage.ItemId}, Name = {forage.Name}", LogLevel.Info);





            // Parse numeric item ID from forage.ItemId (removes "(O)" prefix if present)
            string id = forage.ItemId;
            if (id.StartsWith("(O)") && int.TryParse(id.Substring(3), out int numericId))
                id = numericId.ToString();




            var rule = ModEntry.Instance.GetAllRules()
                .FirstOrDefault(r => r.Type == "Forage" && r.DropsFromObjectIDs.Contains(id));



            if (rule == null)
            {

                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] There is no rule found for this item meaning it's not listed rule is = {rule}", LogLevel.Info);
                return;
            }
                


            if (ModEntry.Instance.Config.DebugMode)
                ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] rule [Add = {rule.AddAmount}, Multiplier = {rule.Multiplier}]", LogLevel.Info);

            int baseAmount = rule.AddAmount;


            int extraCount = 0;
            if (who.professions.Contains(13)) // Gatherer
            {
                int level = who.GetSkillLevel(Farmer.foragingSkill);
                extraCount = (int)(level * ModEntry.Instance.Config.GathererExtraDropPerLevel);
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] Farmer Has Gatherer Profession ExtraCount= {extraCount}", LogLevel.Info);
            }
            if (who.professions.Contains(16)) // Botanist
            {
                int level = who.GetSkillLevel(Farmer.foragingSkill);
                extraCount = (int)(extraCount*1.25);
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] Farmer Has Bontanist Profession ExtraCount= {extraCount}", LogLevel.Info);
            }
            if (who.professions.Contains(17)) // Tracker 
            {
                int level = who.GetSkillLevel(Farmer.foragingSkill);
                extraCount = (int)(extraCount * 1.5);
                if (ModEntry.Instance.Config.DebugMode)
                    ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] Farmer Has Tracker Profession ExtraCount= {extraCount}", LogLevel.Info);
            }

            int totalCount = (baseAmount + extraCount);
            totalCount = (int)Math.Round(totalCount * rule.Multiplier);
            if (ModEntry.Instance.Config.DebugMode)
                ModEntry.Instance.Monitor.Log($"[Foraging][OnHarvestedForagePatch] {rule.Name}: base({baseAmount}) + Extra({extraCount})* Multiplier({rule.Multiplier}) = Total({totalCount})", LogLevel.Info);



            for (int i = 0; i < totalCount; i++)
            {
                Item extra = forage.getOne();
                who.addItemToInventoryBool(extra);
            }
        }
    }
}
