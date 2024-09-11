using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float dropRange = 1f;
    public InventoryManager inventoryManager;
    private TileManager tileManager;
    public Transform dropPoint;


    private Animator animator;
    private Movement movement;
    private HavestDrop havestDrop;

    private UI_Manager ui_manager;


    private void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        tileManager = GameManager.instance.tileManager;
    }
    private void Awake()
    {
        havestDrop = GetComponent<HavestDrop>();
        inventoryManager = GetComponent<InventoryManager>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // Get the direction the player is facing
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.E)) // set keybind
        {
            if (inventoryManager.toolbar.selectedSlot.itemName == "Hoe")
            {
                animator.SetTrigger("isPlowing");
                if (tileManager != null && direction != Vector2.zero)
                {
                    // Calculate the position in front of the player based on the direction
                    Vector3Int position = new Vector3Int(
                        Mathf.RoundToInt(transform.position.x - 1 + direction.x),
                        Mathf.RoundToInt(transform.position.y - 1 + direction.y),
                        0
                    );

                    string tileName = tileManager.GetTileName(position);

                    if (!string.IsNullOrWhiteSpace(tileName))
                    {
                        if (tileName == "Interactable")
                        {
                            StartCoroutine(DelayedInteraction(position));
                        }
                        IEnumerator DelayedInteraction(Vector3Int position)
                        {
                            yield return new WaitForSeconds(0.5f); // Delay for 1 second
                            tileManager.SetInteracted(position);
                        }
                    }

                }
            }
            if (inventoryManager.toolbar.selectedSlot.itemName == "Watering Can")
            {
                animator.SetTrigger("isWatering");
                if (tileManager != null && direction != Vector2.zero)
                {
                    // Calculate the position in front of the player based on the direction
                    Vector3Int position = new Vector3Int(
                        Mathf.RoundToInt(transform.position.x - 1 + direction.x),
                        Mathf.RoundToInt(transform.position.y - 1 + direction.y),
                        0
                    );

                    string tileName = tileManager.GetTileName(position);

                    if (!string.IsNullOrWhiteSpace(tileName))
                    {
                        if (tileName == "Summer_Plowed")
                        {
                            StartCoroutine(DelayedInteraction(position));
                        }
                        IEnumerator DelayedInteraction(Vector3Int position)
                        {
                            yield return new WaitForSeconds(0.5f); // Delay for 1 second
                            tileManager.SetWatered(position);
                        }
                    }

                }
            }
            if (inventoryManager.toolbar.selectedSlot.itemName == "Wheat Seed")
            {
                if (tileManager != null && direction != Vector2.zero)
                {
                    // Calculate the position in front of the player based on the direction
                    Vector3Int position = new Vector3Int(
                        Mathf.RoundToInt(transform.position.x - 1 + direction.x),
                        Mathf.RoundToInt(transform.position.y - 1 + direction.y),
                        0
                    );

                    string tileName = tileManager.GetTileName(position);

                    if (!string.IsNullOrWhiteSpace(tileName))
                    {
                        if (tileName == "Summer_Plowed" || tileName == "wetplowed 1")
                        {
                            tileManager.SetPlantWheat(position);

                            // Check and remove the Wheat Seed from the selected slot
                            if (inventoryManager.toolbar.selectedSlot != null &&
                               inventoryManager.toolbar.selectedSlot.itemName == "Wheat Seed")
                            {
                                inventoryManager.toolbar.selectedSlot.RemoveItem();

                                GameManager.instance.uiManager.RefreshAll();
                            }
                        }
                    }

                }
            }
            if (inventoryManager.toolbar.selectedSlot.itemName == "Axe")
            {
                if (tileManager != null && direction != Vector2.zero)
                {
                    // Calculate the position in front of the player based on the direction
                    Vector3Int position = new Vector3Int(
                        Mathf.RoundToInt(transform.position.x - 1 + direction.x),
                        Mathf.RoundToInt(transform.position.y - 1 + direction.y),
                        0
                    );

                    string tileName = tileManager.GetTileNamePlant(position);

                    if (!string.IsNullOrWhiteSpace(tileName))
                    {
                        if (tileName == "wheat_plant4")
                        {
                            tileManager.SetHavested(position);
                            // Pass the direction to DropItem
                            havestDrop.DropItem(position, direction);
                        }
                    }
                }
            }
            if (inventoryManager.toolbar.selectedSlot.itemName == "Ironsword")
            {
            animator.SetTrigger("IsAttacking");

            }
            
        }
    }

 

    public void DropItem(Item item)
    {
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

            if (direction != Vector2.zero)
            {
                dropPoint.localPosition = direction * dropRange; 
                dropPoint.right = direction;  
            }
        Vector2 spawnLocation = (Vector2)transform.position + direction; // Spawn item in front of player

        Vector2 spawnOffset = Random.insideUnitCircle * 0.5f; // Slight random offset

        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity);

        droppedItem.rb2d.AddForce(direction * 2f + spawnOffset, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}
