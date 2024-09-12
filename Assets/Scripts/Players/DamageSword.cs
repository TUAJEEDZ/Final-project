using UnityEngine;

public class DamageSword : MonoBehaviour
{
    public int damage; //  Damage �ͧ�Һ

    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // �ѧ��ѹ����
    public void Attack()
    {
        
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
