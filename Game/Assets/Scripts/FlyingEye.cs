using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    // Start is called before the first frame update
    // Reference to the animator for player animations
    public Animator animator;
    private int _maxHealth = 100;
    int _currentHealth;

    private float _attackCooldown = 2f;
    private float attackRange = 1.8f;
    [SerializeField] private BoxCollider2D boxCollider;
    public CapsuleCollider2D capsuleCollider;
    [SerializeField] private LayerMask playerLayer;
    PlayerController playerController;
    
    private float cooldownTimer = Mathf.Infinity;
    public bool isDead { get; private set; }
    private float movementSpeed = 2.0f; 
    private Transform playerTransform;
    private bool isAttacking = false;
    
    //animations
    private static readonly int TakeHit = Animator.StringToHash("Take_hit");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Flying = Animator.StringToHash("Flying");

    void Start()
    { 
        _currentHealth = _maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has a "Player" tag.
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        
    }
    
    private void Update()
    {
        if (IsAnimationPlaying(animator, "Take_hit"))
        {
            
        }
        else if (!isAttacking)
        {
            // Check is within attack range.
            if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
            {
                // Start attacking when in range.
                animator.SetTrigger(Flying);
                isAttacking = true;
                animator.SetTrigger(Attack);
                cooldownTimer = 0;
              
            }
            else if (PlayerInsight())
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
                Vector3 targetPosition = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            }
        }
        else 
        {
            // If attacking, check the attack cooldown.
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= _attackCooldown)
            {
                isAttacking = false; // Reset attack state.
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
        // check if player is within attack range
        if (Vector3.Distance(transform.position, playerTransform.position) <= attackRange)
        {
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
            Debug.Log("dead"); // Add this line for debugging
            animator.SetTrigger(Death);
            Disable_FLying_eye();
            isDead = true;
        }
        else
        {
            animator.SetTrigger(TakeHit);
        }
    }

    public void Disable_FLying_eye()
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
