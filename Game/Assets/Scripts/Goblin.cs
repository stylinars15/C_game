using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    // Start is called before the first frame update
    // Reference to the animator for player animations
    public Animator animator;
    public int maxHealth = 100;
    int _currentHealth;

    [SerializeField] private float attackCooldown;
    private float attackRange = 1.9f;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerController playerController;
    
    private float cooldownTimer = Mathf.Infinity;
    private float movementSpeed = 2.0f; // Speed at which the Goblin moves towards the player.
    private Transform playerTransform;
    private bool isAttacking = false;
    
    void Start()
    { 
        _currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a "Player" tag.
        
    }
    
    private void Update()
    {
        if (!isAttacking)
        {
            // Check if the Goblin is within attack range.
            if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                // Start attacking when in range.
                animator.SetBool("IsMoving", false);
                isAttacking = true;
                animator.SetTrigger("Attack");
                cooldownTimer = 0;

                // Here, you should trigger the actual attack, e.g., damage the player.
                // Call a method like DamagePlayer() to deal damage to the player.
              
            }
            else if (playerInsight())
            {
                float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;
                Vector3 localScale = transform.localScale;
                // Flip the sprite if the direction is different.
                if (direction > 0 && localScale.x < 0 || direction < 0 && localScale.x > 0)
                {
                    localScale.x *= -1;
                    transform.localScale = localScale;
                }

                // If the player is in sight, move towards the player.
                animator.SetBool("IsMoving", true);
                Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }
        }
        else 
        {
            // If attacking, check the attack cooldown.
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= attackCooldown)
            {
                isAttacking = false; // Reset attack state.
            }
        }
    }
    
    private bool playerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }
    
    
    // Called when goblin SLASHES
    private void DamagePlayer()
    {
        if (playerInsight())
        {
            playerController.PlayDamageAnimation();
            //barHandler.TakeDamage(5)
        }
    }
    
    // USED IN PLAYER_COMBAT
    public void TakeDamage(int damage) 
    {
        _currentHealth -= damage;

        if(_currentHealth <= 0)
        {
            _currentHealth = 0; // Ensure health doesn't go negative
            Debug.Log("dead"); // Add this line for debugging
            animator.SetTrigger("Death");
            polygonCollider.enabled = false;
            boxCollider.enabled = false;
            this.enabled = false;
        }
        else
        {
            animator.SetTrigger("Take_Hit");
        }
    }
    

}
