using UnityEngine;

/// <summary>
/// A component that can be added to an object to set a dynamic boundary.
/// </summary>
public abstract class AbstractBoundary : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The cameras that this boundary applies to.
	/// </summary>
	public BoundCamera[] Cameras;


	// -----------------------------------------------------------------------------------------------------------------
	// Abstract:

	/// <summary>
	/// Apply this boundary to a camera.
	/// </summary>
	/// <param name="camera">The bound camera.</param>
	protected abstract void ApplyBoundary(BoundCamera camera);


	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		// Attempt to apply the boundary to the provided cameras.
		// If no cameras were provided, attempt to apply it to the main camera.
		if (Cameras.Length > 0) {
			foreach (BoundCamera camera in Cameras) {
				ApplyBoundary(camera);
			}
		} else {
			BoundCamera camera = Camera.main.GetComponent<BoundCamera>();
			if (camera != null) {
				ApplyBoundary(camera);
			}
		}
	}
}