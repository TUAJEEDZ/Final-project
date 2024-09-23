using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;

    public void Attack()
    {
        // �ҷ�ȷҧ�������
        Vector2 targetDirection = (Vector2)Movement.instance.transform.position - (Vector2)transform.position;

        // ���ҧ��ਡ�������
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // ��駤�ҷ�ȷҧ�ͧ��ਡ���
        newBullet.GetComponent<Projectile>().SetDirection(targetDirection.normalized);
    }
}
