using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage the player on contact.
/// </summary>
public class ContactAttack : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float DAMAGE = 0.5f;
	public float KNOCKBACK = 1f;

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:
	private Collider2D col;
	
	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	void Start() {
		col = GetComponent<Collider2D>();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		Health health = collision.collider.gameObject.GetComponent<Health>();
		if (health == null) {
			return;
		}

		if (KNOCKBACK > 0f) {
			health.DamageWithKnockback(DAMAGE, col, KNOCKBACK);
		} else {
			health.Damage(DAMAGE);
		}
	}
}