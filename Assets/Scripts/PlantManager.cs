using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public List<Plant> plants; // Ensure this is initialized
    public Sprite[] wheatGrowthStages;
    void Start()
    {
        plants = new List<Plant>(); // Initialize the list if not done in the Inspector
    }

    public void GrowPlants(int dayCount)
    {
        if (plants == null)
        {
            Debug.LogError("Plants list is null.");
            return;
        }

        foreach (var plant in plants)
        {
            if (plant == null)
            {
                Debug.LogError("Found a null plant in the list.");
                continue; // Skip null plants to avoid exceptions
            }

            plant.Grow(dayCount);
        }
    }

   
}
