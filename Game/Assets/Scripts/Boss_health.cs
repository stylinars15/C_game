using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_health : MonoBehaviour
{
    public Slider healthSlider;
    
    public Gradient Health_colorChange;
    
    public Image healthFill;

    //Healthbar and Power initialize
    public int currentHealth;

    public void Start()
    {
        //Setting up our health and power
        SetMaxBars(300);
        
        
    }

    public void SetMaxBars(int maxHealth)
    {
        healthSlider.maxValue = maxHealth;


        healthSlider.value = maxHealth;
   

        healthFill.color = Health_colorChange.Evaluate(1f);

    }

    public void SetHealth(int currentHealth)
    {
        healthSlider.value = currentHealth; 
        healthFill.color = Health_colorChange.Evaluate(healthSlider.normalizedValue);
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

    
}