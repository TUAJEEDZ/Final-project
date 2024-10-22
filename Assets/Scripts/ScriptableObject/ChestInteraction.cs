using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ������Ѻ Image � Canvas

public class ChestInteraction : MonoBehaviour
{
    private bool isNearChest = false;
    private Movement movement;
    private Animator animator;
    private bool playerInTrigger = false;
    public Text interactionText; // UI Text for interaction
    public Image interactionImage; // UI Image for interaction

    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();

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
        /*if (!GameManager.instance.uiManager.chestPanel.activeSelf)
        {
            movement.ChangeState(PlayerState.walk);
        }
*/
        if (isNearChest)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (movement != null)
                {
                    if (!GameManager.instance.uiManager.chestPanel.activeSelf && !GameManager.instance.uiManager.inventoryPanel.activeSelf)
                    {
                        StartCoroutine(OpenChestCoroutine());
                    }
                    else if (GameManager.instance.uiManager.chestPanel.activeSelf)
                    {
                        StartCoroutine(CloseChestCoroutine());
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (GameManager.instance.uiManager.chestPanel.activeSelf)
                {
                    GameManager.instance.uiManager.ToggleInventoryUI();
                    StartCoroutine(CloseChestCoroutine());
                }
            }
        }
    }

    private IEnumerator OpenChestCoroutine()
    {
        animator.SetTrigger("isOpen");
        movement.ChangeState(PlayerState.interact);
        yield return new WaitForSeconds(0.5f); // Wait for the animation to finish
        GameManager.instance.uiManager.ToggleChestUI();
        GameManager.instance.uiManager.ToggleInventoryUI();
    }

    private IEnumerator CloseChestCoroutine()
    {
        GameManager.instance.uiManager.ToggleChestUI();
        GameManager.instance.uiManager.ToggleInventoryUI();
        movement.ChangeState(PlayerState.walk);
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("isClosing");
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

