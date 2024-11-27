using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen; // GameObject ของหน้าโหลด (Panel)
    public Slider progressBar;       // Progress Bar (Slider)
    public Text progressText;        // ข้อความแสดงเปอร์เซ็นต์ (Text)

    // ฟังก์ชันที่เรียกใช้เมื่อเริ่มโหลด Scene
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // แสดงหน้าโหลด
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // เริ่มโหลด Scene แบบ Async
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // เมื่อการโหลดยังไม่เสร็จ
        while (!operation.isDone)
        {
            // คำนวณ Progress (0 ถึง 1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // อัปเดต Progress Bar และข้อความ
            if (progressBar != null) progressBar.value = progress;
            if (progressText != null) progressText.text = (progress * 100f).ToString("F0") + "%";

            yield return null; // รอเฟรมถัดไป
        }
    }
}
