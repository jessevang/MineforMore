using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineForMore.Classes
{
    internal class Ring
    {
        public static ModEntry Instance { get; private set; }


        // Constructor to receive SMAPI's Helper and Monitor
        public Ring(ModEntry Instance2)
        {
            Instance = Instance2;
        }



        /*Full format: CreateNewRingID/imgAssetName/SellPrice/Name/Price/Edibility/Type/Category/DisplayName/Description/ItemType/Extra1/Extra2/Extra3/Extra4
 * 10000,Amethyst Ring,1000,-300,Ring,A mysterious ring that radiates dark energy.,ring, asset/Amethyst Ring.png
    ringData = "Shadow Ring/1500/-300/Ring/-98/Shadow Ring/A mysterious ring that radiates dark energy./ring"
 */
        // Full format: Name/Price/Edibility/Type/Category/DisplayName/Description/ItemType/Extra1/Extra2/Extra3/Extra4
        // Example: "Shadow Ring/2000/-300/Ring/-98/Shadow Ring/A mysterious ring that radiates dark energy./ring"
        //
        // Explanation:
        // Name        - Internal name of the ring
        // Price       - The shop price (sell price = Price / 2)
        // Edibility   - -300 (for non-edible items like rings)
        // Type        - "Ring" (must be exactly this for rings)
        // Category    - -98 (category ID for rings)
        // DisplayName - The in-game name that appears in menus
        // Description - The in-game description text
        // ItemType    - "ring" (must be exactly this for rings)
        // Extra1-4    - Not used for rings, leave empty

        public void create(int newRingID, string imgAssetName, string ringData)
        {

            // Get the existing ObjectInformation data (where rings are stored)
            var objectData = Instance.Helper.GameContent.Load<Dictionary<int, string>>("Data/ObjectInformation");


            // Check if the ring already exists
            if (!objectData.ContainsKey(newRingID))
            {
                // Full format: Name/BuyPrice/Edibility/Type/Category/DisplayName/Description/ItemType/Extra1/Extra2/Extra3/Extra4
                //"Amethyst Ring/1000/-300/Ring/A mysterious ring that radiates dark energy./ring";
                objectData.Add(newRingID, ringData);

            }

            // Assign the texture to the new ring (using SMAPI's reflection helper)
            if (Game1.objectSpriteSheet != null)
            {
                
               Game1.objectSpriteSheet = Instance.Helper.GameContent.Load<Texture2D>("assets/" + imgAssetName);
            }

            // Assign the texture to the new ring (using the existing ring texture)
            string textureName = "Ruby Ring"; // Replace with the appropriate ring texture name
            var ringTexture = Game1.content.Load<Texture2D>("Objects/" + textureName);

        }

        public void createUsingGameIcon(int newRingID, int existingRingID, string ringData)
        {
            try
            {
                var objectData = Instance.Helper.GameContent.Load<Dictionary<int, string>>("Data/ObjectInformation");

                if (!objectData.ContainsKey(newRingID))
                {
                    objectData.Add(newRingID, ringData);
                }

                if (objectData.TryGetValue(existingRingID, out string existingRingData))
                {
                    string[] existingRingParts = existingRingData.Split('/');

                    // Debug: Log what we're working with
                    Instance.Monitor.Log($"Existing Ring Data: {existingRingData}", LogLevel.Debug);

                    if (existingRingParts.Length > 7)
                    {
                        string existingRingSpriteIndex = existingRingParts[7];

                        Instance.Monitor.Log($"Extracted Sprite Index: {existingRingSpriteIndex}", LogLevel.Debug);

                        string[] newRingParts = ringData.Split('/');
                        if (newRingParts.Length > 7)
                        {
                            newRingParts[7] = existingRingSpriteIndex; // Set the sprite index
                            ringData = string.Join("/", newRingParts);
                            objectData[newRingID] = ringData;

                            Instance.Monitor.Log($"New Ring Data after update: {ringData}", LogLevel.Debug);
                        }
                    }
                    else
                    {
                        Instance.Monitor.Log("ERROR: Existing ring data is missing the sprite index!", LogLevel.Error);
                    }
                }
                else
                {
                    Instance.Monitor.Log($"ERROR: Could not find existing ring with ID {existingRingID}", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Instance.Monitor.Log($"Error in createUsingGameIcon: {ex.Message}", LogLevel.Error);
            }
        }




    }
}
