using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System;
using LevelForMore;
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
        // Patch Farmer.gainExperience (prefix) — your existing behavior
        var gainExp = AccessTools.Method(typeof(Farmer), nameof(Farmer.gainExperience));
        if (gainExp == null)
        {
            monitor.Log("Failed to find Farmer.gainExperience for patching.", LogLevel.Error);
            return;
        }
        harmony.Patch(
            original: gainExp,
            prefix: new HarmonyMethod(typeof(UnlimitedSkillLevel), nameof(ModifiedGainExperience))
        );

        // NEW: Patch Farmer.getBaseExperienceForLevel (postfix) to reflect our thresholds globally
        var getBaseForLevel = AccessTools.Method(typeof(Farmer), nameof(Farmer.getBaseExperienceForLevel), new[] { typeof(int) });
        if (getBaseForLevel == null)
        {
            monitor.Log("Failed to find Farmer.getBaseExperienceForLevel for patching.", LogLevel.Error);
            return;
        }
        harmony.Patch(
            original: getBaseForLevel,
            postfix: new HarmonyMethod(typeof(UnlimitedSkillLevel), nameof(GetBaseExperienceForLevel_Postfix))
        );
    }

    // Prefix: replaces vanilla gainExperience logic where applicable (unchanged from your version)
    public static bool ModifiedGainExperience(Farmer __instance, int which, int howMuch)
    {
        if (which == 5 || howMuch <= 0)
            return false;

        if (!ModEntry.Instance.Config.AllowPlayerToExceedLevel10 && __instance.GetSkillLevel(which) >= 10)
            return true; // Let vanilla logic run (cap at 10)

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

    // NEW: Postfix to replace Farmer.getBaseExperienceForLevel's return value with our thresholds when allowed.
    // Signature must match: static method, (int level, ref int __result)
    public static void GetBaseExperienceForLevel_Postfix(int level, ref int __result)
    {
        // If user does NOT allow >10 levels and level > 10, keep vanilla result (usually -1).
        if (!ModEntry.Instance.Config.AllowPlayerToExceedLevel10 && level > 10)
            return;

        int custom = ModifiedgetBaseExperienceForLevel(level);

        // Only override if our function has a defined threshold
        if (custom >= 0)
            __result = custom;
        // else leave vanilla's __result as-is (for unexpected negative levels, etc.)
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
        else if (level <= 1000)
        {
            // Custom cumulative threshold beyond 10
            return 15000 + ((level - 10) * (1000 + (level * 100)));

            /*
            Examples:
             11 -> 17100
             12 -> 19400
             13 -> 21900
             14 -> 24600
             15 -> 27500
             16 -> 30600
             17 -> 33900
             18 -> 37400
             19 -> 41100
             20 -> 45000
            */
        }
        else
        {
            return -1;
        }
    }

    public static int ModifiedcheckForLevelGain(int oldXP, int newXP)
    {
        for (int level = 1000; level >= 1; level--)
        {
            int required = ModifiedgetBaseExperienceForLevel(level);
            if (required < 0)
                continue;

            if (oldXP < required && newXP >= required)
                return level;
        }

        return -1;
    }
}
