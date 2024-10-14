using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // �ӹǹ������鹢ͧ�آ�Ҿ���
    private float currentHealth;
    [SerializeField] private GameObject monsterPrefab; // Prefab �ͧ�͹�������� spawn
    [SerializeField] private GameObject deathVFXPrefab; // �Ϳ࿡������ͺ�ʵ��
    [SerializeField] private float spawnRadius = 5f; // ������ҧ�ҡ���㹡�� spawn �͹�����
    [SerializeField] private int numMonstersToSpawn = 3; // �ӹǹ�͹�������� spawn
    private bool hasSpawnedMonsters = false; // ������� spawn �͹����������ѧ

    void Start()
    {
        currentHealth = maxHealth; // ��駤�Ҩӹǹ�آ�Ҿ�Ѩ�غѹ�繨ӹǹ�٧�ش
    }

    void Update()
    {
        // ��Ǩ�ͺ���ʹ������ spawn �͹�������������ʹ���¡��� 50%
        if (currentHealth <= maxHealth * 0.5f && !hasSpawnedMonsters)
        {
            SpawnMonsters();
            hasSpawnedMonsters = true; // ����� spawn ���������
        }
    }

    // �ѧ��ѹ����Ѻ������ҧ�͹�����
    void SpawnMonsters()
    {
        HashSet<Vector2> spawnedPositions = new HashSet<Vector2>(); // ������Ѻ���������͹�Ѻ�ѹ

        for (int i = 0; i < numMonstersToSpawn; i++)
        {
            Vector2 spawnPosition;

            // �������ҵ��˹觷������͹�Ѻ�Ѻ�͹����������ҧ��͹˹�ҹ��
            do
            {
                // �������˹������ spawnRadius �ͺ� ��ҹ˹�Һ��
                float offsetX = Random.Range(1f, spawnRadius); // ����Դ��ҹ˹�Һ��
                float offsetY = Random.Range(-spawnRadius, spawnRadius); // ��������� Y

                spawnPosition = new Vector2(transform.position.x + offsetX, transform.position.y + offsetY);

            } while (spawnedPositions.Contains(spawnPosition)); // ��Ǩ�ͺ��ҵ��˹������

            // �ѹ�֡���˹觷���Դ���
            spawnedPositions.Add(spawnPosition);

            // ���ҧ�͹���������˹觷��������
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // �ѧ��ѹ�Ѻ�����������
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // Ŵ�ӹǹ�آ�Ҿ�����Ҥ����������
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // ������آ�Ҿ����ӡ��� 0

        Debug.Log("Boss took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            DetectDeath(); // ���¡����ͺ�ʵ��
        }
    }

    // �ѧ��ѹ����Ѻ��Ǩ�ͺ��õ��
    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Debug.Log("Boss is dead.");
            Die();
        }
    }

    // �ѧ��ѹ����Ѻ�Ѵ�������ͺ�ʵ��
    private void Die()
    {
        Debug.Log("Boss died.");
        Destroy(gameObject); // ����º��������آ�Ҿ�� 0
    }
}
