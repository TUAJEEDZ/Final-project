using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public int ticksUntilReset = 3; // �ӹǹ ticks �������絴ѹ���¹
    private int lastResetTick; // ������� ticks ����ش�������
    private DayNightCycle dayNightCycle; // ��С�ȵ���� dayNightCycle
    private List<GameObject> itemsInScene = new List<GameObject>(); // ��¡���红ͧ㹩ҡ
    private int currentTick; // ������� ticks �Ѩ�غѹ

    private void Awake()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>(); // �� FindObjectOfType �����Ѻ�Թ�ᵹ��ͧ DayNightCycle
    }

    private void Start()
    {
        // �֧ ticks ����ش������絨ҡ PlayerPrefs
        lastResetTick = PlayerPrefs.GetInt("LastResetTick", 0);
        currentTick = 0; // ������� ticks ��� 0
        Debug.Log("Last Reset Tick on Start: " + lastResetTick);
        SpawnItems(); // ���¡�� SpawnItems ������������
    }

    public void CheckDungeonReset()
    {
        if (dayNightCycle == null)
        {
            Debug.LogWarning("DayNightCycle is not set in DungeonManager.");
            return; // �͡�ҡ�ѧ��ѹ�ҡ dayNightCycle �� null
        }

        // ��Ǩ�ͺ��Ҽ�ҹ ticks ���������������
        if (currentTick >= lastResetTick + ticksUntilReset)
        {
            ResetDungeon();
        }
    }

    public void IncrementTick()
    {
        currentTick++; // ���� ticks �ء���駷�����¡
        CheckDungeonReset(); // ��Ǩ�ͺ������絴ѹ���¹�ء���駷�� ticks �������
    }

    private void ResetDungeon()
    {
        Debug.Log("Dungeon Reset!");
        ClearOldItems(); // ź�ͧ����͡
        SpawnItems(); // ���ҧ�ͧ����
        lastResetTick = currentTick; // �Ѿഷ Last Reset Tick
        PlayerPrefs.SetInt("LastResetTick", lastResetTick); // �ѹ�֡ Last Reset Tick
        PlayerPrefs.Save(); // �ѹ�֡ PlayerPrefs
    }

    private void ClearOldItems()
    {
        foreach (GameObject item in itemsInScene) // ᷹�������ͨԡ�ͧ�س�ͧ㹡����Ҷ֧�ͧ
        {
            Destroy(item);
        }
        itemsInScene.Clear(); // ��������¡����ѧ�ҡź
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
}
