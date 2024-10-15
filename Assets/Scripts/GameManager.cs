using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;
    public MapManager mapManager;
    public TileManager tileManager;
    public PlantManager plantManager;
    public UI_Manager uiManager;
    public Player player;
    public DayNightCycle dayNightCycle;
    public SceneTransitionManager sceneTransitionManager; // Reference to SceneTransitionManager
    public Stamina stamina;

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
        plantManager = GetComponent<PlantManager>();
        sceneTransitionManager = GetComponent<SceneTransitionManager>(); // Initialize SceneTransitionManager
        stamina = GetComponent<Stamina>(); // Get Stamina component


        // Find the Player in the current scene, and ensure it persists
        if (player == null)
        {
            player = FindObjectOfType<Player>();
            if (player != null)
            {
                DontDestroyOnLoad(player.gameObject); // Ensure Player persists across scenes
            }
        }
        // Subscribe to the sceneLoaded event to detect scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when the GameManager is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Method called whenever a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentSceneName = scene.name;
        Debug.Log("Current Scene: " + currentSceneName);
    }
}
