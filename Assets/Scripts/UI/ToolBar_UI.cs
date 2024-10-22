using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();

    private Slot_UI selectedSlot;
    private int selectedIndex = 0; // Keep track of the selected index

    private void Start()
    {
        SelectSlot(0); // Already select first slot when starting the game
    }

    private void Update()
    {
        CheckAlphaNumericKey();
        CheckScrollInput();
    }

    public void SelectSlot(Slot_UI slot)
    {
        SelectSlot(slot.slotID);
    }

    public void SelectSlot(int index)
    {
        if (toolbarSlots.Count == 9)
        {
            if (selectedSlot != null)
            {
                selectedSlot.SetHighlight(false); // If slot isn't selected, it will not show highlight
            }
            selectedSlot = toolbarSlots[index];
            selectedSlot.SetHighlight(true); // Highlight selected slot
            selectedIndex = index; // Update the selected index

            GameManager.instance.player.inventoryManager.toolbar.SelectSlot(index);
        }
    }

    private void CheckAlphaNumericKey() // Set key
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { SelectSlot(4); }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { SelectSlot(5); }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { SelectSlot(6); }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { SelectSlot(7); }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { SelectSlot(8); }
    }

    private void CheckScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // Scroll up
        {
            SelectNextSlot();
        }
        else if (scroll < 0f) // Scroll down
        {
            SelectPreviousSlot();
        }
    }

    private void SelectNextSlot()
    {
        int nextIndex = (selectedIndex - 1) % toolbarSlots.Count; // Loop back to first slot if at the end
        SelectSlot(nextIndex);
    }

    private void SelectPreviousSlot()
    {
        int previousIndex = (selectedIndex + 1 + toolbarSlots.Count) % toolbarSlots.Count; // Loop to last slot if at the beginning
        SelectSlot(previousIndex);
    }
}
