using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    private bool isNearBed = false;
    private DayNightCycle dayNightCycle; // ��ҧ�֧ʤ�Ի�� DayNightCycle

    void Start()
    {
        // ���� DayNightCycle 㹩ҡ
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    void Update()
    {
        // ��Ǩ�ͺ��Ҽ��������������§��С����� G
        if (isNearBed && Input.GetKeyDown(KeyCode.G))
        {
            Sleep(); // ���¡�ѧ��ѹ����Ѻ��ù͹
        }
    }

    // ����ͼ������Թ���Ѻ Collider �ͧ��§
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearBed = true; // �����蹪���§
            Debug.Log("Player is near the bed. Press 'G' to sleep.");
        }
    }

    // ����ͼ������Թ�͡�ҡ��鹷�� Collider �ͧ��§
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearBed = false; // �������͡�ҡ��鹷����§
        }
    }

    // �ѧ��ѹ����ͼ����蹹͹
    void Sleep()
    {
        Debug.Log("Player is sleeping...");
        // ��������������ͼ����蹹͹
        if (dayNightCycle != null)
        {
            StartCoroutine(SpeedUpTime());
        }
    }

    // Coroutine ����Ѻ����������
    IEnumerator SpeedUpTime()
    {
        float originalDayDuration = dayNightCycle.dayDuration; // �纤�����һ���

        // ��觡���Թ�ͧ����
        dayNightCycle.dayDuration = 5f; // Ŵ�������Ңͧ 1 �ѹŧ����������Ҽ�ҹ����Ǣ��

        // �ͨ��������Ҩж֧ 6:00 AM ����������ѹ����
        yield return new WaitUntil(() => dayNightCycle.GetHours() == 6);

        // �׹����������Ңͧ�ѹ��Ѻ��軡��
        dayNightCycle.dayDuration = originalDayDuration;

        Debug.Log("It's 6:00 AM. Player woke up.");
    }
}
