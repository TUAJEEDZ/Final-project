using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] mineralPrefabs; // Array ของ mineral prefab
    public GameObject[] monsterPrefabs; // Array ของ monster prefab
    public int numberOfMinerals = 5; // จำนวนที่ต้องการสร้าง
    public int numberOfMonsters = 5; // จำนวนมอนสเตอร์ที่ต้องการสร้าง
    public float spawnAreaWidth = 10f; // ความกว้างของพื้นที่สุ่ม
    public float spawnAreaHeight = 10f; // ความสูงของพื้นที่สุ่ม
    public float checkRadius = 1f; // รัศมีในการตรวจสอบการทับกัน

    private HashSet<Vector2> usedPositions = new HashSet<Vector2>(); // ติดตามตำแหน่งที่ใช้แล้ว

    private void Start()
    {
        SpawnObjects(mineralPrefabs, numberOfMinerals);
        SpawnObjects(monsterPrefabs, numberOfMonsters);
    }

    void SpawnObjects(GameObject[] prefabs, int numberOfObjects)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector2 spawnPosition;
            int attempt = 0;
            bool positionFound = false;

            while (!positionFound && attempt < 100)
            {
                attempt++;

                // สุ่มตำแหน่ง
                float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                float randomY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
                spawnPosition = new Vector2(randomX, randomY);

                // ตรวจสอบการทับซ้อน
                Collider2D overlap = Physics2D.OverlapCircle(spawnPosition, checkRadius);
                if (overlap == null && !usedPositions.Contains(spawnPosition))
                {
                    positionFound = true;
                    usedPositions.Add(spawnPosition); // เพิ่มตำแหน่งในรายการใช้แล้ว

                    // สร้าง prefab ที่สุ่มได้
                    GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
                    Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                }
            }

            if (!positionFound)
            {
                Debug.LogWarning("ไม่สามารถหาตำแหน่งว่างให้ " + prefabs[0].name + " ตัวที่ " + i + " ได้");
            }
        }
    }
}
