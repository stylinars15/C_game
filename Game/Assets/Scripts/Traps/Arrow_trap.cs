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
   
    public Animator ani;

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
   
   private void Start()
   {
       //We are setting cool down timer to attack cooldown, so that when enemy is detected the trap is shooting immediately
       CoolDownTimer = AttackCooldown;
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
        {
            ani.SetBool("Detected", true);      // ready to shoot
        }
        else 
        {
            ani.SetBool("Detected", false);
            CoolDownTimer = AttackCooldown;     // doing this so the trap will attack immediately when it detects the player again
        }

        if (detectionzone_2 != null && detectionzone_2.detectedObjs.Count > 0 ) //detected in zone 2
        {
            CoolDownTimer += Time.deltaTime;
            if (CoolDownTimer >= AttackCooldown)
            {
                print("start");
                ani.SetBool("Shoot", true); // arrow starts shooting
                print("over");
                CoolDownTimer = 0;
            }

        }
        else 
            ani.SetBool("Shoot", false);    // stops shooting

        
    }
    
    public void Disable_Traps()
    {
        ani.SetBool("Detected", false);
        ani.SetBool("Shoot", false);
        this.enabled = false;
    }
    
    
}