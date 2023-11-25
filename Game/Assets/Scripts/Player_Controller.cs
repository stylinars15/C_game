using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    
    public Transform playerTransform; // Reference to the player's transform
    
    public CinemachineVirtualCamera stillcam;
    public CinemachineVirtualCamera followcam;
    
    [SerializeField] private BarHandler barHandler;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Traps traps;
    [SerializeField] private float SlopeCheckDistance;
    [SerializeField] private LayerMask WhatIsGround; 
    
    public LayerMask enemyLayers;
    public Transform defendPoint;
    

    //player movent variables 
    private float _moveSpeed; 
    private float _jumpForce;
    
    private bool  _jumpState;
    private bool  _isDefending;
    private bool _canMove = true;
    
    private float _moveHorizontal; 
    private float _moveVertical; 
    
    
    //player states
    enum PlayerState
    {
        Idle, 
        Running,
        Jumping,
        Falling,
        Defending,
        TakeDamage
    }
    private PlayerState _currentPlayerState = PlayerState.Idle;
    private PlayerState _previousPlayerState = PlayerState.Falling;
    
    //Player animation variables
    Animator _animator;
    string _currenState;
    const string PlayerIdle = "Idle_1";
    const string PlayerRun = "Run_1";
    const string PlayerJump = "Jump_1";
    const string PlayerFall = "Fall_1";
    bool _facingRight = true;
    private static readonly int TakeDamage = Animator.StringToHash("Take_Damage");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Defend = Animator.StringToHash("Defend");

   

    // Start is called before the first frame update
    void Start()
    {
        // getting our players rigidbody component
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        
        
        // setting up the animator
        _animator = gameObject.GetComponent<Animator>();
        
        _moveSpeed = 1f; 
        _jumpForce = 25; 
        _jumpState = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_canMove)
        {
            // player getting damaged 
            if (IsAnimationPlaying(_animator, "Take_Damage") || _currentPlayerState == PlayerState.TakeDamage)
            {
                _currentPlayerState = PlayerState.TakeDamage;
                //cannot move
                _moveHorizontal = 0f;
                _moveVertical = 0f;
            }
            else
            {
                _moveHorizontal = Input.GetAxisRaw("Horizontal");
                _moveVertical = Input.GetAxisRaw("Vertical");
            }
        }
        
        if (transform.position.x < 4.1f)
        {
            stillcam.Priority = 10; // Higher priority for the stillcam
            followcam.Priority = 0; // Lower priority for the followcam
        }
        else
        {
            stillcam.Priority = 0; // Lower priority for the stillcam
            followcam.Priority = 10; // Higher priority for the followcam
        }
        
        
        
        
        
    }

 
    
    private void FixedUpdate() 
    {
        if (_canMove)
        {
            // Determine the movement direction and apply horizontal force.
            if(IsAnimationPlaying(_animator, "Take_Damage"))
            {
                _currentPlayerState = PlayerState.TakeDamage;
            }
            else if (IsAnimationPlaying(_animator, PlayerJump))
            {
                _currentPlayerState = PlayerState.Jumping;
            }
            else if(IsAnimationPlaying(_animator, PlayerFall))
            {
                _currentPlayerState = PlayerState.Falling;
            }
            else
            {
                _currentPlayerState = PlayerState.Idle;
            }
            float horizontalForce = _moveHorizontal * _moveSpeed;
            _rb2D.AddForce(new Vector2(horizontalForce, 0), ForceMode2D.Impulse);
            
            
            
            if(IsAnimationPlaying(_animator, "Take_Damage"))
            {
                _currentPlayerState = PlayerState.TakeDamage;
            }
            // Check for a jump and apply vertical force if conditions are met.
            else if (_previousPlayerState == PlayerState.Falling && !_jumpState && _moveVertical > 0.1f )
            {
                // Check the current vertical velocity.
                if (Mathf.Abs(_rb2D.velocity.y) < 10.0f)
                {
                    _rb2D.AddForce(new Vector2(0, _moveVertical * _jumpForce), ForceMode2D.Impulse);
                    ChangeAnimationState(PlayerJump);
                    _currentPlayerState = PlayerState.Jumping; // Set isJumping to true when jumping.
                }
            }
            // Check if the character has started falling.
            else if (_currentPlayerState == PlayerState.Jumping && _rb2D.velocity.y < 0 )
            {
                ChangeAnimationState(PlayerFall);
                _currentPlayerState = PlayerState.Falling;
            }
            // Change animation state based on movement and flip character if needed.
            else if (_currentPlayerState != PlayerState.Falling && _currentPlayerState != PlayerState.Jumping )
            {
                if (horizontalForce != 0 && (_currentPlayerState != PlayerState.TakeDamage))
                {
                    ChangeAnimationState(PlayerRun);
                    _currentPlayerState = PlayerState.Running;
                    if ((horizontalForce > 0 && !_facingRight) || (horizontalForce < 0 && _facingRight))
                    {
                        Flip();
                    }
                }
                else
                {
                    ChangeAnimationState(PlayerIdle);
                    _currentPlayerState = PlayerState.Idle;
                }
            }
            
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _animator.SetBool(Defend, true);
                _currentPlayerState = PlayerState.Defending;

                if (DefendDirection())
                {
                    _isDefending = true;
                }
                else
                {
                    _isDefending = false;
                }
            }
            else
            {
                _animator.SetBool(Defend, false);
                _isDefending = false;
            }
           
        }
        
        
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            _jumpState = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            _jumpState = true;
        }
    }

    // method checking if the current animation is running
    private void ChangeAnimationState(string newState)
    {
        if (newState == _currenState)
        {
            return;
        }
        
        _animator.Play(newState);
        _currenState = newState; 
    }

    
    // method checks if animation has finished playing
    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && 
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        return false;
    }

    void Flip()
    {
        var o = gameObject;
        Vector3 currentScale = o.transform.localScale;
        currentScale.x *= -1;
        o.transform.localScale = currentScale;

        _facingRight = !_facingRight; 
    }
    
    //Used in goblin DamagePlayer() function
    public void PlayDamageAnimation(int damageAmount)
    {
        // we check if the player is defending and if player is facing the right way
        if (!_isDefending)
        {
            bool isPlayerDead = barHandler.TakeDamage(damageAmount);

            if (isPlayerDead)
            {
                _animator.SetTrigger(Death);
                DisableGame();
            }
            else
            {
                _animator.SetTrigger(TakeDamage);
                _currentPlayerState = PlayerState.TakeDamage;
            }
        }
    }
    
    public void Awake()
    {
        // Get the player's transform component during Awake()
        playerTransform = transform;
    }

    private void DisableGame()
    {
        _canMove = false;
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        // DisableSpawnedEnemies
        enemySpawner.DisableSpawnedEnemies();
        
        // DisableTraps
        traps.Disable_Traps();
        
        //gameover screen
        gameManager.Setup();
    }
    
    
    private bool DefendDirection()
    {
        // Detect enemies in the attack range using OverlapCircleAll
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(defendPoint.position, 0.5f, enemyLayers);
        
        foreach (Collider2D enemyCollider in hitEnemies)
        {
            // Check if the collider is not a trigger collider
            if (!enemyCollider.isTrigger)
            {
                // Check if the collider has a Goblin component
                Goblin enemy = enemyCollider.GetComponent<Goblin>();
                FlyingEye enemy1 = enemyCollider.GetComponent<FlyingEye>();
                if (enemy != null || enemy1 != null)
                {
                    return true;
                }
            }
        }
        // No enemy found in front of the player
        return false;
    }
}



