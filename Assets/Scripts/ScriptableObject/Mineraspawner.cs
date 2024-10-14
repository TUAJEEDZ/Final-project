using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineralspawner : MonoBehaviour
{
    public GameObject[] mineralPrefabs;  // Array ของมอนสเตอร์ prefab หลายตัว
    public int numberOfMinerals = 5;  // จำนวนมอนสเตอร์ที่ต้องการ
    public float spawnAreaWidth = 10f;  // ความกว้างของพื้นที่สุ่ม
    public float spawnAreaHeight = 10f;  // ความสูงของพื้นที่สุ่ม
    public float checkRadius = 1f;  // รัศมีในการตรวจสอบการทับกัน

    private void Start()
    {
        SpawnMinerals();
    }

    void SpawnMinerals()
    {
        for (int i = 0; i < numberOfMinerals; i++)
        {
            Vector2 spawnPosition;
            int attempt = 0;
            bool positionFound = false;

            // 
            while (!positionFound && attempt < 100) // 
            {
                attempt++;

                // 
                float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                float randomY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
                spawnPosition = new Vector2(randomX, randomY);

                // 
                Collider2D overlap = Physics2D.OverlapCircle(spawnPosition, checkRadius);
                if (overlap == null)
                {
                    positionFound = true;

                    //   
                    GameObject randomPrefab = mineralPrefabs[Random.Range(0, mineralPrefabs.Length)];

                    // 
                    Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }
}
