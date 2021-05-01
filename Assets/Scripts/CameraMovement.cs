using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    GameObject hero;
    [SerializeField]
    float timeOffset;
    [SerializeField]
    Vector2 posOffset;


    // Update is called once per frame
    void Update()
    {
        Vector3 startPos = transform.position; // camera start position
        Vector3 endPos = hero.transform.position; // player position / where we want the camera to move

        // add the camera offset so its not directly where the wizzard is. 
        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z = -10;

        // interpolation in between to dampen the hard camera movement / lerping, from start to the end position
        transform.position = Vector3.Lerp(startPos, endPos, timeOffset * Time.deltaTime);
    }
}
