using UnityEngine;

/// <summary>
/// A component that can be added to an object to set a dynamic boundary.
/// </summary>
public abstract class AbstractBoundary : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The boundary holders that this applies to.
	/// This is an array of GameObjects (for the Unity editor's purposes...)
	/// </summary>
	public GameObject[] Targets;


	// -----------------------------------------------------------------------------------------------------------------
	// Abstract:

	/// <summary>
	/// Apply this boundary to a boundary holder.
	/// </summary>
	/// <param name="holder">The boundary holder.</param>
	protected abstract void ApplyBoundary(BoundaryHolder holder);


	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// [UNITY] Called when the object is enabled.
	/// </summary>
	protected void OnEnable() {
		// Attempt to apply the boundary to the provided holders.
		bool applied = false;
		foreach (GameObject obj in Targets) {
			if (obj == null) {
				continue;
			}
			
			foreach (BoundaryHolder holder in obj.GetInterfaces<BoundaryHolder>()) {
				applied = true;
				ApplyBoundary(holder);
			}
		}

		// If no holders were provided, attempt to apply it to the main camera.
		if (!applied) {
			foreach (BoundaryHolder holder in Camera.main.gameObject.GetInterfaces<BoundaryHolder>()) {
				ApplyBoundary(holder);
			}
		}
	}
}