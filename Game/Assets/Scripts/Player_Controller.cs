using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes

public class PlayerController : MonoBehaviour
{
    
    public CinemachineVirtualCamera stillCamera;
    public CinemachineVirtualCamera followCamera;
    
    
    
    private Rigidbody2D _rb2D;
<<<<<<< Updated upstream
    [SerializeField] private BarHandler barHandler;
=======

    public CinemachineVirtualCamera stillcam;
    public CinemachineVirtualCamera followcam;

    
>>>>>>> Stashed changes
    
    //player movent variables 
    private float _moveSpeed; 
    private float _jumpForce;
    private bool  _jumpState;
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
        Take_Damage
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
    
    // Start is called before the first frame update
    void Start()
    {
        // getting our players rigidbody component
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        // setting up the animator
        _animator = gameObject.GetComponent<Animator>();
        
        _moveSpeed = 1f; 
        _jumpForce = 20f; 
        _jumpState = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        // player getting damaged 
        if(IsAnimationPlaying(_animator, "Take_Damage") || _currentPlayerState == PlayerState.Take_Damage)
        {
            _currentPlayerState = PlayerState.Take_Damage;
            //cannot move
            _moveHorizontal = 0f;
            _moveVertical = 0f;
        }
        else
        {
            _moveHorizontal = Input.GetAxisRaw("Horizontal");
            _moveVertical = Input.GetAxisRaw("Vertical");
        }
        
        if (transform.position.x > -1.93f)
        {
            followCamera.gameObject.SetActive(true);
            stillCamera.gameObject.SetActive(false);
        }
        else
        {
            followCamera.gameObject.SetActive(false);
            stillCamera.gameObject.SetActive(true);
        }
        
        
    }
  
    private void FixedUpdate()
    {
        // Determine the movement direction and apply horizontal force.
        if(IsAnimationPlaying(_animator, "Take_Damage"))
        {
            _currentPlayerState = PlayerState.Take_Damage;
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
            _currentPlayerState = PlayerState.Take_Damage;
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
            if (horizontalForce != 0 && (_currentPlayerState != PlayerState.Take_Damage))
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
<<<<<<< Updated upstream
        
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _animator.SetBool("Defend", true);
            _currentPlayerState = PlayerState.Defending;
        }
        else
        {
            _animator.SetBool("Defend", false);
        }
        
=======


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
        
        
        
>>>>>>> Stashed changes
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            _jumpState = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
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
        else
        {
            return false;
        }
        
    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _facingRight = !_facingRight; 
    }
    
    //Used in goblin DamagePlayer() function
    public void PlayDamageAnimation()
    {
        if (!(_animator.GetBool("Defend")))
        {
            bool isPlayerDead = barHandler.TakeDamage(50);

            if (isPlayerDead)
            {
                Debug.Log("dead");
                _animator.SetTrigger("Death");
            }
            else
            {
                _animator.SetTrigger("Take_Damage");
                _currentPlayerState = PlayerState.Take_Damage;
            }
        }
    }
    

    
}


