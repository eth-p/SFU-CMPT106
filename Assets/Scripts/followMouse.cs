using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMouse : MonoBehaviour {

    GameObject target;

    void Start()
    {
        target = GameObject.Find("mouse");
    }

    void Update () {
        transform.LookAt(new Vector3(Input.mousePosition.x, Input.mousePosition.y,0));
        Vector3 targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        transform.LookAt(targetPosition);
    }

}
