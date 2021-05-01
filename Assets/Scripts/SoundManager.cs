using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [Header ("AUDIO EFFECTS")]
    public AudioSource damageClip;
    public AudioSource coinPickup;
    public AudioSource mushroomDeath;
    public AudioSource heroJump;
    public AudioSource stoneDrag;
    public AudioSource healthPotionPickup;

    private static bool soundManagerExists;


    // Start is called before the first frame update
    void Start()
    {
        if(!soundManagerExists)
        {
            soundManagerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
