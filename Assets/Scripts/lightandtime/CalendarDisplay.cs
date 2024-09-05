using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class CalendarDisplay : MonoBehaviour
{
    public Text dayCellPrefab; // Prefab ของช่องวันที่ (Text UI)
    public Transform calendarPanel; // Panel ที่จะแสดงตารางปฏิทิน
    public Text monthNameUI; // UI Element สำหรับชื่อเดือน
    public Color outlineColor = Color.red; // สีของกรอบ (Outline) สำหรับวันที่ปัจจุบัน

    private DayNightCycle dayNightCycle; // อ้างอิงถึงสคริปต์ DayNightCycle
    private bool calendarVisible = false; // สถานะของปฏิทิน

    void Start()
    {
        // ค้นหาอ้างอิงถึงสคริปต์ DayNightCycle
        dayNightCycle = FindObjectOfType<DayNightCycle>();

        if (dayNightCycle == null)
        {
            Debug.LogError("DayNightCycle script not found.");
            return;
        }

        // ตรวจสอบว่า dayCellPrefab, monthNameUI และ calendarPanel ถูกกำหนดหรือไม่
        if (dayCellPrefab == null || monthNameUI == null || calendarPanel == null)
        {
            Debug.LogError("dayCellPrefab, monthNameUI, or calendarPanel is not assigned.");
            return;
        }

        // ซ่อนปฏิทินในตอนเริ่มต้น
        calendarPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        // ตรวจสอบการกดปุ่ม "C"
        if (Input.GetKeyDown(KeyCode.C))
        {
            calendarVisible = !calendarVisible; // เปลี่ยนสถานะการแสดงผลของปฏิทิน
            calendarPanel.gameObject.SetActive(calendarVisible);
            monthNameUI.gameObject.SetActive(calendarVisible); // เปิดปิด monthNameUI ตามสถานะของปฏิทิน

            if (calendarVisible)
            {
                // ดึงปีและเดือนจาก DayNightCycle
                int year = dayNightCycle.GetYearCount();
                int month = dayNightCycle.GetMonthCount();

                // อัพเดทชื่อเดือน
                UpdateMonthName(year, month);

                // สร้างปฏิทินเมื่อเปิด
                GenerateCalendar(year, month);
            }
        }
    }

    void UpdateMonthName(int year, int month)
    {
        // แสดงชื่อเดือน (ใช้ monthNameUI)
        if (monthNameUI != null)
        {
            monthNameUI.text = DateTimeFormatInfo.CurrentInfo.GetMonthName(month) + " " + year;
            monthNameUI.fontStyle = FontStyle.Bold; // ปรับแต่งให้ชื่อเดือนโดดเด่น
            // Ensure this only if using dynamic font
            // monthNameUI.fontSize = 24; // Example to change font size
        }
        else
        {
            Debug.LogError("monthNameUI is not assigned.");
        }
    }

    void GenerateCalendar(int year, int month)
    {
        // ลบวันที่เก่าออกถ้ามี
        foreach (Transform child in calendarPanel)
        {
            Destroy(child.gameObject);
        }

        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        int daysInMonth = DateTime.DaysInMonth(year, month);
        int startDay = (int)firstDayOfMonth.DayOfWeek;

        // สร้างแถวสำหรับชื่อวัน
        string[] dayNames = DateTimeFormatInfo.CurrentInfo.AbbreviatedDayNames; // ชื่อวันแบบย่อ (อา, จ, อ, พ, ฯลฯ)
        for (int i = 0; i < dayNames.Length; i++)
        {
            Text dayNameCell = Instantiate(dayCellPrefab, calendarPanel);
            dayNameCell.text = dayNames[i];
            dayNameCell.fontStyle = FontStyle.Bold; // ปรับแต่งให้ชื่อวันโดดเด่น
            // Ensure this only if using dynamic font
            // dayNameCell.fontSize = 18; // Example to change font size
        }

        // สร้างช่องว่างสำหรับวันก่อนหน้าของเดือน
        for (int i = 0; i < startDay; i++)
        {
            Instantiate(dayCellPrefab, calendarPanel);
        }

        // ดึงวันที่ปัจจุบันจากสคริปต์ DayNightCycle
        int currentDay = dayNightCycle.GetDayCount();
        Debug.Log("Current day from DayNightCycle: " + currentDay);

        // สร้างช่องวันที่สำหรับแต่ละวันในเดือน
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
