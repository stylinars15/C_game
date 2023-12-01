using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_trap : MonoBehaviour
{



    public Animator anim;

    public Collider2D box; 
   
    private bool triggered;
    private bool active;
    

    private bool PlayerHit;
    
    public PlayerController playerController; 

    
    private float cooldownTimer = 0f;
    private float activeTimer = 0f;
    private float cooldownDuration = 2f; // 4 seconds cooldown
    private float activeDuration = 1f; // 1 second active
    private bool damagetaken = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHit = true; 
        }    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHit = true; 
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHit = false;
        }
    }
    
    public void CheckForPlayerDamage()
    {
        if (PlayerHit && !damagetaken)
        {
            playerController.PlayDamageAnimation(10); // Apply damage
            damagetaken = true; 
        }
    }

    
    private void Update()
    {
        // Cooldown phase
        if (!anim.GetBool("Fire"))
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                anim.SetBool("Fire", true); // Activate the trap
                cooldownTimer = 0; // Reset the cooldown timer
            }
        }
        // Active phase
        else
        {
            activeTimer += Time.deltaTime;
            if (activeTimer >= activeDuration)
            {
                anim.SetBool("Fire", false); // Deactivate the trap
                activeTimer = 0; // Reset the active timer
                damagetaken = false;
            }
            
            
        }

   
    }
}