using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{


    Animator animator;
    SpriteRenderer spriteRenderer;
    new BoxCollider2D collider2D;

    bool doorOpen;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        checkIfDoorOpen();
    }

    void checkIfDoorOpen()
    {
        // if doorOpen is true , play door open animation else play the door closed animation;
        if(doorOpen)
        {
            animator.Play("door_open");
            collider2D.isTrigger = true;
            
        }
        else if(!doorOpen)
        {
            animator.Play("door_closed");
            collider2D.isTrigger = false;
          
        }
    }

    public void DoorSwitch(bool isTriggered)
    {
        if(isTriggered)
        {
            doorOpen = true;
        }
        else if(!isTriggered)
        {
            doorOpen = false;
        }
    }
}
