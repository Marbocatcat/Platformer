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

    Animator animator;
    Rigidbody2D heroBody2D;
    Vector3 baseScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        heroBody2D = GetComponent<Rigidbody2D>();
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
        handleDirection(baseScale);

        Debug.Log(isTouchingWall);
    }

    private void FixedUpdate()
    {
        handleMovement();
        handleJumping();
    }

    void handleMovement()
    {
        heroBody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, heroBody2D.velocity.y);
    }
    void handleJumping()
    {
        if (canJump && isGrounded) // if i can jump, not on the ground , and not falling then I can jump.
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
        else if(!isMoving && isGrounded)
        {
            animator.Play("hero_idle");
        }
        else if(isTouchingWall && isGrounded)
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
    }
    void handleDirection(Vector3 oldbaseScale)
    {
        Vector3 newBaseScale = oldbaseScale;
        float velocity = heroBody2D.velocity.x;
        isMoving = false;

        if (velocity < -0.1)
        {
            newBaseScale.x = -oldbaseScale.x;
            isMoving = true;
        }
        else if (velocity > 0.1)
        {
            newBaseScale.x = oldbaseScale.x;
            isMoving = true;
        }
      

        transform.localScale = newBaseScale;
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

        if(groundLineCast)
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
    }
    void checkObjectinFront()
    {
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer); // makes a box that will check whats in front

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube( wallCheck.position, wallCheckSize);
    }
}
