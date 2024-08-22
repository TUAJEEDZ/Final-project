using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public int damageAmount = 1; // ������ 'public' ��������Ҷ١��Ѻ��ҡ Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Damage Amount: " + damageAmount); // ��Ǩ�ͺ��ҷ�����
                playerHealth.TakeDamage(damageAmount); // �觤�Ҵ����
            }
        }
    }
}
