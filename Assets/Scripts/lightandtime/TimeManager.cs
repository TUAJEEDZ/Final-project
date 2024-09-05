using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance; // Singleton instance

    public float time = 0.25f; // Default time (06:00 AM)
    public int dayCount = 1;
    public float dayDuration = 1800f; // Duration of a day in seconds
    public Text timeText; // UI Text for displaying time

    private void Awake()
    {
        // Implement the singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }

        // Load Day and Time from PlayerPrefs
        dayCount = PlayerPrefs.GetInt("DayCount", 1);
        time = PlayerPrefs.GetFloat("TimeOfDay", 0.25f); // Default 06:00 AM
    }

    private void Update()
    {
        // Update time
        time += Time.deltaTime / dayDuration;
        if (time >= 1f)
        {
            time -= 1f;
            dayCount++;
        }

        // Save Day and Time to PlayerPrefs
        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetFloat("TimeOfDay", time);
        PlayerPrefs.Save();

        // Update the UI Text with the current day and time
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        if (timeText != null)
        {
            int hours = Mathf.FloorToInt(time * 24f);
            int minutes = Mathf.FloorToInt((time * 24f * 60f) % 60f);
            timeText.text = $"Day {dayCount} : {hours:00}:{minutes:00}";
        }
    }

    public void ResetDayAndTime()
    {
        dayCount = 1;
        time = 0.25f; // 06:00 AM

        // Save the reset values to PlayerPrefs
        PlayerPrefs.SetInt("DayCount", dayCount);
        PlayerPrefs.SetFloat("TimeOfDay", time);
        PlayerPrefs.Save();

        // Immediately update the UI with the reset values
        UpdateTimeText();
    }
}
