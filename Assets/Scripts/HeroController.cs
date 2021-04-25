using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [Header("Jump Physics")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] Transform groundCheckPlaceholder;
    private bool canJump;
    private bool isJumping;

    [Header("Wall Check")]
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;
    [SerializeField] LayerMask wallLayer;
    private bool isTouchingWall;

    private bool isMoving;
    private bool isGrounded;
    private bool isFalling;
    private bool isFacingRight = true;

    float xHorizontal;

    Animator animator;
    Rigidbody2D heroBody2D;
    SpriteRenderer spriteRenderer;
    Vector3 baseScale;
   

    void Start()
    {
        animator = GetComponent<Animator>();
        heroBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        jumpCheck();
        groundCheck();
        animationControl();
        checkIfFalling();
        checkObjectinFront();
        handleDirection();


        Debug.Log(xHorizontal);
    }

    private void FixedUpdate()
    {
        handleMovement();
        handleJumping();
    }

    void handleMovement()
    {
        xHorizontal = Input.GetAxisRaw("Horizontal");
        heroBody2D.velocity = new Vector2( xHorizontal * speed, heroBody2D.velocity.y);
    }
    void handleJumping()
    {
        if (canJump && isGrounded) // if i can jump, not on the ground , and not falling then I can jump.
        {
            heroBody2D.AddForce(new Vector2(heroBody2D.velocity.x, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
        else if(canJump && checkObjectinFront())
        {
            heroBody2D.AddForce(new Vector2(heroBody2D.velocity.x, jumpForce), ForceMode2D.Impulse);
            canJump = false;
        }
        
    }
    void animationControl()
    {


        if (isMoving && isGrounded && !isTouchingWall)
        {
            animator.Play("hero_run");
        }
        else if(isGrounded && isTouchingWall && isMoving)
        {
            animator.Play("hero_push");
        }
        else if(isGrounded && !isMoving)
        {
            animator.Play("hero_idle");
        }
        /*
        if (isMoving && isGrounded && !isTouchingWall)
        {
            animator.Play("hero_run");
        }
        else if(!isMoving && isGrounded)
        {
            animator.Play("hero_idle");
        }
        else if(isTouchingWall && isGrounded && isMoving)
        {
            animator.Play("hero_push");
        }
        else if (isFalling && !isGrounded)
        {
            animator.Play("hero_falling");
        }
        else if (isJumping)
        {
            animator.Play("hero_jump");
        }
        */
    }
    void handleDirection()
    {
        if (xHorizontal < 0 && !isFacingRight)
        {
            flip();
            isMoving = true;
        }
        else if (xHorizontal > 0 && isFacingRight)
        {
            flip();
            isMoving = true;
            
        }
        else
        {
            isMoving = false;
        }
    }
    void checkIfFalling()
    {
        float yVelocity = heroBody2D.velocity.y;
   
        if(yVelocity < -0.1)
        {
            isFalling = true;
            isJumping = false;
        }
        else if(yVelocity > 0.1)
        {
            isJumping = true;
            isFalling = false;
        } 
    }
    void groundCheck()
    {
        RaycastHit2D groundLineCast = Physics2D.Linecast(transform.position, groundCheckPlaceholder.position, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D stoneLineCast = Physics2D.Linecast(transform.position, groundCheckPlaceholder.position, 1 << LayerMask.NameToLayer("Stone"));

        if (groundLineCast || stoneLineCast)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        Debug.DrawLine(transform.position, groundLineCast.point, Color.green);
    }
    void jumpCheck()
    {
        if (Input.GetButtonDown("Jump"))
        {
            canJump = true;
        }
        else if(Input.GetButtonUp("Jump") && heroBody2D.velocity.y > 0)
        {
            heroBody2D.velocity = new Vector2(heroBody2D.velocity.x, heroBody2D.velocity.y * .5f); // half jump
        }
    }

    bool checkObjectinFront()
    {
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer); // makes a box that will check whats in front

        return isTouchingWall;
    }

    void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180 , 0);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
