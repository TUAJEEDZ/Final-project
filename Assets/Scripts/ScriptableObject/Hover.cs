using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float hoverSpeed = 1f; // Speed of the hover
    public float hoverHeight = 0.1f; // Height of the hover

    private Vector3 startPosition; // Starting position of the collectable
   
    private void Awake()
    {
        startPosition = transform.position; // Store the initial position
    }
    private void Update()
    {
        Hovere();
    }

    private void Hovere()
    {
        //This line calculates the new Y position of the object to create a hovering effect.
        float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);

    }
}
