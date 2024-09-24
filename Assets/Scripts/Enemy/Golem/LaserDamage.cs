using System.Collections;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageDelay = 1f; // เวลาหน่วงก่อนทำดาเมจ

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ตรวจสอบว่าชนกับผู้เล่น
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ApplyDamageAfterDelay(other));
        }
    }

    private IEnumerator ApplyDamageAfterDelay(Collider2D player)
    {
        // รอเวลาที่กำหนดไว้
        yield return new WaitForSeconds(damageDelay);

        // ทำดาเมจให้ผู้เล่นหลังจากเวลาหน่วง
        PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            Debug.Log("เลเซอร์โดนผู้เล่นหลังจากเวลาหน่วง!");
        }
    }
}
