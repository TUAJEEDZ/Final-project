using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tilemap plantMap;
    [SerializeField] private Tilemap cutMap;
    [SerializeField] private Tilemap treeMap;
    [SerializeField] private Tile hiddenInteractableTile; // select tile to hide
    [SerializeField] private Tile plowedTile;  // select tile to replace hidden tile
    [SerializeField] private Tile wetTile;
    [SerializeField] private Tile[] wheatStages;  // Array to hold wheat growth stages
    [SerializeField] private Tile[] tomatoStages;  // Array to hold tomato growth stages
    [SerializeField] private Tile[] plantableTile;

    // Track planted tiles with both type and growth stage
    private Dictionary<Vector3Int, (string cropType, int growthStage)> plantedTiles = new Dictionary<Vector3Int, (string cropType, int growthStage)>();

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
            plantedTiles[position] = ("wheat", 0); // Track crop type and growth stage
        }
    }

    public void SetPlantTomato(Vector3Int position)
    {
        if (tomatoStages.Length > 0) // Ensure there are growth stages defined
        {
            plantMap.SetTile(position, tomatoStages[0]); // Set the first stage of tomato
            plantedTiles[position] = ("tomato", 0); // Track crop type and growth stage
        }
    }

    public void SetHarvested(Vector3Int position)
    {
        plantMap.SetTile(position, hiddenInteractableTile);  // Setting the interacted tile
        plantedTiles.Remove(position); // Remove from tracking
    }

    public void UpdateGrowthStage(Vector3Int position)
    {
        if (plantedTiles.TryGetValue(position, out var cropData))
        {
            string cropType = cropData.cropType;
            int currentStage = cropData.growthStage;
            currentStage++;

            // Update the tile based on the crop type
            if (cropType == "wheat" && currentStage < wheatStages.Length)
            {
                plantMap.SetTile(position, wheatStages[currentStage]); // Update to the next wheat stage
                plantedTiles[position] = (cropType, currentStage); // Update tracking
            }
            else if (cropType == "tomato" && currentStage < tomatoStages.Length)
            {
                plantMap.SetTile(position, tomatoStages[currentStage]); // Update to the next tomato stage
                plantedTiles[position] = (cropType, currentStage); // Update tracking
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
        foreach (var position in plantMap.cellBounds.allPositionsWithin)
        {
            string tileName = GetTileName(position);

            if (!string.IsNullOrWhiteSpace(tileName))
            {
                if (tileName == "wetplowed 1")
                {
                    if (plantMap.HasTile(position))
                    {
                        if (plantedTiles.ContainsKey(position))
                        {
                            UpdateGrowthStage(position); // Attempt to update the growth stage
                            SetInteracted(position);
                        }
                    }
                }
            }
        }
    }
}
