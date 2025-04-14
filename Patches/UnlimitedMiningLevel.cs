using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using MineForMore;
using StardewValley.Menus;

internal class UnlimitedMiningLevel
{
    private readonly Config _config;

    public UnlimitedMiningLevel(Config config)
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
            prefix: new HarmonyMethod(typeof(UnlimitedMiningLevel), nameof(ModifiedGainExperience))
        );
    }

    public static bool ModifiedGainExperience(Farmer __instance, int which, int howMuch)
    {
        if (which == 5 || howMuch <= 0)
            return false;

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
        __instance.experiencePoints[which] = newXP;

        int oldLevel = which switch
        {
            0 => __instance.farmingLevel.Value,
            1 => __instance.fishingLevel.Value,
            2 => __instance.foragingLevel.Value,
            3 => __instance.miningLevel.Value,
            4 => __instance.combatLevel.Value,
            5 => __instance.luckLevel.Value, // should not happen
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

        return false; // Prevent original method from running
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
            return 15000 + ((level - 10) * (5000 + (level * 200)));
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
