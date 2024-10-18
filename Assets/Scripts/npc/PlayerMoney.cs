using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    public int currentMoney = 1000; // จำนวนเงินเริ่มต้นของผู้เล่น
    public Text moneyText; // อ้างอิงถึง UI Text ที่แสดงจำนวนเงิน

    private void Start()
    {
        UpdateMoneyUI(); // อัพเดต UI เมื่อเกมเริ่มต้น
    }

    public bool HasEnoughMoney(int cost)
    {
        return currentMoney >= cost;
    }

    public void SpendMoney(int amount)
    {
        if (HasEnoughMoney(amount))
        {
            currentMoney -= amount;
            UpdateMoneyUI(); // อัพเดต UI หลังจากใช้เงิน
        }
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI(); // อัพเดต UI หลังจากเพิ่มเงิน
    }

    // เพิ่มฟังก์ชันนี้
    public int GetMoney()
    {
        return currentMoney; // คืนค่าจำนวนเงินปัจจุบัน
    }

    // เพิ่มฟังก์ชันนี้
    public void RemoveMoney(int amount)
    {
        SpendMoney(amount); // เรียกใช้ฟังก์ชัน SpendMoney
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + currentMoney; // อัพเดตข้อความใน UI Text
        }
    }
}
