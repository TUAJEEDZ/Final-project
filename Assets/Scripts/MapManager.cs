using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    [SerializeField] private GameObject farmMap;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject houseMap;
    [SerializeField] private GameObject dungeon;
    [SerializeField] private GameObject UI;


    private bool isFarmOn = false;
    private bool isDoorOn = false;
    private bool isUiOn = false;


    private void Awake()
    {
        SetFarmOn(true);
    }

    public void SetFarmOn(bool isOn)    
    {

        isFarmOn = isOn;
        farmMap.SetActive(isOn);
        isDoorOn = isOn;
        door.SetActive(isOn);

    }

    public void SetUiOn(bool isOn)
    {
        isUiOn = isOn;
        UI.SetActive(isOn);

    }

    // Method to check the state
    public bool IsFarmOn()
    {
        return isFarmOn;
    }
}
