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

	public LayerMask[] GroundLayers = { };
	public int TicksIdle = 120;
	public int TicksMove = 60;
	public int TicksAttacking = 60;

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

	void Update() {
	}

	void FixedUpdate() {
		UpdateState();
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
		animator.SetTrigger("Hurt");
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Enemy_ZombieResearcher:

	void UpdateState() {
	}

	void ActAttack() {
	}

	void ActIdle() {
	}

	void ActWalk() {
		if (--moveTicks < 0) {
			moveTicks = TicksMove;

			// Determine direction.
			int rng = UnityEngine.Random.Range(0, 2);
			Debug.Log(rng);
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
			body.velocity = moveDirection;
		} else {
			
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