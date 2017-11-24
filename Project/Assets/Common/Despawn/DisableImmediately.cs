using UnityEngine;

/// <summary>
/// A script that immediately disables the GameObject.
/// </summary>
public class DisableImmediately : MonoBehaviour {
	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		gameObject.SetActive(false);
	}
}
