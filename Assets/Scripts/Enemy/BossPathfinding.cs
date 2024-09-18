using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPathfinding : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.5f;

    private Rigidbody2D rb;
    private Vector2 moveDir;
    private Knockback knockback;

    private void Awake()
    {
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // หากกำลังถูกกระแทก, หยุดการเคลื่อนไหว
        if (knockback != null && knockback.gettingKnockedBack)
        {
            moveDir = Vector2.zero; // หยุดการเคลื่อนไหว
            return;
        }

        // เคลื่อนไหวตามทิศทางที่กำหนด
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        if (knockback != null && knockback.gettingKnockedBack)
        {
            return; // หากกำลังถูกกระแทก, หยุดการเคลื่อนไหว
        }

        // คำนวณทิศทางการเคลื่อนไหว
        moveDir = (targetPosition - rb.position).normalized;
    }

    public void StopMoving()
    {
        moveDir = Vector2.zero;
    }

    // Method ใหม่เพื่อรับทิศทางการเคลื่อนไหวปัจจุบัน
    public Vector2 GetCurrentDirection()
    {
        return moveDir;
    }
}
