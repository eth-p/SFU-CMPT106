using UnityEngine;

/// <summary>
/// The standard health manager for entities.
/// </summary>
public class Health : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The maximum health of an entity.
	/// </summary>
	public float Maximum = 10f;
	
	/// <summary>
	/// The number of invincibility frames given to an entity when damaged.
	/// </summary>
	public uint Iframes = 10;



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

	/// <summary>
	/// The current invincibility frames.
	/// </summary>
	public uint Invincibility {
		get { return invincibility; }
		set { invincibility = value;  }
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected float health = 10f;
	protected uint invincibility = 0;

	protected DeathBehaviour[] on_death;
	protected HurtBehaviour[] on_hurt;
	protected Rigidbody2D body;
	protected Collider2D col;

	
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
		EmitHurt(amount);
		
		// If dead, run Kill().
		if (health <= 0f) {
			Kill();
		}
		
		// If no invincibility, run OnVulnerable().
		if (invincibility == 0) {
			EmitVulnerable();
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
		EmitDeath();
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Run the DeathBehaviour.OnDeath handlers.
	/// </summary>
	protected void EmitDeath() {
		foreach (DeathBehaviour handler in on_death) {
			handler.OnDeath();
		}
	}
	
	/// <summary>
	/// Run the HurtBehaviour.OnHurt handlers.
	/// </summary>
	protected void EmitHurt(float amount) {
		foreach (HurtBehaviour handler in on_hurt) {
			handler.OnHurt(amount);
		}
	}
	
	/// <summary>
	/// Run the HurtBehaviour.OnVulnerable handlers.
	/// </summary>
	protected void EmitVulnerable() {
		foreach (HurtBehaviour handler in on_hurt) {
			handler.OnVulnerable();
		}
	}
	
	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	public void Start() {
		on_death = this.GetInterfaces<DeathBehaviour>();
		on_hurt = this.GetInterfaces<HurtBehaviour>();
		body = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		Value = Maximum;
    }

	
	/// <summary>
	/// [UNITY] Called every tick.
	/// </summary>
	public void FixedUpdate() {
		if (invincibility > 0) {
			if (--invincibility < 1) {
				EmitVulnerable();
			}
		}
	}
}