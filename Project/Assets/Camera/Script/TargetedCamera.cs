using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.WSA;

public class TargetedCamera : MonoBehaviour {
	
	// -----------------------------------------------------------------------------------------------------------------
	// Constant:

	protected const float Z = -10f;
	protected Vector2 SIZE = new Vector2(17.8f, 10f);
	
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public GameObject Target;
	public float Dampening = 1f;
	public float Distance = 1f;
	

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	protected Vector2 last;
	protected Camera cam;
	protected CameraManipulator manip;

	protected Vector2 min;
	protected Vector2 max;
	protected Vector2 min_ortho;
	protected Vector2 max_ortho;
	
	// -----------------------------------------------------------------------------------------------------------------
	// API:

	void Reposition(Vector2 pos) {
		last = pos;
		
		// Apply boundaries.
		pos = pos.Clamp(min_ortho, max_ortho);
		
		// Manipulate position.
		if (manip != null) {
			manip.ManipulateCamera(ref pos);
		}
		
		// Apply boundaries again and update.
		pos = pos.Clamp(min_ortho, max_ortho);
		this.transform.position = new Vector3(pos.x, pos.y, Z);
	}

	void Focus(GameObject target) {
		Target = target;
		manip = target == null ? null : target.GetInterface<CameraManipulator>();

		// Find boundary objects.
		GameObject bound_tl = GameObject.Find("Bound_TL");
		GameObject bound_tr = GameObject.Find("Bound_TR");
		GameObject bound_bl = GameObject.Find("Bound_BL");
		GameObject bound_br = GameObject.Find("Bound_BR");
		
		// Deactivate boundary objects.
		bound_tl.SetActive(false);
		bound_tr.SetActive(false);
		bound_bl.SetActive(false);
		bound_br.SetActive(false);

		// Calculate min/max world coords.
		min = new Vector2(
			Math.Max(bound_tl.transform.position.x, bound_bl.transform.position.x) + 0.5f,
			Math.Max(bound_bl.transform.position.y, bound_br.transform.position.y) + 0.5f
		);
		
		max = new Vector2(
			Math.Min(bound_tr.transform.position.x, bound_br.transform.position.x) - 0.5f,
			Math.Min(bound_tl.transform.position.y, bound_tr.transform.position.y) - 0.5f
		);

		// Calculate min/max camera coords.
		min_ortho = min + (SIZE / 2);
		max_ortho = max - (SIZE / 2);
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	// Camera:

	void DebugDraw() {
		if (DebugSettings.CAMERA_LIMITS) {
			Debug.DrawLine(min, new Vector2(min.x, max.y), Color.magenta);
			Debug.DrawLine(min, new Vector2(max.x, min.y), Color.magenta);
			Debug.DrawLine(max, new Vector2(min.x, max.y), Color.magenta);
			Debug.DrawLine(max, new Vector2(max.x, min.y), Color.magenta);
		}
	}

	void Follow() {
		Vector2 camPos = last;
		Vector2 tarPos = Target.transform.position;
		Vector2 next = camPos;

		// Set position to target.
		if (Mathf.Abs(camPos.x - tarPos.x) > Distance || Mathf.Abs(camPos.y - tarPos.y) > Distance) {
			next = tarPos;
		}

		// Dampen.
		if (Dampening > 0f) {
			next = Vector2.Lerp(camPos, next, Time.deltaTime * Dampening);
		}

		// Update position.
		Reposition(next);
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	void Start() {
		// Get components.
		cam = GetComponent<Camera>();
		
		// Set up.
		Focus(Target == null ? GameObject.Find("Player") : Target);
		Reposition(Target.transform.position);
	}

	void Update() {
		DebugDraw();
		if (Target != null) {
			Follow();
		}
	}
}