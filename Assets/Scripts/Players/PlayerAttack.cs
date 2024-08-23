using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f; // ���С������
    public LayerMask enemyLayer; // �������ͧ�ѵ��
    public Transform attackPoint; // �ش����繨ش������鹢ͧ�������
    public float knockbackForce = 5f; // �ç���ᷡ�ͧ�ѵ��

    private Animator animator; // �������ҧ�ԧ�֧ Animator �ͧ����Ф�

    private void Start()
    {
        animator = GetComponent<Animator>(); // �Ѻ��� component Animator
    }

    private void Update()
    {
        // ��Ǩ�ͺ��á���������
        if (Input.GetButtonDown("Fire1"))
        {
            Attack(); // ���¡�ѧ��ѹ����
        }
    }

    void Attack()
    {
        // ��Ǩ�ͺ������駤�� attackPoint �����ѧ
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned."); // ����͹㹡óշ��������駤��
            return;
        }

        // �֧��ҷ�ȷҧ����ѹ˹�Ҩҡ Animator
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        // ��Ѻ�����ع�ͧ attackPoint �����ȷҧ�������
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        if (direction != Vector2.zero)
        {
            attackPoint.localPosition = direction * attackRange;  // ��˹����˹觢ͧ attackPoint �����ȷҧ�������
            attackPoint.right = direction;  // ��ع attackPoint ����ѹ˹�ҵ����ȷҧ�������
        }

        // ��Ǩ�ͺ������ѵ������������������
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        Debug.Log("Attack triggered. Number of enemies hit: " + hitEnemies.Length);

        // �Ѵ��ä���������¡Ѻ�ѵ�ٷ���������������
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>(); // �Ѻ component �آ�Ҿ�ͧ�ѵ��
            Knockback knockback = enemyCollider.GetComponent<Knockback>(); // �Ѻ component ��á��ᷡ�ͧ�ѵ��
            Flash flash = enemyCollider.GetComponent<Flash>(); // �Ѻ component �Ϳ࿡�� Flash �ͧ�ѵ��

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // �觤��������������ѵ��

                if (knockback != null)
                {
                    knockback.GetKnockedBack(transform, knockbackForce); // ������ѵ�١���繡�Ѻ
                }

                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // ���¡��ҹ�Ϳ࿡�� Flash
                }
            }
        }
    }

    // �ѧ��ѹ����Ѻ�ʴ��������� Editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red; // ��駤���բͧ Gizmos
        Gizmos.DrawWireSphere(attackPoint.position, attackRange); // �Ҵǧ��������ʴ���������
    }
}
