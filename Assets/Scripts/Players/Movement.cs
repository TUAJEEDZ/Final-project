using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    dead // เพิ่มสถานะ 'dead' เพื่อจัดการเมื่อผู้เล่นตาย
}

public class Movement : MonoBehaviour
{
    public static Movement instance; // Singleton instance

    public float speed = 0; // Speed of the character
    public Animator animator; // Reference to the Animator component
    public Vector3 direction; // Direction vector for movement
    private SpriteRenderer spriteRenderer;
    private Knockback knockback;

    private PlayerState currentState = PlayerState.walk; // ประกาศและกำหนดค่าเริ่มต้นของ currentState

    private void Awake()
    {
        // Implement the singleton pattern
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

    private void Start()
    {
        // No need to set the starting position now
    }

    private void Update()
    {
        // ถ้าผู้เล่นอยู่ในสถานะ 'dead' หยุดการอัปเดตการเคลื่อนไหว
        if (currentState == PlayerState.dead) return;

        // Get input from the horizontal and vertical axes
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (knockback.gettingKnockedBack) { return; } // Add semicolon here

        // Check for attack input and state
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }

        // Create a direction vector from the input
        direction = new Vector3(horizontal, vertical, 0).normalized;

        // Move the character based on the direction and speed
        transform.position += direction * speed * Time.deltaTime;

        // Update the animator with the movement direction
        AnimatorMovement(direction);
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null; // Wait for one frame
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

    // เมธอดนี้ใช้ในการเปลี่ยนสถานะของผู้เล่น
    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
}
