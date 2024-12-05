using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToMainMenu : MonoBehaviour
{
    void Update()
    {
        // ตรวจสอบการกดปุ่ม Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.LogWarning("กดอยู่แม่นบ่");
            GameManager.instance.mapManager.SetUiOn(false);
            // ย้ายไปยัง Scene MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
