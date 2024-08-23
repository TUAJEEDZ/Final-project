using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f; // ระยะการโจมตี
    public LayerMask enemyLayer; // เลเยอร์ของศัตรู
    public Transform attackPoint; // จุดที่เป็นจุดเริ่มต้นของการโจมตี
    public float knockbackForce = 5f; // แรงกระแทกของศัตรู

    private Animator animator; // ตัวแปรอ้างอิงถึง Animator ของตัวละคร

    private void Start()
    {
        animator = GetComponent<Animator>(); // รับค่า component Animator
    }

    private void Update()
    {
        // ตรวจสอบการกดปุ่มโจมตี
        if (Input.GetButtonDown("Fire1"))
        {
            Attack(); // เรียกฟังก์ชันโจมตี
        }
    }

    void Attack()
    {
        // ตรวจสอบว่าได้ตั้งค่า attackPoint หรือยัง
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned."); // แจ้งเตือนในกรณีที่ไม่ได้ตั้งค่า
            return;
        }

        // ดึงค่าทิศทางการหันหน้าจาก Animator
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        // ปรับการหมุนของ attackPoint ตามทิศทางการโจมตี
        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        if (direction != Vector2.zero)
        {
            attackPoint.localPosition = direction * attackRange;  // กำหนดตำแหน่งของ attackPoint ตามทิศทางและระยะ
            attackPoint.right = direction;  // หมุน attackPoint ให้หันหน้าตามทิศทางการโจมตี
        }

        // ตรวจสอบว่ามีศัตรูในระยะโจมตีหรือไม่
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        Debug.Log("Attack triggered. Number of enemies hit: " + hitEnemies.Length);

        // จัดการความเสียหายกับศัตรูที่อยู่ในระยะโจมตี
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>(); // รับ component สุขภาพของศัตรู
            Knockback knockback = enemyCollider.GetComponent<Knockback>(); // รับ component การกระแทกของศัตรู
            Flash flash = enemyCollider.GetComponent<Flash>(); // รับ component เอฟเฟกต์ Flash ของศัตรู

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // ส่งความเสียหายให้ศัตรู

                if (knockback != null)
                {
                    knockback.GetKnockedBack(transform, knockbackForce); // ทำให้ศัตรูกระเด็นกลับ
                }

                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // เรียกใช้งานเอฟเฟกต์ Flash
                }
            }
        }
    }

    // ฟังก์ชันสำหรับแสดงการโจมตีใน Editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red; // ตั้งค่าสีของ Gizmos
        Gizmos.DrawWireSphere(attackPoint.position, attackRange); // วาดวงกลมเพื่อแสดงระยะโจมตี
    }
}
