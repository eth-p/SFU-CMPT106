using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage an entity on contact.
/// </summary>
public class ContactAttack : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float Damage = 0.5f;
	public float Knockback = 1f;

	public LayerMask IgnoreLayers;

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:
	
	private Collider2D col;
	
	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	void Start() {
		col = GetComponent<Collider2D>();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		// Check if layer ignored.
		if (IgnoreLayers.Contains(collision.gameObject.layer)) {
			return; // Ignored.
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