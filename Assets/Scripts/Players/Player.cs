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
    public ItemManager itemManager;

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
                movement.ChangeState(PlayerState.interact);
                animator.SetTrigger("isPlowing");
                if (tileManager != null && direction != Vector2.zero)
                {
                    // Calculate the position in front of the player based on the direction
                    Vector3Int position = new Vector3Int(
                        Mathf.RoundToInt(transform.position.x - 1 + direction.x),
                        Mathf.RoundToInt(transform.position.y - 1 + direction.y),
                        0
                    );
                    StartCoroutine(DelayedInteraction(position));
                    IEnumerator DelayedInteraction(Vector3Int position)
                    {
                        yield return new WaitForSeconds(0.4f); 
                        movement.ChangeState(PlayerState.walk);
                        string tileName = tileManager.GetTileName(position);

                        if (!string.IsNullOrWhiteSpace(tileName))
                        {
                            if (tileName == "Interactable")
                            {


                               
                                tileManager.SetInteracted(position);
                            }
                        }
                    }
                }
            }
            if (inventoryManager.toolbar.selectedSlot.itemName == "Watering Can")
            {
                movement.ChangeState(PlayerState.interact);
                animator.SetTrigger("isWatering");

                // Calculate the position in front of the player based on the direction
                Vector3Int position = new Vector3Int(
                    Mathf.RoundToInt(transform.position.x - 1 + direction.x),
                    Mathf.RoundToInt(transform.position.y - 1 + direction.y),
                    0
                );
                StartCoroutine(DelayedInteraction(position));
                IEnumerator DelayedInteraction(Vector3Int position)
                {
                    yield return new WaitForSeconds(0.5f); 
                    movement.ChangeState(PlayerState.walk);
                    if (tileManager != null && direction != Vector2.zero)
                    {

                        string tileName = tileManager.GetTileName(position);

                        if (!string.IsNullOrWhiteSpace(tileName))
                        {
                            if (tileName == "Summer_Plowed")
                            {
                                tileManager.SetWatered(position);
                            }
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

                    if (tileManager.IsPlantableTile(tileName))
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
            if (inventoryManager.toolbar.selectedSlot.itemName == "Axe")
            {
                movement.ChangeState(PlayerState.interact);
                animator.SetTrigger("isCutting");
               

                Vector3Int position = new Vector3Int(
                       Mathf.RoundToInt(transform.position.x - 1 + direction.x),
                       Mathf.RoundToInt(transform.position.y - 1 + direction.y),
                       0
                   );
                StartCoroutine(DelayedInteraction(position));
                
                IEnumerator DelayedInteraction(Vector3Int position)
                {
                    yield return new WaitForSeconds(0.4f);
                    movement.ChangeState(PlayerState.walk);
                    if (tileManager != null && direction != Vector2.zero)
                    {
               
                        string tileName = tileManager.GetTileNamePlant(position);

                        if (!string.IsNullOrWhiteSpace(tileName))
                        {

                            if (tileName == "wheat_plant4")
                            {
                                tileManager.SetHavested(position);
                                // Pass the direction to DropItem
                                //havestDrop.DropItem();
                                // Spawn the item after harvesting
                                Item item = GameManager.instance.itemManager.GetItemByName("Wheat");
                                if (item != null)
                                {
                                    DropItem(item); // Call the DropItem method to spawn the item
                                }
                            }
                        }
                    }       
                }
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

        Item droppedItem = Instantiate(item, spawnLocation, Quaternion.identity);

        droppedItem.rb2d.AddForce(direction * 3f + spawnOffset, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }
}