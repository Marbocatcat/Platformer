using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{

    Animator animator;
    SpriteRenderer spriteRenderer;
    AudioSource lootChestAudio;


    private bool lootOpening;
    private bool lootOpened;
    private bool isLooted;

    public GameObject potionOut;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lootChestAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        handleAnimation();
    }

    void handleAnimation()
    {
        if(lootOpening)
        {
            
            
            lootOpened = true;
            
            animator.Play("loot_box_open");
        }

        if(lootOpened)
        {
            spriteRenderer.name = "loot_box_open";
           
            if (!isLooted)
            {
                lootChestAudio.Play();
                StartCoroutine("openLoot");
            }
            
        }
        else
        {
            animator.Play("loot_box");
        }

    }



    void spitOutCoin()
    {
        GameObject potion = (GameObject)Instantiate(potionOut, new Vector3(transform.position.x, transform.position.y + 1f, 0), transform.rotation);
    }

    IEnumerator openLoot()
    {
        isLooted = true;
        yield return new WaitForSeconds(.5f);
        spitOutCoin();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            lootOpening = true;
            
        }
    }
}
