using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    // Reference to the animator for player animations
    public Animator animator;
    // Reference to the transform that represents the attack point
    public Transform attackPoint;
    // Layer mask for detecting enemies
    public LayerMask enemyLayers;
    
    // Damage value to enemy
    public int attackDamage = 40;
    // Radius of the attack range
    public float attackRange = 0.5f;
    // Attacks per sec
    public float attackRate = 1f;
    float nextAttack = 0f;

    void Update()
    {
        // check if attack is alowed after past time
        if (Time.time >= nextAttack)
        {
            // Check for the attack input (e.g., pressing Space key)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttack = Time.time + 1f / attackRate;
            }
        }
    }

    void Attack()
    {
        // Trigger the attack animation in the animator
        animator.SetTrigger("Attack");

        // Detect enemies in the attack range using OverlapCircleAll
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Goblin>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }

        // Draw a wire sphere to visualize the attack range
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}