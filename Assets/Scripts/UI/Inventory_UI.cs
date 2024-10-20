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
        if (inventory == null)
        {
            Debug.LogError("Inventory is null. Cannot remove item.");
            return;
        }

        if (UI_Manager.draggedSlot == null)
        {
            Debug.LogError("Dragged slot is null. Cannot remove item.");
            return;
        }

        if (UI_Manager.draggedSlot.slotID < 0 || UI_Manager.draggedSlot.slotID >= inventory.slots.Count)
        {
            Debug.LogError("Invalid slotID. Cannot remove item.");
            return;
        }

        Item itemToDrop = GameManager.instance.itemManager.GetItemByName(
            inventory.slots[UI_Manager.draggedSlot.slotID].itemName);

        if (itemToDrop != null)
        {
            if (UI_Manager.dragSingle)
            {
                GameManager.instance.player.DropItem(itemToDrop);
                inventory.Remove(UI_Manager.draggedSlot.slotID);  // Remove 1 in slot
            }
            else
            {
                GameManager.instance.player.DropItem(itemToDrop, inventory.slots[UI_Manager.draggedSlot.slotID].count);
                inventory.Remove(UI_Manager.draggedSlot.slotID,
                    inventory.slots[UI_Manager.draggedSlot.slotID].count); // Remove all in slot
            }

            Refresh();
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
        Debug.Log("Dragging: " + UI_Manager.draggedSlot.name);   
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
