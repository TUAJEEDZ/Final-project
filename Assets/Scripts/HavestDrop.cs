using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavestDrop : MonoBehaviour
{
    public GameObject[] dropItems;
    public float dropChance = 1.0f;
    public int minDropCount = 1;
    public int maxDropCount = 2;

    private Animator animator;

    public void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    // Updated DropItem method to accept direction
    public void DropItem()
    {
        float horizontal = animator.GetFloat("Horizontal");
        float vertical = animator.GetFloat("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical).normalized;

        if (dropItems.Length > 0)
        {
            if (Random.value <= dropChance)
            {
                int dropCount = GetRandomDropCount();

                for (int i = 0; i < dropCount; i++)
                {
                    int randomIndex = Random.Range(0, dropItems.Length);
                    GameObject dropItem = dropItems[randomIndex];

                    Vector3Int position = new Vector3Int(
                        Mathf.RoundToInt(transform.position.x  + direction.x),
                        Mathf.RoundToInt(transform.position.y  + direction.y),
                        0
                    );

                    Instantiate(dropItem, position, Quaternion.identity);

                    Debug.Log("Dropped item: " + dropItem.name);
                }
            }
            else
            {
                Debug.Log("Item did not drop due to drop chance.");
            }
        }
        else
        {
            Debug.LogWarning("No items to drop.");
        }
    }

    private int GetRandomDropCount()
    {
        return Random.Range(minDropCount, maxDropCount + 1);
    }
}
