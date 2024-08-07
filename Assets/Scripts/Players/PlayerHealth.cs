using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;

    private void Start()
    {
        currentHealth = maxHealth;
        knockback = GetComponent<Knockback>(); // ��Ǩ�ͺ����������ա����ҧ�֧ Knockback ���ҧ�١��ͧ
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy && canTakeDamage)
        {
            TakeDamage(1);
            knockback.GetKnockedBack(other.gameObject.transform, knockBackThrustAmount);
        }
    }

    private void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // ���Ŵ��� health ����ҡ���ٹ������ź�����

        canTakeDamage = false;
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // �ӡ�èѴ�������� Health ���ٹ�� �� ��õ�¢ͧ����Ф�
            Debug.Log("Player is dead!");
        }

        StartCoroutine(DamageRecoveryRoutine());
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
