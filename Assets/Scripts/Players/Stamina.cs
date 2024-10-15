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
        UpdateStaminaUI(); // ��駤��������鹢ͧ slider
    }

    public void UseStamina(int amount)
    {
        if (CurrentStamina >= amount)
        {
            CurrentStamina -= amount;
            UpdateStaminaUI(); // �Ѿവ slider �ء���駷���� Stamina
        }
        else
        {
            Debug.Log("Stamina ����!");
        }
    }

    // �ѧ��ѹ�Ѿവ Slider
    private void UpdateStaminaUI()
    {
        staminaSlider.value = (float)CurrentStamina / maxStamina; // �Ѿവ��Ңͧ slider ����ʴ���� currentStamina
    }
    public void RecoverStamina(int amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, maxStamina);
        Debug.Log("Stamina recovered: " + amount);
        staminaSlider.value = (float)CurrentStamina / maxStamina;
    }



}
