using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handFollowing : MonoBehaviour
{


    public float speed = 10f;

    void Start()
    {
       
    }

    void Update()
    {
        Vector2 direction = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (angle < 85 && angle > -85)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        }
    }
}