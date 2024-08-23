using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        coll.enabled = false; // Disable the collider at the start
        StartCoroutine(EnableColliderAfterDelay(2f)); // Enable the collider after second
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
                player.inventory.Add("Backpack", item); // Add item to inventory
                Destroy(gameObject); // Destroy the collectable
            }
        }
    }
}
