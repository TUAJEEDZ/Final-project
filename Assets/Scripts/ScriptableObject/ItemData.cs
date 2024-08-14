using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]    //use for add new item data
public class ItemData : ScriptableObject 
{
    public string itemName = "Item Name";
    public Sprite icon;
}