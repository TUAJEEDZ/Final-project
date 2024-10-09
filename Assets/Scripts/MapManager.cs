using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private GameObject farmMap;
    [SerializeField] private GameObject houseMap;

    private bool isFarmOn = false;

    private void Awake()
    {
        SetFarmOn(true);
    }

    public void SetFarmOn(bool isOn)    
    {
        isFarmOn = isOn;
        farmMap.SetActive(isOn);
    }

    // Method to check the state
    public bool IsFarmOn()
    {
        return isFarmOn;
    }
}
