using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb2D;
    
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
        Falling
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
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");
    }
  
    private void FixedUpdate()
    {

        // Determine the movement direction and apply horizontal force.
        if (IsAnimationPlaying(_animator, PlayerJump))
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
        
        
        // Check for a jump and apply vertical force if conditions are met.
        if (_previousPlayerState == PlayerState.Falling && !_jumpState && _moveVertical > 0.1f )
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
            if (horizontalForce != 0)
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
    
    public void PlayDamageAnimation()
    {
        // Assuming you have a "TakeDamage" trigger parameter in your Animator controller.
        _animator.SetTrigger("Take_Damage");
    }
    

}

