using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform focus; //camera's following object
    public float dampeningTime;
    Vector3 offset;
    float lowestX = -2.5f;
    float greatestX = 2.5f;

    // Use this for initialization
    void Start()
    {
        offset = new Vector3(transform.position.x - focus.position.x, 0f, -10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 focusPosition = focus.position + offset;
        transform.position = Vector3.Lerp(transform.position, focusPosition, dampeningTime * Time.deltaTime);
        if (transform.position.x < lowestX)
        {
            transform.position = new Vector3(lowestX, transform.position.y, transform.position.z);
        }
        if (transform.position.x > greatestX)
        {
            transform.position = new Vector3(greatestX, transform.position.y, transform.position.z);
        }
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}