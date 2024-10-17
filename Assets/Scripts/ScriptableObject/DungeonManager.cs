using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private List<GameObject> itemsInScene = new List<GameObject>(); // ��¡���红ͧ㹩ҡ

    private void Start()
    {
        SpawnItems(); // ���¡�� SpawnItems ������������
    }

    public void SpawnItems()
    {
        Debug.Log("Spawning new items...");

        MonsterSpawner monsterSpawner = FindObjectOfType<MonsterSpawner>();
        if (monsterSpawner != null)
        {
            // ���ҧ���
            for (int i = 0; i < monsterSpawner.numberOfMinerals; i++)
            {
                GameObject mineral = InstantiateRandom(monsterSpawner.mineralPrefabs);
                if (mineral != null)
                    itemsInScene.Add(mineral);
            }

            // ���ҧ�͹�����
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
        // Logic ����Ѻ����ҵ��˹觡���ԴẺ����
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)); // ��Ѻ�����鹷���Դ��ԧ�ͧ�س
    }

    private void ClearOldItems()
    {
        foreach (GameObject item in itemsInScene) // ᷹�������ͨԡ�ͧ�س�ͧ㹡����Ҷ֧�ͧ
        {
            Destroy(item);
        }
        itemsInScene.Clear(); // ��������¡����ѧ�ҡź
    }
}
