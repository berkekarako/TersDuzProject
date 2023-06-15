using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 13f;

    private int jump = 0;


    private enum MovementState { idle, running, jumping, falling }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if(rb.bodyType != RigidbodyType2D.Static)
        {
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        }
        
        if(IsGrounded())
        {
            jump = 0;
        }

        if(Input.GetButtonDown("Jump"))
        {
            if (jump == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jump++;
            }
            else if (jump == 1 && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jump++;
            }
           


        }

       UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            if (rb.bodyType != RigidbodyType2D.Static)
            {
                sprite.flipX = false;
            }
            state = MovementState.running;
        }
        else if (dirX < 0)
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

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
       return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
