using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_trap : MonoBehaviour
{



    public Animator anim;

    public Collider2D box; 
    public Collider2D box2; 
    
    private bool triggered;
    private bool active;
    

    private bool PlayerHit;
    
    public PlayerController playerController; 

    public Dectection_Zone detectionzone_1,  detectionzone_2;
    
    private float cooldownTimer = 0f;
    private float activeTimer = 0f;
    public float cooldownDuration = 2f; 
    private float activeDuration = 1f; // the animation duration is 1 second 
    private bool damagetaken = false;
    
    
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
        if (detectionzone_2 != null && detectionzone_2.detectedObjs.Count > 0)
        {
            
            if (detectionzone_1 != null && detectionzone_1.detectedObjs.Count > 0)
            {
                PlayerHit = true;
            }
            else PlayerHit = false;
            
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
    
    
    public void Disable_Traps()
    {
        anim.SetBool("Fire", false);
        this.enabled = false;
    }
}