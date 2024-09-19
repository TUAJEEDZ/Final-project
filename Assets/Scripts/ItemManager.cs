using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour   //adjust new added item by name 
{
    public Item[] items;

    private Dictionary<string, Item> nameToItemDict =
        new Dictionary<string, Item>();
                        
    private void Awake()
    {
        foreach(Item item in items)
        {
            AddItem(item);
        }
    }

    private void AddItem(Item item)
    {
        if(!nameToItemDict.ContainsKey(item.data.itemName))
        {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }
    public Item GetItemByName(string key)
    {
        if(nameToItemDict.ContainsKey(key))
        {
            return nameToItemDict[key];
        }
        return null;
    }

    // Method to spawn an item by name
    public void SpawnItem(string itemName, Vector3 position, Quaternion rotation)
    {
        Item itemPrefab = GetItemByName(itemName);
        if (itemPrefab != null)
        {
            Instantiate(itemPrefab, position, rotation);
        }
        else
        {
            Debug.LogWarning("Item not found: " + itemName);
        }
    }
}
