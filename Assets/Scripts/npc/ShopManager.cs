using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public List<string> shopItems; // รายการสินค้าที่ร้านขาย
    public InventoryManager inventoryManager; // อ้างอิงไปยัง Inventory Manager
    public ItemManager itemManager; // อ้างอิงไปยัง Item Manager
    public UI_Manager uiManager; // อ้างอิงไปยัง UI Manager

    private void Start()
    {
        // สร้างรายการสินค้าที่ขายในร้าน
        shopItems = new List<string> { "Axe", "Hoe", "Wheat Seed", "Wood","Carrot Seed","Carrot" };
    }

    public void BuyItem(string itemName)
    {
        // เพิ่มไอเท็มลงใน Inventory ของผู้เล่น
        inventoryManager.Add("Toolbar", itemName);
        Debug.Log($"Bought: {itemName}");

        // รีเฟรช UI ทุกครั้งหลังจากซื้อไอเท็ม
       GameManager.instance.uiManager.RefreshAll();

        // คุณสามารถลด Coins ของผู้เล่นที่นี่ได้
    }

    public Item GetItemByName(string itemName)
    {
        return itemManager.GetItemByName(itemName); // ได้รับไอเท็มจาก ItemManager
    }
}