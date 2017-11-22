using UnityEngine;

public interface CameraManipulator {

	/// <summary>
	/// Manipulate the camera position.
	/// </summary>
	/// <param name="vec">The camera's desired position.</param>
	void ManipulateCamera(ref Vector2 pos);

}
