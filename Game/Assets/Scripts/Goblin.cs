using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    // Start is called before the first frame update
    // Reference to the animator for player animations
    public Animator animator;
    private int maxHealth = 100;
    int _currentHealth;

    private float attackCooldown = 2f;
    private float attackRange = 1.9f;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private PolygonCollider2D polygonCollider;
    [SerializeField] private LayerMask playerLayer;

    private PlayerController playerController;
    private float cooldownTimer = Mathf.Infinity;
    public bool isDead { get; private set; }
    private float movementSpeed = 2.0f; // Speed at which the Goblin moves towards the player.
    private Transform playerTransform;
    private bool isAttacking;
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int TakeHit = Animator.StringToHash("TakeHit");
    private static readonly int Death = Animator.StringToHash("Death");

    void Start()
    { 
        _currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a "Player" tag.
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        if (!isAttacking)
        {
            // Check if the Goblin is within attack range.
            if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                // Start attacking when in range.
                animator.SetBool(IsMoving, false);
                isAttacking = true;
                animator.SetTrigger(Attack);
                cooldownTimer = 0;
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
                animator.SetBool(IsMoving, true);
                Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(IsMoving, false);
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
        // check if player is within attackrange
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            playerController.PlayDamageAnimation(5);
        }
    }
    
    // USED IN PLAYER_COMBAT
    public void TakeDamage(int damage) 
    {
        _currentHealth -= damage;
        animator.SetBool(IsMoving,false);

        if(_currentHealth <= 0)
        {
            _currentHealth = 0; // Ensure health doesn't go negative
            animator.SetTrigger(Death);
            DisableGoblin();
            isDead = true;
        }
        else
        {
            animator.SetTrigger(TakeHit);
        }
    }

    public void DisableGoblin()
    {
        polygonCollider.enabled = false;
        boxCollider.enabled = false;
        this.enabled = false;
    }
    
    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        return false;
    }
    

}
