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
    [SerializeField] private Tile hiddenInteractableTile; //select tile to hide
    [SerializeField] private Tile plowedTile;  // select tile to replace hiddentile
    [SerializeField] private Tile wetTile;
    [SerializeField] private Tile[] wheatStages;  // Array to hold wheat growth stages
    [SerializeField] private Tile[] plantableTile;

    private Dictionary<Vector3Int, int> plantedTiles = new Dictionary<Vector3Int, int>(); // Track growth stages

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
            plantedTiles[position] = 0; // Track growth stage
        }
    }

    public void SetHavested(Vector3Int position)
    {
        plantMap.SetTile(position, hiddenInteractableTile);  // Setting the interacted tile
        plantedTiles.Remove(position); // Remove from tracking
    }

    public void UpdateGrowthStage(Vector3Int position)
    {
        if (plantedTiles.TryGetValue(position, out int currentStage))
        {
            currentStage++;
            if (currentStage < wheatStages.Length) // Check if within the bounds of stages
            {
                plantMap.SetTile(position, wheatStages[currentStage]); // Update to the next growth stage
                plantedTiles[position] = currentStage; // Update growth stage tracking
            }
            else
            {
                // Optionally, handle the case where the crop is fully grown (harvestable)
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
