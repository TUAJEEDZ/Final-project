using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false; // Used to specify if this is an enemy projectile
    [SerializeField] private float projectileRange = 10f;
    [SerializeField] private int damage;

    private Vector3 startPosition;
    private Vector2 direction;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();

        // Check for collisions only with the player if this is an enemy projectile
        if (player != null && isEnemyProjectile && !other.isTrigger)
        {
            // Deal damage to the player
            player.TakeDamage(damage);

            // Instantiate the hit effect when the projectile hits something
            Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);

            // Destroy the projectile when it hits the player
            Destroy(gameObject);
        }
    }

    private void DetectFireDistance()
    {
        // Destroy the projectile if it moves beyond the specified range
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        // Move the projectile in the set direction
        transform.Translate(direction * Time.deltaTime * moveSpeed);
    }
}
