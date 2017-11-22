using UnityEngine;

public class Health : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float Maximum = 10f;
	public int Iframes = 10;

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	protected float health = 10f;
	protected int invincibility = 0;

	protected DeathBehaviour[] on_death;
	protected HurtBehaviour[] on_hurt;
	protected Rigidbody2D body;
	protected Collider2D col;

	// -----------------------------------------------------------------------------------------------------------------
	// Public:

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
	/// <param name="attacker">The attacker.</param>
	/// <param name="knockback">The knockback scale.</param>
	public void DamageWithKnockback(float damage, Collider2D attacker, float knockback) {
		if (invincibility > 0)
			return;

		// Calculate and apply knockback.
		if (body != null) {
			Bounds pb = col.bounds;
			Bounds ab = attacker.bounds;
			
			Vector2 kbvec = new Vector2(
				Mathf.Sign(pb.center.x - ab.center.x) * knockback,
				1f * knockback
			);
			
			if ((int) Mathf.Sign(body.velocity.x) == (int) Mathf.Sign(kbvec.x)) {
				body.velocity += kbvec;
			} else {
				body.velocity = kbvec;
			}
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
		col = GetComponent<Collider2D>();
		Value = Maximum;
	}

	public void FixedUpdate() {
		if (invincibility > 0) {
			if (--invincibility < 1) {
				// Run "OnVulnerable" handlers.
				foreach (HurtBehaviour handler in on_hurt) {
					handler.OnVulnerable();
				}
			}
		}
	}
}