using UnityEngine;

public class DamageSword : MonoBehaviour
{
    public int damage; // ค่า Damage ของดาบ

    //private Animator animator; // ลบตัวแปรนี้

    void Start()
    {
        //animator = GetComponent<Animator>(); // ลบการตั้งค่าตัวแปรนี้
    }

    // ฟังก์ชันโจมตี
    public void Attack()
    {
        // ไม่ต้องเล่นอนิเมชันใดๆ แล้ว
        // ทำได้แค่ทำการโจมตี
    }

    // เมื่อ Player เก็บดาบชนศัตรู จะเกิดการตรวจจับความเสียหาย
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // เข้าถึงสคริปต์ health ของศัตรูและทำความเสียหาย
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // ใช้ค่าความเสียหายของดาบ
            }
        }
    }
}
