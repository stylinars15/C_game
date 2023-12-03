using UnityEngine;

/*
public class Skeleton : MonoBehaviour
{
    // Start is called before the first frame update
    // Reference to the animator for player animations
    public Transform attackPoint;
    public Animator animator;
    private int _maxHealth = 100;
    int _currentHealth;

    private float _attackCooldown = 4f;
    public float attackRange;
    [SerializeField] private BoxCollider2D boxCollider;
    public CapsuleCollider2D capsuleCollider;
    [SerializeField] private LayerMask playerLayer;
    public PlayerController playerController;
    public Transform playerTransform;
    
    private float cooldownTimer = Mathf.Infinity;
    public bool isDead { get; private set; }
    private float movementSpeed = 0.8f;
    private bool isAttacking;
    private bool isDefending;
    
    //animations
    private static readonly int TakeHit = Animator.StringToHash("Takehit");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Shield = Animator.StringToHash("Shield");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Attack");

    void Start()
    { 
        _currentHealth = _maxHealth;
    }
    
    private void Update()
    {
        if (!isAttacking)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

            // Check is within attack range.
            if (hitPlayer != null && hitPlayer.gameObject.CompareTag("Player"))
            {
                animator.SetBool(Walk,false);
                if (Random.value > 0.5f)
                {
                    isAttacking = true;
                    animator.SetBool(Attack,true);
                    cooldownTimer = 0;
                }
                else
                {
                    isAttacking = true;
                    animator.SetBool(Shield,true);
                    cooldownTimer = 0;
                }
            }
            else if (PlayerInsight())
            {
                // Check if the player is above the enemy by a certain threshold.
                if (playerTransform.position.y > (transform.position.y+3.2))
                {
                    animator.SetBool(Walk, false);
                }
                else
                {
                    animator.SetBool(Walk, true);
                    float direction = playerTransform.position.x > transform.position.x ? 1f : -1f;
                    Vector3 localScale = transform.localScale;
        
                    // Flip the sprite if the direction is different.
                    if ((direction > 0 && localScale.x < 0) || (direction < 0 && localScale.x > 0))
                    {
                        localScale.x *= -1;
                        transform.localScale = localScale;
                    }

                    // If the player is in sight and not above, move towards the player.
                    Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
                }
            }

        }
        else 
        {
            // If attacking, check the attack cooldown.
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= _attackCooldown)
            {
                isAttacking = false; // Reset attack state.
                animator.SetBool(Attack,false);// Reset attack state.
                animator.SetBool(Shield,false);// Reset attack state.
            }
        }
    }
    
    private bool PlayerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            new Vector2(boxCollider.bounds.size.x, boxCollider.bounds.size.y),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }
    
    
    // Called when enemy attacks
    private void DamagePlayer()
    {
        // Check if the player is within the circular attack range around the attack point.
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        // If a collider is found and it's the player, apply damage.
        if (hitPlayer.gameObject.CompareTag("Player"))
        {
            // Apply damage only if the player is within the attack range.
            playerController.PlayDamageAnimation(5);
        }
    }
    
    
    // called when player attack
    public bool TakeDamage(int damage)
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        if (animator.GetBool(Shield) )
        {
            if (hitPlayer.gameObject.CompareTag("Player"))
            {
                return false;
            }
        }

        // The skeleton should take damage if it's not defending or if it's defending but the player is not in front
        if (!isDefendingAndPlayerInFront)
        {
            animator.SetBool(Shield,false);
            // Reduce health
            _currentHealth -= damage;

            // Check if health falls below or equals zero
            if (_currentHealth <= 0)
            {
                Die();
            }
            else
            {
                animator.SetTrigger(TakeHit);
            }
            return true;
        }
        
    }


    private void Die()
    {
        _currentHealth = 0; // Ensure health doesn't go negative
        animator.SetTrigger(Death);
        Disable_Skeleton();
        isDead = true;
    }


    public void Disable_Skeleton()
    {
        capsuleCollider.enabled = false;
        boxCollider.enabled = false;
        this.enabled = false;
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

*/
