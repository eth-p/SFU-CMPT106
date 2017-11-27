using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.WSA;

/// <summary>
/// A camera that follows a target.
/// </summary>
public class TrackingCamera : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The tracking target. If this is null, it will be the player.
	/// </summary>
	public GameObject Target;

	/// <summary>
	/// The distance before tracking will start.
	/// </summary>
	public float Distance = 0f;

	/// <summary>
	/// The dampening speed.
	/// </summary>
	public float Speed = 1f;


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected Vector2 wanted;
	protected Vector2 dampened;


	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Track a new target.
	/// </summary>
	/// <param name="target">The target to track.</param>
	void Track(GameObject target) {
		Target = target;
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Reposition the camera.
	/// </summary>
	void Reposition() {
		transform.position = new Vector3(dampened.x, dampened.y, transform.position.z);
	}

	/// <summary>
	/// Dampen the camera.
	/// </summary>
	/// <param name="deltaTime">The time delta.</param>
	public void Dampen(float deltaTime) {
		if (Speed > 0f) {
			dampened = Vector2.Lerp(dampened, wanted, Speed * deltaTime);
		} else {
			dampened = wanted;
		}
	}

	/// <summary>
	/// Calculate the ideal position for tracking.
	/// </summary>
	void Recalculate() {
		if (Target == null) {
			return;
		}
		
		Vector2 target = Target.transform.position;

		// Calculate if the distance is great enough.
		if (Mathf.Abs(dampened.x - target.x) >= Distance || Mathf.Abs(dampened.y - target.y) >= Distance) {
			wanted = target;
		}
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	void Start() {
		dampened = transform.position;
		Track(Target == null ? GameObject.Find("Player") : Target);
	}

	/// <summary>
	/// [UNITY] Called every frame.
	/// </summary>
	void Update() {
		if (Target == null) {
			return;
		}
		
		Recalculate();
		Dampen(Time.deltaTime);
		Reposition();
	}
}