using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour
{
    public Slider healthSlider;
    public Slider powerSlider;
    
    public Gradient Health_colorChange;
    public Gradient Power_colorChange;
    
    public Image healthFill;
    public Image powerFill;

    public void SetMaxBars(int maxHealth, int maxPower)
    {
        healthSlider.maxValue = maxHealth;
        powerSlider.maxValue = maxPower;

        healthSlider.value = maxHealth;
        powerSlider.value = maxPower;

        healthFill.color = Health_colorChange.Evaluate(1f);
        powerFill.color = Power_colorChange.Evaluate(1f);
    }

    public void SetHealth(int currentHealth)
    {
        healthSlider.value = currentHealth; 
        healthFill.color = Health_colorChange.Evaluate(healthSlider.normalizedValue);
    }
    
    public void SetPower(int currentPower)
    {
        powerSlider.value = currentPower;
        powerFill.color = Power_colorChange.Evaluate(powerSlider.normalizedValue);
    }

}
