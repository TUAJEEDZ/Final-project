using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stamina : MonoBehaviour
{
    [SerializeField] private int maxStamina = 10;
    [SerializeField] private Slider staminaSlider;

    public int CurrentStamina;

    void Start()
    {
        CurrentStamina = maxStamina;
        UpdateStaminaUI(); // ตั้งค่าเริ่มต้นของ slider
    }

    public void UseStamina(int amount)
    {
        if (CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
            UpdateStaminaUI(); // อัพเดต slider ทุกครั้งที่ใช้ Stamina
        }
        else
        {
            Debug.Log("Stamina ไม่พอ!");
        }
    }

    // ฟังก์ชันอัพเดต Slider
    private void UpdateStaminaUI()
    {
        staminaSlider.value = (float)CurrentStamina / maxStamina; // อัพเดตค่าของ slider ให้แสดงตาม currentStamina
    }
    public void RecoverStamina(int amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, maxStamina);
        Debug.Log("Stamina recovered: " + amount);
        staminaSlider.value = (float)CurrentStamina / maxStamina;
    }



}
