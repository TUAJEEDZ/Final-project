using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public int damageAmount = 1; // ใช้คำว่า 'public' เพื่อให้ค่าถูกปรับได้จาก Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Damage Amount: " + damageAmount); // ตรวจสอบค่าที่ส่งไป
                playerHealth.TakeDamage(damageAmount); // ส่งค่าดาเมจ
            }
        }
    }
}
