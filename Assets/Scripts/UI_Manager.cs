using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();

    public GameObject inventoryPanel;
    public GameObject removePanel;
    public GameObject chestPanel;
    public GameObject ExitPanel;
    public GameObject tipPanel;


    public List<Inventory_UI> inventoryUIs;

    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;

    private void Awake()
    {
        inventoryPanel.SetActive(false);
        removePanel.SetActive(false);
        chestPanel.SetActive(false);
        ExitPanel.SetActive(false);
        tipPanel.SetActive(false);
        Initialize();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventoryUI();
        }

        if (Input.GetKey(KeyCode.LeftShift)) //keybind for drop single item(Hold)
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleExitUI();
        }
    }

    public void ToggleInventoryUI()
    {
        if (inventoryPanel != null)
        {
            if (!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                removePanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else
            {
                chestPanel.SetActive(false);
                inventoryPanel.SetActive(false);
                removePanel.SetActive(false);
            }
        }
    }

    public void ToggleTipUI()
    {
        if (inventtipPaneloryPanel != null)
        {
            if (!tipPanel.activeSelf)
            {
                tipPanel.SetActive(true);
            }
            else
            {
                tipPanel.SetActive(false);
            }
        }
    }

    public void ToggleChestUI()
    {
        if (chestPanel != null)
        {
            if (!chestPanel.activeSelf)
            {
                chestPanel.SetActive(true);
                removePanel.SetActive(true);
                RefreshInventoryUI("Chest");
            }
            else
            {
                chestPanel.SetActive(false);
                removePanel.SetActive(false);
            }
        }
    }

    public void ToggleExitUI()
    {
        if( ExitPanel != null)
        {
            if (!ExitPanel.activeSelf)
            {
                ExitPanel.SetActive(true);
                removePanel.SetActive(true);
            }
            else
            {
                ExitPanel.SetActive(false);
                removePanel.SetActive(false);
            }
        }
    }

    public void RefreshInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }
    public void RefreshAll()
    {
        foreach(KeyValuePair<string, Inventory_UI> keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }

    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if(inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }

        Debug.LogWarning("There is not inventory ui for " + inventoryName);
        return null;
    }

    void Initialize()
    {
        foreach(Inventory_UI ui in inventoryUIs)
        {
            if(!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}
