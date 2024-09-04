using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAi : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChasingPlayer
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Animator animator;
    private EnemyDetection enemyDetection;
    private Coroutine roamingRoutine;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        animator = GetComponent<Animator>();
        enemyDetection = GetComponent<EnemyDetection>();
        state = State.Roaming;
        roamingRoutine = StartCoroutine(RoamingRoutine()); // Start roaming routine
    }

    private void Update()
    {
        if (enemyDetection.PlayerInRange)
        {
            state = State.ChasingPlayer;
            UpdateAnimation(enemyDetection.Player.position);

            // Stop roaming when chasing player
            if (roamingRoutine != null)
            {
                StopCoroutine(roamingRoutine);
                roamingRoutine = null;
            }
        }
        else
        {
            if (state != State.Roaming)
            {
                state = State.Roaming;

                // Restart roaming routine when player is out of range
                if (roamingRoutine == null)
                {
                    roamingRoutine = StartCoroutine(RoamingRoutine());
                }
            }
        }
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingRoutine();
            enemyPathfinding.MoveTo(roamPosition);

            // Update Animator with movement direction
            animator.SetFloat("Horizontal", roamPosition.x);
            animator.SetFloat("Vertical", roamPosition.y);

            // Check if the skeleton is moving
            bool isMoving = roamPosition != Vector2.zero;
            animator.SetBool("isMoving", isMoving);

            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingRoutine()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void UpdateAnimation(Vector3 playerPosition)
    {
        Vector2 direction = (playerPosition - transform.position).normalized;

        // Update Animator with movement direction
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        // Set the isMoving parameter to true since the skeleton is chasing the player
        animator.SetBool("isMoving", true);
    }
}
