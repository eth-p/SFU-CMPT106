﻿using UnityEngine;

/// <summary>
/// Extension functions that assist in AI targeting.  
/// </summary>
static public class TargetingHelper {
	// -----------------------------------------------------------------------------------------------------------------
	// EXTENSIONS: GameObject

	/// <summary>
	/// Check to see if something is in the line of sight.
	/// </summary>
	/// <param name="self">The GameObject.</param>
	/// <param name="target">The target.</param>
	/// <param name="distance">The sight distance.</param>
	/// <param name="obstructions">An array of LayerMasks that will obstruct the line of sight.</param>
	/// <returns>True or false.</returns>
	static public bool CanSee(this GameObject self, Rigidbody2D target, float distance, LayerMask[] obstructions) {
		if (Mathf.Abs(target.transform.position.x - self.transform.position.x) > distance) {
			return false;
		}

		// Calculate ray direction.
		float angle = AngleHelper.RadiansBetween(self.transform.position, target.position);
		Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));

		// DEBUG: Draw Rays
		if (DebugSettings.RAYCAST_LOS) {
			Debug.DrawRay(self.transform.position, direction * 8f, Color.red, 1f);
		}

		// Raycast.
		RaycastHit2D[] los = Physics2D.RaycastAll(self.transform.position, direction, distance);
		foreach (RaycastHit2D hit in los) {
			foreach (LayerMask layer in obstructions) {
				if (layer.Contains(hit.collider.gameObject.layer))
					return false; // Obstructed.
			}

			if (hit.rigidbody != null && hit.rigidbody == target) {
				Debug.Log("FOUND");
				return true; // Found them!
			}
		}

		// Too far.
		return false;
	}
}