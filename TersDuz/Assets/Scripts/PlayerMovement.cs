using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private float horizontal = 0f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isCrouching = false;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;


    [SerializeField] private LayerMask jumpableGround;


    private int jump = 0;
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffSet;
    
    

    private enum MovementState { idle, running, jumping, falling, crouch, crawl }

    [SerializeField] private AudioSource jumpSoundEffect;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        originalColliderSize = coll.size;
        originalColliderOffSet = coll.offset;

    }

    private void Update()
    {
        Movement();
        UpdateAnimationState();
    }


    private void UpdateAnimationState()
    {
        MovementState state;

        if (horizontal > 0f)
        {
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                sprite.flipX = false;
            }
            state = MovementState.running;
        }
        else if (horizontal < 0)
        {
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                sprite.flipX = true;
            }
            state = MovementState.running;  
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        if (isCrouching) 
        {
            state = MovementState.crouch;
            if (horizontal > 0 || horizontal < 0)
            {
                state = MovementState.crawl;
            }
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Movement()
    {
        if (isCrouching)
        {
            horizontal = Input.GetAxisRaw("Horizontal") / 2;
            Crouch();
        }
        else
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            coll.size = originalColliderSize;
            coll.offset = originalColliderOffSet;
        }

        if (rb.bodyType != RigidbodyType2D.Static)
        {
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        }

        if (IsGrounded())
        {
            jump = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jump == 0)
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jump++;
            }
            else if (jump == 1 && IsGrounded())
            {
                jumpSoundEffect.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                jump++;
            }
        }

        if (Input.GetKey(KeyCode.S) && IsGrounded())
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
    }

    private void Crouch()
    {
        Vector2 newColliderSize = originalColliderSize;
        newColliderSize.y /= 2;
        coll.size = newColliderSize;

        Vector2 colliderOffset = originalColliderOffSet;
        colliderOffset.y -= originalColliderSize.y / 4;
        coll.offset = colliderOffset;

    }
}
