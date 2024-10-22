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
    public Text priceText;
    public InputField quantityInput;
    public Image itemIcon;
    public Button increaseQuantityButton;
    public Button decreaseQuantityButton;
    public Button sellButton;
    public GameObject warningPanel; // เปลี่ยนจาก Text เป็น Panel
    public Text warningMessageText; // Text ภายใน Panel ที่จะแสดงข้อความ

    private bool isPlayerNearby = false;

    void Start()
    {
        playerMoney = FindObjectOfType<PlayerMoney>();
        playerInventoryManager = GameManager.instance.player.inventoryManager;

        backpackInventory = playerInventoryManager.GetInventoryByName("Backpack");
        toolbarInventory = playerInventoryManager.GetInventoryByName("Toolbar");

        sellUI.SetActive(false);
        warningPanel.SetActive(false); // เริ่มต้นซ่อนไว้
        itemDropdown.onValueChanged.AddListener(delegate { UpdateItemUI(); });
        quantityInput.onValueChanged.AddListener(delegate { UpdatePrice(); });
        increaseQuantityButton.onClick.AddListener(IncreaseQuantity);
        decreaseQuantityButton.onClick.AddListener(DecreaseQuantity);
        sellButton.onClick.AddListener(SellSelectedItem);

        quantityInput.text = "1"; // กำหนดค่าเริ่มต้นของ InputField ให้เป็น 1
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
        UpdateItemUI();
    }

    public void CloseSellUI()
    {
        sellUI.SetActive(false);
        warningPanel.SetActive(false); // ปิดข้อความแจ้งเตือนเมื่อปิด UI
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
            Inventory.Slot itemSlot = GetItemSlotFromInventories(itemName, quantity);

            if (itemSlot != null)
            {
                int totalPrice = itemPrices[itemIndex] * quantity;
                int itemSlotIndex = GetInventoryForSlot(itemSlot).slots.IndexOf(itemSlot);
                GetInventoryForSlot(itemSlot).Remove(itemSlotIndex, quantity);
                playerMoney.AddMoney(totalPrice);
                Debug.Log($"ขาย {quantity} {itemName} สำเร็จ! ได้รับเงิน {totalPrice}");

                ResetUI();
            }
            else
            {
                Debug.Log("ไอเท็มนี้ไม่มีใน Inventory หรือจำนวนไม่พอ!");
                warningMessageText.text = "Not enough items to sell"; // แสดงข้อความใน Text ที่อยู่ใน Panel
                warningPanel.SetActive(true); // เปิด Panel ขึ้นมา
                StartCoroutine(HideWarningPanelAfterDelay(2));
            } // Close the 'if' for itemSlot null check here
        }
        else
        {
            Debug.Log("NPC ไม่สามารถรับซื้อไอเท็มนี้ได้!");
        }
    }

    private IEnumerator HideWarningPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningPanel.SetActive(false); // ซ่อน Panel หลังจากเวลาที่กำหนด
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

    private void UpdateItemUI()
    {
        string selectedItem = itemDropdown.options[itemDropdown.value].text;
        int itemIndex = shopItems.FindIndex(item => item.itemName == selectedItem);
        if (itemIndex != -1)
        {
            itemIcon.sprite = shopItems[itemIndex].icon;
            UpdatePrice();
            Debug.Log($"ไอคอนและราคาสำหรับ {selectedItem} ถูกโหลด: {itemIcon.sprite != null}");
        }
        else
        {
            Debug.Log("ไม่พบไอคอนสำหรับไอเท็มนี้!");
        }
    }

    private void UpdatePrice()
    {
        int quantity;
        if (!int.TryParse(quantityInput.text, out quantity) || quantity <= 0)
        {
            quantity = 1;
        }

        string selectedItem = itemDropdown.options[itemDropdown.value].text;
        int itemIndex = shopItems.FindIndex(item => item.itemName == selectedItem);
        if (itemIndex != -1)
        {
            int totalPrice = itemPrices[itemIndex] * quantity;
            priceText.text = "ราคารวม: " + totalPrice.ToString();
        }
        else
        {
            priceText.text = "ไม่พบราคา";
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
        UpdatePrice();
    }

    public void DecreaseQuantity()
    {
        int currentQuantity;
        if (!int.TryParse(quantityInput.text, out currentQuantity))
        {
            currentQuantity = 1;
        }
        if (currentQuantity > 1)
        {
            currentQuantity--;
        }
        quantityInput.text = currentQuantity.ToString();
        UpdatePrice();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("ผู้เล่นอยู่ใกล้ NPC");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            sellUI.SetActive(false);
            Debug.Log("ผู้เล่นออกห่างจาก NPC");
        }
    }
}
