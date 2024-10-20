using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCSeller : MonoBehaviour
{
    private PlayerMoney playerMoney;
    private InventoryManager playerInventoryManager;
    private Inventory backpackInventory;
    private Inventory toolbarInventory;

    public List<ItemData> shopItems;  
    public List<int> itemPrices;

    public GameObject sellUI;
    public Dropdown itemDropdown;
    public InputField quantityInput;
    public Image itemIcon;

    private bool isPlayerNearby = false; // เช็คว่าผู้เล่นอยู่ใกล้ NPC หรือไม่

    void Start()
    {
        playerMoney = FindObjectOfType<PlayerMoney>();
        playerInventoryManager = GameManager.instance.player.inventoryManager;

        backpackInventory = playerInventoryManager.GetInventoryByName("Backpack");
        toolbarInventory = playerInventoryManager.GetInventoryByName("Toolbar");

        sellUI.SetActive(false);
        itemDropdown.onValueChanged.AddListener(delegate { UpdateItemIcon(); });
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            OpenSellUI();
        }
    }

    private void OpenSellUI()
    {
        itemDropdown.ClearOptions();
        List<string> itemNames = new List<string>();
        foreach (ItemData item in shopItems)
        {
            itemNames.Add(item.itemName);
        }
        itemDropdown.AddOptions(itemNames);
        sellUI.SetActive(true);
        UpdateItemIcon();
    }

    public void CloseSellUI()
    {
        sellUI.SetActive(false);
    }

    public void SellSelectedItem()
    {
        string itemName = itemDropdown.options[itemDropdown.value].text;
        int quantity;

        if (!int.TryParse(quantityInput.text, out quantity) || quantity <= 0)
        {
            Debug.Log("กรุณากรอกจำนวนที่ถูกต้อง");
            return;
        }

        int itemIndex = shopItems.FindIndex(item => item.itemName == itemName);
        if (itemIndex != -1)
        {
            int totalPrice = itemPrices[itemIndex] * quantity;
            Inventory.Slot itemSlot = GetItemSlotFromInventories(itemName, quantity);

            if (itemSlot != null)
            {
                int itemSlotIndex = GetInventoryForSlot(itemSlot).slots.IndexOf(itemSlot);
                GetInventoryForSlot(itemSlot).Remove(itemSlotIndex, quantity);
                playerMoney.AddMoney(totalPrice);
                Debug.Log($"ขาย {quantity} {itemName} สำเร็จ! ได้รับเงิน {totalPrice}");

                ResetUI();
            }
            else
            {
                Debug.Log("ไอเท็มนี้ไม่มีใน Inventory หรือจำนวนไม่พอ!");
            }
        }
        else
        {
            Debug.Log("NPC ไม่สามารถรับซื้อไอเท็มนี้ได้!");
        }

        sellUI.SetActive(false);
    }

    private void ResetUI()
    {
        GameManager.instance.uiManager.RefreshAll();
        Debug.Log("รีเซ็ต UI เสร็จสิ้น");
    }

    private Inventory.Slot GetItemSlotFromInventories(string itemName, int quantity)
    {
        Inventory.Slot backpackSlot = backpackInventory.GetItemSlotByName(itemName);
        if (backpackSlot != null && backpackSlot.count >= quantity)
        {
            return backpackSlot;
        }

        Inventory.Slot toolbarSlot = toolbarInventory.GetItemSlotByName(itemName);
        if (toolbarSlot != null && toolbarSlot.count >= quantity)
        {
            return toolbarSlot;
        }

        return null;
    }

    private Inventory GetInventoryForSlot(Inventory.Slot slot)
    {
        if (backpackInventory.slots.Contains(slot))
        {
            return backpackInventory;
        }
        else if (toolbarInventory.slots.Contains(slot))
        {
            return toolbarInventory;
        }

        return null;
    }

    private void UpdateItemIcon()
    {
        string selectedItem = itemDropdown.options[itemDropdown.value].text;
        int itemIndex = shopItems.FindIndex(item => item.itemName == selectedItem);
        if (itemIndex != -1)
        {
            itemIcon.sprite = shopItems[itemIndex].icon;
            Debug.Log($"ไอคอนสำหรับ {selectedItem} ถูกโหลด: {itemIcon.sprite != null}");
        }
        else
        {
            Debug.Log("ไม่พบไอคอนสำหรับไอเท็มนี้!");
        }
    }

    public void IncreaseQuantity()
    {
        int currentQuantity;
        if (!int.TryParse(quantityInput.text, out currentQuantity))
        {
            currentQuantity = 1;
        }
        currentQuantity++;
        quantityInput.text = currentQuantity.ToString();
    }

    public void DecreaseQuantity()
    {
        int currentQuantity;
        if (!int.TryParse(quantityInput.text, out currentQuantity))
        {
            currentQuantity = 0;
        }
        if (currentQuantity > 0)
        {
            currentQuantity--;
        }
        quantityInput.text = currentQuantity.ToString();
    }

    // ฟังก์ชันที่เรียกเมื่อผู้เล่นเข้ามาใกล้ NPC
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("ผู้เล่นอยู่ใกล้ NPC");
        }
    }

    // ฟังก์ชันที่เรียกเมื่อผู้เล่นออกห่างจาก NPC
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            sellUI.SetActive(false); // ปิด UI เมื่อผู้เล่นออกห่าง
            Debug.Log("ผู้เล่นออกห่างจาก NPC");
        }
    }
}
