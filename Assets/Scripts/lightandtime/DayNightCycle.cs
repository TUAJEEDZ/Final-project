using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections.Generic;

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight2D;
    public List<Light2D> spotLights2D;
    public Color dayColor = Color.white;
    public Color nightColor = Color.blue;
    public Color transparentColor = new Color(0, 0, 0, 0); // สีโปร่งใส
    public bool flas = true;
    public float dayDuration = 1800f; // 30 นาที
    public Text timeText;
    private float time;

    void Start()
    {
        time = 0.25f; // ตั้งค่าเริ่มต้นเวลาเป็น 06:00 น.

        if (globalLight2D == null)
        {
            globalLight2D = FindObjectOfType<Light2D>();
            if (globalLight2D == null)
            {
                Debug.LogWarning("ไม่พบ Global Light 2D ใน Scene กรุณากำหนดไว้ใน Inspector.");
            }
        }

        if (spotLights2D == null || spotLights2D.Count == 0)
        {
            spotLights2D = new List<Light2D>(FindObjectsOfType<Light2D>());
            spotLights2D.Remove(globalLight2D);
            if (spotLights2D.Count == 0)
            {
                Debug.LogWarning("ไม่พบ Spot Lights 2D ใน Scene กรุณากำหนดไว้ใน Inspector.");
            }
        }

        if (timeText == null)
        {
            Debug.LogWarning("ยังไม่มี UI Text ที่กำหนดเพื่อแสดงเวลา กรุณากำหนดใน Inspector.");
        }
    }

    void Update()
    {
        if (globalLight2D == null || spotLights2D == null || spotLights2D.Count == 0)
        {
            Debug.LogWarning("Global Light 2D หรือ Spot Lights 2D ยังไม่ได้กำหนด.");
            return;
        }

        time += Time.deltaTime / dayDuration;
        time %= 1f;

        // กำหนดสีของ Global Light 2D ด้วยการผสมสีอย่างราบรื่น
        if (time >= 0.833f || time < 0.208f) // 20:00 น. ถึง 05:00 น.
        {
            // ผสมสีระหว่าง nightColor และ transparentColor
            if (time >= 0.833f && time < 0.875f) // 20:00 น. ถึง 21:00 น.
            {
                float t = Mathf.InverseLerp(0.833f, 0.875f, time);
                globalLight2D.color = Color.Lerp(nightColor, transparentColor, t);
            }
            else if (time >= 0.958f || time < 0.042f) // 23:00 น. ถึง 01:00 น.
            {
                globalLight2D.color = transparentColor;
            }
            else if (time >= 0.042f && time < 0.208f) // 01:00 น. ถึง 05:00 น.
            {
                float t = Mathf.InverseLerp(0.042f, 0.208f, time);
                globalLight2D.color = Color.Lerp(transparentColor, nightColor, t);
            }
            else
            {
                globalLight2D.color = nightColor;
            }
        }
        else // 05:00 น. ถึง 20:00 น.
        {
            if (time >= 0.208f && time < 0.292f) // 05:00 น. ถึง 07:00 น.
            {
                float t = Mathf.InverseLerp(0.208f, 0.292f, time);
                globalLight2D.color = Color.Lerp(nightColor, dayColor, t);
            }
            else if (time >= 0.792f && time < 0.833f) // 19:00 น. ถึง 20:00 น.
            {
                float t = Mathf.InverseLerp(0.792f, 0.833f, time);
                globalLight2D.color = Color.Lerp(dayColor, nightColor, t);
            }
            else
            {
                globalLight2D.color = dayColor;
            }
        }

        // เปิด/ปิด Spot Lights 2D ตามเวลา
        foreach (var spotLight in spotLights2D)
        {
            if (time >= 0.833f || time < 0.208f) // เปิดตอนกลางคืน
            {
                spotLight.enabled = flas;
            }
            else // ปิดตอนกลางวัน
            {
                spotLight.enabled = !flas;
            }
        }

        // แสดงเวลาใน UI Text
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(time * 24f);
            int minutes = Mathf.FloorToInt((time * 24f * 60f) % 60f);
            timeText.text = $"Time: {hours:00}:{minutes:00}";
        }
    }
}
