using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackRotationAngle = 0f; // Additional rotation if needed

    private Animator animator; // Reference to the Animator

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogError("Attack Point is not assigned.");
            return;
        }

        // Get the player's attack direction from the Animator
        float attackDirection = animator.GetFloat("AttackDirection"); // Use an appropriate parameter for direction

        // Rotate the attackPoint based on the player's attack direction
        attackPoint.rotation = Quaternion.Euler(0, 0, attackRotationAngle + attackDirection);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        Debug.Log("Attack triggered. Number of enemies hit: " + hitEnemies.Length);

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(10); // Send damage to the enemy
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
