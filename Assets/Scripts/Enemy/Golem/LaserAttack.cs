using System.Collections;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform firePoint;  // Where the laser spawns

    private bool canAttack = true;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("��辺 Player! �ô��Ǩ�ͺ����������ѵ�� Player ���� 'Player'.");
        }
    }

    private void Update()
    {
        if (player == null) return;  // ��ͧ�ѹ��ͼԴ��Ҵ��������ҡ player �� null

        // ��Ǩ�ͺ��Ҽ�������������С�õ�Ǩ�Ѻ�������
        if (Vector3.Distance(transform.position, player.position) <= detectRange && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;

        // Instantiate and animate laser
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        Animator laserAnim = laser.GetComponent<Animator>();

        if (laserAnim != null)
        {
            laserAnim.SetTrigger("Fire");
        }

        // �����������ӧҹ (�� 0.5 �Թҷ�)
        yield return new WaitForSeconds(3f);

        // �����������
        Destroy(laser);

        // �����Ҥ�Ŵ�ǹ�
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ��Ǩ�ͺ��Ҫ��Ѻ Player ��ҹ��
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                // ��Ǩ�ͺ��� playerCollider �դ���๹�� Knockback �������
                Knockback knockback = other.GetComponent<Knockback>();
                if (knockback != null)
                {
                    float knockBackThrust = 5f; // ��˹���ҡ��ᷡ�������ͧ���
                    knockback.GetKnockedBack(transform, knockBackThrust); // �觵��˹�����ç���ᷡ
                }

                // ��Ǩ�ͺ��� playerCollider �դ���๹�� Flash �������
                Flash flash = other.GetComponent<Flash>();
                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // ���¡��ҹ�Ϳ࿡�� Flash
                }
            }
        }
        // ������������ͪ��Ѻ Boss
    }

    // Optional: for visualizing the detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
