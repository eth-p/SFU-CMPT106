using UnityEngine;

public class PlayerController : MonoBehaviour, DeathBehaviour, HurtBehaviour {
	
	protected const bool DEBUG = true;

	protected const float SPEED = 1.5f;
	protected const float SPEED_MAX = 4.5f;

	// -----------------------------------------------------------------------------------------------------------------
	// CONFIGURABLE VARIABLES:

	public LayerMask[] GroundLayers;
	public int Jumps = 1;

	// -----------------------------------------------------------------------------------------------------------------
	// INTERNAL VARIABLES:
	
	private int jumpsRemaining;
	private bool jumping;
	private bool falling;
	private float fallingLastY;

	private Health health;
	private Collider2D col;
	private Rigidbody2D body;
	private Animator animator;

	// -----------------------------------------------------------------------------------------------------------------
	// UNITY:
	void Start() {
		body = GetComponent<Rigidbody2D>();
		body.drag = 10;

		health = GetComponent<Health>();
		animator = GetComponent<Animator>();
		col = GetComponent<Collider2D>();
	}

	void Update() {
		HandleAnimation();
	}

	void FixedUpdate() {
		HandleGrounded();
		HandleMovement();
	}

	// -----------------------------------------------------------------------------------------------------------------
	// IMPLEMENT: DeathBehaviour, HurtBehaviour

	public void OnDeath() {
		Debug.Log("YOU DIED!");
	}

	public void OnHurt(float amount) {
		Debug.Log("You took " + amount + " damage.");
	}
	

	// -----------------------------------------------------------------------------------------------------------------
	// PLAYER:

	/// <summary>
	/// Update the animator states.
	/// </summary>
	void HandleAnimation() {
		var axis = Input.GetAxis("Horizontal");

		// Walking animations.
		if (Mathf.Abs(axis) < 0.1) {
			animator.SetBool("Idle", true);
		} else {
			animator.SetBool("Idle", false);
			animator.SetTrigger(axis < 0 ? "Flip Left" : "Flip Right");
			animator.ResetTrigger(axis < 0 ? "Flip Right" : "Flip Left");
		}

		// Falling animations.
		if (falling && body.position.y > fallingLastY) {
			fallingLastY = body.position.y;
		}

		animator.SetBool("Jumping", jumping);
		animator.SetBool("Falling", falling && body.velocity.y < -0.1);
		animator.SetFloat("FallDistance", fallingLastY - body.position.y);
	}

	/// <summary>
	/// Check if the player is on the ground, and update variables accordingly.
	/// </summary>
	void HandleGrounded() {
		// Calculate ray positions.
		float rayY = col.bounds.min.y;
		float rayMinX = col.bounds.min.x;
		float rayDiffX = col.bounds.max.x - rayMinX;

		Vector2[] rays = new Vector2[3];
		rays[0] = new Vector2(rayMinX, rayY);
		rays[rays.Length - 1] = new Vector2(rayMinX + rayDiffX, rayY);

		int nx = rays.Length - 1;
		for (int i = 1; i < nx; i++) {
			// Generate rays with even spacing between 1 and n-1.
			rays[i] = new Vector2(rayMinX + (i / (float) nx * rayDiffX), rayY);
		}

		// DEBUG: Draw Rays
		if (DEBUG) {
			foreach (var ray in rays) {
				Debug.DrawRay(ray, new Vector2(0f, -0.1f), Color.blue);
			}
		}

		// Cast rays to check if standing on ground.
		foreach (var ray in rays) {
			foreach (var mask in GroundLayers) {
				if (Physics2D.Raycast(ray, Vector2.down, 0.1f, mask.value)) {
					falling = false;
					jumping = false;
					jumpsRemaining = Jumps;
					return;
				}
			}
		}

		if (!falling) {
			falling = true;
			fallingLastY = body.position.y;
		}
	}

	/// <summary>
	/// Check what buttons are pressed, and move the player if necessary.
	/// </summary>
	void HandleMovement() {
		var axis = Input.GetAxis("Horizontal");
		var move = axis * SPEED;
		var vx = body.velocity.x;
		var vy = body.velocity.y;

		// Jumping.
		if (jumpsRemaining > 0 && Input.GetButtonDown("Jump")) {
			jumping = true;
			jumpsRemaining--;
			vy = 30;
		}

		// Movement.
		if (Mathf.Abs(vx + move) < SPEED_MAX || ((int) Mathf.Sign(vx) != (int) Mathf.Sign(move))) {
			vx += move;
		}

		// Update.
		body.velocity = new Vector2(
			vx,
			vy
		);
	}
}