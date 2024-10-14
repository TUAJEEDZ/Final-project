using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackRange = 1f; // ���С������
    public LayerMask enemyLayer; // �������ͧ�ѵ��
    public InventoryManager inventoryManager; // �к� Inventory �ͧ������
    public float knockbackForce = 5f; // �ç���ᷡ�ͧ�ѵ��
    public float attackCooldown = 1f; // �������� cooldown �����ҧ�������

    private Animator animator; // �������ҧ�ԧ�֧ Animator �ͧ����Ф�
    private Vector2 attackDirection; // ��ȷҧ�������
    private DamageSword equippedSword; // �Һ����������ҹ
    private float lastAttackTime = 0f; // ����㹡�����դ�������ش
    private Movement movement;

    private void Start()
    {
        animator = GetComponent<Animator>(); // �Ѻ��� component Animator
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
                // �֧�����ŴҺ�ҡ��ͧ toolbar
                Inventory.Slot selectedSlot = inventoryManager.toolbar.selectedSlot;

                if (selectedSlot != null && selectedSlot.itemName != "")
                {
                    Item item = GameManager.instance.itemManager.GetItemByName(selectedSlot.itemName);

                    if (item != null)
                    {
                        equippedSword = item.GetComponent<DamageSword>();

                        if (equippedSword != null)
                        {
                            Attack(); // ���¡�ѧ��ѹ����
                            movement.ChangeState(PlayerState.interact);
                            animator.SetTrigger("IsAttacking");
                            lastAttackTime = Time.time; // �ѹ�֡���ҷ�����դ�������ش

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

        equippedSword.Attack(); // ���͹����ѹ����

        foreach (Collider2D enemyCollider in hitEnemies)
        {
            EnemyHealth enemyHealth = enemyCollider.GetComponent<EnemyHealth>(); // �Ѻ component �آ�Ҿ�ͧ�ѵ��
            BossHealth bossHealth = enemyCollider.GetComponent<BossHealth>(); // �Ѻ component �آ�Ҿ�ͧ�ѵ��
            Knockback knockback = enemyCollider.GetComponent<Knockback>(); // �Ѻ component ��á��ᷡ�ͧ�ѵ��
            Flash flash = enemyCollider.GetComponent<Flash>(); // �Ѻ component �Ϳ࿡�� Flash �ͧ�ѵ��

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(equippedSword.damage); // ���� damage �ҡ�Һ������͡

                if (knockback != null)
                {
                    knockback.GetKnockedBack(transform, knockbackForce); // ������ѵ�١���繡�Ѻ
                }

                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // ���¡��ҹ�Ϳ࿡�� Flash
                }
            }
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(equippedSword.damage); // ���� damage �ҡ�Һ������͡

               /* if (knockback != null)
                {
                    knockback.GetKnockedBack(transform, knockbackForce); // ������ѵ�١���繡�Ѻ
                }*/

                if (flash != null)
                {
                    StartCoroutine(flash.FlashRoutine()); // ���¡��ҹ�Ϳ࿡�� Flash
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // ��駤���բͧ Gizmos
        Gizmos.DrawWireSphere(transform.position + (Vector3)attackDirection * attackRange, attackRange); // �Ҵǧ��������ʴ���������
    }
}
