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

    private void Start()
    {
        currentHealth = maxHealth;
        knockback = GetComponent<Knockback>(); // Ensure Knockback component is attached

        if (healthSlider == null)
        {
            Debug.LogError("HealthSlider reference not set in the Inspector.");
            return;
        }

        UpdateHealthSlider(); // Corrected method name
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
