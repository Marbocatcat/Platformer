using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;

    [Header("Wall Check")]
    [SerializeField] Transform wallCheck;
    [SerializeField] Vector2 wallCheckSize;
    [SerializeField] LayerMask wallLayer;
    private bool isTouchingWall;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float radius;
    [SerializeField] LayerMask groundLayer;
    private bool isTouchingGround;

    private bool isFacingRight = true;
    private bool isHit;
    private float staggerJump = .5f;
    private bool hitCheck;

    public float health;
    private bool dead;

    Animator animator;
    Rigidbody2D mushroomRigidBody;

    
    // Start is called before the first frame update
    void Start()
    {
        mushroomRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    bool checkObjectinFront()
    {
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer); // makes a box that will check whats in front

        return isTouchingWall;
    }

    bool checkGround()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.transform.position, radius, groundLayer);

        return isTouchingGround;
    }
    void handleMoving()
    {
        if(isFacingRight && !isHit && !dead)
        {
            mushroomRigidBody.velocity = new Vector2(speed, mushroomRigidBody.velocity.y);
        }
        else if(!isFacingRight && !isHit && !dead)
        {
            mushroomRigidBody.velocity = new Vector2(-speed, mushroomRigidBody.velocity.y);
        }
        else if(isHit)
        {
            mushroomRigidBody.velocity = Vector2.zero;
        }
       
    }
    void handleDirection()
    {
        if(checkObjectinFront() || isTouchingGround == false)
        {
            if(isFacingRight)
            {
                flip();
            }
            else if(!isFacingRight)
            {
                flip();
            }
        }
 
    }
    
    void handleDeath()
    {
        if(health <= 0)
        {
            dead = true;
            animator.Play("mushroom_die");
            StartCoroutine("destroyMushroom");
        }
    }
    void handleHealth()
    {
        if(isHit == true && hitCheck == false)
        {
            health-=1;
            hitCheck = true;
            StartCoroutine("hitCheckReset");
        }
    }
    void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }
    void handleAnimation()
    {
        if(isHit && !dead)
        {
            
            animator.Play("mushroom_hit");
            StartCoroutine("resetHit");
            
        }
        else if(!isHit && !dead)
        {
            animator.Play("mushroom_walk");
        }
    }

    IEnumerator destroyMushroom()
    {
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }

    IEnumerator resetHit()
    {
        yield return new WaitForSeconds(.2f);
        isHit = false;
    }

  
    IEnumerator hitCheckReset()
    {
        yield return new WaitForSeconds(.5f);
        hitCheck = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkObjectinFront();
        checkGround();
        handleDirection();
        handleAnimation();
        handleMoving();
        handleDeath();
        handleHealth();


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Sword"))
        {
            isHit = true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);

        Gizmos.DrawWireSphere(groundCheck.transform.position, radius);
    }
}
