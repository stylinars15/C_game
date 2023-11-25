using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private float jumpForce;
    [SerializeField] private float slopeCheckDistance;
    [SerializeField] private float maxSlopeAngle;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private PhysicsMaterial2D noFriction;
    [SerializeField] private PhysicsMaterial2D fullFriction;

    private bool isGrounded;
    private bool isOnSlope;
    private bool isJumping;
    private bool canWalkOnSlope;
    private bool canJump;
    
    private float xInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;

    private Vector2 capsuleColliderSize;

    private Vector2 slopeNormalPerp;

    
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = gameObject.GetComponent<CapsuleCollider2D>();

        capsuleColliderSize = cc.size;
    }

    private void FixedUpdate()
    {
        SlopeCheck(); 
        
        
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position + (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));

        //SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    
    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
       

    }
    
    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast( checkPos, Vector2.down, slopeCheckDistance, whatIsGround);
        
        Debug.Log("Capsule Collider Size: " + capsuleColliderSize);
        Debug.Log("Transform position: " + transform.position);
        Debug.Log("Hit Point: " + hit.point);
        
        if (hit)
        {
            //slopeNormalPerp = Vector2.Perpendicular(hit.normal);
            //slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);
            
            Debug.DrawRay(hit.point, hit.normal, Color.red);

        }

        
    }

     
    
}
