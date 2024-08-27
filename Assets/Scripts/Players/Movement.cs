using System.Collections;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    dead
}

public class Movement : MonoBehaviour
{
    public static Movement instance;

    public float speed = 0;
    public Animator animator;
    public Vector3 direction;
    private SpriteRenderer spriteRenderer;
    private Knockback knockback;

    public PlayerState currentState { get; private set; } = PlayerState.walk;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        knockback = GetComponent<Knockback>();
    }

    private void Update()
    {
        if (currentState == PlayerState.dead || currentState == PlayerState.interact) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (knockback.gettingKnockedBack) { return; }

        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }

        direction = new Vector3(horizontal, vertical, 0).normalized;
        transform.position += direction * speed * Time.deltaTime;

        AnimatorMovement(direction);
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.33f);
        currentState = PlayerState.walk;
    }

    void AnimatorMovement(Vector3 direction)
    {
        if (animator != null)
        {
            if (direction.magnitude > 0)
            {
                animator.SetBool("IsMoving", true);
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
    }

    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
}
