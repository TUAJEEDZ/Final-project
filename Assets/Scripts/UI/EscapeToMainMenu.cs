using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeToMainMenu : MonoBehaviour
{
    void Update()
    {
        // ��Ǩ�ͺ��á����� Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.LogWarning("��������蹺�");
            GameManager.instance.mapManager.SetUiOn(false);
            // ������ѧ Scene MainMenu
            SceneManager.LoadScene("MainMenu");
        }
    }
}
