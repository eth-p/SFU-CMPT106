using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Damage the player on contact.
/// </summary>
public class ContactAttack : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// CONFIGURABLE VARIABLES:

	public float DAMAGE = 0.5f;
	public float KNOCKBACK = 1f;

	// -----------------------------------------------------------------------------------------------------------------
	// UNITY:

	void OnCollisionEnter2D(Collision2D col) {
		Health health = col.collider.gameObject.GetComponent<Health>();
		if (health == null) {
			return;
		}

		if (KNOCKBACK > 0f) {
			health.DamageWithKnockback(DAMAGE, transform.position, KNOCKBACK);
		} else {
			health.Damage(DAMAGE);
		}
	}
}