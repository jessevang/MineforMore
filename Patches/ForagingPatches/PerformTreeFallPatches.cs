using MineForMore;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using StardewValley;
using Microsoft.Xna.Framework;
using HarmonyLib;
using StardewModdingAPI;
using System.Linq;
using System.Xml.Schema;

namespace MineForMore.Patches.ForagingPatches
{
    internal class PerformTreeFallPatch
    {
        public void Apply(Harmony harmony, IMonitor monitor)
        {
            var orig = AccessTools.Method(typeof(Tree), "performTreeFall", new[] { typeof(Tool), typeof(int), typeof(Vector2) });
            if (orig is null)
            {
                monitor.Log("Failed to find Tree.performTreeFall", LogLevel.Error);
                return;
            }

            var postfix = new HarmonyMethod(typeof(PerformTreeFallPatch), nameof(Postfix));
            harmony.Patch(original: orig, postfix: postfix);
        }

        public static void Postfix(Tree __instance, Tool t, int explosion, Vector2 tileLocation)
        {
            if (t is not Axe axe)
                return;

            var who = axe.getLastFarmerToUse();
            if (who is null || !Context.IsMainPlayer)
                return;

            string key = __instance.treeType.Value switch
            {
                "1" => "TreeOak",
                "2" => "TreeMaple",
                "3" => "TreePine",
                "7" => "TreeMushroom",
                "8" => "TreeMahogany",
                _ => null
            };

            if (key == null)
                return;

            var rules = ModEntry.Instance.GetAllRules()
                .Where(r => r.SkillType == "Foraging" && r.DropsFromObjectIDs.Contains(key));

            int currentForagingLevel = who.GetSkillLevel(Farmer.foragingSkill);

            foreach (var rule in rules)
            {

                if (rule.Type != "Wood" && rule.Type != "Hardwood" && rule.Type != "Seed")
                {
                    continue;
                }


                int flat = 0;
                int scaled = 0;
                int total = 0;
                if (rule.Type == "Wood" || rule.Type == "Hardwood")
                {
                    flat = rule.AddAmount;
                    scaled = who.professions.Contains(12) ? (int)Math.Floor(ModEntry.Instance.Config.ForesterWoodPerLevelBonus * currentForagingLevel) : 0;

                }
                else if (rule.Type == "Seed")
                {
                    flat = rule.AddAmount;
                    scaled = who.professions.Contains(12) ? (int)Math.Floor(ModEntry.Instance.Config.ForesterSeedPerLevelBonus * currentForagingLevel) : 0;

                }
                total = (int)((flat + scaled) * rule.Multiplier);
                
                if (total <= 0)
                    continue;



                Vector2 dropTile = tileLocation;

                if (__instance.falling.Value)
                {
                    int dir = __instance.shakeLeft.Value ? -1 : 1;
                    dropTile += new Vector2(dir * 4, 0);

                    Vector2 spawn = (dropTile * 64f) + new Vector2(
                        Game1.random.Next(-24, 25),
                        Game1.random.Next(-24, 25)
                    );
                    Game1.delayedActions.Add(new DelayedAction(1800, () =>
                    {
                        for (int i = 0; i < total; i++)
                        {
                            Item item = ItemRegistry.Create(rule.ObjectID);
                            Game1.createItemDebris(item, spawn, -1, __instance.Location);

                        }
                        if ((rule.Type == "Wood" || rule.Type == "Hardwood"))
                        {
                            TryDropLumberjackHardwood(who, tileLocation, __instance);
                        }
                        

                    }));
                }
                else if (!__instance.falling.Value && (rule.Type == "Wood" || rule.Type == "Hardwood"))
                {
                    for (int i = 0; i < total; i++)
                    {
                        Vector2 StompSpawn = (dropTile * 64f) + new Vector2(
                            Game1.random.Next(-24, 25),
                            Game1.random.Next(-24, 25)
                        );

                        Item item = ItemRegistry.Create(rule.ObjectID);
                        Game1.createItemDebris(item, StompSpawn, -1, __instance.Location);

                    }

                    TryDropLumberjackHardwood(who, tileLocation, __instance);
                }
            
           
            
            
            }


        }


        //Used if user has the Foraging Profession LumberJack to add drops on Tree Fall
        private static void TryDropLumberjackHardwood(Farmer who, Vector2 tileLocation, Tree tree)
        {
            if (!who.professions.Contains(14)) // Lumberjack
                return;

            int foragingLevel = who.GetSkillLevel(Farmer.foragingSkill);
            float chance = ModEntry.Instance.Config.LumberjackHardwordDropChancePerLevelBonus * foragingLevel;

            if (Game1.random.NextDouble() >= chance)
                return;

           int total = (int)(foragingLevel * ModEntry.Instance.Config.LumberjackHardwoodPerLevelBonus);

            for (int i = 0; i < total; i++)
            {
                Vector2 dropTile = tileLocation;
                if (tree.falling.Value)
                {
                    int dir = tree.shakeLeft.Value ? -1 : 1;
                    dropTile += new Vector2(dir * 4, 0);
                }

                Vector2 spawn = (dropTile * 64f) + new Vector2(
                    Game1.random.Next(-24, 25),
                    Game1.random.Next(-24, 25)
                );

                Item item = ItemRegistry.Create("(O)709"); // Hardwood
                Game1.createItemDebris(item, spawn, -1, tree.Location);
            }
        }

    }


}
