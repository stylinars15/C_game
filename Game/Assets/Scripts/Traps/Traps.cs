using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] private float AttackCooldown; 
    private float CoolDownTimer; 
    [SerializeField] private Transform ArrowPoint; 
    [SerializeField] private GameObject[] arrows;
   
    public Animator animation;


    public Dectection_Zone detectionzone_1,  detectionzone_2; 

   
   public void ShootArrow()
    {
        int arrowIndex = FindArrows();
        if (arrowIndex >= 0 && arrowIndex < arrows.Length)
        {
            GameObject arrow = arrows[arrowIndex];
            if (arrow != null)
            {
                arrow.transform.position = ArrowPoint.position;
                Enemy_Projectile projectile = arrow.GetComponent<Enemy_Projectile>();
                if (projectile != null)
                {
                    projectile.ActiveProjectile();
                }
            }
        }
    }
   

    private int FindArrows()
    {
        
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
            {
                return i; 
            }
        }
        return 0; 
    }

   

    private void Update()
    {
        if (detectionzone_1 != null && detectionzone_1.detectedObjs.Count > 0) 
            animation.SetBool("Detected", true);
        else 
            animation.SetBool("Detected", false);

        if (detectionzone_2 != null && detectionzone_2.detectedObjs.Count > 0)
        {
            CoolDownTimer += Time.deltaTime;

            if (CoolDownTimer >= AttackCooldown)
            {
                animation.SetBool("Shoot", true);

                // Check if the animation has finished playing
                AnimatorStateInfo stateInfo = animation.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.normalizedTime >= 0.1f)
                {
                    // The animation has finished playing (normalizedTime == 1.0)
                    // Perform your action here, e.g., call ShootArrow()
                    //ShootArrow();

                    // Reset the cooldown timer
                    CoolDownTimer = 0f;
                }
            }
        }
        else
        {
            animation.SetBool("Shoot", false);
        }
    }
    


  
}