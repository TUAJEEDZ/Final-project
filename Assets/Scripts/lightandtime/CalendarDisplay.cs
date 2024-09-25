using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class CalendarDisplay : MonoBehaviour
{
    public Text dayCellPrefab;
    public Transform calendarPanel;
    public Text monthNameUI;
    public Color outlineColor = Color.red;

    private DayNightCycle dayNightCycle;
    private bool calendarVisible = false;

    void Start()
    {
        dayNightCycle = FindObjectOfType<DayNightCycle>();

        if (dayNightCycle == null)
        {
            Debug.LogError("DayNightCycle script not found.");
            return;
        }

        if (dayCellPrefab == null || monthNameUI == null || calendarPanel == null)
        {
            Debug.LogError("dayCellPrefab, monthNameUI, or calendarPanel is not assigned.");
            return;
        }

        calendarPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            calendarVisible = !calendarVisible;
            calendarPanel.gameObject.SetActive(calendarVisible);
            monthNameUI.gameObject.SetActive(calendarVisible);

            if (calendarVisible)
            {
                int year = dayNightCycle.GetYearCount();
                int month = dayNightCycle.GetMonthCount();

                UpdateMonthName(year, month);
                GenerateCalendar(year, month);
            }
        }
    }

    void UpdateMonthName(int year, int month)
    {
        if (monthNameUI != null)
        {
            monthNameUI.text = DateTimeFormatInfo.CurrentInfo.GetMonthName(month) + " " + year;
            monthNameUI.fontStyle = FontStyle.Bold;
        }
        else
        {
            Debug.LogError("monthNameUI is not assigned.");
        }
    }

    void GenerateCalendar(int year, int month)
    {
        foreach (Transform child in calendarPanel)
        {
            Destroy(child.gameObject);
        }

        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        int daysInMonth = DateTime.DaysInMonth(year, month);
        int startDay = (int)firstDayOfMonth.DayOfWeek;

        string[] dayNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames;
        for (int i = 0; i < dayNames.Length; i++)
        {
            Text dayNameCell = Instantiate(dayCellPrefab, calendarPanel);
            dayNameCell.text = dayNames[i];
            dayNameCell.fontStyle = FontStyle.Bold;
        }

        for (int i = 0; i < startDay; i++)
        {
            Instantiate(dayCellPrefab, calendarPanel);
        }

        int currentDay = dayNightCycle.GetDayCount();
        Debug.Log("Current day from DayNightCycle: " + currentDay);

        for (int day = 1; day <= daysInMonth; day++)
        {
            Text dayCell = Instantiate(dayCellPrefab, calendarPanel);
            dayCell.text = day.ToString();

            Outline outline = dayCell.GetComponent<Outline>();

            if (outline != null)
            {
                if (day == currentDay)
                {
                    outline.enabled = true;
                    outline.effectColor = outlineColor;
                }
                else
                {
                    outline.enabled = false;
                }
            }
            else if (day == currentDay)
            {
                Debug.LogWarning("Outline component not found on the day cell prefab!");
            }
        }
    }
}
