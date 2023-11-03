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
    public int currentPower;

    public void Start()
    {
        //Setting up our health and power
        SetMaxBars(maxHealth, maxPower);

         
    }

    public void SetMaxBars(int maxHealth, int maxPower)
    {
        healthSlider.maxValue = maxHealth;
        powerSlider.maxValue = maxPower;

        healthSlider.value = maxHealth;
        powerSlider.value = 0;

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
    
    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Check if the player's health is less than or equal to zero.
        bool isDead = currentHealth <= 0;

        // Updating our health
        SetHealth(currentHealth);
        
        // Return a boolean value indicating whether the player is dead.
        return isDead;
    }
    
    //Powerbar function test
    
    void GetPower(int power)
    {
        currentPower += power; 
        //updating our health
        SetPower(currentPower);
        
    }

}

