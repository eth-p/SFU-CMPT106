using UnityEngine;

/// <summary>
/// Increase the maximum health of an entity on contact.
/// </summary>
public class HealthContainerPickupController : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float Amount = 1f;

	public LayerMask AffectLayers;
	
	// -----------------------------------------------------------------------------------------------------------------
	// Unity:


	void OnTriggerEnter2D(Collider2D col) {
		// Check if it affects the layer of the collided object.
		if (!AffectLayers.Contains(col.gameObject.layer)) {
			return;
		}
		
		// Get collided entity health component.
		Health health = col.gameObject.GetComponent<Health>();
		if (health == null) {
			return; // Not healable.
		}

		// Increase health and heal entity.
		health.Maximum += Amount;
		health.Value += Amount;
		
		// Destroy pickup.
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

}