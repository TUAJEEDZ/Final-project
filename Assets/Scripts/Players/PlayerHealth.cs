using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;

    private void Start()
    {
        currentHealth = maxHealth;
        knockback = GetComponent<Knockback>(); // ตรวจสอบให้แน่ใจว่ามีการอ้างถึง Knockback อย่างถูกต้อง
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy && canTakeDamage)
        {
            TakeDamage(1);
            knockback.GetKnockedBack(other.gameObject.transform, knockBackThrustAmount);
        }
    }

    private void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return; // ไม่ลดค่า health ถ้าหากเป็นศูนย์หรือลบไปแล้ว

        canTakeDamage = false;
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            // ทำการจัดการเมื่อ Health เป็นศูนย์ เช่น การตายของตัวละคร
            Debug.Log("Player is dead!");
        }

        StartCoroutine(DamageRecoveryRoutine());
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }
}
