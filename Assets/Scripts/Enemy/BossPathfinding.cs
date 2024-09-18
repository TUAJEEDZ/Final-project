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
        // �ҡ���ѧ�١���ᷡ, ��ش�������͹���
        if (knockback != null && knockback.gettingKnockedBack)
        {
            moveDir = Vector2.zero; // ��ش�������͹���
            return;
        }

        // ����͹��ǵ����ȷҧ����˹�
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void MoveTo(Vector2 targetPosition)
    {
        if (knockback != null && knockback.gettingKnockedBack)
        {
            return; // �ҡ���ѧ�١���ᷡ, ��ش�������͹���
        }

        // �ӹǳ��ȷҧ�������͹���
        moveDir = (targetPosition - rb.position).normalized;
    }

    public void StopMoving()
    {
        moveDir = Vector2.zero;
    }

    // Method ���������Ѻ��ȷҧ�������͹��ǻѨ�غѹ
    public Vector2 GetCurrentDirection()
    {
        return moveDir;
    }
}
