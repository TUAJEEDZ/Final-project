using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public int sceneBuildIndex; // Index of the scene to transition to
    public Text interactionText; // UI Text for interaction
    public Image interactionImage; // UI Image for interaction
    private bool playerInTrigger = false;

    private MapManager mapManager;

    // Position to exit the door
    public Vector3 doorExitPosition;

    // Number of ticks to wait before using the door again
    public int waitTicks = 3;

    private void Start()
    {
        // Hide interaction UI on start
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;

            // Show interaction UI when player is near
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
            }
            if (interactionImage != null)
            {
                interactionImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;

            // Hide interaction UI when player leaves
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false);
            }
            if (interactionImage != null)
            {
                interactionImage.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            // Check if the player can use the door
            if (CanUseDoor())
            {
                // Save the door position when exiting
                PlayerPrefs.SetFloat("DoorPositionX", doorExitPosition.x);
                PlayerPrefs.SetFloat("DoorPositionY", doorExitPosition.y);
                PlayerPrefs.SetFloat("DoorPositionZ", doorExitPosition.z);

                // Load the new scene
                GameManager.instance.sceneTransitionManager.LoadSceneByIndex(sceneBuildIndex);

                // Handle farm state based on the current scene
                string currentSceneName = GameManager.instance.sceneTransitionManager.GetActiveSceneName();
                if (currentSceneName == "main")
                {
                    GameManager.instance.mapManager.SetFarmOn(false);
                }
                else if (currentSceneName == "Dungeon")
                {
                    GameManager.instance.mapManager.SetFarmOn(true);
                    GameManager.instance.tickmanager.currentTick = 0;
                }
                else if (currentSceneName == "SeedShop" || currentSceneName == "Sell" || currentSceneName == "ToolsShop")
                {
                    GameManager.instance.mapManager.SetFarmOn(true);
                }

                // Update the last used tick for the door
                PlayerPrefs.SetInt("LastUsedTick", GameManager.instance.tickmanager.GetCurrentTick());
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("You must wait before using this door again.");
            }
        }
    }

    private bool CanUseDoor()
    {
        //int lastUsedTick = PlayerPrefs.GetInt("LastUsedTick", 0);
        int currentTick = GameManager.instance.tickmanager.GetCurrentTick();

        // Check if the required number of ticks have passed
        return currentTick >= waitTicks;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if there is a saved door position
        if (PlayerPrefs.HasKey("DoorPositionX"))
        {
            float x = PlayerPrefs.GetFloat("DoorPositionX");
            float y = PlayerPrefs.GetFloat("DoorPositionY");
            float z = PlayerPrefs.GetFloat("DoorPositionZ");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(x, y, z);
            }
            else
            {
                Debug.LogWarning("Player object not found");
            }
        }
    }

    private void OnEnable()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
