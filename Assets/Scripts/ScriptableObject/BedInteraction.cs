using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    private bool isNearBed = false;
    private DayNightCycle dayNightCycle; // อ้างถึงสคริปต์ DayNightCycle

    void Start()
    {
        // ค้นหา DayNightCycle ในฉาก
        dayNightCycle = FindObjectOfType<DayNightCycle>();
    }

    void Update()
    {
        // ตรวจสอบว่าผู้เล่นอยู่ใกล้เตียงและกดปุ่ม G
        if (isNearBed && Input.GetKeyDown(KeyCode.G))
        {
            Sleep(); // เรียกฟังก์ชันสำหรับการนอน
        }
    }

    // เมื่อผู้เล่นเดินชนกับ Collider ของเตียง
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearBed = true; // ผู้เล่นชนเตียง
            Debug.Log("Player is near the bed. Press 'G' to sleep.");
        }
    }

    // เมื่อผู้เล่นเดินออกจากพื้นที่ Collider ของเตียง
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isNearBed = false; // ผู้เล่นออกจากพื้นที่เตียง
        }
    }

    // ฟังก์ชันเมื่อผู้เล่นนอน
    void Sleep()
    {
        Debug.Log("Player is sleeping...");
        // เร่งเวลาในเกมเมื่อผู้เล่นนอน
        if (dayNightCycle != null)
        {
            StartCoroutine(SpeedUpTime());
        }
    }

    // Coroutine สำหรับการเร่งเวลา
    IEnumerator SpeedUpTime()
    {
        float originalDayDuration = dayNightCycle.dayDuration; // เก็บค่าเวลาปกติ

        // เร่งการเดินของเวลา
        dayNightCycle.dayDuration = 5f; // ลดระยะเวลาของ 1 วันลงเพื่อให้เวลาผ่านไปเร็วขึ้น

        // รอจนกว่าเวลาจะถึง 6:00 AM โดยไม่ข้ามไปวันใหม่
        yield return new WaitUntil(() => dayNightCycle.GetHours() == 6);

        // คืนค่าระยะเวลาของวันกลับสู่ปกติ
        dayNightCycle.dayDuration = originalDayDuration;

        Debug.Log("It's 6:00 AM. Player woke up.");
    }
}
