using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMove_Ref : MonoBehaviour
{
    public int sceneBuildIndex;
    public Text interactionText; // Drag your UI Text component here in the Inspector
    public Image interactionImage; // Drag your UI Image component here in the Inspector
    private bool playerInTrigger = false;

    private void Start()
    {
        // Hide the text and image at the start
        interactionText.gameObject.SetActive(false);
        interactionImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            interactionText.gameObject.SetActive(true); // Show the text when player enters trigger area
            interactionImage.gameObject.SetActive(true); // Show the image when player enters trigger area

          
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            interactionText.gameObject.SetActive(false); // Hide the text when player exits trigger area
            interactionImage.gameObject.SetActive(false); // Hide the image when player exits trigger area
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);
        }
    }
}
