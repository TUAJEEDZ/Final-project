using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject shopPanel;
    public Transform shopContent;
    public GameObject itemButtonPrefab;
    public NPCShop npcShop;

    // UI สำหรับการกรอกจำนวนไอเท็ม
    public GameObject quantityPanel;
    public InputField quantityInput;
    public Button confirmButton;
    public Button closeQuantityPanelButton; // ปุ่มปิด panel จำนวน
    public Text itemNameText;
    public Text itemPriceText;
    public Button increaseButton; // ปุ่มเพิ่มจำนวน
    public Button decreaseButton; // ปุ่มลดจำนวน
    public Image itemImage; // เพิ่มตัวแปรสำหรับเก็บ Image ของไอเทม
    public Button closeButton; // เพิ่มปุ่มปิด shopPanel

    private Item selectedItem;
    private int selectedItemPrice;

    private void Awake()
    {
        if (!npcShop) npcShop = GetComponent<NPCShop>();
        quantityPanel.SetActive(false); // ซ่อน panel ตั้งแต่เริ่มต้น

        closeQuantityPanelButton.onClick.AddListener(CloseQuantityPanel); // เพิ่ม listener สำหรับปิด panel จำนวน
        closeButton.onClick.AddListener(CloseShop); // เพิ่ม listener สำหรับปิด shopPanel
    }

    private void Start()
    {
        CloseShop();
        increaseButton.onClick.AddListener(IncreaseQuantity);
        decreaseButton.onClick.AddListener(DecreaseQuantity);
    }

    public void ToggleShop()
    {
        bool isOpen = !shopPanel.activeSelf;
        shopPanel.SetActive(isOpen);
        if (isOpen) RefreshShopUI();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void RefreshShopUI()
    {
        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < npcShop.shopItems.Count; i++)
        {
            Item item = npcShop.shopItems[i];
            int price = npcShop.itemPrices[i]; // ดึงราคาจาก NPCShop

            if (item?.data == null) continue;

            GameObject itemButton = Instantiate(itemButtonPrefab, shopContent);
            itemButton.GetComponentInChildren<Text>().text = item.data.itemName + " - $" + price;
            itemButton.GetComponentInChildren<Image>().sprite = item.data.icon;

            int itemIndex = i; // เก็บ index ของ item เพื่อนำมาใช้งานใน listener
            itemButton.GetComponent<Button>().onClick.AddListener(() => OpenQuantityPanel(item, price));
        }
    }

    // เปิด panel สำหรับให้ผู้เล่นกรอกจำนวนไอเท็ม
    private void OpenQuantityPanel(Item item, int price)
    {
        selectedItem = item;
        selectedItemPrice = price;

        itemNameText.text = item.data.itemName;
        itemPriceText.text = "Price: $" + price;
        quantityInput.text = "1"; // ตั้งค่าเริ่มต้นเป็น 1

        itemImage.sprite = item.data.icon; // แสดงรูปไอเทม

        quantityPanel.SetActive(true); // เปิด panel
    }

    // ฟังก์ชันสำหรับการซื้อไอเท็ม
    public void ConfirmPurchase()
    {
        int quantity;
        if (int.TryParse(quantityInput.text, out quantity) && quantity > 0)
        {
            int totalPrice = selectedItemPrice * quantity; // คำนวณราคารวม

            npcShop.BuyItem(selectedItem, totalPrice); // ซื้อไอเท็มพร้อมราคารวม
            quantityPanel.SetActive(false); // ปิด panel หลังจากซื้อ
        }
        else
        {
            Debug.Log("จำนวนไอเท็มไม่ถูกต้อง");
        }
    }

    private void CloseQuantityPanel()
    {
        quantityPanel.SetActive(false); // ปิด panel
    }

    private void IncreaseQuantity()
    {
        int currentQuantity;
        if (int.TryParse(quantityInput.text, out currentQuantity))
        {
            currentQuantity++; // เพิ่มจำนวน
            quantityInput.text = currentQuantity.ToString(); // แสดงจำนวนใหม่
            UpdateTotalPrice(currentQuantity); // อัปเดตราคา
        }
    }

    private void DecreaseQuantity()
    {
        int currentQuantity;
        if (int.TryParse(quantityInput.text, out currentQuantity) && currentQuantity > 1)
        {
            currentQuantity--; // ลดจำนวน
            quantityInput.text = currentQuantity.ToString(); // แสดงจำนวนใหม่
            UpdateTotalPrice(currentQuantity); // อัปเดตราคา
        }
    }

    private void UpdateTotalPrice(int quantity)
    {
        int totalPrice = selectedItemPrice * quantity; // คำนวณราคารวม
        itemPriceText.text = "Price: $" + totalPrice; // แสดงราคาที่อัปเดต
    }

    private void OnEnable()
    {
        confirmButton.onClick.AddListener(ConfirmPurchase);
    }

    private void OnDisable()
    {
        confirmButton.onClick.RemoveListener(ConfirmPurchase);
    }
}
