using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Shock_trap : MonoBehaviour
{



    public Animator anim;

    public List<Collider2D> box = new List<Collider2D>();
   
    private bool triggered;
    private bool active;
    

    private bool PlayerHit;
    
    public PlayerController playerController; 
    public Dectection_Zone detectionzone_1,  detectionzone_2;

    
    private float cooldownTimer = 0f;
    private float activeTimer = 0f;
    public float cooldownDuration = 4f; // 4 seconds cooldown
    private float activeDuration = 1f; // the animation duration is 1 second 
    private float HitDuration = 4f; 
    private float CurrentTime = 4f; 
    
    


    

    
    public void CheckForPlayerSlow()        //checking if player is hit, during the animation. This is a animation event
    {
        if (PlayerHit && (CurrentTime >=  HitDuration))
        {
            print("done");
            CurrentTime = 0;
            playerController._moveSpeed = 0.5f; // slow player down 
            playerController._jumpForce = 15;     
        }
    }

  

    private void Update()
    {
        Debug.Log("detectedObjs Count: " + detectionzone_2.detectedObjs.Count);

        if (detectionzone_2 != null && detectionzone_2.detectedObjs.Count > 0)
        {
            if (detectionzone_1 != null && detectionzone_1.detectedObjs.Count > 0)
            {
                PlayerHit = true;
            }
            else PlayerHit = false;


            if (CurrentTime < HitDuration) // stop increasing the time after a given duration 
            {
                CurrentTime += Time.deltaTime;
            }

            // Cooldown phase
            if (!anim.GetBool("Shock"))
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= cooldownDuration)
                {
                    anim.SetBool("Shock", true); // Activate the trap
                    cooldownTimer = 0; // Reset the cooldown timer

                }
            }
            // Active phase
            else
            {
                activeTimer += Time.deltaTime;
                if (activeTimer >= activeDuration)
                {
                    anim.SetBool("Shock", false); // Deactivate the trap
                    activeTimer = 0; // Reset the active timer

                }

            }

            if (!PlayerHit && CurrentTime >= HitDuration)
            {
                playerController._moveSpeed = 1f; //reset speed for the player 
                playerController._jumpForce = 25;
            }

        }

        // if (detectionzone_2.detectedObjs.Count == 0)
        // {
        //     anim.SetBool("Shock", false); // Deactivate the trap
        //     playerController._moveSpeed = 1f; //reset speed for the player 
        //     playerController._jumpForce = 25;  
        // }
        


    }
    
    public void Disable_Traps()
    {
        anim.SetBool("Shock", false);
        this.enabled = false;
    }
}