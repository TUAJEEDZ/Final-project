using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DayNightCycle : MonoBehaviour
{
    public Light2D globalLight2D;
    public List<Light2D> spotLights2D;
    public Color dayColor = Color.white;
    public Color nightColor = Color.blue;
    public Color transparentColor = new Color(0, 0, 0, 0);
    public bool flas = true;
    public float dayDuration = 1800f; 
    public Text timeText;
    private float time;
    private int dayCount;

    void Start()
    {
        // โหลดค่า Day และ Time จาก PlayerPrefs
        dayCount = PlayerPrefs.GetInt("DayCount", 1);
        time = PlayerPrefs.GetFloat("TimeOfDay", 0.25f); // Default 06:00 AM

        if (globalLight2D == null)
        {
            globalLight2D = FindObjectOfType<Light2D>();
            if (globalLight2D == null)
            {
                Debug.LogWarning("Global Light 2D ไม่พบ กรุณาใส่ใน Inspector.");
            }
        }

        if (spotLights2D == null || spotLights2D.Count == 0)
        {
            spotLights2D = new List<Light2D>(FindObjectsOfType<Light2D>());
            spotLights2D.Remove(globalLight2D);
            if (spotLights2D.Count == 0)
            {
                Debug.LogWarning("Spot Lights 2D ไม่พบ กรุณาใส่ใน Inspector.");
            }
        }

        if (timeText == null)
        {
            Debug.LogWarning("UI Text สำหรับแสดงเวลาไม่ถูกกำหนด กรุณาใส่ใน Inspector.");
        }
    }

    void Update()
    {
        if (globalLight2D == null || spotLights2D == null || spotLights2D.Count == 0)
        {
            Debug.LogWarning("Global Light 2D หรือ Spot Lights 2D ไม่ถูกกำหนด.");
            return;
        }

        // ตรวจสอบว่ามีการกำหนดค่า timeText หรือไม่
        if (timeText == null)
        {
            Debug.LogWarning("timeText ไม่ถูกกำหนดค่าใน Inspector.");
            return;
        }

        time += Time.deltaTime / dayDuration;
        if (time >= 1f)
        {
            time -= 1f;
            dayCount++; // เพิ่มจำนวนวัน
        }

        // บันทึกค่า Day และ Time ปัจจุบัน
        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetFloat("TimeOfDay", time);
        PlayerPrefs.Save();

        // เปลี่ยนสีของ Global Light 2D อย่างราบรื่น
        if (time >= 0.833f || time < 0.208f) // กลางคืน
        {
            if (time >= 0.833f && time < 0.875f) // เปลี่ยนเป็นกลางคืน
            {
                float t = Mathf.InverseLerp(0.833f, 0.875f, time);
                globalLight2D.color = Color.Lerp(nightColor, transparentColor, t);
            }
            else if (time >= 0.958f || time < 0.042f) // กลางคืนเต็มที่
            {
                globalLight2D.color = transparentColor;
            }
            else if (time >= 0.042f && time < 0.208f) // เปลี่ยนจากกลางคืน
            {
                float t = Mathf.InverseLerp(0.042f, 0.208f, time);
                globalLight2D.color = Color.Lerp(transparentColor, nightColor, t);
            }
            else
            {
                globalLight2D.color = nightColor;
            }
        }
        else // กลางวัน
        {
            if (time >= 0.208f && time < 0.292f) // เปลี่ยนเป็นกลางวัน
            {
                float t = Mathf.InverseLerp(0.208f, 0.292f, time);
                globalLight2D.color = Color.Lerp(nightColor, dayColor, t);
            }
            else if (time >= 0.792f && time < 0.833f) // เปลี่ยนจากกลางวัน
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
            if (time >= 0.833f || time < 0.208f) // กลางคืน
            {
                spotLight.enabled = flas;
            }
            else // กลางวัน
            {
                spotLight.enabled = !flas;
            }
        }

        // แสดงวันและเวลาใน UI Text
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(time * 24f);
            int minutes = Mathf.FloorToInt((time * 24f * 60f) % 60f);
            timeText.text = $"Day {dayCount} : {hours:00}:{minutes:00}";
        }
    }

    // ฟังก์ชันรีเซ็ตค่า Day และ Time
    public void ResetDayAndTime()
    {
        Debug.Log("ResetDayAndTime called");

        // รีเซ็ตค่าในเกม แต่ไม่ลบ PlayerPrefs
        dayCount = 1;
        time = 0.25f; // 06:00 AM

        // บันทึกค่าที่รีเซ็ตไปยัง PlayerPrefs
        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetFloat("TimeOfDay", time);
        PlayerPrefs.Save();

        // อัปเดต UI ทันที
        if (timeText != null)
        {
            timeText.text = $"Day {dayCount} :06:00";
        }

        Debug.Log("Day and Time reset to Day 1, Time 06:00 AM");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DayNightCycle))]
public class DayNightCycleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DayNightCycle script = (DayNightCycle)target;
        if (GUILayout.Button("Reset Day and Time"))
        {
            script.ResetDayAndTime();
        }
    }
}
#endif
