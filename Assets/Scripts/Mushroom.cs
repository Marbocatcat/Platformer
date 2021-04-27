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

    private bool isFacingRight = true;

    Rigidbody2D mushroomRigidBody;

    
    // Start is called before the first frame update
    void Start()
    {
        mushroomRigidBody = GetComponent<Rigidbody2D>();
    }
    bool checkObjectinFront()
    {
        isTouchingWall = Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer); // makes a box that will check whats in front

        return isTouchingWall;
    }
    void handleMoving()
    {
        if(isFacingRight)
        {
            mushroomRigidBody.velocity = new Vector2(speed, mushroomRigidBody.velocity.y);
        }
        else if(!isFacingRight)
        {
            mushroomRigidBody.velocity = new Vector2(-speed, mushroomRigidBody.velocity.y);
        }
       
    }
    void handleDirection()
    {
        if(checkObjectinFront())
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

    void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        checkObjectinFront();
        handleDirection();
        handleMoving();

        Debug.Log(checkObjectinFront());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(wallCheck.position, wallCheckSize);
    }
}
