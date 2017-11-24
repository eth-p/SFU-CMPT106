using UnityEngine;

/// <summary>
/// A script that despawns the GameObject after a number of ticks.
/// </summary>
public class DespawnInTicks : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The number of ticks to exist for.
	/// </summary>
	public uint Ticks = 120;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected uint elapsed;
	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:
	
	/// <summary>
	/// [UNITY] Called every tick.
	/// </summary>
	protected void FixedUpdate() {
		if ((++elapsed) > Ticks) {
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
