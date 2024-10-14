using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralHealth : MonoBehaviour
{
    [SerializeField] private int mineralHealth = 10;
    [SerializeField] private GameObject deathVFXPrefab;
    private int currentHealth;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = mineralHealth; 

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; 
        Debug.Log("Enemy took damage. Current health: " + currentHealth); 

        if (currentHealth <= 0)
        {
            DetectDeath();  
        }
    }
    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Debug.Log("Enemy is dead. Preparing to die."); 
            Die(); 
        }
    }
    private void Die()
    {
        Debug.Log("Enemy died.");

        EnemyDrop enemyDrop = GetComponent<EnemyDrop>();
        if (enemyDrop != null)
        {
            enemyDrop.DropItem();
        }

        Destroy(gameObject); 
    }

}
