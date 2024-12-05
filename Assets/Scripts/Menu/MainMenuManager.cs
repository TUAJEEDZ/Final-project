using System.Collections;  // เพิ่มบรรทัดนี้
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // สำหรับใช้งาน UI

public class MainMenuManager : MonoBehaviour
{
    // อ้างอิงถึงปุ่ม
    public Button startButton;
    public Button exitButton; // เพิ่มปุ่ม Exit

    // อ้างอิงถึงหน้าโหลดและ UI ที่เกี่ยวข้อง
    public GameObject loadingScreen; // หน้าโหลด (Panel)
    public Slider progressBar;       // Progress Bar
    public Text progressText;        // ข้อความแสดงเปอร์เซ็นต์

    private void Start()
    {
        // เชื่อมฟังก์ชัน StartGame กับปุ่ม startButton
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);

        }
        else
        {
            Debug.LogError("Start Button is not assigned in the Inspector!");
        }

        // เชื่อมฟังก์ชัน ExitGame กับปุ่ม exitButton
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("Exit Button is not assigned in the Inspector!");
        }
    }

    public void StartGame()
    {
        // เริ่มโหลด Scene ใหม่
        Debug.Log("Starting the game...");

        // แสดงหน้าโหลด
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);

        }

        // โหลด Scene ที่ต้องการ
        StartCoroutine(LoadSceneAsync("main"));
        GameManager.instance.mapManager.SetUiOn(true);

    }

    public void ExitGame()
    {
        // ฟังก์ชันสำหรับออกจากเกม
        Debug.Log("Exiting the game...");
        Application.Quit(); // คำสั่งนี้ใช้ได้เมื่อ Build ออกมาเป็นแอปพลิเคชันจริง
    }

    // Coroutine สำหรับการโหลด Scene แบบ Async
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // เริ่มโหลด Scene แบบ Async
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // ขณะกำลังโหลด Scene
        while (!operation.isDone)
        {
            // คำนวณสถานะการโหลด (ค่าระหว่าง 0 ถึง 1)
            float progress = Mathf.Clamp01(operation.progress / 0.5f);

            // อัปเดตค่า Progress Bar
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            // อัปเดตข้อความเปอร์เซ็นต์
            if (progressText != null)
            {
                progressText.text = (progress * 100f).ToString("F0") + "%";
            }

            yield return null; // รอเฟรมถัดไป
        }

        // เมื่อโหลดเสร็จแล้ว
        Debug.Log("Scene Loaded Successfully!");
    }
}
