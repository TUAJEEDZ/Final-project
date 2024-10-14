using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ForestManager : MonoBehaviour
{
    [SerializeField] private Tilemap cutMap;
    [SerializeField] private Tilemap treeMap;
    [SerializeField] private Tile hiddenInteractableTile; // select tile to hide

    // New dictionary to track tree health
    private Dictionary<Vector3Int, int> treeHealth = new Dictionary<Vector3Int, int>();

    // Default tree health value
    private int defaultTreeHealth = 10; // Number of hits required to cut down the tree

    void Start()
    {
       
    }

    public void SetCutted(Vector3Int position)
    {
        treeMap.SetTile(position, hiddenInteractableTile);  // Setting the interacted tile
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

   
}
