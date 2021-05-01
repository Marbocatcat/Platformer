using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    // make a singleton, so other scripts can access.
    public static ScreenShake instance;

    private float shakeTimeRemaining; // how long we want the shake to last for,
    private float shakePower; // amount of shake we want
    private float shakeFadeTime; // how much time till the shake fades;
    private float shakeRotation;

    public float rotationMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            StartShake(.5f, .5f);
        }
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime); // it smoothly goes to 0 from shake power ,for smooth transition.

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0 , 0 , shakeRotation * Random.Range(-1f, 1f));

    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;


        shakeFadeTime = power / length;
        shakeRotation = power * rotationMultiplier;


    }

}
