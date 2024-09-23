using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float detectionRange = 5f;
    public LayerMask playerLayer;
    public float attackCooldown = 1.5f;

    private Transform player; // ใช้ฟิลด์นี้สำหรับการตรวจจับผู้เล่น
    public Transform Player
    {
        get { return player; }
    }

    private bool playerInRange = false;
    public bool PlayerInRange
    {
        get { return playerInRange; }
    }

    private bool canAttack = true;

    private void Update()
    {
        DetectPlayer();
        if (playerInRange)
        {
            ChasePlayer();

            if (Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                if (canAttack)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
        }
    }

    private void DetectPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.transform;
                playerInRange = true;
                return;
            }
        }
        playerInRange = false;
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, 2f * Time.deltaTime);
    }

    IEnumerator AttackPlayer()
    {
        canAttack = false;
        Debug.Log("ศัตรูกำลังโจมตีผู้เล่น!");

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
