using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject 
{
    public string itemName = "Item Name";  // ชื่อของไอเท็ม
    public Sprite icon;                    // ไอคอนของไอเท็ม
}
