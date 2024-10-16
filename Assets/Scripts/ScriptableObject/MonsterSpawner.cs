using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // ประกาศตัวแปรสำหรับมอนสเตอร์
    public GameObject[] mineralPrefabs; // ประกาศตัวแปรสำหรับแร่
    public int numberOfMonsters; // จำนวนมอนสเตอร์ที่ต้องสร้าง
    public int numberOfMinerals; // จำนวนแร่ที่ต้องสร้าง

    public void SpawnObjects(GameObject[] prefabs, int numberOfObjects)
    {
        // ลอจิกสำหรับการ spawn
        for (int i = 0; i < numberOfObjects; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            Instantiate(prefabs[randomIndex], GetRandomPosition(), Quaternion.identity);
        }
    }

    private Vector3 GetRandomPosition()
    {
        // สุ่มตำแหน่ง (ปรับให้เหมาะสมกับพื้นที่ในเกมของคุณ)
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }
}
