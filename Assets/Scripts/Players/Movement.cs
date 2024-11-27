using System.Collections;
using UnityEngine;

public enum PlayerState
{
    walk,
    interact,
    dead
}

public class Movement : MonoBehaviour
{
    public static Movement instance;

    public VectorValue startingPosition;
    public float walkSpeed = 3f;
    public float runSpeed = 6f; // Speed when running
    private float currentSpeed;

    public Animator animator;
    public Vector3 direction;
    private SpriteRenderer spriteRenderer;
    private Knockback knockback;

    public AudioSource audioSource; // ตัวแปรสำหรับ AudioSource
    public AudioClip walkingSound; // เสียงเดิน
    public AudioClip runningSound; // เสียงวิ่ง
    private bool isSoundPlaying = false; // ตรวจสอบว่าเสียงกำลังเล่นอยู่หรือไม่
    private bool isRunning = false; // ตรวจสอบว่ากำลังวิ่งหรือเดิน

    public PlayerState currentState { get; private set; } = PlayerState.walk;

    private void Start()
    {
        // Set the starting position from the VectorValue
        transform.position = startingPosition.initialValue;
    }

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

        // Check if Left Shift is held for running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed; // Run
            isRunning = true; // กำลังวิ่ง
        }
        else
        {
            currentSpeed = walkSpeed; // Walk
            isRunning = false; // กำลังเดิน
        }

        direction = new Vector3(horizontal, vertical, 0).normalized;
        transform.position += direction * currentSpeed * Time.deltaTime;

        AnimatorMovement(direction);
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

                // เล่นเสียงเดินหรือวิ่งตามสถานะ
                AudioClip currentClip = walkingSound;
                float pitch = 1.0f; // ค่าความเร็วเสียงปกติ

                if (isRunning)
                {
                    currentClip = runningSound;
                    pitch = 1.4f; // ปรับความเร็วเสียงวิ่งให้เร็วขึ้น
                }

                if (!isSoundPlaying && currentClip != null)
                {
                    audioSource.clip = currentClip;
                    audioSource.loop = true; // ให้เสียงวนซ้ำ
                    audioSource.pitch = pitch; // ตั้งค่าความเร็วเสียง
                    audioSource.Play();
                    isSoundPlaying = true;
                }
                else if (audioSource.clip != currentClip || audioSource.pitch != pitch)
                {
                    // เปลี่ยนเสียงหรือความเร็วเมื่อสถานะเปลี่ยน
                    audioSource.Stop();
                    audioSource.clip = currentClip;
                    audioSource.pitch = pitch; // ตั้งค่าความเร็วเสียง
                    audioSource.Play();
                }
            }
            else
            {
                animator.SetBool("IsMoving", false);

                // หยุดเสียงเมื่อหยุดเดิน/วิ่ง
                if (isSoundPlaying)
                {
                    audioSource.Stop();
                    isSoundPlaying = false;
                }
            }
        }
    }

    public void ChangeState(PlayerState newState)
    {
        currentState = newState;
    }
}
