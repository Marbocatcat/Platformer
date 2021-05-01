using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{

    private SoundManager soundManager;

    private bool isPlayerPushing;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(isPlayerPushing)
        {
            soundManager.stoneDrag.Play();
        }


        if(collision.gameObject.CompareTag("Player"))
        {
            isPlayerPushing = true;
            soundManager = FindObjectOfType<SoundManager>();
        }
    }
}
