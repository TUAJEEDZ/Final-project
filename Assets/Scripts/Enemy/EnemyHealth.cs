using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 100; // จำนวนเริ่มต้นของสุขภาพของศัตรู
    [SerializeField] private GameObject deathVFXPrefab;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth; // ตั้งค่าปัจจุบันของสุขภาพเป็นค่าที่เริ่มต้น
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // ลดจำนวนสุขภาพตามความเสียหายที่รับ
        Debug.Log("Enemy took damage. Current health: " + currentHealth); // แสดงสุขภาพหลังจากรับความเสียหาย

        if (currentHealth <= 0)
        {
            DetectDeath(); // เรียกใช้ฟังก์ชันจัดการเมื่อศัตรูตาย
        }
    }

    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Debug.Log("Enemy is dead. Preparing to die."); // แสดงข้อความเมื่อศัตรูตาย
            Die(); // ตรวจสอบสุขภาพและจัดการเมื่อตาย
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died.");

        // เรียกใช้ฟังก์ชัน DropItem
        EnemyDrop enemyDrop = GetComponent<EnemyDrop>();
        if (enemyDrop != null)
        {
            enemyDrop.DropItem();
        }

        Destroy(gameObject); // ทำลาย GameObject ของศัตรู
    }

}
