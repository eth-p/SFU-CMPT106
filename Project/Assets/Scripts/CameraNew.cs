using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;

public class CameraNew : MonoBehaviour {

	public GameObject target;
	public Transform room_top_left;
	public Transform room_top_right;
	public Transform room_bottom_left;
	public Transform room_bottom_right;
	
	public bool smooth;
	
	private Collider2D col;
	private float min_x;
	private float max_x;
	private float min_y;
	private float max_y;

	// Use this for initialization
	void Start () {
		col = target.GetComponent<Collider2D>();

		min_x = Mathf.Max(room_top_left.position.x + 0.5f, room_bottom_left.position.x + 0.5f);
		max_x = Mathf.Min(room_top_right.position.x - 0.5f, room_bottom_right.position.x - 0.5f);
		
		min_y = Mathf.Max(room_bottom_right.position.x + 0.5f, room_bottom_left.position.x + 0.5f);
		max_y = Mathf.Min(room_top_right.position.x - 0.5f, room_top_right.position.x - 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		float x = col.bounds.min.x - 5f;
		float y = col.bounds.min.y;

		x = Mathf.Max(x, min_x);
		x = Mathf.Min(x, max_x);
		
		this.transform.position = new Vector3(x, y, -10);
	}
}
