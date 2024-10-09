using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotCount;

    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;

    private ItemManager itemManager;

    private void Awake()
    {
        backpack = new Inventory(backpackSlotCount);
        toolbar = new Inventory(toolbarSlotCount);

        inventoryByName.Add("Backpack", backpack);
        inventoryByName.Add("Toolbar", toolbar);

        itemManager = FindObjectOfType<ItemManager>(); // Find ItemManager in the scene

        if (itemManager == null)
        {
            Debug.LogError("ItemManager not found in the scene!");
        }
    }

    public void Add(string inventoryName, Item item)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            inventoryByName[inventoryName].Add(item);
        }
    }

    public void Add(string inventoryName, string itemName)
    {
        if (inventoryByName.ContainsKey(inventoryName))
        {
            Item item = itemManager.GetItemByName(itemName);
            if (item != null)
            {
                inventoryByName[inventoryName].Add(item);
            }
            else
            {
                Debug.LogWarning($"Item '{itemName}' not found in ItemManager!");
            }
        }
        else
        {
            Debug.LogWarning($"Inventory '{inventoryName}' not found!");
        }
    }


    public Inventory GetInventoryByName(string inventoryName)
    {
        if(inventoryByName.ContainsKey(inventoryName))
        {
            return inventoryByName[inventoryName];
        }

        return null;
    }

}
