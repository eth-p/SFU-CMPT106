using UnityEngine;

/// <summary>
/// Heal an entity on contact.
/// </summary>
public class HealthPickupController : MonoBehaviour {
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

		// Heal entity.
		health.Value += Amount;
		
		// Destroy pickup.
		gameObject.SetActive(false);
		Destroy(gameObject);
	}

}