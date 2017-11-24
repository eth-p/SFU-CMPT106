using UnityEngine;

/// <summary>
/// Damage an entity on contact.
/// </summary>
public class ContactAttack : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The amount of damage done.
	/// </summary>
	public float Damage = 0.5f;
	
	/// <summary>
	/// The knockback done.
	/// </summary>
	public float Knockback = 5f;

	/// <summary>
	/// The layers which can be attacked.
	/// </summary>
	public LayerMask AffectLayers;

	// -----------------------------------------------------------------------------------------------------------------
	// Variables:
	
	private Collider2D col;
	
	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Check if the attack affects an object.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <returns>True if the attack affects it, false otherwise.</returns>
	bool Affects(GameObject obj) {
		return AffectLayers.Contains(obj.layer);
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	void Start() {
		col = GetComponent<Collider2D>();
	}

	/// <summary>
	/// [UNITY] Called when the object collides with something else.
	/// </summary>
	void OnCollisionEnter2D(Collision2D collision) {
		// Check if it affects the layer of the collided object.
		if (!Affects(collision.gameObject)) {
			return;
		}
		
		// Get collided entity health component.
		Health health = collision.gameObject.GetComponent<Health>();
		if (health == null) {
			return; // Not attackable.
		}

		if (Knockback > 0f) {
			health.DamageWithKnockback(Damage, col, Knockback);
		} else {
			health.Damage(Damage);
		}
	}

}