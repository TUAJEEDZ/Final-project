using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f; // Attack range
    public LayerMask enemyLayer; // Enemy layer
    public InventoryManager inventoryManager; // Player inventory system
    public float knockbackForce = 5f; // Knockback force for enemies
    public float attackCooldown = 1f; // Cooldown duration between attacks

    private Animator animator; // Reference to character's Animator
    private Vector2 attackDirection; // Attack direction
    private DamageSword equippedSword; // Reference to equipped sword
    private Pickaxedamage equippedPickaxe; // Reference to equipped pickaxe
    private float lastAttackTime = 0f; // Time of the last attack
    private Movement movement;
    private Stamina stamina; // Reference to stamina system

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get Animator component
        stamina = GetComponent<Stamina>(); // Get Stamina component
    }

    private void Awake()
    {
        inventoryManager = GetComponent<InventoryManager>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // Get the item from the selected toolbar slot
                Inventory.Slot selectedSlot = inventoryManager.toolbar.selectedSlot;

                if (selectedSlot != null && !string.IsNullOrEmpty(selectedSlot.itemName))
                {
                    Item item = GameManager.instance.itemManager.GetItemByName(selectedSlot.itemName);

                    if (item != null)
                    {
                        // Check if the item is a sword or a pickaxe
                        equippedSword = item.GetComponent<DamageSword>();
                        equippedPickaxe = item.GetComponent<Pickaxedamage>();

                        if (equippedSword != null)
                        {
                            AttackWithSword(); // ไม่ต้องใช้ stamina สำหรับดาบ
                        }
                        else if (equippedPickaxe != null)
                        {
                            if (stamina.CurrentStamina >= equippedPickaxe.Stamina)
                            // ตรวจสอบ stamina สำหรับ pickaxe เท่านั้น
                            {
                                AttackWithPickaxe(); // ใช้ pickaxe
                                stamina.UseStamina(equippedPickaxe.Stamina); // ใช้ stamina สำหรับ pickaxe
                            }
                            else
                            {
                                Debug.Log("Stamina ไม่พอ!");
                            }
                        }
                    }
                }
            }
        }
    }

    void AttackWithSword()
    {
        Attack(); // Call the attack function
        movement.ChangeState(PlayerState.interact);
        animator.SetTrigger("IsAttacking");
        lastAttackTime = Time.time;

        StartCoroutine(DelayedInteraction());
    }

    void AttackWithPickaxe()
    {
        Attack(); // Call the attack function
        movement.ChangeState(PlayerState.interact);
        animator.SetTrigger("isMining");
        lastAttackTime = Time.time;

        StartCoroutine(DelayedInteraction());
    }

    private IEnumerator DelayedInteraction()
    {
        yield return new WaitForSeconds(0.5f);
        movement.ChangeState(PlayerState.walk);
    }

    void Attack()
    {
        // Check if either equippedSword or equippedPickaxe is null
        if (equippedSword == null && equippedPickaxe == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        attackDirection = new Vector2(horizontal, vertical).normalized;

        if (attackDirection == Vector2.zero)
        {
            Debug.LogWarning("No attack direction found.");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position + (Vector3)attackDirection * attackRange, attackRange, enemyLayer);

        Debug.Log("Attack triggered. Number of enemies hit: " + hitEnemies.Length);

        if (equippedSword != null)
        {
            equippedSword.Attack(); // Play sword attack animation
        }

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>(); // Get enemy health component
            BossHealth bossHealth = enemyCollider.GetComponent<BossHealth>(); // Get boss health component
            MineralHealth mineralHealth = enemyCollider.GetComponent<MineralHealth>(); // Get mineral health component
            Knockback knockback = enemyCollider.GetComponent<Knockback>(); // Get enemy knockback component
            Flash flash = enemyCollider.GetComponent<Flash>(); // Get enemy flash effect component

            if (equippedSword != null && enemyHealth != null)
            {
                enemyHealth.TakeDamage(equippedSword.damage); // Use sword damage for enemies
                HandleKnockbackAndFlash(knockback, flash);
            }
            else if (equippedSword != null && bossHealth != null)
            {
                bossHealth.TakeDamage(equippedSword.damage); // Use sword damage for bosses
                HandleKnockbackAndFlash(knockback, flash);
            }
            else if (equippedPickaxe != null && mineralHealth != null)
            {
                mineralHealth.TakeDamage(equippedPickaxe.damage); // Use pickaxe damage for minerals
            }
        }
    }

    private void HandleKnockbackAndFlash(Knockback knockback, Flash flash)
    {
        if (knockback != null)
        {
            knockback.GetKnockedBack(transform, knockbackForce); // Apply knockback to the enemy
        }

        if (flash != null)
        {
            StartCoroutine(flash.FlashRoutine()); // Trigger flash effect
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the color of the Gizmos
        Gizmos.DrawWireSphere(transform.position + (Vector3)attackDirection * attackRange, attackRange); // Draw a circle to show attack range
    }
}
