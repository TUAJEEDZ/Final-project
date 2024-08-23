using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public GameObject[] dropItems; // อาร์เรย์ของไอเทมที่สามารถดรอปได้
    public float dropChance = 1.0f; // โอกาสในการดรอปไอเทม
    public int minDropCount = 1; // จำนวนต่ำสุดที่ดรอป
    public int maxDropCount = 5; // จำนวนสูงสุดที่ดรอป

    public void DropItem()
    {
        if (dropItems.Length > 0)
        {
            // ตรวจสอบว่าเราจะดรอปไอเทมหรือไม่ โดยใช้ dropChance
            if (Random.value <= dropChance)
            {
                // สุ่มจำนวนไอเท็มที่จะดรอป
                int dropCount = GetRandomDropCount();

                // สุ่มไอเท็มและดรอปตามจำนวนที่สุ่มได้
                for (int i = 0; i < dropCount; i++)
                {
                    int randomIndex = Random.Range(0, dropItems.Length);
                    GameObject dropItem = dropItems[randomIndex];

                    // สร้างไอเทมในตำแหน่งของศัตรู
                    Instantiate(dropItem, transform.position, Quaternion.identity);

                    Debug.Log("Dropped item: " + dropItem.name); // แสดงข้อความ Debug เมื่อดรอปไอเทม
                }
            }
            else
            {
                Debug.Log("Item did not drop due to drop chance."); // แสดงข้อความเมื่อไม่ดรอป
            }
        }
        else
        {
            Debug.LogWarning("No items to drop."); // แสดงข้อความเตือนเมื่อไม่มีไอเทมในอาร์เรย์
        }
    }

    private int GetRandomDropCount()
    {
        // สุ่มจำนวนไอเท็มที่จะดรอป
        return Random.Range(minDropCount, maxDropCount + 1); // +1 เพื่อให้รวมจำนวน maxDropCount ด้วย
    }
}
