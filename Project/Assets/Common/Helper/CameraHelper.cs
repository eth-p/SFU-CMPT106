using UnityEngine;

/// <summary>
/// A helper class for working with Unity's cameras.
/// </summary>
public static class CameraHelper {
	// -----------------------------------------------------------------------------------------------------------------
	// Extension: Camera

	/// <summary>
	/// Get the size of the area that that the camera sees in world space units.
	/// </summary>
	/// <param name="camera">The camera.</param>
	/// <returns>The camera coverage.</returns>
	public static Vector2 Coverage(this Camera camera) {
		return new Vector2(
			camera.aspect * camera.orthographicSize * 2,
			camera.orthographicSize * 2
		);
	}
}