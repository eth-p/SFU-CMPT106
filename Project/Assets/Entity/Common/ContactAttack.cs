using UnityEngine;

/// <summary>
/// Damage an entity on contact.
/// </summary>
public class ContactAttack : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float Damage = 0.5f;
	public float Knockback = 1f;

	public LayerMask AffectLayers;

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:
	
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
	// Unity:

	void Start() {
		col = GetComponent<Collider2D>();
	}

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
			Debug.Log("KB");
			health.DamageWithKnockback(Damage, col, Knockback);
		} else {
			health.Damage(Damage);
		}
	}

}