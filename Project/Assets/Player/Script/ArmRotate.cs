using System;
using UnityEngine;

public class ArmRotate : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Constants:

	protected const float ROTATION_SPEED = 8f;

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	private PlayerController player;

	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	void Start() {
		player = GetComponentInParent<PlayerController>();
	}

	void Update() {
		ApplyRotation();
		ApplyFlip();
	}

	// -----------------------------------------------------------------------------------------------------------------
	// ArmRotate:

	/// <summary>
	/// Rotate the arm based on the angle between the cursor and the 
	/// </summary>
	void ApplyRotation() {
		// Calculate angle.
		float angle = AngleHelper.RadiansBetween(Camera.main.ScreenToWorldPoint(new Vector3(
			Input.mousePosition.x,
			Input.mousePosition.y,
			10
		)), player.transform.position);
		

		// Apply rotation.
		Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, angle * Mathf.Rad2Deg + 90f));
		if (ROTATION_SPEED > 0) {
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, ROTATION_SPEED * Time.deltaTime);
		} else {
			transform.rotation = rotation;
		}
	}

	/// <summary>
	/// Flip the player sprite depending on the angle of the arm.
	/// </summary>
	void ApplyFlip() {
		float angle = transform.rotation.eulerAngles.z - 180f;
		float abs = Mathf.Abs(angle);
		if (abs < 10 || abs > 170) {
			return;
		}

		if (angle < 0f) {
			transform.localPosition = new Vector2(-0.1f, 0.166f);
			transform.localScale = new Vector3(-1f, 1f, 1f);
			player.FaceLeft();
		} else {
			transform.localPosition = new Vector2(0.118f, 0.166f);
			transform.localScale = new Vector3(1f, 1f, 1f);
			player.FaceRight();
		}
	}
}