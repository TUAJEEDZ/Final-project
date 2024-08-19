using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;
    [SerializeField] private Slider healthSlider; // Added [SerializeField] to ensure it can be set in the Inspector

    private int currentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    private void Start()
    {

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>(); // Ensure Knockback component is attached
        currentHealth = maxHealth;


        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider reference not set in the Inspector.");
            return;
        }

        UpdateHealthSlider(); // Corrected method name
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var enemy = other.gameObject.GetComponent<EnemyAI>() as MonoBehaviour ??
                    other.gameObject.GetComponent<SkeletonAi>() as MonoBehaviour;

        if (enemy != null && canTakeDamage)
        {
            TakeDamage(1);
            knockback.GetKnockedBack(other.gameObject.transform, knockBackThrustAmount);
            StartCoroutine(flash.FlashRoutine());
        }
    }


    public void HealPlayer()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            UpdateHealthSlider(); // Corrected method name
        }
    }

    private void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        canTakeDamage = false;
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Debug.Log("Player is dead!");
        }

        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider(); // Corrected method name
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player Death");
        }
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    private void UpdateHealthSlider() // Corrected method name
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
