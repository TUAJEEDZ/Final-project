using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item[] items;  // Array of items to be initialized in the inspector

    private Dictionary<string, Item> nameToItemDict = new Dictionary<string, Item>();

    private void Awake()
    {
        // Initialize the dictionary with items
        foreach (Item item in items)
        {
            AddItem(item);
        }
    }

    private void AddItem(Item item)
    {
        // Add item to dictionary if it doesn't already exist
        if (!nameToItemDict.ContainsKey(item.data.itemName))
        {
            nameToItemDict.Add(item.data.itemName, item);
        }
    }

    public Item GetItemByName(string key)
    {
        // Return the item if it exists, otherwise return null
        if (nameToItemDict.ContainsKey(key))
        {
            return nameToItemDict[key];
        }
        return null;
    }
}
