using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;
    public MapManager mapManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public Player player;
    public InventoryManager inventoryManager; // Added InventoryManager reference

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Ensure the GameManager persists across scenes
        }

        // Initialize the managers
        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UI_Manager>();
        mapManager = GetComponent<MapManager>();
        inventoryManager = GetComponent<InventoryManager>(); // Initialize InventoryManager

        // Find the Player in the current scene, and ensure it persists
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player != null)
            {
                DontDestroyOnLoad(player.gameObject); // Ensure Player persists across scenes
            }
        }
    }

    public InventoryManager GetInventoryManager() // Optional getter method
    {
        return inventoryManager;
    }
}
