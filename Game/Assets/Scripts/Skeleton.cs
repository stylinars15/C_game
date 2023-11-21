using UnityEngine;

public class Skeleton : MonoBehaviour
{
    // Start is called before the first frame update
    // Reference to the animator for player animations
    public Animator animator;
    private int _maxHealth = 100;
    int _currentHealth;

    private float _attackCooldown = 3f;
    private float attackRange = 2.3f;
    [SerializeField] private BoxCollider2D boxCollider;
    public CapsuleCollider2D capsuleCollider;
    [SerializeField] private LayerMask playerLayer;
    PlayerController playerController;
    
    private float cooldownTimer = Mathf.Infinity;
    public bool isDead { get; private set; }
    private float movementSpeed = 0.8f;
    private Transform playerTransform;
    private bool isAttacking;
    
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
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a "Player" tag.
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        if (!isAttacking)
        {
            // Check is within attack range.
            if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                animator.SetBool(Walk,false);
                isAttacking = true;
                animator.SetBool(Attack,true);
                cooldownTimer = 0;
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
        // Check if the player is within attack range.
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
            // Apply damage only if the player is in front of the enemy.
            playerController.PlayDamageAnimation(5);
        }
    }


    
    // USED IN PLAYER_COMBAT
    public void TakeDamage(int damage) 
    {
        _currentHealth -= damage;

        if(_currentHealth <= 0)
        {
            _currentHealth = 0; // Ensure health doesn't go negative
            animator.SetTrigger(Death);
            Disable_Skeleton();
            isDead = true;
        }
        else
        {
            animator.SetTrigger(TakeHit);
        }
    }

    public void Disable_Skeleton()
    {
        capsuleCollider.enabled = false;
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
