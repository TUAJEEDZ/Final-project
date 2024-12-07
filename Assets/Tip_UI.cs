using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tip_UI : MonoBehaviour
{
    [SerializeField] private Sprite[] tipPages;
    [SerializeField] private Image tutorialImage;    // Reference to the Image component

    private int currentPage = 0;                   // Track the current tutorial stage

    private void Start()
    {
        UpdateTutorialImage();
    }

    // Method to show the next tutorial stage
    public void ShowNextStage()
    {
        currentPage = (currentPage + 1) % tipPages.Length;
        UpdateTutorialImage();
    }

    // Method to show the previous tutorial stage
    public void ShowPreviousStage()
    {
        currentPage = (currentPage - 1 + tipPages.Length) % tipPages.Length;
        UpdateTutorialImage();
    }

    // Update the sprite in the Image component
    private void UpdateTutorialImage()
    {
        tutorialImage.sprite = tipPages[currentPage];
    }

    public void openTip()
    {
        GameManager.instance.uiManager.ToggleTipUI();
    }
}
