using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] mineralPrefabs; // Array �ͧ mineral prefab
    public GameObject[] monsterPrefabs; // Array �ͧ monster prefab
    public int numberOfMinerals = 5; // �ӹǹ����ͧ������ҧ
    public int numberOfMonsters = 5; // �ӹǹ�͹��������ͧ������ҧ
    public float spawnAreaWidth = 10f; // �������ҧ�ͧ��鹷������
    public float spawnAreaHeight = 10f; // �����٧�ͧ��鹷������
    public float checkRadius = 1f; // �����㹡�õ�Ǩ�ͺ��÷Ѻ�ѹ

    private HashSet<Vector2> usedPositions = new HashSet<Vector2>(); // �Դ������˹觷��������

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

                // �������˹�
                float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                float randomY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
                spawnPosition = new Vector2(randomX, randomY);

                // ��Ǩ�ͺ��÷Ѻ��͹
                Collider2D overlap = Physics2D.OverlapCircle(spawnPosition, checkRadius);
                if (overlap == null && !usedPositions.Contains(spawnPosition))
                {
                    positionFound = true;
                    usedPositions.Add(spawnPosition); // �������˹����¡��������

                    // ���ҧ prefab ���������
                    GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
                    Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                }
            }

            if (!positionFound)
            {
                Debug.LogWarning("�������ö�ҵ��˹���ҧ��� " + prefabs[0].name + " ��Ƿ�� " + i + " ��");
            }
        }
    }
}
