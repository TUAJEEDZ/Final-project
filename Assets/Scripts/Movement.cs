using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 2; // Speed of the character
    public Animator animator; // Reference to the Animator component
    public Vector3 direction; // Direction vector for movement
    public VectorValue startingPosition; // Reference to the starting position

    private void Start()
    {
        // Set the starting position from the VectorValue
        transform.position = startingPosition.initialValue;
    }

    private void Update()
    {
        // Get input from the horizontal and vertical axes
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Create a direction vector from the input
        direction = new Vector3(horizontal, vertical, 0).normalized;

        // Move the character based on the direction and speed
        transform.position += direction * speed * Time.deltaTime;

        // Update the animator with the movement direction
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
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
    }
}
