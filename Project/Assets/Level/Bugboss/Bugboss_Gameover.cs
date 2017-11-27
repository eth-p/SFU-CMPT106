using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bugboss_Trigger : MonoBehaviour, DeathBehaviour {
	public GameObject[] lights_off;
	public GameObject[] lights_on;
	public GameObject boss;
	public StaticBoundary boundary;
	public BoundCamera camera;

	protected bool start = false;
	protected bool triggered = false;
	protected Vector3 cpos;

	public void OnDeath() {
		// Disable other components.
		foreach (Behaviour comp in gameObject.GetComponents<Behaviour>()) {
			comp.enabled = false;
		}

		enabled = true;
		triggered = true;
		gameObject.SetActive(true);

		// Disable camera scripts.
		foreach (MonoBehaviour script in camera.gameObject.GetComponents<MonoBehaviour>()) {
			script.enabled = false;
		}
		
		cpos = camera.transform.position;
		boundary.gameObject.SetActive(true);
		camera.RecalculateBounds();
		

		// Lights
		foreach (GameObject obj in lights_off) {
			foreach (Light light in obj.GetComponents<Light>()) {
				light.enabled = false;
			}
		}

		foreach (GameObject obj in lights_on) {
			foreach (Light light in obj.GetComponents<Light>()) {
				light.enabled = true;
			}
		}
	}

	public void Update() {
		if (triggered && !start) {
			// Move camera.
			Vector2 ideal = camera.Ideal;
			Vector2 pos = Vector2.Lerp(cpos, ideal, Time.deltaTime * 2f);
			camera.transform.position = cpos = new Vector3(pos.x, pos.y, cpos.z);
			camera.DebugDrawBoundaries();

			// Check if moved.
			Vector2 diff = (Vector2) cpos - ideal;
			if (Mathf.Abs(diff.x) < 0.1f && Mathf.Abs(diff.y) < 0.1f) {
				start = true;
				camera.gameObject.GetComponent<TrackingCamera>().enabled = true;
				camera.gameObject.GetComponent<MouseCamera>().enabled = true;
				camera.enabled = true;
				boss.SetActive(true);
			}
		}
	}
}