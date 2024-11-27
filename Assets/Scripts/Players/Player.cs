using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float dropRange = 1f;
    public InventoryManager inventoryManager;
    private TileManager tileManager;
    public Transform dropPoint;

    public Transform DropParent;

    private PlayerAttack playerAttack;
    private Stamina stamina;
    private Animator animator;
    private Movement movement;
    private HavestDrop havestDrop;
    public ItemManager itemManager;

    private UI_Manager ui_manager;


    private void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        tileManager = GameManager.instance.tileManager;
        inventoryManager.Add("Toolbar", "Stone Axe", 1);
        inventoryManager.Add("Toolbar", "Stone Hoe", 1);
        inventoryManager.Add("Toolbar", "Stone Sickle", 1);
        inventoryManager.Add("Toolbar", "Stone Watering Can", 1);
        inventoryManager.Add("Toolbar", "Wheat Seed", 10);
        inventoryManager.Add("Toolbar", "Tomato Seed", 10);
        inventoryManager.Add("Toolbar", "Fertilizer", 10);
        inventoryManager.Add("Toolbar", "Stone Pickaxe", 1);
        inventoryManager.Add("Toolbar", "Longsword", 1);
        inventoryManager.Add("Backpack", "Copper Axe", 1);
        inventoryManager.Add("Backpack", "Iron Axe", 1);
        inventoryManager.Add("Backpack", "Copper Hoe", 1);
        inventoryManager.Add("Backpack", "Iron Hoe", 1);
        inventoryManager.Add("Backpack", "Copper Watering Can", 1);
        inventoryManager.Add("Backpack", "Iron Watering Can", 1);

        GameManager.instance.uiManager.RefreshAll();
    }
    private void Awake()
    {
        havestDrop = GetComponent<HavestDrop>();
        inventoryManager = GetComponent<InventoryManager>();
        movement = GetComponent<Movement>();
        playerAttack = GetComponent<PlayerAttack>();

        // Initialize DropParent at the start
        UpdateDropParent();
    }

    private void Update()
    {
        // Get the direction the player is facing
        string currentSceneName = GameManager.instance.sceneTransitionManager.GetActiveSceneName();
        // Update DropParent when scene changes
        UpdateDropParent();

        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        if (!GameManager.instance.uiManager.inventoryPanel.activeSelf &&
             currentSceneName != "Sell" &&
             currentSceneName != "SeedShop" &&
             currentSceneName != "ToolsShop" &&
             currentSceneName != "Homeplayer")
        {
            if (Time.time >= playerAttack.lastAttackTime + playerAttack.actionCooldown)
            {
                if (Input.GetButtonDown("Fire1")) // set keybind
                {
                    if (inventoryManager.toolbar.selectedSlot.itemName == "Stone Hoe")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 3)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isPlowing");
                            GameManager.instance.stamina.UseStamina(3);
                            playerAttack.ActionCoolDown();
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
                                        else if (tileManager.IsPlantableTile(tileName))
                                        {
                                            tileManager.SetFill(position);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Copper Hoe")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 2)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isPlowing");
                            GameManager.instance.stamina.UseStamina(2);
                            playerAttack.ActionCoolDown();
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
                                        else if (tileManager.IsPlantableTile(tileName))
                                        {
                                            tileManager.SetFill(position);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Iron Hoe")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 1)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isPlowing");
                            GameManager.instance.stamina.UseStamina(1);
                            playerAttack.ActionCoolDown();
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
                                        else if (tileManager.IsPlantableTile(tileName))
                                        {
                                            tileManager.SetFill(position);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Stone Watering Can")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 3)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isWatering");
                            GameManager.instance.stamina.UseStamina(3);
                            playerAttack.ActionCoolDown();
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
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Iron Watering Can")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 1)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isWatering");
                            GameManager.instance.stamina.UseStamina(1);
                            playerAttack.ActionCoolDown();
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
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Copper Watering Can")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 2)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isWatering");
                            GameManager.instance.stamina.UseStamina(2);
                            playerAttack.ActionCoolDown();
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
                                string PlantName = tileManager.GetTileNamePlant(position);

                                if (string.IsNullOrWhiteSpace(PlantName))
                                {
                                    // Plant the wheat at the calculated position
                                    tileManager.SetPlantWheat(position);

                                    // Check and remove the Wheat Seed from the selected slot
                                    if (inventoryManager.toolbar.selectedSlot != null &&
                                        inventoryManager.toolbar.selectedSlot.itemName == "Wheat Seed")
                                    {
                                        inventoryManager.toolbar.selectedSlot.RemoveItem();

                                        // Refresh the UI to reflect the changes
                                        GameManager.instance.uiManager.RefreshAll();
                                    }
                                }
                            }
                            else
                            {
                                // Optionally, you can add feedback to the player that the tile is not plantable
                                Debug.Log("Cannot plant on this tile.");
                            }
                        }
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Tomato Seed")
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
                                string PlantName = tileManager.GetTileNamePlant(position);

                                if (string.IsNullOrWhiteSpace(PlantName))
                                {
                                    // Plant the wheat at the calculated position
                                    tileManager.SetPlantTomato(position);

                                    // Check and remove the Wheat Seed from the selected slot
                                    if (inventoryManager.toolbar.selectedSlot != null &&
                                        inventoryManager.toolbar.selectedSlot.itemName == "Tomato Seed")
                                    {
                                        inventoryManager.toolbar.selectedSlot.RemoveItem();

                                        // Refresh the UI to reflect the changes
                                        GameManager.instance.uiManager.RefreshAll();
                                    }
                                }
                            }
                            else
                            {
                                // Optionally, you can add feedback to the player that the tile is not plantable
                                Debug.Log("Cannot plant on this tile.");
                            }
                        }
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Fertilizer")
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
                                string isfertilized = tileManager.GetTileNamefertilized(position);

                                if (isfertilized != "fertilizer")
                                { // Plant the wheat at the calculated position
                                    tileManager.Setfertilized(position);

                                    // Check and remove the Wheat Seed from the selected slot
                                    if (inventoryManager.toolbar.selectedSlot != null &&
                                        inventoryManager.toolbar.selectedSlot.itemName == "Fertilizer")
                                    {
                                        inventoryManager.toolbar.selectedSlot.RemoveItem();

                                        // Refresh the UI to reflect the changes
                                        GameManager.instance.uiManager.RefreshAll();
                                    }
                                }
                            }
                            else
                            {
                                // Optionally, you can add feedback to the player that the tile is not plantable
                                Debug.Log("Cannot fertilize on this tile.");
                            }
                        }
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Stone Sickle")
                    {
                        movement.ChangeState(PlayerState.interact);
                        animator.SetTrigger("isHarvesting");
                        playerAttack.ActionCoolDown();

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
                                        tileManager.SetHarvested(position);
                                        // Pass the direction to DropItem
                                        //havestDrop.DropItem();
                                        // Spawn the item after harvesting

                                        DropItem("Wheat"); // Call the DropItem method to spawn the item

                                    }
                                    else if (tileName == "tomato4")
                                    {
                                        tileManager.SetPickup(position);
                                        // Pass the direction to DropItem
                                        //havestDrop.DropItem();
                                        // Spawn the item after harvesting

                                        DropItem("Tomato"); // Call the DropItem method to spawn the item

                                    }
                                }
                            }
                        }
                    }
                    if (inventoryManager.toolbar.selectedSlot.itemName == "Stone Axe")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 3)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isCutting");
                            GameManager.instance.stamina.UseStamina(3);
                            playerAttack.ActionCoolDown();

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
                                if (tileManager != null && direction != Vector2.zero)
                                {

                                    string tileName = tileManager.GetTileNameTree(position);

                                    if (!string.IsNullOrWhiteSpace(tileName))
                                    {
                                        if (tileName == "cutabletree1")
                                        {
                                            tileManager.DamageTree(position);
                                            tileManager.DamageTree(position);
                                            //inventoryManager.Add("Backpack", "Wood");
                                            DropItem("Wood", 1);
                                        }
                                    }

                                }
                            }
                        }
                    }
                    if (inventoryManager.toolbar.selectedSlot.itemName == "Copper Axe")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 2)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isCutting");
                            GameManager.instance.stamina.UseStamina(2);
                            playerAttack.ActionCoolDown();

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
                                if (tileManager != null && direction != Vector2.zero)
                                {

                                    string tileName = tileManager.GetTileNameTree(position);

                                    if (!string.IsNullOrWhiteSpace(tileName))
                                    {
                                        if (tileName == "cutabletree1")
                                        {
                                            tileManager.DamageTree(position);
                                            tileManager.DamageTree(position);
                                            tileManager.DamageTree(position);
                                            /*inventoryManager.Add("Backpack", "Wood");
                                            inventoryManager.Add("Backpack", "Wood");*/
                                            DropItem("Wood", 2);
                                        }
                                    }

                                }
                            }
                        }
                    }

                    if (inventoryManager.toolbar.selectedSlot.itemName == "Iron Axe")
                    {
                        if (GameManager.instance.stamina.CurrentStamina >= 1)
                        {
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("isCutting");
                            GameManager.instance.stamina.UseStamina(1);
                            playerAttack.ActionCoolDown();

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
                                if (tileManager != null && direction != Vector2.zero)
                                {

                                    string tileName = tileManager.GetTileNameTree(position);

                                    if (!string.IsNullOrWhiteSpace(tileName))
                                    {
                                        if (tileName == "cutabletree1")
                                        {
                                            tileManager.DamageTree(position);
                                            tileManager.DamageTree(position);
                                            tileManager.DamageTree(position);
                                            tileManager.DamageTree(position);
                                            tileManager.DamageTree(position);
                                            /*inventoryManager.Add("Backpack", "Wood");
                                            inventoryManager.Add("Backpack", "Wood");*/
                                            DropItem("Wood", 3);
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void UpdateDropParent()
    {
        string currentSceneName = GameManager.instance.sceneTransitionManager.GetActiveSceneName();

        switch (currentSceneName)
        {
            case "main":
                DropParent = GameObject.Find("MapFarm").transform;
                break;
            case "Homeplayer":
                DropParent = GameObject.Find("map").transform;
                break;
            case "SeedShop":
                DropParent = GameObject.Find("map").transform;
                break;
            case "Sell":
                DropParent = GameObject.Find("map").transform;
                break;
            case "ToolsShop":
                DropParent = GameObject.Find("map").transform;
                break;
            case "Dungeon":
                DropParent = GameObject.Find("Dungeonmap").transform;
                break;
            case "Bossscene":
                DropParent = GameObject.Find("Bossmap").transform;
                break;
            default:
                DropParent = null; // Set it to null or a default parent if needed
                break;
        }

        if (DropParent == null)
        {
            Debug.LogWarning("DropParent could not be found for the current scene.");
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
        droppedItem.transform.SetParent(DropParent.transform, true);

        droppedItem.rb2d.AddForce(direction * 3f + spawnOffset, ForceMode2D.Impulse);
    }

    public void DropItem(string itemName)
    {
        // Retrieve the item using the provided name
        Item item = GameManager.instance.itemManager.GetItemByName(itemName);
        if (item == null)
        {
            Debug.LogWarning($"Item '{itemName}' not found!");
            return; // Exit if the item doesn't exist
        }

        // Get the direction based on the animator's parameters
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        // Calculate the drop point based on the direction
        if (direction != Vector2.zero)
        {
            dropPoint.localPosition = direction * dropRange;
            dropPoint.right = direction;
        }

        // Calculate the spawn location in front of the player
        Vector2 spawnLocation = (Vector2)transform.position + direction; // Spawn item in front of player
        Vector2 spawnOffset = Random.insideUnitCircle * 0.5f; // Slight random offset

        // Instantiate the item
        Item droppedItem = Instantiate(item, spawnLocation, Quaternion.identity);

        droppedItem.transform.SetParent(DropParent.transform, true);

        // Add force to the dropped item
        droppedItem.rb2d.AddForce(direction * 3f + spawnOffset, ForceMode2D.Impulse);
    }

    public void DropItem(Item item, int numToDrop)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    }

    public void DropItem(string itemName, int numToDrop)
    {
        // Retrieve the item using the provided name
        Item item = GameManager.instance.itemManager.GetItemByName(itemName);
        if (item == null)
        {
            Debug.LogWarning($"Item '{itemName}' not found!");
            return; // Exit if the item doesn't exist
        }

        // Get the direction based on the animator's parameters
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        // Calculate the drop point based on the direction
        if (direction != Vector2.zero)
        {
            dropPoint.localPosition = direction * dropRange;
            dropPoint.right = direction;
        }

        for (int i = 0; i < numToDrop; i++)
        {
            // Calculate the spawn location in front of the player
            Vector2 spawnLocation = (Vector2)transform.position + direction; // Spawn item in front of player
            Vector2 spawnOffset = Random.insideUnitCircle * 0.5f; // Slight random offset

            // Instantiate the item
            Item droppedItem = Instantiate(item, spawnLocation, Quaternion.identity);

            droppedItem.transform.SetParent(DropParent.transform, true);

            // Add force to the dropped item
            droppedItem.rb2d.AddForce(direction * 3f + spawnOffset, ForceMode2D.Impulse);
        }
    }

}