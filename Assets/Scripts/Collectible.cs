using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool pickedUp;
    AudioSource potionAudio;

    bool isAudioPlayed;


    void Start()
    {
        potionAudio = GetComponent<AudioSource>();
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
            handlePotionSound();
            Destroy(gameObject, .5f);
        }

    }

    void handlePotionSound()
    {
        if(!isAudioPlayed)
        {
            potionAudio.Play();
            isAudioPlayed = true;
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
