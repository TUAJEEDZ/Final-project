using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs;  // Array �ͧ�͹����� prefab ���µ��
    public int numberOfMonsters = 5;  // �ӹǹ�͹��������ͧ���
    public float spawnAreaWidth = 10f;  // �������ҧ�ͧ��鹷������
    public float spawnAreaHeight = 10f;  // �����٧�ͧ��鹷������
    public float checkRadius = 1f;  // �����㹡�õ�Ǩ�ͺ��÷Ѻ�ѹ

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

            // �������ҵ��˹����������Ѻ�Ѻ�͹��������
            while (!positionFound && attempt < 100) // �ӡѴ��þ������ҵ��˹�������ա����§���ǹ�ٻ�������ش
            {
                attempt++;

                // �������˹����㹢ͺࢵ����˹�
                float randomX = Random.Range(-spawnAreaWidth / 2, spawnAreaWidth / 2);
                float randomY = Random.Range(-spawnAreaHeight / 2, spawnAreaHeight / 2);
                spawnPosition = new Vector2(randomX, randomY);

                // ��Ǩ�ͺ��ҵ��˹觹����ҧ�������
                Collider2D overlap = Physics2D.OverlapCircle(spawnPosition, checkRadius);
                if (overlap == null)
                {
                    positionFound = true;

                    // �������͡ prefab �͹�����
                    GameObject randomPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

                    // ���ҧ�͹���������˹觷��������
                    Instantiate(randomPrefab, spawnPosition, Quaternion.identity);
                }
            }

            if (!positionFound)
            {
                Debug.LogWarning("�������ö�ҵ��˹���ҧ����͹������Ƿ�� " + i + " ��");
            }
        }
    }
}
