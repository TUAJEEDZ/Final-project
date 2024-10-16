using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public int ticksUntilReset = 3; // จำนวน ticks ที่จะรีเซ็ตดันเจี้ยน
    private int lastResetTick; // ตัวแปรเก็บ ticks ล่าสุดที่รีเซ็ต
    private DayNightCycle dayNightCycle; // ประกาศตัวแปร dayNightCycle
    private List<GameObject> itemsInScene = new List<GameObject>(); // รายการเก็บของในฉาก
    private int currentTick; // ตัวแปรเก็บ ticks ปัจจุบัน

    private void Awake()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>(); // ใช้ FindObjectOfType เพื่อรับอินสแตนซ์ของ DayNightCycle
    }

    private void Start()
    {
        // ดึง ticks ล่าสุดที่รีเซ็ตจาก PlayerPrefs
        lastResetTick = PlayerPrefs.GetInt("LastResetTick", 0);
        currentTick = 0; // เริ่มต้น ticks ที่ 0
        Debug.Log("Last Reset Tick on Start: " + lastResetTick);
        SpawnItems(); // เรียกใช้ SpawnItems เมื่อเริ่มต้น
    }

    public void CheckDungeonReset()
    {
        if (dayNightCycle == null)
        {
            Debug.LogWarning("DayNightCycle is not set in DungeonManager.");
            return; // ออกจากฟังก์ชันหาก dayNightCycle เป็น null
        }

        // ตรวจสอบว่าผ่าน ticks ที่ตั้งไว้หรือไม่
        if (currentTick >= lastResetTick + ticksUntilReset)
        {
            ResetDungeon();
        }
    }

    public void IncrementTick()
    {
        currentTick++; // เพิ่ม ticks ทุกครั้งที่เรียก
        CheckDungeonReset(); // ตรวจสอบการรีเซ็ตดันเจี้ยนทุกครั้งที่ ticks เพิ่มขึ้น
    }

    private void ResetDungeon()
    {
        Debug.Log("Dungeon Reset!");
        ClearOldItems(); // ลบของเก่าออก
        SpawnItems(); // สร้างของใหม่
        lastResetTick = currentTick; // อัพเดท Last Reset Tick
        PlayerPrefs.SetInt("LastResetTick", lastResetTick); // บันทึก Last Reset Tick
        PlayerPrefs.Save(); // บันทึก PlayerPrefs
    }

    private void ClearOldItems()
    {
        foreach (GameObject item in itemsInScene) // แทนที่ด้วยลอจิกของคุณเองในการเข้าถึงของ
        {
            Destroy(item);
        }
        itemsInScene.Clear(); // เคลียร์รายการหลังจากลบ
    }

    public void SpawnItems()
    {
        Debug.Log("Spawning new items...");

        MonsterSpawner monsterSpawner = FindObjectOfType<MonsterSpawner>();
        if (monsterSpawner != null)
        {
            // สร้างแร่
            for (int i = 0; i < monsterSpawner.numberOfMinerals; i++)
            {
                GameObject mineral = InstantiateRandom(monsterSpawner.mineralPrefabs);
                if (mineral != null)
                    itemsInScene.Add(mineral);
            }

            // สร้างมอนสเตอร์
            for (int i = 0; i < monsterSpawner.numberOfMonsters; i++)
            {
                GameObject monster = InstantiateRandom(monsterSpawner.monsterPrefabs);
                if (monster != null)
                    itemsInScene.Add(monster);
            }
        }
    }

    private GameObject InstantiateRandom(GameObject[] prefabs)
    {
        Vector3 position = GetRandomSpawnPosition();
        return Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Logic สำหรับการหาตำแหน่งการเกิดแบบสุ่ม
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)); // ปรับตามพื้นที่เกิดจริงของคุณ
    }
}
