using System.Collections;
using UnityEngine;

public class GolemBossAi : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType; // Ensure this implements IEnemy
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float roamChangeDirFloat = 5f;
    [SerializeField] private float chaseRange = 10f;

    [SerializeField] private float roamAmplitude = 2f; // Amplitude of vertical movement
    [SerializeField] private float roamSpeed = 1f; // Speed of vertical movement

    private bool canAttack = true;
    private bool isWalking = false;

    private enum State
    {
        Roaming,
        ChasingPlayer,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private BossPathfinding bossPathfinding;
    private Animator animator;

    private void Awake()
    {
        bossPathfinding = GetComponent<BossPathfinding>();
        animator = GetComponent<Animator>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = new Vector2(transform.position.x, transform.position.y); // Start at current Y position
    }

    private void Update()
    {
        if (Movement.instance == null) return;

        UpdateState();
        UpdateAnimator();
    }

    private void UpdateState()
    {
        switch (state)
        {
            case State.Roaming:
                HandleRoaming();
                break;
            case State.ChasingPlayer:
                HandleChasingPlayer();
                break;
            case State.Attacking:
                HandleAttacking();
                break;
        }
    }

    private void HandleRoaming()
    {
        timeRoaming += Time.deltaTime;

        // Keep the X position fixed and oscillate in the Y direction
        roamPosition.y = transform.position.y + Mathf.Sin(timeRoaming * roamSpeed) * roamAmplitude;

        bossPathfinding.MoveTo(new Vector2(transform.position.x, roamPosition.y));
        isWalking = true;

        float distanceToPlayer = Vector2.Distance(transform.position, Movement.instance.transform.position);
        if (distanceToPlayer < attackRange)
        {
            state = State.Attacking;
            isWalking = false; // Set walking to false when attacking
        }
        else if (distanceToPlayer < chaseRange)
        {
            state = State.ChasingPlayer;
        }
    }

    private void HandleChasingPlayer()
    {
        // Keep the X position fixed and move vertically toward the player's Y position
        bossPathfinding.MoveTo(new Vector2(transform.position.x, Movement.instance.transform.position.y));
        isWalking = true;

        float distanceToPlayer = Vector2.Distance(transform.position, Movement.instance.transform.position);
        if (distanceToPlayer > chaseRange)
        {
            state = State.Roaming;
            isWalking = false;
        }
        else if (distanceToPlayer < attackRange)
        {
            state = State.Attacking;
            isWalking = false; // Set walking to false when attacking
        }
    }

    private void HandleAttacking()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Movement.instance.transform.position);

        if (distanceToPlayer > attackRange)
        {
            state = State.Roaming;
            return;
        }

        // Keep the X position fixed and move vertically toward the player's Y position while attacking
        bossPathfinding.MoveTo(new Vector2(transform.position.x, Movement.instance.transform.position.y));

        if (canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy)?.Attack();

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", isWalking);

            // Set animator parameters based on the movement direction
            Vector2 movementDirection = bossPathfinding.GetCurrentDirection();
            animator.SetFloat("Horizontal", 0); // Always set horizontal to 0 since we are not moving on X-axis
            animator.SetFloat("Vertical", movementDirection.y);
        }
    }
}
