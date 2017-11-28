using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSpitController : MonoBehaviour {

    GameObject spider;
	// Use this for initialization
	void Start () {
        spider = GameObject.Find("Enemy_Spider");
    }

    // Update is called once per frame
    void Update () {
        Physics2D.IgnoreLayerCollision(29, 8);
        Destroy(gameObject, 0.5f);
    }
    
}
