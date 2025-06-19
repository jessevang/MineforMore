using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System.Linq;

namespace MineForMore.Patches.ForagingPatches
{
    internal class ResourceClumpDestroyedPatch
    {
        public void Apply(Harmony harmony, IMonitor monitor)
        {
            var method = AccessTools.Method(typeof(ResourceClump), nameof(ResourceClump.destroy));
            if (method is null)
            {
                monitor.Log("Failed to find ResourceClump.destroy", LogLevel.Error);
                return;
            }

            var postfix = new HarmonyMethod(typeof(ResourceClumpDestroyedPatch), nameof(Postfix));
            harmony.Patch(method, postfix: postfix);
        }

        public static void Postfix(ResourceClump __instance, Tool t, GameLocation location, Vector2 tileLocation)
        {
            if (t == null || !Context.IsMainPlayer || !t.getLastFarmerToUse().IsLocalPlayer)
                return;

            if (t is not Axe axe)
                return;

            var who = axe.getLastFarmerToUse();
            int id = __instance.parentSheetIndex.Value;
            ModEntry.Instance.Monitor.Log($"[MineForMore] ResourceClump ID = {id}", LogLevel.Debug);

            string idKey = id switch
            {
                600 => "LargeStump",
                602 => "LargeLog",
                _ => null
            };
            ModEntry.Instance.Monitor.Log($"[MineForMore] Mapped ID {id} to key '{idKey}'", LogLevel.Debug);

            if (idKey == null)
                return;

            var rule = ModEntry.Instance.GetAllRules().FirstOrDefault(r => r.DropsFromObjectIDs.Contains(idKey));

            ModEntry.Instance.Monitor.Log($"[MineForMore] Rule is {rule} ", LogLevel.Debug);
            if (rule == null)
                return;

            Farmer farmer = t.getLastFarmerToUse();
            int skillLevel = farmer.GetSkillLevel(2); // Foraging
            int extra = rule.AddAmount + (int)(skillLevel * rule.Multiplier);

            if (extra <= 0)
                return;

            Vector2 offset = new Vector2(__instance.width.Value / 2f, __instance.height.Value / 2f) * 64f;

            int currentForaginglevel = who.GetSkillLevel(Farmer.foragingSkill);
            int flat = rule.AddAmount;
            int scaled = who.professions.Contains(12) ? (int)Math.Floor(ModEntry.Instance.Config.ForesterWoodPerLevelBonus * currentForaginglevel) : 0;
            int total = (int)((flat + scaled) * rule.Multiplier);

            Game1.delayedActions.Add(new DelayedAction(100, () =>
            {
                //Game1.createMultipleObjectDebris(, (int)tileLocation.X, (int)tileLocation.Y, extra, farmer.UniqueMultiplayerID);
                for (int i = 0; i < total; i++)
                {
                    Vector2 spawn = (tileLocation * 64f) + new Vector2(
                                   Game1.random.Next(-24, 25),  // X offset in pixels
                                   Game1.random.Next(-24, 25)   // Y offset in pixels
                               );

                    Item item = ItemRegistry.Create(rule.ObjectID);
                    Game1.createItemDebris(item, spawn, -1, __instance.Location);
                }
            }));

            ModEntry.Instance.Monitor.Log($"[MineForMore] Extra hardwood dropped: {extra} from {idKey}", LogLevel.Debug);
        }
    }
}
