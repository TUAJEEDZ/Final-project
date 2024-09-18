using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100; // �ӹǹ������鹢ͧ�آ�Ҿ�ͧ�ѵ��
    [SerializeField] private GameObject deathVFXPrefab;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth; // ��駤�һѨ�غѹ�ͧ�آ�Ҿ�繤�ҷ���������
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Ŵ�ӹǹ�آ�Ҿ�������������·���Ѻ
        Debug.Log("Enemy took damage. Current health: " + currentHealth); // �ʴ��آ�Ҿ��ѧ�ҡ�Ѻ�����������

        if (currentHealth <= 0)
        {
            DetectDeath(); // ���¡��ѧ��ѹ�Ѵ���������ѵ�ٵ��
        }
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Debug.Log("Enemy is dead. Preparing to die."); // �ʴ���ͤ���������ѵ�ٵ��
            Die(); // ��Ǩ�ͺ�آ�Ҿ��ШѴ�������͵��
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");

        // ���¡��ѧ��ѹ DropItem
        EnemyDrop enemyDrop = GetComponent<EnemyDrop>();
        if (enemyDrop != null)
        {
            enemyDrop.DropItem();
        }

        Destroy(gameObject); // ����� GameObject �ͧ�ѵ��
    }

}
