using System.Collections.Generic;
using UnityEngine;

public class NPCShop : MonoBehaviour
{
    public List<Item> shopItems; // รายการไอเท็มที่ร้านค้าจะขาย
    public List<int> itemPrices; // ราคาของแต่ละไอเท็มที่ตั้งใน Inspector
    public ShopUI shopUI;
    private bool isPlayerNearby;

    private void Awake()
    {
        if (!shopUI) shopUI = FindObjectOfType<ShopUI>();
    }

    public void BuyItem(Item item, int totalPrice)
    {
        if (item?.data == null) return;

        var playerInventory = GameManager.instance?.player?.inventoryManager;
        var playerMoney = GameManager.instance?.player?.GetComponent<PlayerMoney>();

        if (playerMoney != null && playerMoney.HasEnoughMoney(totalPrice))
        {
            for (int i = 0; i < totalPrice / itemPrices[shopItems.IndexOf(item)]; i++) // เพิ่มจำนวนไอเท็มในอินเวนทอรี
            {
                playerInventory?.Add("Toolbar", item.data.itemName);
            }
            GameManager.instance?.uiManager?.RefreshInventoryUI("Toolbar");
            playerMoney.SpendMoney(totalPrice); // หักเงินหลังจากซื้อสินค้า
            Debug.Log($"ซื้อ {item.data.itemName} จำนวน {totalPrice / itemPrices[shopItems.IndexOf(item)]} เรียบร้อย");
        }
        else
        {
            Debug.Log("เงินไม่พอสำหรับการซื้อไอเท็มนี้");
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.F))
        {
            shopUI.ToggleShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            shopUI.CloseShop();
        }
    }
}
