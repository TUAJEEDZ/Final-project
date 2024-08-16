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
    public Color transparentColor = new Color(0, 0, 0, 0);
    public bool flas = true;
    public float dayDuration = 1800f; 
    public Text timeText;
    private float time;

    void Start()
    {
        time = 0.25f; // Set initial time to 06:00 AM

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
                Debug.LogWarning("No Spot Lights 2D found. Please assign them in the Inspector.");
            }
        }

        if (timeText == null)
        {
            Debug.LogWarning("No UI Text assigned for time display. Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        if (globalLight2D == null || spotLights2D == null || spotLights2D.Count == 0)
        {
            Debug.LogWarning("Global Light 2D or Spot Lights 2D not assigned.");
            return;
        }

        time += Time.deltaTime / dayDuration;
        time %= 1f;

        // Smoothly transition the color of Global Light 2D
        if (time >= 0.833f || time < 0.208f) // Night time
        {
            if (time >= 0.833f && time < 0.875f) // Transition to night
            {
                float t = Mathf.InverseLerp(0.833f, 0.875f, time);
                globalLight2D.color = Color.Lerp(nightColor, transparentColor, t);
            }
            else if (time >= 0.958f || time < 0.042f) // Fully night
            {
                globalLight2D.color = transparentColor;
            }
            else if (time >= 0.042f && time < 0.208f) // Transition from night
            {
                float t = Mathf.InverseLerp(0.042f, 0.208f, time);
                globalLight2D.color = Color.Lerp(transparentColor, nightColor, t);
            }
            else
            {
                globalLight2D.color = nightColor;
            }
        }
        else // Day time
        {
            if (time >= 0.208f && time < 0.292f) // Transition to day
            {
                float t = Mathf.InverseLerp(0.208f, 0.292f, time);
                globalLight2D.color = Color.Lerp(nightColor, dayColor, t);
            }
            else if (time >= 0.792f && time < 0.833f) // Transition from day
            {
                float t = Mathf.InverseLerp(0.792f, 0.833f, time);
                globalLight2D.color = Color.Lerp(dayColor, nightColor, t);
            }
            else
            {
                globalLight2D.color = dayColor;
            }
        }

        // Toggle Spot Lights 2D based on time
        foreach (var spotLight in spotLights2D)
        {
            if (time >= 0.833f || time < 0.208f) // Night time
            {
                spotLight.enabled = flas;
            }
            else // Day time
            {
                spotLight.enabled = !flas;
            }
        }

        // Display time in UI Text
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(time * 24f);
            int minutes = Mathf.FloorToInt((time * 24f * 60f) % 60f);
            timeText.text = $"Time: {hours:00}:{minutes:00}";
        }
    }
}
