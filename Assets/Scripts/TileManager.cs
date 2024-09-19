using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tilemap plantMap;
    [SerializeField] private Tile hiddenInteractableTile; //select tile to hide
    [SerializeField] private Tile plowedTile;  // select tile to replace hiddentile
    [SerializeField] private Tile wetTile;
    [SerializeField] private Tile wheat;
    [SerializeField] private Tile[] plantableTile;


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

    public void SetWatered(Vector3Int position)
    {
        interactableMap.SetTile(position, wetTile);  // Set plowedTile to wateredTile
    }
            
            public void SetPlantWheat(Vector3Int position)
            {
                plantMap.SetTile(position, wheat);  //plantwheat
            }

    public void SetHavested(Vector3Int position)
    {
        plantMap.SetTile(position, hiddenInteractableTile);  // Setting the interacted tile
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
}

