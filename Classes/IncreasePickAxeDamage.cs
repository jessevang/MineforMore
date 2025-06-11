using StardewValley.Tools;
using StardewValley;

namespace MineForMore.Classes
{
    internal class IncreasePickAxeDamage
    {

        double IncreasePickAxeDamagePerMiningLevel = 0.0;

        public IncreasePickAxeDamage(double _increasePickDamagePerMiningLevel)
        {
            IncreasePickAxeDamagePerMiningLevel = _increasePickDamagePerMiningLevel;
        }

        public void ApplyIncreasedDamage()
        {

            // Check if the player is using the pickaxe
            if (Game1.player.CurrentTool is Pickaxe pickaxe)
            {
                // Get the player's Mining level

                int bonusDamage = (int)(Game1.player.MiningLevel * IncreasePickAxeDamagePerMiningLevel);


                // Set base damage based on pickaxe upgrade level
                int baseDamage = GetBaseDamageForPickaxe(pickaxe);


                //Need to add food buffs
                pickaxe.additionalPower.Set(baseDamage + bonusDamage);
            }
        }

        private int GetBaseDamageForPickaxe(Pickaxe pickaxe)
        {
            // Determine base damage based on pickaxe upgrade level
            switch (pickaxe.UpgradeLevel)
            {
                case 0: // Basic pickaxe
                    return 1;
                case 1: // Copper pickaxe
                    return 2;
                case 2: // Steel pickaxe
                    return 3;
                case 3: // Gold pickaxe
                    return 4;
                case 4: // Iridium pickaxe
                    return 5;
                default:
                    return 1; // Fallback for unknown upgrade levels
            }
        }
    }
}
