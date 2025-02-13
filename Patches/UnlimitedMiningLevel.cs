
using StardewModdingAPI;
using StardewValley;
using HarmonyLib;
using Spacechase.Shared.Patching;
using MineForMore;
using StardewValley.Menus;
using Microsoft.Xna.Framework;



/// <summary>Applies Harmony patches to <see cref="GameLocation"/>.</summary>

internal class UnlimitedMiningLevel : BasePatcher
{

    private readonly Config _config;

    public UnlimitedMiningLevel(Config config)
    {
        _config = config;
    }

    public override void Apply(Harmony harmony, IMonitor monitor)
    {
        

        harmony.Patch(
            original: this.RequireMethod<Farmer>(
            nameof(Farmer.gainExperience) // Correct parameter types
        ),
        prefix: this.GetHarmonyMethod(nameof(ModifiedGainExperience)) // prefix method


         );


    }



    /*Summary of What This Code Does
    *Handles experience gain for skills (except luck, which is ignored).
    *Checks if the player is on the server and queues experience messages for clients.
    *Implements a special "Mastery Level" system once the player reaches level 25.
    *Calculates level-up based on experience thresholds.
    *Applies level-ups properly by modifying the correct skill variable.
    *Ensures the player sees level-up notifications when appropriate.

     */
    public static void ModifiedGainExperience(int which, int howMuch)
    {
        // If gaining experience in luck (which == 5) or amount is zero or negative, exit early.
        if (which == 5 || howMuch <= 0)
        {
            return;
        }

        // If the player is not the local player and the game is running as the server, queue the experience gain message for the player.
        if (!Game1.player.IsLocalPlayer && Game1.IsServer)
        {
            Game1.player.queueMessage(17, Game1.player, which, howMuch);
            return;
        }

        // If the player's level is 25 or higher, apply Mastery experience instead.
        if (Game1.player.Level >= 25)
        {
            int old = MasteryTrackerMenu.getCurrentMasteryLevel();

            // Increment Mastery experience (halved for farming, full for others).
            Game1.stats.Increment("MasteryExp", Math.Max(1, (which == 0) ? (howMuch / 2) : howMuch));

            // If the player has leveled up in Mastery, display a notification.
            if (MasteryTrackerMenu.getCurrentMasteryLevel() > old)
            {
                Game1.showGlobalMessage(Game1.content.LoadString("Strings\\1_6_Strings:Mastery_newlevel"));
                Game1.playSound("newArtifact", null);
            }
        }

        // Determine if the experience gained is enough to level up.
        int newLevel = ModifiedcheckForLevelGain(Game1.player.experiencePoints[which], Game1.player.experiencePoints[which] + howMuch);

        // Add the experience points to the player's total.
        Game1.player.experiencePoints[which] += howMuch;

        int oldLevel = -1; // Store the player's current level before applying changes.

        // If `newLevel` is valid (i.e., the player leveled up), update the player's skill level.
        if (newLevel != -1)
        {
            switch (which)
            {
                case 0: // Farming
                    oldLevel = Game1.player.farmingLevel.Value;
                    Game1.player.farmingLevel.Value = newLevel;
                    break;
                case 3: // Mining
                    oldLevel = Game1.player.miningLevel.Value;
                    Game1.player.miningLevel.Value = newLevel;
                    break;
                case 1: // Fishing
                    oldLevel = Game1.player.fishingLevel.Value;
                    Game1.player.fishingLevel.Value = newLevel;
                    break;
                case 2: // Foraging
                    oldLevel = Game1.player.foragingLevel.Value;
                    Game1.player.foragingLevel.Value = newLevel;
                    break;
                case 5: // Luck (Shouldn't reach here because of the early return)
                    oldLevel = Game1.player.luckLevel.Value;
                    Game1.player.luckLevel.Value = newLevel;
                    break;
                case 4: // Combat
                    oldLevel = Game1.player.combatLevel.Value;
                    Game1.player.combatLevel.Value = newLevel;
                    break;
            }
        }

        // If the new level is not higher than the old level, exit early.
        if (newLevel <= oldLevel)
        {
            return;
        }

        // Add each new level gained to the `newLevels` list to trigger the level-up menu.
        for (int i = oldLevel + 1; i <= newLevel; i++)
        {
            Game1.player.newLevels.Add(new Point(which, i));

            // If this is the first new level gained, display a notification about new crafting ideas.
            if (Game1.player.newLevels.Count == 1)
            {
                Game1.showGlobalMessage(Game1.content.LoadString("Strings\\1_6_Strings:NewIdeas"));
            }
        }
    }



    //not a harmony patch
    public static int ModifiedgetBaseExperienceForLevel(int level)
    {

        if (level <=10)
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
            return 15000 + ((level-10) * (5000 + (level * 200)));  //Each level exp increases by 5000 + additional 200 per each level after 10
        }
       

    }

    //not for harmony patch
    public static int ModifiedcheckForLevelGain(int oldXP, int newXP)
    {
        for (int level = 21; level >= 1; level--)
        {
            if (oldXP < ModifiedgetBaseExperienceForLevel(level) && newXP >= ModifiedgetBaseExperienceForLevel(level))
            {

                return level;
            }
        }

        return -1;
    }




}