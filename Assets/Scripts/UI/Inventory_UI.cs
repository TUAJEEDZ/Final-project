using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_UI : MonoBehaviour
{
    public string inventoryName;
    public List<Slot_UI> slots = new List<Slot_UI>();
    [SerializeField] private Canvas canvas;

    private bool dragSingle;
    private Inventory inventory;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    void Start()
    {
        inventory = GameManager.instance.player.inventoryManager.GetInventoryByName(inventoryName);
        SetupSlots();
        Refresh();
    }

    public void Refresh()
    {
        if (slots.Count == inventory.slots.Count)   // Update inventory
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (inventory.slots[i].itemName != "")
                {
                    slots[i].SetItem(inventory.slots[i]);
                }
                else
                {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove() // Remove item in inventory UI
    {
        if (UI_Manager.draggedSlot == null)
        {
            Debug.LogError("Dragged slot is null. Cannot remove item.");
            return;
        }

        // Retrieve the correct inventory from the dragged slot
        Inventory draggedInventory = UI_Manager.draggedSlot.inventory;

        if (draggedInventory == null)
        {
            Debug.LogError("Dragged inventory is null. Cannot remove item.");
            return;
        }

        // Check if the draggedSlot slotID is valid for the dragged inventory
        if (UI_Manager.draggedSlot.slotID < 0 || UI_Manager.draggedSlot.slotID >= draggedInventory.slots.Count)
        {
            Debug.LogError("Invalid slotID for dragged inventory. Cannot remove item.");
            return;
        }

        // Get the item to drop based on the draggedSlot's inventory and item
        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            draggedInventory.slots[UI_Manager.draggedSlot.slotID].itemName);

        if (itemToDrop != null)
        {
            if (UI_Manager.dragSingle)
            {
                GameManager.instance.player.DropItem(itemToDrop);
                draggedInventory.Remove(UI_Manager.draggedSlot.slotID);  // Remove 1 in slot
            }
            else
            {
                GameManager.instance.player.DropItem(itemToDrop, draggedInventory.slots[UI_Manager.draggedSlot.slotID].count);
                draggedInventory.Remove(UI_Manager.draggedSlot.slotID, draggedInventory.slots[UI_Manager.draggedSlot.slotID].count); // Remove all in slot
            }

            // Refresh only the dragged inventory UI
            GameManager.instance.uiManager.RefreshInventoryUI(draggedInventory == GameManager.instance.player.inventoryManager.backpack ? "Backpack" : "Toolbar");
        }
        else
        {
            Debug.LogWarning("Item to drop is null. Cannot proceed.");
        }

        UI_Manager.draggedSlot = null;
    }


    public void SlotBeginDrag(Slot_UI slot)
    {
        UI_Manager.draggedSlot = slot;
        UI_Manager.draggedIcon = Instantiate(UI_Manager.draggedSlot.itemIcon); // Make it look like the icon is being dragged out
        UI_Manager.draggedIcon.transform.SetParent(canvas.transform);  // Ensuring that it is rendered in the UI
        UI_Manager.draggedIcon.raycastTarget = false;              // It won't interfere with other UI interactions
        UI_Manager.draggedIcon.rectTransform.sizeDelta = new Vector2(50, 50); // Resize item icon
    }

    public void SlotDrag()
    {
        MoveToMousePosition(UI_Manager.draggedIcon.gameObject); // Move icon with mouse position
        //Debug.Log("Dragging: " + UI_Manager.draggedSlot.name);   
    }

    public void SlotEndDrag()
    {
        Destroy(UI_Manager.draggedIcon.gameObject);
        UI_Manager.draggedIcon = null;
    }

    public void SlotDrop(Slot_UI slot)
    {
        if (UI_Manager.dragSingle)
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory);
        }
        else
        {
            UI_Manager.draggedSlot.inventory.MoveSlot(UI_Manager.draggedSlot.slotID, slot.slotID, slot.inventory,
                UI_Manager.draggedSlot.inventory.slots[UI_Manager.draggedSlot.slotID].count);
        }
        GameManager.instance.uiManager.RefreshAll();
    }

    private void MoveToMousePosition(GameObject toMove)
    {
        if (canvas != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, null, out position);
            toMove.transform.position = canvas.transform.TransformPoint(position);
        }
    }

    void SetupSlots()
    {
        int counter = 0;

        foreach (Slot_UI slot in slots)
        {
            slot.slotID = counter;
            counter++;
            slot.inventory = inventory; // Assign the inventory to each slot
        }
    }
}
