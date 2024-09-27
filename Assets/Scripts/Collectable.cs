using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    public static Collectable instance;

    private Collider2D coll;
    public float hoverSpeed = 1f; // Speed of the hover
    public float hoverHeight = 0.1f; // Height of the hover

    private Vector3 startPosition; // Starting position of the collectable

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // Ensure the GameManager persists across scenes
        }

        coll = GetComponent<Collider2D>();
        coll.enabled = false; // Disable the collider at the start
        StartCoroutine(EnableColliderAfterDelay(0.3f)); // Enable the collider after delay
        startPosition = transform.position; // Store the initial position
    }

    private void Update()
    {
        Hover();
    }

    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        coll.enabled = true; // Enable the collider after the delay
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player && coll.enabled)
        {
            Item item = GetComponent<Item>();

            if (item != null)
            {
                player.inventoryManager.Add("Backpack", item); // Add item to inventory
                Destroy(gameObject); // Destroy the collectable
            }
        }
    }

    // Hover up and down in the Y-axis
    private void Hover()
    {
        //This line calculates the new Y position of the object to create a hovering effect.
        float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);
    }
}
