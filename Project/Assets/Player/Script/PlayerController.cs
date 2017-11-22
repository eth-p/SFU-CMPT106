using UnityEngine;

public class PlayerController : MonoBehaviour, DeathBehaviour, HurtBehaviour, CameraManipulator {
	// -----------------------------------------------------------------------------------------------------------------
	// Constants:

	protected const float SPEED = 1.5f;
	protected const float SPEED_MAX_FORWARDS = 4.5f;
	protected const float SPEED_MAX_BACKWARDS = 3.5f;


	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public LayerMask GroundLayers;
	public LayerMask EnemyLayers;
	public int Jumps = 1;


	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	private int[] cache_EnemyLayers;

	private int jumpsRemaining;
	private bool jumping;
	private bool falling;
	private float fallingLastY;

	private bool facingLeft;

	private Health health;
	private Collider2D col;
	private Rigidbody2D body;
	private Animator animator;

	private ArmRotate armRotate;


	// -----------------------------------------------------------------------------------------------------------------
	// Unity:
	void Start() {
		body = GetComponent<Rigidbody2D>();
		body.drag = 10;

		health = GetComponent<Health>();
		animator = GetComponent<Animator>();
		col = GetComponent<Collider2D>();

		armRotate = GetComponentInChildren<ArmRotate>();

		// Set up layer caches.
		cache_EnemyLayers = EnemyLayers.ToLayers();
	}

	void Update() {
		ApplyAnimation();
	}

	void FixedUpdate() {
		CheckGrounded();
		ApplyMovement();
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Implement: DeathBehaviour, HurtBehaviour

	public void OnDeath() {
		Debug.Log("YOU DIED!");
	}

	public void OnHurt(float amount) {
		foreach (int layer in cache_EnemyLayers) {
			Physics2D.IgnoreLayerCollision(gameObject.layer, layer, true);
		}

		Debug.Log("You took " + amount + " damage. You're at " + health.Value + " health.");
	}

	public void OnVulnerable() {
		foreach (int layer in cache_EnemyLayers) {
			Physics2D.IgnoreLayerCollision(gameObject.layer, layer, false);
		}

		Debug.Log("Vulnerable");
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Implement: CameraManipulator

	private Vector2 cam_manip;

	/// <summary>
	/// Manipulate the camera to move closer towards where the cursor is pointing.
	/// </summary>
	public void ManipulateCamera(ref Vector2 pos) {
		Vector3 cursor = Input.mousePosition;

		// Calculate offsets.
		float mx = (cursor.x - (Screen.width / 2f)) / Screen.width;
		float my = (cursor.y - (Screen.height / 2f)) / Screen.height;

		// Clamp and modify offsets.
		mx = Mathf.Clamp(mx, -0.5f, 0.5f) * 2.5f;
		my = Mathf.Clamp(my, -0.5f, 0.5f) * 1f;

		// Dampen.
		cam_manip = Vector2.Lerp(cam_manip, new Vector2(mx, my), Time.deltaTime * 2f);

		// Update camera.
		pos += cam_manip;
	}


	// -----------------------------------------------------------------------------------------------------------------
	// API:

	public void FaceLeft() {
		if (facingLeft) return;

		facingLeft = true;
		animator.SetTrigger("Flip Left");
		animator.ResetTrigger("Flip Right");
	}

	public void FaceRight() {
		if (!facingLeft) return;

		facingLeft = false;
		animator.SetTrigger("Flip Right");
		animator.ResetTrigger("Flip Left");
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Player:

	/// <summary>
	/// Update the animator states.
	/// </summary>
	void ApplyAnimation() {
		var axis = Input.GetAxis("Horizontal");

		// Walking animations.
		if (Mathf.Abs(axis) < 0.1) {
			animator.SetBool("Idle", true);
		} else {
			animator.SetBool("Idle", false);
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
	/// Check what buttons are pressed, and move the player if necessary.
	/// </summary>
	void ApplyMovement() {
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
		float max = (move < 0 && facingLeft) || (move > 0 && !facingLeft) ? SPEED_MAX_FORWARDS : SPEED_MAX_BACKWARDS;
		if (Mathf.Abs(vx + move) < max || ((int) Mathf.Sign(vx) != (int) Mathf.Sign(move))) {
			vx += move;
		}

		// Update.
		body.velocity = new Vector2(
			vx,
			vy
		);
	}

	/// <summary>
	/// Check if the player is on the ground, and update variables accordingly.
	/// </summary>
	void CheckGrounded() {
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
		if (DebugSettings.RAYCAST_GROUNDCHECK) {
			foreach (var ray in rays) {
				Debug.DrawRay(ray, new Vector2(0f, -0.1f), Color.blue);
			}
		}

		// Cast rays to check if standing on ground.
		foreach (var ray in rays) {
			if (Physics2D.Raycast(ray, Vector2.down, 0.1f, GroundLayers)) {
				falling = false;
				jumping = false;
				jumpsRemaining = Jumps;
				return;
			}
		}

		if (!falling) {
			falling = true;
			fallingLastY = body.position.y;
		}
	}
}