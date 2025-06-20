using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using MineForMore;
using StardewValley.Menus;

internal class UnlimitedSkillLevel
{
    private readonly Config _config;

    public UnlimitedSkillLevel(Config config)
    {
        _config = config;
    }

    public void Apply(Harmony harmony, IMonitor monitor)
    {
        var method = AccessTools.Method(typeof(Farmer), nameof(Farmer.gainExperience));
        if (method == null)
        {
            monitor.Log("Failed to find Farmer.gainExperience for patching.", LogLevel.Error);
            return;
        }

        harmony.Patch(
            original: method,
            prefix: new HarmonyMethod(typeof(UnlimitedSkillLevel), nameof(ModifiedGainExperience))
        );
    }

    public static bool ModifiedGainExperience(Farmer __instance, int which, int howMuch)
    {
        if (which == 5 || howMuch <= 0)
            return false;

        if (!ModEntry.Instance.Config.AllowPlayerToExceedLevel10 && __instance.GetSkillLevel(which) >= 10)
            return true; // Let vanilla logic run instead which will cap skill levels at 10

        if (!__instance.IsLocalPlayer && Game1.IsServer)
        {
            __instance.queueMessage(17, __instance, which, howMuch);
            return false;
        }

        if (__instance.Level >= 25)
        {
            int old = MasteryTrackerMenu.getCurrentMasteryLevel();
            Game1.stats.Increment("MasteryExp", Math.Max(1, which == 0 ? (howMuch / 2) : howMuch));

            if (MasteryTrackerMenu.getCurrentMasteryLevel() > old)
            {
                Game1.showGlobalMessage(Game1.content.LoadString("Strings\\1_6_Strings:Mastery_newlevel"));
                Game1.playSound("newArtifact", null);
            }
        }

        int oldXP = __instance.experiencePoints[which];
        int newXP = oldXP + howMuch;
        int newLevel = ModifiedcheckForLevelGain(oldXP, newXP);

        if (!ModEntry.Instance.Config.AllowPlayerToExceedLevel10 && newLevel > 10)
            return false;

        __instance.experiencePoints[which] = newXP;

        int oldLevel = which switch
        {
            0 => __instance.farmingLevel.Value,
            1 => __instance.fishingLevel.Value,
            2 => __instance.foragingLevel.Value,
            3 => __instance.miningLevel.Value,
            4 => __instance.combatLevel.Value,
            5 => __instance.luckLevel.Value,
            _ => -1
        };

        if (newLevel <= oldLevel)
            return false;

        switch (which)
        {
            case 0: __instance.farmingLevel.Value = newLevel; break;
            case 1: __instance.fishingLevel.Value = newLevel; break;
            case 2: __instance.foragingLevel.Value = newLevel; break;
            case 3: __instance.miningLevel.Value = newLevel; break;
            case 4: __instance.combatLevel.Value = newLevel; break;
        }

        for (int i = oldLevel + 1; i <= newLevel; i++)
        {
            __instance.newLevels.Add(new Point(which, i));
            if (__instance.newLevels.Count == 1)
            {
                Game1.showGlobalMessage(Game1.content.LoadString("Strings\\1_6_Strings:NewIdeas"));
            }
        }

        return false;
    }

    public static int ModifiedgetBaseExperienceForLevel(int level)
    {
        if (level <= 10)
        {
            return level switch
            {
                1 => 100,
                2 => 380,
                3 => 770,
                4 => 1300,
                5 => 2150,
                6 => 3300,
                7 => 4800,
                8 => 6900,
                9 => 10000,
                10 => 15000,
                _ => -1,
            };
        }
        else
        {
            return 15000 + ((level - 10) * (1000 + (level * 100)));

            /*Level	XP required at level (increment)
            Level	Calculation	                                        XP Required
            11	    15000 + (1 × (1000 + 11×100)) = 15000 + 1×2100	    17,100
            12	    15000 + (2 × (1000 + 12×100)) = 15000 + 2×2200	    19,400
            13	    15000 + (3 × (1000 + 13×100)) = 15000 + 3×2300	    21,900
            14	    15000 + (4 × (1000 + 14×100)) = 15000 + 4×2400	    24,600
            15	    15000 + (5 × (1000 + 15×100)) = 15000 + 5×2500	    27,500
            16	    15000 + (6 × (1000 + 16×100)) = 15000 + 6×2600	    30,600
            17	    15000 + (7 × (1000 + 17×100)) = 15000 + 7×2700	    33,900
            18	    15000 + (8 × (1000 + 18×100)) = 15000 + 8×2800	    37,400
            19	    15000 + (9 × (1000 + 19×100)) = 15000 + 9×2900	    41,100
            20	    15000 + (10× (1000 + 20×100)) = 15000 + 10×3000	    45,000 (about twice as much exp as level 10(15,000xp)
            
            At level 50 you'll need   141,000xp
            At level 75 you'll need   247,500xp
            At level 100 you'll need  375,000xp
             
             */
        }
    }

    public static int ModifiedcheckForLevelGain(int oldXP, int newXP)
    {
        for (int level = 25; level >= 1; level--)
        {
            int required = ModifiedgetBaseExperienceForLevel(level);
            if (oldXP < required && newXP >= required)
                return level;
        }

        return -1;
    }
}
