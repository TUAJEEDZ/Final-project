using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3; // Fixed the typo here

    private int currentHealth;
    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
    }

    private void Start()
    {
        currentHealth = startingHealth; // Fixed the typo here
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockback.GetKnockedBack(Movement.instance.transform, 15f); // Fixed the typo here
        DetectDeath(); // Fixed the typo here
    }

    private void DetectDeath() // Fixed the typo here
    {
        if (currentHealth <= 0)
        {
            
            Destroy(gameObject); // Fixed the typo here
        }
    }
}
