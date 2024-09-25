using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] //use for select item data that already added in ItemManager 
public class Item : MonoBehaviour
{
    public ItemData data;

    [HideInInspector]  public Rigidbody2D rb2d;
    public bool isCollected; // เพิ่มสถานะการเก็บ

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
}
