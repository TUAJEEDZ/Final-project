using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;  // Array ของมอนสเตอร์ prefab หลายตัว
    public int numberOfMonsters = 5;  // จำนวนมอนสเตอร์ที่ต้องการ
    public float spawnAreaWidth = 10f;  // ความกว้างของพื้นที่สุ่ม
    public float spawnAreaHeight = 10f;  // ความสูงของพื้นที่สุ่ม
    public float checkRadius = 1f;  // รัศมีในการตรวจสอบการทับกัน

    private void Start()
    {
        SpawnMonsters();
    }

    void SpawnMonsters()
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector2 spawnPosition;
            int attempt = 0;
            bool positionFound = false;

            // พยายามหาตำแหน่งใหม่ที่ไม่ทับกับมอนสเตอร์อื่น
            while (!positionFound && attempt < 100) // จำกัดการพยายามหาตำแหน่งเพื่อหลีกเลี่ยงการวนลูปไม่สิ้นสุด
            {
                attempt++;

                // สุ่มตำแหน่งภายในขอบเขตที่กำหนด
                float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                float randomY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
                spawnPosition = new Vector2(randomX, randomY);

                // ตรวจสอบว่าตำแหน่งนี้ว่างหรือไม่
                Collider2D overlap = Physics2D.OverlapCircle(spawnPosition, checkRadius);
                if (overlap == null)
                {
                    positionFound = true;

                    // สุ่มเลือก prefab มอนสเตอร์
                    GameObject randomPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

                    // สร้างมอนสเตอร์ที่ตำแหน่งที่สุ่มได้
                    Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                }
            }

            if (!positionFound)
            {
                Debug.LogWarning("ไม่สามารถหาตำแหน่งว่างให้มอนสเตอร์ตัวที่ " + i + " ได้");
            }
        }
    }
}
