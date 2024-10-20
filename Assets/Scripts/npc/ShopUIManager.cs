using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public GameObject shopUI; // อ้างอิงไปยัง Shop UI (Panel)
    public Button[] itemButtons; // ปุ่มสำหรับไอเท็มแต่ละรายการ
    public Text[] priceTexts; // ข้อความสำหรับแสดงราคาของไอเท็มแต่ละรายการ
    public Button closeButton; // ปุ่มปิดร้านค้า

    private ShopManager shopManager;

    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();

        // ซ่อน UI ตอนเริ่มเกม
        shopUI.SetActive(false);

        // กำหนดการทำงานของปุ่มแต่ละปุ่ม
        for (int i = 0; i < itemButtons.Length; i++)
        {
            int index = i; // ต้องสร้างตัวแปรแยกเพื่อป้องกันปัญหาจาก Closure
            itemButtons[i].onClick.AddListener(() => BuyItem(index));
        }

        // ปิดร้านค้าด้วยปุ่มปิด
        closeButton.onClick.AddListener(CloseShop);
    }

    public void OpenShop()
    {
        // เปิดร้านค้าและแสดงรายการสินค้า
        shopUI.SetActive(true);
        for (int i = 0; i < shopManager.shopItems.Count; i++)
        {
            Item item = shopManager.GetItemByName(shopManager.shopItems[i]);
            if (item != null)
            {
                // ตั้งค่าภาพของปุ่มด้วยไอคอนของไอเท็ม
                itemButtons[i].GetComponent<Image>().sprite = item.data.icon; // ตั้งค่าภาพไอคอนของไอเท็ม
                priceTexts[i].text = $"{item.data.itemName}: 100 Coins"; // กำหนดราคา
            }
        }
    }

    public void CloseShop() // เปลี่ยนเป็น public
    {
        shopUI.SetActive(false); // ปิด UI ร้านค้า
    }

    private void BuyItem(int index)
    {
        if (index < shopManager.shopItems.Count)
        {
            string itemName = shopManager.shopItems[index];
            shopManager.BuyItem(itemName);
        }
    }
}
