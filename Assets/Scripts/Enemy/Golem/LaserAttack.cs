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
            Debug.LogError("ไม่พบ Player! โปรดตรวจสอบให้แน่ใจว่าวัตถุ Player มีแท็ก 'Player'.");
        }
    }

    private void Update()
    {
        if (player == null) return;  // ป้องกันข้อผิดพลาดเพิ่มเติมหาก player เป็น null

        // ตรวจสอบว่าผู้เล่นอยู่ในระยะการตรวจจับหรือไม่
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

        // รอให้เลเซอร์ทำงาน (เช่น 0.5 วินาที)
        yield return new WaitForSeconds(3f);

        // ทำลายเลเซอร์
        Destroy(laser);

        // รอเวลาคูลดาวน์
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่าชนกับ Player เท่านั้น
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                // ตรวจสอบว่า playerCollider มีคอมโพเนนต์ Knockback หรือไม่
                Knockback knockback = other.GetComponent<Knockback>();
                if (knockback != null)
                {
                    float knockBackThrust = 5f; // กำหนดค่ากระแทกตามที่ต้องการ
                    knockback.GetKnockedBack(transform, knockBackThrust); // ส่งตำแหน่งและแรงกระแทก
                }

                // ตรวจสอบว่า playerCollider มีคอมโพเนนต์ Flash หรือไม่
                Flash flash = other.GetComponent<Flash>();
                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // เรียกใช้งานเอฟเฟกต์ Flash
                }
            }
        }
        // ไม่ทำอะไรเมื่อชนกับ Boss
    }

    // Optional: for visualizing the detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
