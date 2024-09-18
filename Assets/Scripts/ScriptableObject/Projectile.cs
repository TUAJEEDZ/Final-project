using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false; // ใช้สำหรับระบุว่ามันเป็นโปรเจกไทล์ของศัตรูหรือไม่
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

        // ตรวจสอบการชนเฉพาะกับผู้เล่นเท่านั้นเมื่อเป็นโปรเจกไทล์ของศัตรู
        if (player != null && isEnemyProjectile && !other.isTrigger)
        {
            // ทำดาเมจให้ผู้เล่น
            player.TakeDamage(damage);

            // สร้างเอฟเฟกต์เมื่อโปรเจกไทล์ชน
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);

            // ทำลายโปรเจกไทล์เมื่อชนกับผู้เล่น
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        // ทำลายโปรเจกไทล์เมื่อมันเคลื่อนที่เกินระยะที่กำหนด
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        // เคลื่อนที่โปรเจกไทล์ไปในทิศทางที่ตั้งไว้
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
}
