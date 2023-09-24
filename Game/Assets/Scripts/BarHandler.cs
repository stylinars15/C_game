using System;
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

    //Healthbar and Power initialize
    public int maxHealth = 100;
    public int currentHealth;

    public int maxPower = 100;
    public int currentPower = 0;

    public void Start()
    {
        //Setting up our health and power
        SetMaxBars(maxHealth,maxPower);
        //Setting current health to 100
        currentHealth = maxHealth;
        //Setting current power to 0
        SetPower(currentPower);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
            GetPower(20);
        }
    }

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
    
    //Healthbar function test
    void TakeDamage(int damage)
    {
        currentHealth -= damage; 
        //updating our health
        SetHealth(currentHealth);
    }
    //Powerbar function test
    void GetPower(int power)
    {
        currentPower += power; 
        //updating our health
        SetPower(currentPower);
        
    }

}
