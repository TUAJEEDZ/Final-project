using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // เพื่อใช้กับ Image ใน Canvas

public class BedInteraction : MonoBehaviour
{
    private bool isNearBed = false;
    private DayNightCycle dayNightCycle;
    private Movement movement;
    public Image fadeImage; // Image ที่จะใช้สำหรับเฟด
    public Text interactionText; // UI Text for interaction
    public Image interactionImage; // UI Image for interaction
    private bool playerInTrigger = false;

    [SerializeField] private Stamina stamina; // ��ҧ�ԧ��ҹ Inspector
    [SerializeField] private PlayerHealth playerHealth; // ��ҧ�ԧ��ҹ Inspector

    void Start()
    {
        stamina = GetComponent<Stamina>(); // Get Stamina component

        // ค้นหา DayNightCycle ในฉาก
        dayNightCycle = FindObjectOfType<DayNightCycle>();

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        movement = FindObjectOfType<Movement>();
        playerHealth = FindObjectOfType<PlayerHealth>(); // Get Stamina component

        if (movement == null)
        {
            Debug.LogError("Movement script not found on the player or in the scene!");
        }

        if (fadeImage == null)
        {
            Debug.LogError("Fade Image not assigned!");
        }
    }

    void Update()
    {
        if (isNearBed && Input.GetKeyDown(KeyCode.E))
        {
            if (dayNightCycle != null && movement != null)
            {
                movement.ChangeState(PlayerState.interact); // หยุดผู้เล่นขยับ
                StartCoroutine(FadeAndSleep()); // เรียกใช้ Coroutine สำหรับเฟดและเร่งเวลา
                GameManager.instance.stamina.RecoverStamina(100); //  stamina 5 
                playerHealth.HealPlayer(100);
            }
        }
    }

    private IEnumerator FadeAndSleep()
    {
        // เฟดจอเป็นสีดำ
        yield return StartCoroutine(FadeToBlack());

        // เร่งเวลาขณะนอนหลับ
        yield return StartCoroutine(SpeedUpTime());

        // เฟดกลับไปเป็นภาพเดิม
        yield return StartCoroutine(FadeToClear());

        // เมื่อถึงเวลา 6 โมงเช้าแล้ว ผู้เล่นจะสามารถขยับได้อีกครั้ง
        movement.ChangeState(PlayerState.walk);
    }

    private IEnumerator FadeToBlack()
    {
        float fadeDuration = 2f; // ระยะเวลาในการเฟดเป็นสีดำ
        Color fadeColor = fadeImage.color;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            fadeColor.a = Mathf.Lerp(0, 1, normalizedTime); // เพิ่ม alpha จาก 0 ถึง 1
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 1;
        fadeImage.color = fadeColor; // ทำให้มั่นใจว่า alpha เป็น 1
    }

    private IEnumerator FadeToClear()
    {
        float fadeDuration = 2f; // ระยะเวลาในการเฟดกลับมาเป็นภาพปกติ
        Color fadeColor = fadeImage.color;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            fadeColor.a = Mathf.Lerp(1, 0, normalizedTime); // ลด alpha จาก 1 เป็น 0
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 0;
        fadeImage.color = fadeColor; // ทำให้มั่นใจว่า alpha เป็น 0
    }

    private IEnumerator SpeedUpTime()
    {
        float originalDayDuration = dayNightCycle.dayDuration;
        dayNightCycle.dayDuration = 5f; // เร่งเวลาให้เร็วกว่าปกติ

        // รอจนกว่าเวลาจะถึง 6:00 AM
        yield return new WaitUntil(() => dayNightCycle.GetHours() == 6);

        dayNightCycle.dayDuration = originalDayDuration;
        Debug.Log("It's 6:00 AM. Player woke up.");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearBed = true;
            Debug.Log("Player is near the bed. Press 'E' to sleep.");
        }
        playerInTrigger = true;

        // Show interaction UI when player is near
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(true);
        }
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearBed = false;
        }

        playerInTrigger = false;

        // Hide interaction UI when player leaves
        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
        }
        if (interactionImage != null)
        {
            interactionImage.gameObject.SetActive(false);
        }
    }
}

