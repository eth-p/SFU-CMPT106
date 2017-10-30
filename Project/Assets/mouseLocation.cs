using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLocation : MonoBehaviour {

    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 mouseLocation = new Vector3(Input.mousePosition.x, Input.mousePosition.y , 0);
        Vector3 playerLocation = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        playerLocation = Camera.main.WorldToScreenPoint(playerLocation);
        transform.position = mouseLocation-playerLocation;
	}
}
