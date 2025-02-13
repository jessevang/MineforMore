using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineForMore.Classes
{
    internal class Recipe
    {
        public static ModEntry Instance { get; private set; }


        // Constructor to receive SMAPI's Helper and Monitor
        public Recipe(ModEntry Instance2)
        {
            Instance = Instance2;
        }


        public void AddQuickSmeltRecipes()
        {

            string recipeKey = "QuickSmelt (Cu)"; // Unique key for the recipe
            string recipeValue = "378 10 382 2/Home/334/false/Mining 10/Quicksmelt (Cu)";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "QuickSmelt (Fe)"; // Unique key for the recipe
            recipeValue = "380 10 382 2/Home/335/false/Mining 10/Quicksmelt (Fe)";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "QuickSmelt (Au)"; // Unique key for the recipe
            recipeValue = "384 10 382 2/Home/336/false/Mining 10/Quicksmelt (Au)";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "QuickSmelt (Ir)"; // Unique key for the recipe
            recipeValue = "386 10 382 2/Home/337/false/Mining 10/Quicksmelt (Ir)";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "QuickSmelt (Ra)"; // Unique key for the recipe
            recipeValue = "909 10 382 2/Home/910/false/Mining 10/Quicksmelt (Ra)";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            
        }

        

        public void AddGemologistRecipes()
        {
            /*Full format: CreateNewRingID/imgAssetName/SellPrice/Name/Price/Edibility/Type/Category/DisplayName/Description/ItemType/Extra1/Extra2/Extra3/Extra4
            * 10000,Amethyst Ring,1000,-300,Ring,A mysterious ring that radiates dark energy.,ring, asset/Amethyst Ring.png
               strObj = "Shadow Ring/1500/-300/Ring/-98/Shadow Ring/A mysterious ring that radiates dark energy./ring"  */
            //Ring ring = new Ring(Instance);
            //ring.createUsingGameIcon(10000, 888, "Perfect Glowstone Ring/5000/-300/Ring/-98/Perfect Glowstone Ring/Triple the Light of a Glowstone Ring/ring/0"); 


            //Unlocks Jewelry Rings
            String recipeKey = "Amethyst Rings";
            String recipeValue = "66 1/Home/529/true/Mining 10/";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);
            //Instance.Monitor.Log("Perfect Amethyst Rings Recipe added!", LogLevel.Info);

            recipeKey = "Ruby Rings";
            recipeValue = "64 1/Home/534/true/Mining 10/";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "Topaz Ring";
            recipeValue = "68 1/Home/530/true/Mining 9/";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "Aquamarine Ring";
            recipeValue = "62 1/Home/631/true/Mining 9/";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "Jade Ring";
            recipeValue = "70 1/Home/532/true/Mining 9/";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);

            recipeKey = "Emerald Ring";
            recipeValue = "60 1/Home/533/true/Mining 9/";
            CraftingRecipe.craftingRecipes.Add(recipeKey, recipeValue);


        }
    }
}
