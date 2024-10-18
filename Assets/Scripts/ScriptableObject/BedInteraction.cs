using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ������Ѻ Image � Canvas

public class BedInteraction : MonoBehaviour
{
    private bool isNearBed = false;
    private DayNightCycle dayNightCycle;
    private Movement movement;
    public Image fadeImage; // Image ����������Ѻ࿴
    public Text interactionText; // UI Text for interaction
    public Image interactionImage; // UI Image for interaction
    private bool playerInTrigger = false;

    void Start()
    {
        // ���� DayNightCycle 㹩ҡ
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
                movement.ChangeState(PlayerState.interact); // ��ش�����蹢�Ѻ
                StartCoroutine(FadeAndSleep()); // ���¡�� Coroutine ����Ѻ࿴����������
            }
        }
    }

    private IEnumerator FadeAndSleep()
    {
        // ࿴�����մ�
        yield return StartCoroutine(FadeToBlack());

        // ������Ң�й͹��Ѻ
        yield return StartCoroutine(SpeedUpTime());

        // ࿴��Ѻ����Ҿ���
        yield return StartCoroutine(FadeToClear());

        // ����Ͷ֧���� 6 ���������� �����蹨�����ö��Ѻ���ա����
        movement.ChangeState(PlayerState.walk);
    }

    private IEnumerator FadeToBlack()
    {
        float fadeDuration = 2f; // ��������㹡��࿴���մ�
        Color fadeColor = fadeImage.color;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            fadeColor.a = Mathf.Lerp(0, 1, normalizedTime); // ���� alpha �ҡ 0 �֧ 1
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 1;
        fadeImage.color = fadeColor; // ������������ alpha �� 1
    }

    private IEnumerator FadeToClear()
    {
        float fadeDuration = 2f; // ��������㹡��࿴��Ѻ�����Ҿ����
        Color fadeColor = fadeImage.color;

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            fadeColor.a = Mathf.Lerp(1, 0, normalizedTime); // Ŵ alpha �ҡ 1 �� 0
            fadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 0;
        fadeImage.color = fadeColor; // ������������ alpha �� 0
    }

    private IEnumerator SpeedUpTime()
    {
        float originalDayDuration = dayNightCycle.dayDuration;
        dayNightCycle.dayDuration = 5f; // �������������ǡ��һ���

        // �ͨ��������Ҩж֧ 6:00 AM
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

