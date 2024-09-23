using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy
{
    [SerializeField] private GameObject bulletPrefab;

    public void Attack()
    {
        // หาทิศทางเป้าหมาย
        Vector2 targetDirection = (Vector2)Movement.instance.transform.position - (Vector2)transform.position;

        // สร้างโปรเจกไทล์ใหม่
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // ตั้งค่าทิศทางของโปรเจกไทล์
        newBullet.GetComponent<Projectile>().SetDirection(targetDirection.normalized);
    }
}
