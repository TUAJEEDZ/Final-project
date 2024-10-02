using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public int sceneBuildIndex; // Index ของฉากที่ต้องการย้ายไป
    public Text interactionText; // ลาก UI Text ของคุณมาที่นี่ใน Inspector
    public Image interactionImage; // ลาก UI Image ของคุณมาที่นี่ใน Inspector
    private bool playerInTrigger = false;

    private MapManager mapManager;

    // กำหนดตำแหน่งใหม่โดยใช้ Vector3
    public Vector3 doorExitPosition;

    private void Start()
    {
        // ซ่อนข้อความและภาพเมื่อเริ่มเกม
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;

            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true); // แสดงข้อความเมื่อผู้เล่นเข้าไปในพื้นที่ประตู
            }
            if (interactionImage != null)
            {
                interactionImage.gameObject.SetActive(true); // แสดงภาพเมื่อผู้เล่นเข้าไปในพื้นที่ประตู
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;

            // ตรวจสอบว่า interactionText และ interactionImage ไม่เป็น null ก่อนที่จะเข้าถึง
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(false); // ซ่อนข้อความเมื่อผู้เล่นออกจากพื้นที่ประตู
            }
            else
            {
                Debug.LogWarning("interactionText is null");
            }

            if (interactionImage != null)
            {
                interactionImage.gameObject.SetActive(false); // ซ่อนภาพเมื่อผู้เล่นออกจากพื้นที่ประตู
            }
            else
            {
                Debug.LogWarning("interactionImage is null");
            }
        }
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            // บันทึกตำแหน่งประตูเมื่อออกจากประตู
            PlayerPrefs.SetFloat("DoorPositionX", doorExitPosition.x);
            PlayerPrefs.SetFloat("DoorPositionY", doorExitPosition.y);
            PlayerPrefs.SetFloat("DoorPositionZ", doorExitPosition.z);

            // โหลดฉากใหม่
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);

            if (GameManager.instance.mapManager.IsFarmOn() == false)
            {
                GameManager.instance.mapManager.SetFarmOn(true);
            }
            else
            {
                GameManager.instance.mapManager.SetFarmOn(false);
            }

        }

        
    }

    // ฟังก์ชันนี้จะถูกเรียกเมื่อฉากใหม่ถูกโหลด
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ตรวจสอบว่ามีการบันทึกตำแหน่งของประตูหรือไม่
        if (PlayerPrefs.HasKey("DoorPositionX"))
        {
            // หาตำแหน่งที่บันทึกไว้และย้ายผู้เล่นไปยังตำแหน่งนั้น
            float x = PlayerPrefs.GetFloat("DoorPositionX");
            float y = PlayerPrefs.GetFloat("DoorPositionY");
            float z = PlayerPrefs.GetFloat("DoorPositionZ");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(x, y, z);
            }
            else
            {
                Debug.LogWarning("Player object not found");
            }
        }
    }

    private void OnEnable()
    {
        // สมัคร event เมื่อฉากถูกโหลด
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // ยกเลิกการสมัคร event เมื่อ script นี้ถูกปิด
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
