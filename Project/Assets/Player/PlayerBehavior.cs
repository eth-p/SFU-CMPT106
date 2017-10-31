using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

	protected const bool DEBUG = false;
	
	protected const float SPEED = 1.5f;
	protected const float SPEED_MAX = 4.5f;

	public LayerMask[] groundLayers;

	private int jumps;
	private int jumpsMax = 1;
	private bool jumping;
	private bool falling;
	private float falling_last_y;
	
	private Collider2D col;
	private Rigidbody2D body;
	private Animator animator;
 
	void Start() {
		this.body = GetComponent<Rigidbody2D>();
		this.body.drag = 10;
		
		this.animator = GetComponent<Animator>();
		this.col = GetComponent<Collider2D>();

	}
	
	void Update() {
		this.HandleAnimation();
	}

	void FixedUpdate() {
		this.HandleGrounded();
		this.HandleMovement();
	}

	void HandleAnimation() {
		var axis = Input.GetAxis("Horizontal");
		
		// Walking animations.
		if (Mathf.Abs(axis) < 0.1) {
			this.animator.SetBool("Idle", true);
		} else {
			this.animator.SetBool("Idle", false);
			this.animator.SetTrigger(axis < 0 ? "Flip Left" : "Flip Right");
			this.animator.ResetTrigger(axis < 0 ? "Flip Right" : "Flip Left");
		}
		
		// Falling animations.
		this.animator.SetBool("Jumping", jumping);
		this.animator.SetBool("Falling", falling && this.body.velocity.y < -0.1);
		this.animator.SetFloat("FallDistance", falling_last_y - this.body.position.y);
	}

	void HandleGrounded() {
		// Calculate ray positions.
		float rayY = this.col.bounds.min.y;
		float rayMinX = this.col.bounds.min.x;
		float rayDiffX = this.col.bounds.max.x - rayMinX;
		
		Vector2[] rays = new Vector2[3];
		rays[0] = new Vector2(rayMinX, rayY);
		rays[rays.Length - 1] = new Vector2(rayMinX + rayDiffX, rayY);
		
		int nx = rays.Length - 1;
		for (int i = 1; i < nx; i++) { // Genderate rays with even spacing between 1 and n-1.
			Debug.Log("Derpy: " + (i / (float) nx * rayDiffX));
			rays[i] = new Vector2(rayMinX + (i / (float) nx * rayDiffX), rayY);
		}

		// DEBUG: Draw Rays
		if (DEBUG) {
			foreach (var ray in rays) {
				Debug.DrawRay(ray, Vector3.down, Color.blue);
			}
		}
		
		// Cast rays to check if standing on ground.
		foreach (var ray in rays) {
			foreach (var mask in groundLayers) {
				if (Physics2D.Raycast(ray, Vector2.down, 0.1f, mask.value)) {
					falling = false;
					jumping = false;
					jumps = jumpsMax;
					return;
				}
			}
		}

		if (!falling) {
			falling = true;
			falling_last_y = this.body.position.y;
		}
	}

	void HandleMovement() {
		var axis = Input.GetAxis("Horizontal");
		var move = axis * SPEED;
		var vx = this.body.velocity.x;
		var vy = this.body.velocity.y;
		
		// Jumping.
		if (jumps > 0 && Input.GetButtonDown("Jump")) {
			jumping = true;
			jumps--;
			vy = 30;
		}
		
		// Movement.
		vx += move;
		
		// Update.
		this.body.velocity = new Vector2(
			Mathf.Clamp(vx, -SPEED_MAX, SPEED_MAX),
			vy
		);

	}
	
}
