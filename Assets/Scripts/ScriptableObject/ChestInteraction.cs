using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ������Ѻ Image � Canvas

public class ChestInteraction : MonoBehaviour
{
    private bool isNearChest = false;
    private Movement movement;
    private bool playerInTrigger = false;
    public Text interactionText; // UI Text for interaction
    public Image interactionImage; // UI Image for interaction

    void Start()
    {
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        movement = FindObjectOfType<Movement>();

        if (movement == null)
        {
            Debug.LogError("Movement script not found on the player or in the scene!");
        }
    }

    void Update()
    {
        if (isNearChest && Input.GetKeyDown(KeyCode.E))
        {
            if (movement != null)
            {
                if (!GameManager.instance.uiManager.chestPanel.activeSelf && !GameManager.instance.uiManager.inventoryPanel.activeSelf)
                {
                    movement.ChangeState(PlayerState.interact); 
                    GameManager.instance.uiManager.ToggleChestUI();
                    GameManager.instance.uiManager.ToggleInventoryUI();
                }
                else if (GameManager.instance.uiManager.chestPanel.activeSelf)
                {
                    GameManager.instance.uiManager.ToggleChestUI();
                    GameManager.instance.uiManager.ToggleInventoryUI();
                    movement.ChangeState(PlayerState.walk); 
                }
          
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearChest = true;
            Debug.Log("Player is near the bed. Press 'E' to open chest.");
        }
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

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearChest = false;
        }

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

