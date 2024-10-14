using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f; // ระยะการโจมตี
    public LayerMask enemyLayer; // เลเยอร์ของศัตรู
    public InventoryManager inventoryManager; // ระบบ Inventory ของผู้เล่น
    public float knockbackForce = 5f; // แรงกระแทกของศัตรู
    public float attackCooldown = 1f; // ระยะเวลา cooldown ระหว่างการโจมตี

    private Animator animator; // ตัวแปรอ้างอิงถึง Animator ของตัวละคร
    private Vector2 attackDirection; // ทิศทางการโจมตี
    private DamageSword equippedSword; // ดาบที่ผู้เล่นใช้งาน
    private float lastAttackTime = 0f; // เวลาในการโจมตีครั้งล่าสุด
    private Movement movement;

    private void Start()
    {
        animator = GetComponent<Animator>(); // รับค่า component Animator
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
                // ดึงข้อมูลดาบจากช่อง toolbar
                Inventory.Slot selectedSlot = inventoryManager.toolbar.selectedSlot;

                if (selectedSlot != null && selectedSlot.itemName != "")
                {
                    Item item = GameManager.instance.itemManager.GetItemByName(selectedSlot.itemName);

                    if (item != null)
                    {
                        equippedSword = item.GetComponent<DamageSword>();

                        if (equippedSword != null)
                        {
                            Attack(); // เรียกฟังก์ชันโจมตี
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("IsAttacking");
                            lastAttackTime = Time.time; // บันทึกเวลาที่โจมตีครั้งล่าสุด

                            StartCoroutine(DelayedInteraction());
                            IEnumerator DelayedInteraction()
                            {
                                yield return new WaitForSeconds(0.5f);
                                movement.ChangeState(PlayerState.walk);

                            }
                        }
                        else
                        {
                            Debug.LogWarning("Selected item is not a DamageSword.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No item found by the selected item name.");
                    }
                }
                else
                {
                    Debug.LogWarning("No item selected.");
                }
            }
        }
    }

    void Attack()
    {
        if (equippedSword == null)
        {
            Debug.LogWarning("No sword equipped.");
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

        equippedSword.Attack(); // เล่นอนิเมชันโจมตี

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>(); // รับ component สุขภาพของศัตรู
            BossHealth bossHealth = enemyCollider.GetComponent<BossHealth>(); // รับ component สุขภาพของศัตรู
            Knockback knockback = enemyCollider.GetComponent<Knockback>(); // รับ component การกระแทกของศัตรู
            Flash flash = enemyCollider.GetComponent<Flash>(); // รับ component เอฟเฟกต์ Flash ของศัตรู

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(equippedSword.damage); // ใช้ค่า damage จากดาบที่เลือก

                if (knockback != null)
                {
                    knockback.GetKnockedBack(transform, knockbackForce); // ทำให้ศัตรูกระเด็นกลับ
                }

                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // เรียกใช้งานเอฟเฟกต์ Flash
                }
            }
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(equippedSword.damage); // ใช้ค่า damage จากดาบที่เลือก

               /* if (knockback != null)
                {
                    knockback.GetKnockedBack(transform, knockbackForce); // ทำให้ศัตรูกระเด็นกลับ
                }*/

                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // เรียกใช้งานเอฟเฟกต์ Flash
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // ตั้งค่าสีของ Gizmos
        Gizmos.DrawWireSphere(transform.position + (Vector3)attackDirection * attackRange, attackRange); // วาดวงกลมเพื่อแสดงระยะโจมตี
    }
}
