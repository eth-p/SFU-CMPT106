using UnityEngine;

/// <summary>
/// A script that immediately despawns the GameObject.
/// </summary>
public class DespawnImmediately : MonoBehaviour {
	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
