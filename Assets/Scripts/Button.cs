using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Button : MonoBehaviour
{

    private bool isTriggered;

    [SerializeField] GameObject door;
    [SerializeField] Sprite button;
    [SerializeField] Sprite buttonPressed;

    SpriteRenderer spriteRenderer;

    private bool isPressed;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        doorAnimation();
        doorSwitch();
    }

    void doorSwitch()
    {

        if(isPressed)
        {
            door.GetComponent<Door>().DoorSwitch(true);
        }
        else if(!isPressed)
        {
            door.GetComponent<Door>().DoorSwitch(false);
        }
    }
    void doorAnimation()
    {
        if(isPressed)
        {
            spriteRenderer.sprite = buttonPressed;
        }
        else
        {
            spriteRenderer.sprite = button;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Stone"))
        {
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }
    }
}
