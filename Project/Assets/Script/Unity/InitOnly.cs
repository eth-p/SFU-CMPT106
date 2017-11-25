using UnityEngine;

/// <summary>
/// A script that immediate disables the GameObject.
/// </summary>
public class InitOnly : MonoBehaviour {
	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		gameObject.SetActive(false);
	}
}