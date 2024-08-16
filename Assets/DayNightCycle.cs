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

        globalLight2D.color = Color.Lerp(nightColor, dayColor, Mathf.PingPong(time * 2f, 1f));

        foreach (var spotLight in spotLights2D)
        {
            if (time > 0.25f && time < 0.80f)
            {
                spotLight.enabled = !flas;
            }
            else
            {
                spotLight.enabled = flas;
            }
        }

        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(time * 24f);
            int minutes = Mathf.FloorToInt((time * 24f * 60f) % 60f);
            timeText.text = $"time: {hours:00}:{minutes:00}";
        }
    }
}
