using UnityEngine;

public class DamageSword : MonoBehaviour
{
    public int damage; //  Damage ของดาบ

    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // ฟังก์ชันโจมตี
    public void Attack()
    {
        
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
