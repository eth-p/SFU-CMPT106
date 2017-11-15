using UnityEngine;

public class Health : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// CONFIGURABLE VARIABLES:

	public float Maximum = 10f;
	public int Iframes = 10;

	// -----------------------------------------------------------------------------------------------------------------
	// INTERNAL VARIABLES:

	protected float health = 10f;
	protected int invincibility = 0;

	protected DeathBehaviour[] on_death;
	protected HurtBehaviour[] on_hurt;
	protected Rigidbody2D body;

	// -----------------------------------------------------------------------------------------------------------------
	// PUBLIC VARIABLES:

	/// <summary>
	/// The current health value.
	/// </summary>
	public float Value {
		get { return health; }
		set {
			health = Mathf.Clamp(value, 0, Maximum);
			if (health <= 0f) {
				Kill();
			}
		}
	}

	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Heal the entity.
	/// </summary>
	/// <param name="amount">The recovery value.</param>
	public void Heal(float amount) {
		health = Mathf.Min(health + amount, Maximum);
	}

	/// <summary>
	/// Damage the entity.
	/// </summary>
	/// <param name="amount">The damage value.</param>
	public void Damage(float amount) {
		if (invincibility > 0)
			return;

		// Apply invincibility and health damage.
		invincibility = Iframes;
		health -= amount;
		if (health <= 0f) {
			Kill();
		}

		// Run "OnHurt" handlers.
		foreach (HurtBehaviour handler in on_hurt) {
			handler.OnHurt(amount);
		}
	}

	/// <summary>
	/// Damage the entity and knock it back.
	/// </summary>
	/// <param name="damage">The damage value.</param>
	/// <param name="knockback">The knockback vector.</param>
	public void DamageWithKnockback(float damage, Vector2 knockback) {
		if (invincibility > 0)
			return;

		// Apply knockback.
		if (body != null) {
			body.AddForce(knockback);
		}

		// Apply damage.
		Damage(damage);
	}

	/// <summary>
	/// Damage the entity and knock it back.
	/// </summary>
	/// <param name="damage">The damage value.</param>
	/// <param name="attacker">The position of the attacker.</param>
	/// <param name="knockback">The knockback scale.</param>
	public void DamageWithKnockback(float damage, Vector2 attacker, float knockback) {
		if (invincibility > 0)
			return;

		// Calculate and apply knockback.
		if (body != null) {
			float angle = Mathf.Atan2(
				transform.position.x - attacker.x,
				transform.position.y - attacker.y
			);

			float vx = Mathf.Cos(angle) * -knockback;
			float vy = 0; // Mathf.Sin(angle) * knockback;

			Debug.Log(Mathf.Sign(body.velocity.x));
			Debug.Log(Mathf.Sign(vx));
			Debug.Log(vx);
			if ((int) Mathf.Sign(body.velocity.x) == (int) Mathf.Sign(vx)) {
				body.velocity += new Vector2(vx, vy);
			} else {
				Debug.Log("REPLACE");
				body.velocity = new Vector2(vx, vy);
			}
			
			Debug.Log(body.velocity);
		}

		// Apply damage.
		Damage(damage);
	}

	/// <summary>
	/// Kill the entity.
	/// 
	/// If the entity has a DeathBehaviour, it will run that.
	/// </summary>
	public void Kill() {
		if (on_death.Length == 0) {
			enabled = false;
			return;
		}

		// Run "OnDeath" handlers.
		foreach (DeathBehaviour handler in on_death) {
			handler.OnDeath();
		}
	}

	// -----------------------------------------------------------------------------------------------------------------
	// UNITY:
	public void Start() {
		on_death = this.GetInterfaces<DeathBehaviour>();
		on_hurt = this.GetInterfaces<HurtBehaviour>();
		body = GetComponent<Rigidbody2D>();
	}

	public void FixedUpdate() {
		if (invincibility > 0) {
			invincibility--;
		}
	}
}