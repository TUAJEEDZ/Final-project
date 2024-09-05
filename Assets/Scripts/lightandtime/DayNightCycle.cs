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
    public bool flash = true; // Renamed for clarity
    public float dayDuration = 1800f; 
    public Text timeText;
    private float time;
    private int dayCount;
    private int monthCount;
    private int yearCount;

    void Start()
    {
        dayCount = PlayerPrefs.GetInt("DayCount", 1);
        monthCount = PlayerPrefs.GetInt("MonthCount", 1);
        yearCount = PlayerPrefs.GetInt("YearCount", 2024); // Set default year to 2024
        time = PlayerPrefs.GetFloat("TimeOfDay", 0.25f); // Default 06:00 AM

        if (globalLight2D == null)
        {
            globalLight2D = FindObjectOfType<Light2D>();
            if (globalLight2D == null)
            {
                Debug.LogWarning("Global Light 2D not found. Please assign it in the Inspector.");
            }
        }

        if (spotLights2D == null || spotLights2D.Count == 0)
        {
            spotLights2D = new List<Light2D>(FindObjectsOfType<Light2D>());
            spotLights2D.Remove(globalLight2D);
            if (spotLights2D.Count == 0)
            {
                Debug.LogWarning("Spot Lights 2D not found. Please assign them in the Inspector.");
            }
        }

        if (timeText == null)
        {
            Debug.LogWarning("UI Text for displaying time is not assigned. Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (globalLight2D == null || spotLights2D == null || spotLights2D.Count == 0)
        {
            Debug.LogWarning("Global Light 2D or Spot Lights 2D not assigned.");
            return;
        }

        if (timeText == null)
        {
            Debug.LogWarning("timeText is not assigned in Inspector.");
            return;
        }

        time += Time.deltaTime / dayDuration;
        if (time >= 1f)
        {
            time -= 1f;
            dayCount++;
            if (dayCount > 30) // Assuming each month has 30 days
            {
                dayCount = 1;
                monthCount++;
                if (monthCount > 12)
                {
                    monthCount = 1;
                    yearCount++;
                }
            }
        }

        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetInt("MonthCount", monthCount);
        PlayerPrefs.SetInt("YearCount", yearCount);
        PlayerPrefs.SetFloat("TimeOfDay", time);
        PlayerPrefs.Save();

        // Update global light color
        UpdateGlobalLightColor();

        // Toggle Spot Lights 2D
        foreach (var spotLight in spotLights2D)
        {
            spotLight.enabled = (time >= 0.833f || time < 0.208f) ? flash : !flash;
        }

        // Update UI Text
        int hours = Mathf.FloorToInt(time * 24f);
        int minutes = Mathf.FloorToInt((time * 24f * 60f) % 60f);
        timeText.text = $"Day {dayCount} : {hours:00}:{minutes:00}";
    }

    private void UpdateGlobalLightColor()
    {
        if (time >= 0.833f || time < 0.208f) // Night
        {
            globalLight2D.color = GetNightColor();
        }
        else // Day
        {
            globalLight2D.color = GetDayColor();
        }
    }

    private Color GetNightColor()
    {
        if (time >= 0.833f && time < 0.875f)
        {
            float t = Mathf.InverseLerp(0.833f, 0.875f, time);
            return Color.Lerp(nightColor, transparentColor, t);
        }
        if (time >= 0.958f || time < 0.042f)
        {
            return transparentColor;
        }
        if (time >= 0.042f && time < 0.208f)
        {
            float t = Mathf.InverseLerp(0.042f, 0.208f, time);
            return Color.Lerp(transparentColor, nightColor, t);
        }
        return nightColor;
    }

    private Color GetDayColor()
    {
        if (time >= 0.208f && time < 0.292f)
        {
            float t = Mathf.InverseLerp(0.208f, 0.292f, time);
            return Color.Lerp(nightColor, dayColor, t);
        }
        if (time >= 0.792f && time < 0.833f)
        {
            float t = Mathf.InverseLerp(0.792f, 0.833f, time);
            return Color.Lerp(dayColor, nightColor, t);
        }
        return dayColor;
    }

    public void ResetDayAndTime()
    {
        dayCount = 1;
        monthCount = 1;
        yearCount = 2024; // Reset year to 2024
        time = 0.25f; // 06:00 AM

        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetInt("MonthCount", monthCount);
        PlayerPrefs.SetInt("YearCount", yearCount);
        PlayerPrefs.SetFloat("TimeOfDay", time);
        PlayerPrefs.Save();

        if (timeText != null)
        {
            timeText.text = $"Day {dayCount} : 06:00";
        }
    }

    public int GetDayCount()
    {
        return dayCount;
    }

    public int GetMonthCount()
    {
        return monthCount;
    }

    public int GetYearCount()
    {
        return yearCount;
    }

    public int GetHours()
    {
        return Mathf.FloorToInt(time * 24f);
    }

    public int GetMinutes()
    {
        return Mathf.FloorToInt((time * 24f * 60f) % 60f);
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
