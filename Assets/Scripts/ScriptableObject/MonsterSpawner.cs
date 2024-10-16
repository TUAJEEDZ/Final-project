using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject[] monsterPrefabs; // ��С�ȵ��������Ѻ�͹�����
    public GameObject[] mineralPrefabs; // ��С�ȵ��������Ѻ���
    public int numberOfMonsters; // �ӹǹ�͹��������ͧ���ҧ
    public int numberOfMinerals; // �ӹǹ������ͧ���ҧ

    public void SpawnObjects(GameObject[] prefabs, int numberOfObjects)
    {
        // �ͨԡ����Ѻ��� spawn
        for (int i = 0; i < numberOfObjects; i++)
        {
            int randomIndex = Random.Range(0, prefabs.Length);
            Instantiate(prefabs[randomIndex], GetRandomPosition(), Quaternion.identity);
        }
    }

    private Vector3 GetRandomPosition()
    {
        // �������˹� (��Ѻ�����������Ѻ��鹷������ͧ�س)
        return new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }
}
