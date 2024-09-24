using System.Collections;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
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
        if (player == null) return;

        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, player.position) <= detectRange && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;

        // ���ҧ����������ʴ����˹�
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        Debug.Log($"������١���ҧ�����˹�: {laser.transform.position}");

        Animator laserAnim = laser.GetComponent<Animator>();
        if (laserAnim != null)
        {
            laserAnim.SetTrigger("Fire");
        }

        // �����������ӧҹ����
        yield return new WaitForSeconds(3f);

        // �������������ѧ�ҡ��á�з��������
        Destroy(laser);

        // �ͪ�ǧ��Ŵ�ǹ�
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Optional: visualize detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
