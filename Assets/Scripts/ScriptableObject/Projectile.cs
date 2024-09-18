using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false; // ������Ѻ�к�����ѹ����ਡ���ͧ�ѵ���������
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private int damage;

    private Vector3 startPosition;
    private Vector2 direction;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        // ��Ǩ�ͺ��ê�੾�СѺ��������ҹ�����������ਡ���ͧ�ѵ��
        if (player != null && isEnemyProjectile && !other.isTrigger)
        {
            // �Ӵ������������
            player.TakeDamage(damage);

            // ���ҧ�Ϳ࿡���������ਡ��쪹
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);

            // �������ਡ�������ͪ��Ѻ������
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        // �������ਡ���������ѹ����͹����Թ���з���˹�
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        // ����͹�����ਡ����㹷�ȷҧ��������
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
}
