using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAi : MonoBehaviour
{
    private enum State
    {
        Roaming
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Animator animator;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>(); // Assign the pathfinding component
        animator = GetComponent<Animator>(); // Assign the animator
        state = State.Roaming; // Initialize the state
        StartCoroutine(RoamingRoutine());
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
}
