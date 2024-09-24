using System.Collections;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageDelay = 1f; // ����˹�ǧ��͹�Ӵ����

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��Ǩ�ͺ��Ҫ��Ѻ������
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ApplyDamageAfterDelay(other));
        }
    }

    private IEnumerator ApplyDamageAfterDelay(Collider2D player)
    {
        // �����ҷ���˹����
        yield return new WaitForSeconds(damageDelay);

        // �Ӵ��������������ѧ�ҡ����˹�ǧ
        PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("������ⴹ��������ѧ�ҡ����˹�ǧ!");
        }
    }
}
