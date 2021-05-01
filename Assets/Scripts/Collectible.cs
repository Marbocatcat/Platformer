using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool pickedUp;

    private SoundManager soundManager;
    private Animator animator;

    bool isAudioPlayed;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        animator = GetComponent<Animator>();
        floatUp();
    }

    // Update is called once per frame
    void Update()
    {
        handleAnimation();
    }

    void floatUp()
    {
        // offsets the coin a bit higher when dropped.
        transform.position = new Vector2(transform.position.x, transform.position.y - .3f);
    }
    void handleAnimation()
    {
        if (pickedUp)
        {
            if(this.CompareTag("Coin"))
            {
                
                animator.Play("coin_pickup");
                Destroy(gameObject, .5f);
            }
            else if(this.CompareTag("Potion"))
            {
                
                Destroy(gameObject, .5f);
            }

            handleSound();
        }

    }

    void handleSound()
    {
        if(this.CompareTag("Coin"))
        {
            if (!isAudioPlayed)
            {
                soundManager.coinPickup.Play();
                isAudioPlayed = true;
            }
        }

        else if(this.CompareTag("Potion"))
        {
            if(!isAudioPlayed)
            {
                soundManager.healthPotionPickup.Play();
                isAudioPlayed = true;
            }
        }
       
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // play pick up animation then destroy game
            pickedUp = true;
        }
    }
}
