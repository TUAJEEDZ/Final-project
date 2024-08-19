using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class npc : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;  
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;

    public Transform[] moveSpots; // จุดที่ NPC จะเดินไป
    public float speed; // ความเร็วในการเดินของ NPC
    private int nextSpotIndex;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nextSpotIndex = 0;
    }

    void Update()
    {
        if (!playerIsClose)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.F) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                ZeroText();
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if(dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    void Move()
    {
        // NPC จะเดินไปยังจุดถัดไป
        if (moveSpots.Length > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[nextSpotIndex].position, speed * Time.deltaTime);

            // หากถึงจุดที่กำหนดไว้แล้วให้เปลี่ยนไปยังจุดถัดไป
            if (Vector2.Distance(transform.position, moveSpots[nextSpotIndex].position) < 0.2f)
            {
                nextSpotIndex = (nextSpotIndex + 1) % moveSpots.Length;
            }
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ZeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            rb.velocity = Vector2.zero; // หยุดการเคลื่อนไหวเมื่อผู้เล่นเข้ามาใกล้
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            ZeroText();
        }
    }
}
