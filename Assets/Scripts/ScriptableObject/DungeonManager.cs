using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private List<GameObject> itemsInScene = new List<GameObject>(); // รายการเก็บของในฉาก

    private void Start()
    {
        SpawnItems(); // เรียกใช้ SpawnItems เมื่อเริ่มต้น
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

    private void ClearOldItems()
    {
        foreach (GameObject item in itemsInScene) // แทนที่ด้วยลอจิกของคุณเองในการเข้าถึงของ
        {
            Destroy(item);
        }
        itemsInScene.Clear(); // เคลียร์รายการหลังจากลบ
    }
}
