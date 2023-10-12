using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
{

    [SerializeField] private float speed; 
    [SerializeField] private float ResetTime;
    private float LifeTime;
    public PlayerController playerController; 
    
    


    public void ActiveProjectile()
    {
        //Lifetime is how long the projectile last if it hits nothing
        LifeTime = 1; 
        gameObject.SetActive(true);
    }
    

    private void Update()
    {

        float movementspeed = speed * Time.deltaTime; 
        transform.Translate(movementspeed,0,0);
        
        LifeTime += Time.deltaTime;
        if (LifeTime > ResetTime) 
            gameObject.SetActive(false);
        
    }

    private void OnTriggerEnter2D(Collider2D HitInfo)
    {
        string tag  = HitInfo.gameObject.tag;

        if (tag == "Player")
        {
            gameObject.SetActive(false);
            playerController.PlayDamageAnimation();
        }
        
    }
}