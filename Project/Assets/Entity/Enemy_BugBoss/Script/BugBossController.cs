using UnityEngine;

/// <summary>
/// A last-minute derivation of ZombieResearcherController
/// </summary>
public class BugBossController : MonoBehaviour, DeathBehaviour, HurtBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Constants:

	protected const float SPEED_ATTACK = 20f;


	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The layers which cause a wall collision.
	/// </summary>
	public LayerMask GroundLayers;
	
	/// <summary>
	/// The number of ticks before a state change when the entity is idle. 
	/// </summary>
	public int TicksDazed = 60;
	

	
	// -----------------------------------------------------------------------------------------------------------------
	// Enum:

	/// <summary>
	/// The entity's state.
	/// </summary>
	public enum State {
		ATTACK,
		DAZED
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	private int dazedTicks;
	private State state;

	private Rigidbody2D body;
	private Animator animator;

	private Vector2 moveDirection;
	private Health health;


	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	void Start() {
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
		health = GetComponent<Health>();

        GameObject backGroundMusic = GameObject.Find("level_background&revised");
        AudioSource backAudioSrc = backGroundMusic.GetComponent<AudioSource>();
        backAudioSrc.Pause();

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();

        moveDirection.x = -1f;
		state = State.DAZED;
		health.Invincibility = 60;
		dazedTicks = 60;
	}

	/// <summary>
	/// [UNITY] Called every tick.
	/// </summary>
	void FixedUpdate() {
		if (state == State.ATTACK) {
			health.Invincibility = 1;
			body.velocity = moveDirection;
		}
		
		if (dazedTicks > 0) {
			if (--dazedTicks <= 0) {
				animator.SetTrigger("Attack");
				state = State.ATTACK;
				if (moveDirection.x < 0) {
					FaceRight();
				} else {
					FaceLeft();
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (GroundLayers.Contains(col.gameObject.layer)) {
			animator.SetTrigger("Dazed");
			state = State.DAZED;
			dazedTicks = TicksDazed;
		}
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Implement: DeathBehaviour, HurtBehaviour

	/// <inheritdoc cref="DeathBehaviour.OnDeath"/>
	public void OnDeath() {
		animator.SetTrigger("Death");
		gameObject.SetActive(false);
		Destroy(this);
	}

	/// <inheritdoc cref="HurtBehaviour.OnHurt"/>
	public void OnHurt(float amount) {
		animator.SetBool("Hurt", true);
	}

	/// <inheritdoc cref="HurtBehaviour.OnVulnerable"/>
	public void OnVulnerable() {
		animator.SetBool("Hurt", false);
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:


	/// <summary>
	/// Make the entity face left.
	/// </summary>
	void FaceLeft() {
		Vector3 ls = transform.localScale;
		if (ls.x < 0) {
			moveDirection = new Vector2(-SPEED_ATTACK, 0.1f);
			transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
		}
	}

	/// <summary>
	/// Make the entity face right.
	/// </summary>
	void FaceRight() {
		Vector3 ls = transform.localScale;
		if (ls.x > 0) {
			moveDirection = new Vector2(SPEED_ATTACK, 0.1f);
			transform.localScale = new Vector3(-ls.x, ls.y, ls.z);
		}
	}
}