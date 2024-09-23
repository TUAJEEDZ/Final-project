using System.Collections;
using UnityEngine;

public class GolemBossAi : MonoBehaviour
{
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private MonoBehaviour enemyType; // Ensure this implements IEnemy
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;
    [SerializeField] private float roamChangeDirFloat = 5f;
    [SerializeField] private float chaseRange = 10f;

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
        roamPosition = GetRoamingPosition();
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
        bossPathfinding.MoveTo(roamPosition);
        isWalking = true;

        float distanceToPlayer = Vector2.Distance(transform.position, Movement.instance.transform.position);
        if (distanceToPlayer < attackRange)
        {
            state = State.Attacking;
            isWalking = false;
        }
        else if (distanceToPlayer < chaseRange)
        {
            state = State.ChasingPlayer;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
            timeRoaming = 0f;
        }
    }

    private void HandleChasingPlayer()
    {
        bossPathfinding.MoveTo(Movement.instance.transform.position);
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
            isWalking = false;
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

        if (canAttack)
        {
            canAttack = false;
            (enemyType as IEnemy)?.Attack();

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            if (stopMovingWhileAttacking)
            {
                bossPathfinding.StopMoving();
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        return new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
    }

    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetBool("isMoving", isWalking);

            Vector2 movementDirection = bossPathfinding.GetCurrentDirection();
            animator.SetFloat("Horizontal", movementDirection.x);
            animator.SetFloat("Vertical", movementDirection.y);
        }
    }
}
