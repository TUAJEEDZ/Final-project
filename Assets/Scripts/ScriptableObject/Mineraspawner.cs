using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mineralspawner : MonoBehaviour
{
    public GameObject[] mineralPrefabs;  // Array �ͧ�͹����� prefab ���µ��
    public int numberOfMinerals = 5;  // �ӹǹ�͹��������ͧ���
    public float spawnAreaWidth = 10f;  // �������ҧ�ͧ��鹷������
    public float spawnAreaHeight = 10f;  // �����٧�ͧ��鹷������
    public float checkRadius = 1f;  // �����㹡�õ�Ǩ�ͺ��÷Ѻ�ѹ

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
