using UnityEngine;

public class DamageSword : MonoBehaviour
{
    public int damage; // ��� Damage �ͧ�Һ

    //private Animator animator; // ź����ù��

    void Start()
    {
        //animator = GetComponent<Animator>(); // ź��õ�駤�ҵ���ù��
    }

    // �ѧ��ѹ����
    public void Attack()
    {
        // ����ͧ���͹����ѹ�� ����
        // ������ӡ������
    }

    // ����� Player �纴Һ���ѵ�� ���Դ��õ�Ǩ�Ѻ�����������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // ��Ҷ֧ʤ�Ի�� health �ͧ�ѵ����зӤ����������
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // ���Ҥ���������¢ͧ�Һ
            }
        }
    }
}
