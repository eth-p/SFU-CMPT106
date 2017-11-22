using UnityEngine;

/// <summary>
/// Controller for a bullet projectile.
/// </summary>
public class BulletController : ProjectileController {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float Damage = 1f;
	public float Knockback = 5f;

	public LayerMask AffectLayers;
	
	// -----------------------------------------------------------------------------------------------------------------
	// ProjectileController:

	public override void OnCollide(Collider2D col) {
		// Check if collided object is affected.
		if (!AffectLayers.Contains(col.gameObject.layer)) {
			return;
		}
		
		// Get health component of collided object.
		// If it can't be found, we didn't collide with anything attackable.
		Health health = col.gameObject.GetComponent<Health>();
		if (health == null) {
			return;
		}
		
		// Attack colided object.
		if (Knockback > 0f) {
			health.DamageWithKnockback(Damage, col, Knockback);
		} else {
			health.Damage(Damage);
		}
	}

}