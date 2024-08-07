using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // Fixed the variable name

    private void OnTriggerEnter2D(Collider2D other) // Fixed the method signature
    {
        // Removed unnecessary semicolon and corrected the method call
        EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damageAmount); // Fixed the method name
        }
    }
}
