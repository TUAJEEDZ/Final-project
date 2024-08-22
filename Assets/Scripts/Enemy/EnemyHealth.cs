using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100; // �ӹǹ������鹢ͧ�آ�Ҿ�ͧ�ѵ��
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
            Debug.Log("Enemy is dead. Preparing to die."); // �ʴ���ͤ���������ѵ�ٵ��
            Die(); // ��Ǩ�ͺ�آ�Ҿ��ШѴ�������͵��
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died."); // �ʴ���ͤ���������ѵ�ٶ١�����
        // ������èѴ�������͵�� �� �������͹�����蹡�õ��, ��÷���� GameObject �繵�
        Destroy(gameObject); // ����� GameObject �ͧ�ѵ��
    }
}
