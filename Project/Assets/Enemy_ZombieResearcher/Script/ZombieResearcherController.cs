using System;
using UnityEngine;

public class ZombieResearcherController : MonoBehaviour, DeathBehaviour, HurtBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Constants:

	protected const float SPEED_WALK = 1.5f;
	protected const float SPEED_ATTACK = 2f;

	protected float SEE_DISTANCE = 8f;
	protected float WALK_DISTANCE = 1f;


	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public LayerMask GroundLayers;
	public int TicksIdle = 60;
	public int TicksMove = 60;
	public int TicksAttacking = 180;

	// -----------------------------------------------------------------------------------------------------------------
	// Enum:

	public enum State {
		IDLE,
		WALK,
		ATTACK
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	private int moveTicks;
	private Vector2 moveDirection;
	
	private int updateTicks;
	private State state;

	private Health health;
	private Rigidbody2D body;
	private Animator animator;

	private GameObject player;
	private Rigidbody2D player_body;

	private Vector2 anchor;


	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	void Start() {
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();

		player = GameObject.Find("Player");
		player_body = player.GetComponent<Rigidbody2D>();

		state = State.WALK;
		anchor = transform.position;
	}

	void FixedUpdate() {
		if (--updateTicks < 0) {
			UpdateState();
		}
		
		switch (state) {
			case State.WALK:
				ActWalk();
				break;

			case State.ATTACK:
				ActAttack();
				break;

			case State.IDLE:
				ActIdle();
				break;
		}
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Implement: DeathBehaviour, HurtBehaviour

	public void OnDeath() {
		animator.SetTrigger("Death");
	}

	public void OnHurt(float amount) {
		animator.SetBool("Hurt", true);
	}

	public void OnVulnerable() {
		animator.SetBool("Hurt", false);
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Enemy_ZombieResearcher:

	void UpdateState() {
		if (gameObject.CanSee(player_body, SEE_DISTANCE, GroundLayers)) {
			animator.ResetTrigger("Target Lost");
			animator.SetTrigger("Target Acquired");
			state = State.ATTACK;
			updateTicks = TicksAttacking;
		} else {
			animator.SetTrigger("Target Lost");
			if (UnityEngine.Random.Range(0, 2) == 1) {
				state = State.WALK;
				updateTicks = TicksMove;
			} else {
				state = State.IDLE;
				updateTicks = TicksIdle;
			}
		}
	}

	void ActAttack() {
		body.velocity = new Vector2(Mathf.Sign(player_body.position.x - body.position.x) * SPEED_WALK, 0f);
		if (body.velocity.x < 0) {
			FlipLeft();
		} else {
			FlipRight();
		}
	}

	void ActIdle() {
	}

	void ActWalk() {
		if (--moveTicks < 0) {
			moveTicks = TicksMove;

			// Determine direction.
			int rng = UnityEngine.Random.Range(0, 2);
			if (rng == 0) {
				FlipLeft();
				moveDirection = new Vector2(-SPEED_WALK, 0);
			} else {
				FlipRight();
				moveDirection = new Vector2(SPEED_WALK, 0);
			}
		}
		
		// Update.
		Vector2 distance = anchor + new Vector2(transform.position.x, transform.position.y);
		if (Mathf.Abs(distance.x) < WALK_DISTANCE || Mathf.Sign(moveDirection.x) != Mathf.Sign(distance.x)) {
			animator.SetTrigger("Walk");
			body.velocity = moveDirection;
		} else {
			animator.SetTrigger("Idle");
		}
	}

	void FlipLeft() {
		Vector3 ls = transform.localScale;
		if (ls.x < 0) {
			transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
		}
	}

	void FlipRight() {
		Vector3 ls = transform.localScale;
		if (ls.x > 0) {
			transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
		}
	}
}