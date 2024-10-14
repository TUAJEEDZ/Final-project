using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tilemap plantMap;
    [SerializeField] private Tilemap cutMap;
    [SerializeField] private Tilemap fertilizedMap;
    [SerializeField] private Tilemap treeMap;
    [SerializeField] private Tile hiddenInteractableTile; // select tile to hide
    [SerializeField] private Tile plowedTile;  // select tile to replace hidden tile
    [SerializeField] private Tile wetTile;
    [SerializeField] private Tile fertilizedTile;
    [SerializeField] private Tile[] wheatStages;  // Array to hold wheat growth stages
    [SerializeField] private Tile[] tomatoStages;  // Array to hold tomato growth stages
    [SerializeField] private Tile[] plantableTile;

    private Dictionary<string, int> cropTickRequirements = new Dictionary<string, int>
    {
        { "wheat", 2 },    // Wheat requires 2 ticks per growth stage
        { "tomato", 3 }     // Tomato requires 3 ticks per growth stage
    };

    // Track planted tiles with both type, growth stage, and current tick count
    private Dictionary<Vector3Int, (string cropType, int growthStage, int tickCount)> plantedTiles = new Dictionary<Vector3Int, (string cropType, int growthStage, int tickCount)>();

    // New dictionary to track tree health
    private Dictionary<Vector3Int, int> treeHealth = new Dictionary<Vector3Int, int>();

    // Default tree health value
    private int defaultTreeHealth = 10; // Number of hits required to cut down the tree

    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            if (interactableMap.HasTile(position))
            {
                interactableMap.SetTile(position, hiddenInteractableTile); // set tile to hidden
            }
        }
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, plowedTile);  // Setting the interacted tile
    }

    public void SetFill(Vector3Int position)
    {
        interactableMap.SetTile(position, hiddenInteractableTile);  // Setting the interacted tile
    }

    public void Setfertilized(Vector3Int position)
    {
        fertilizedMap.SetTile(position, fertilizedTile);  // Setting the interacted tile
    }

    public void SetCutted(Vector3Int position)
    {
        treeMap.SetTile(position, hiddenInteractableTile);  // Setting the interacted tile
    }

    public void SetWatered(Vector3Int position)
    {
        interactableMap.SetTile(position, wetTile);  // Set plowedTile to wateredTile
    }

    public void SetPlantWheat(Vector3Int position)
    {
        if (wheatStages.Length > 0) // Ensure there are growth stages defined
        {
            plantMap.SetTile(position, wheatStages[0]); // Set the first stage of wheat
            plantedTiles[position] = ("wheat", 0, 0); // Track crop type, growth stage, and tick count
        }
    }

    public void SetPlantTomato(Vector3Int position)
    {
        if (tomatoStages.Length > 0) // Ensure there are growth stages defined
        {
            plantMap.SetTile(position, tomatoStages[0]); // Set the first stage of tomato
            plantedTiles[position] = ("tomato", 0, 0); // Track crop type, growth stage, and tick count
        }
    }

    public void SetHarvested(Vector3Int position)
    {
        plantMap.SetTile(position, hiddenInteractableTile);  // Setting the interacted tile
        plantedTiles.Remove(position); // Remove from tracking
        fertilizedMap.SetTile(position, hiddenInteractableTile);
    }

    public void SetPickup(Vector3Int position)
    {
        if (plantedTiles.TryGetValue(position, out var cropData))
        {
            string cropType = cropData.cropType;
            int currentStage = cropData.growthStage;

            // Decrease the growth stage by 1 if it's greater than 0
            if (currentStage > 0)
            {
                int newStage = currentStage - 1;

                // Update the tile based on the crop type
               /* if (cropType == "wheat" && wheatStages.Length > 0)
                {
                    plantMap.SetTile(position, wheatStages[newStage]); // Set the previous stage of wheat
                }*/
                if (cropType == "tomato" && tomatoStages.Length > 0)
                {
                    plantMap.SetTile(position, tomatoStages[newStage]); // Set the previous stage of tomato
                }

                // Update the growth stage in the dictionary
                plantedTiles[position] = (cropType, newStage, cropData.tickCount);
            }
            else
            {
                // Optionally, handle the case where the crop is already at stage 0 (no further harvesting possible)
                Debug.Log("Crop is already at the lowest growth stage.");
            }
        }
    }

    // New function to damage the tree
    public void DamageTree(Vector3Int position)
    {
        if (treeHealth.ContainsKey(position))
        {
            treeHealth[position] -= 2; // Decrease tree health by 1

            // Check if the tree health has reached zero
            if (treeHealth[position] <= 0)
            {
                SetCutted(position); // Cut down the tree
                Debug.Log("Tree cut down at " + position);
            }
            else
            {
                Debug.Log("Tree health remaining: " + treeHealth[position]);
            }
        }
        else
        {
            // Initialize the tree health if this is the first interaction
            treeHealth[position] = defaultTreeHealth - 2;
            Debug.Log("Tree health remaining: " + treeHealth[position]);
        }
    }

    public void UpdateGrowthStage(Vector3Int position)
    {
        if (plantedTiles.TryGetValue(position, out var cropData))
        {
            string cropType = cropData.cropType;
            int currentStage = cropData.growthStage;
            int tickCount = cropData.tickCount;

            // Update the tile based on the crop type
            if (cropType == "wheat" && currentStage < wheatStages.Length - 1)
            {
                plantMap.SetTile(position, wheatStages[currentStage + 1]); // Update to the next wheat stage
                plantedTiles[position] = (cropType, currentStage + 1, 0); // Reset tick count and advance stage
            }
            else if (cropType == "tomato" && currentStage < tomatoStages.Length - 1)
            {
                plantMap.SetTile(position, tomatoStages[currentStage + 1]); // Update to the next tomato stage
                plantedTiles[position] = (cropType, currentStage + 1, 0); // Reset tick count and advance stage
            }
            else
            {
                // Optionally, handle fully grown crops
            }
        }
    }

    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }

        return "";
    }

    public bool IsPlantableTile(string tileName)
    {
        foreach (Tile tile in plantableTile)
        {
            if (tile != null && tile.name == tileName)
            {
                return true;
            }
        }
        return false;
    }

    public string GetTileNamePlant(Vector3Int position)
    {
        if (plantMap != null)
        {
            TileBase plant_tile = plantMap.GetTile(position);

            if (plant_tile != null)
            {
                return plant_tile.name;
            }
        }

        return "";
    }

    public string GetTileNamefertilized(Vector3Int position)
    {
        if (fertilizedMap != null)
        {
            TileBase fertilized_tile = fertilizedMap.GetTile(position);

            if (fertilized_tile != null)
            {
                return fertilized_tile.name;
            }
        }

        return "";
    }

    public string GetTileNameTree(Vector3Int position)
    {
        if (cutMap != null)
        {
            TileBase cut_tile = cutMap.GetTile(position);

            if (cut_tile != null)
            {
                return cut_tile.name;
            }
        }
        return "";
    }

    public void CheckPlantGrowth()
    {
        List<Vector3Int> positionsToGrow = new List<Vector3Int>();
        List<Vector3Int> positionsToUpdateTick = new List<Vector3Int>();

        foreach (var position in plantedTiles.Keys) // First, collect positions that need to grow and those that just need a tick increment
        {
            var cropData = plantedTiles[position];
            string cropType = cropData.cropType;
            int tickCount = cropData.tickCount;

            // Get the tile name at the current position
            string tileName = GetTileName(position);

            // Only proceed if the tile is "wetplowed 1"
            if (tileName == "wetplowed 1")
            {
                string isfertilized = GetTileNamefertilized(position);

                if (isfertilized == "fertilizer")
                {
                    tickCount++;
                    tickCount++;// Increment the tick count
                }
                else
                {
                    tickCount++;  // Increment the tick count
                }

                // Check if enough ticks have passed to grow the plant
                if (tickCount >= cropTickRequirements[cropType])
                {
                    positionsToGrow.Add(position);  // Collect positions that should update growth stage
                    SetInteracted(position);
                }
                else
                {
                    // Collect positions to update the tick count
                    positionsToUpdateTick.Add(position);
                    SetInteracted(position);
                }
            }
        }

        // Now, update tick counts for those that are not yet ready to grow
        foreach (var position in positionsToUpdateTick)
        {
            var cropData = plantedTiles[position];
            plantedTiles[position] = (cropData.cropType, cropData.growthStage, cropData.tickCount + 1);
        }

        // Finally, update growth stages for those that reached the tick requirement
        foreach (var position in positionsToGrow)
        {
            UpdateGrowthStage(position);
        }
    }

}
