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
    private bool isFacingRight = false;

    [Header("Attack Check")]
    [SerializeField] BoxCollider2D attackCollider;

    [Header("Wall Jump")]
    [SerializeField] float wallJumpTime;
    [SerializeField] float wallSlideSpeed;
    private float jumpTime;
    private bool isWallSliding = false;
    private bool isAttacking;
    float xHorizontal;

    Animator animator;
    Rigidbody2D heroBody2D;
    SpriteRenderer spriteRenderer;
    Vector3 baseScale;


    void handleSwordAttack()
    {
        if (Input.GetKeyDown("mouse 0"))
        {
            isAttacking = true;
            attackCollider.enabled = true;
        }
        
    }
    void handleMovement()
    {
        xHorizontal = Input.GetAxisRaw("Horizontal");
        heroBody2D.velocity = new Vector2(xHorizontal * speed, heroBody2D.velocity.y);

        if(xHorizontal != 0)
        {
            isMoving = true;
        }
        else if(xHorizontal == 0)
        {
            isMoving = false;
        }


    }
    void handleJumping()
    {
        if (canJump && isGrounded || isWallSliding && canJump) // if i can jump, not on the ground , and not falling then I can jump.
        {
            heroBody2D.AddForce(new Vector2(heroBody2D.velocity.x, jumpForce), ForceMode2D.Impulse);
            canJump = false;

        }


    }
    IEnumerator resetSwordAttack()
    {
        yield return new WaitForSeconds(.1f);
        isAttacking = false;
        attackCollider.enabled = false;

    }
    void animationControl()
    {
        if (isMoving && isGrounded && !isAttacking)
        {
            if (isTouchingWall)
            {
                animator.Play("hero_push");
            }
            else
            {
                animator.Play("hero_run");
            }
        }
        if (isAttacking)
        {
            animator.Play("hero_sword_attack");
            StartCoroutine("resetSwordAttack");
        }

        if (!isMoving && isGrounded && !isAttacking)
        {
            animator.Play("hero_idle");
        }


        if (!isGrounded && !isWallSliding)
        {
            if (isJumping && !isAttacking)
            {
                animator.Play("hero_jump");
            }
            else if (isFalling && !isAttacking)
            {
                animator.Play("hero_falling");
            }
        }

        if (isWallSliding)
        {
            animator.Play("hero_idle");
        }
    }
    void handleDirection()
    {
        if (xHorizontal > 0 && !isFacingRight)
        {
            flip();
        }
        else if (xHorizontal < 0 && isFacingRight)
        {
            flip();
        }
    }

    void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
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
        else if(yVelocity == 0)
        {
            isJumping = false;
            isFalling = false;
        }
    }
    void groundCheck()
    {
        RaycastHit2D groundLineCast = Physics2D.Linecast(transform.position, groundCheckPlaceholder.position, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D stoneLineCast = Physics2D.Linecast(transform.position, groundCheckPlaceholder.position, 1 << LayerMask.NameToLayer("Wall"));

        if (groundLineCast == true || stoneLineCast == true)
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
    void handleWallJump()
    {
        if(checkObjectinFront() == true && !isGrounded && xHorizontal !=0) // if a wall is infront of you and you are not in the ground and you are pushing left or right.
        {
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime; // creates a timer, jump time is equal to the current time + wallJumpTime.

        } else if(jumpTime < Time.time)
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            heroBody2D.velocity = new Vector2(heroBody2D.velocity.x, Mathf.Clamp(heroBody2D.velocity.y, wallSlideSpeed, float.MaxValue));

        }
    }
    bool checkObjectinFront()
    {
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer); // makes a box that will check whats in front

        return isTouchingWall;
    }
   


    void Start()
    {
        attackCollider.enabled = false;
        animator = GetComponent<Animator>();
        heroBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
    }
    void Update()
    {
        jumpCheck();
        groundCheck();
        animationControl();
        checkIfFalling();
        checkObjectinFront();
        handleDirection();
        handleWallJump();
        handleSwordAttack();
    }
    private void FixedUpdate()
    {
        handleMovement();
        handleJumping();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
