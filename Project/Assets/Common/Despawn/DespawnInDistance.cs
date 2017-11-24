using UnityEngine;

/// <summary>
/// A script that despawns the GameObject after it reaches a certain distance.
/// </summary>
public class DespawnInDistance : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The distance to exist for.
	/// </summary>
	public float Distance = 30f;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected Vector2 initial;
	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:
	
	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		initial = transform.position;
	}
	
	/// <summary>
	/// [UNITY] Called every tick.
	/// </summary>
	protected void FixedUpdate() {
		Vector2 pos = ((Vector2) transform.position) - initial;
		if (((pos.x * pos.x) + (pos.y * pos.y)) > (Distance * Distance)) {
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
}
