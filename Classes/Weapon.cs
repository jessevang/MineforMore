using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineForMore.Classes
{
    internal class Weapon
    {


        //Name/MinDamage/MaxDamage/Knockback/Speed/Defense/Precision/CritChance/CritMultiplier/MineDropVar/MineDropMinLevel/MineDropMaxLevel/Type/Description
        //Shadow Blade/30/45/1.5/4/0/0/0.02/3/0/-1/-1/Sword/An ancient sword imbued with dark energy.

        public void createNewWeapon(int newWeaponID, string weaponData)
        {
            if (!Context.IsMainPlayer) return;

            var weaponsData = Game1.content.Load<Dictionary<int, string>>("Data/Weapons");
            newWeaponID = 11000; // Custom weapon ID

            // Add the weapon if it doesn't exist
            if (!weaponsData.ContainsKey(newWeaponID))
            {

                weaponsData.Add(newWeaponID, weaponData);
            }

            // Add the crafting recipe
            string recipeKey = "Shadow Blade";
            string recipeValue = $"{newWeaponID} 1 335 10 336 5 337 2/Home/21/true/Combat 5/";
            if (!CraftingRecipe.craftingRecipes.ContainsKey(recipeKey))
            {
                CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            }
        }
    }
}
