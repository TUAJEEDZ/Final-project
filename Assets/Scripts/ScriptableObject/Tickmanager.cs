using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tickmanager : MonoBehaviour
{
    public int currentTick = 0;
    //public float tickInterval = 1.0f; // Duration in seconds between each tick

    private void Start()
    {
        // Start the tick system

    }

   /* private IEnumerator TickCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickInterval);
            currentTick++;
        }
    }*/

    public int GetCurrentTick()
    {
        return currentTick;
    }

    public void UpdatecurrentTick()
    {
        currentTick++;
    }
}
