using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire_trap : MonoBehaviour
{
    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay; 
    [SerializeField] private float activeTime;

    private Animator anim;
    private SpriteRenderer spriteRend;

    public Collider2D box; 
   
    private bool triggered;
    private bool active;

    public float Cooldowntimer; 
    public float Hit_Cooldowntimer; 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Player entered the box");
            triggered = true;
            // Additional logic for when the player enters the box
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("Player is still in the box");
            if (active) 
            {
                // Logic while the player remains in the box, e.g., apply damage over time
                collision.GetComponent<PlayerController>().PlayDamageAnimation(10);
            }
        }
    }

    

    // Rest of your update logic
    private void Update()
    {
        
    }
}