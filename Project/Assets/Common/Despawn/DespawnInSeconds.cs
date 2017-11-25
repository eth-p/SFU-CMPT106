using UnityEngine;

/// <summary>
/// A script that despawns the GameObject after a number of seconds.
/// </summary>
public class DespawnInSeconds : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The number of seconds to exist for.
	/// </summary>
	public float Seconds = 2f;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected float elapsed = 0f;
	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:
	
	/// <summary>
	/// [UNITY] Called every frame.
	/// </summary>
	protected void Update() {
		if ((elapsed += Time.deltaTime) > Seconds) {
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
