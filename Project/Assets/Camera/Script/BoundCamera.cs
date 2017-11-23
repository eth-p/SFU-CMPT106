using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A camera restricted to a specific area in the world.
/// </summary>
public class BoundCamera : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Constant:

	protected Vector2 SIZE = new Vector2(17.8f, 10f);

	
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The camera boundaries.
	/// </summary>
	public LinkedList<Boundary> Boundaries = new LinkedList<Boundary>();


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected Vector2 min;
	protected Vector2 max;

	protected Camera cam;


	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Recalculate the minimum and maximum coordinates.
	/// </summary>
	public void Recalculate() {
		min = new Vector2(float.MinValue, float.MinValue);
		max = new Vector2(float.MaxValue, float.MaxValue);

		LinkedListNode<Boundary> node = Boundaries.First;
		LinkedListNode<Boundary> last;
		while (node != null) {
			Boundary boundary = node.Value;
			last = node;
			node = node.Next;

			// Remove dead boundaries.
			if (boundary.Dead) {
				Boundaries.Remove(last);
				continue;
			}

			// Ignore disabled boundaries.
			if (!boundary.Enabled) {
				continue;
			}

			// Update dynamic boundaries.
			if (boundary.Dynamic) {
				boundary.Update();
			}

			// Apply boundary.
			BoundaryType type = boundary.Type;

			if (type == BoundaryType.TOP || type == BoundaryType.TOP_LEFT || type == BoundaryType.TOP_RIGHT) {
				max.y = Mathf.Min(max.y, boundary.Location.y);
			} else if (type == BoundaryType.BOTTOM || type == BoundaryType.BOTTOM_LEFT || type == BoundaryType.BOTTOM_RIGHT) {
				min.y = Mathf.Max(min.y, boundary.Location.y);
			}

			if (type == BoundaryType.LEFT || type == BoundaryType.TOP_LEFT || type == BoundaryType.BOTTOM_LEFT) {
				min.x = Mathf.Max(min.x, boundary.Location.x);
			} else if (type == BoundaryType.RIGHT || type == BoundaryType.TOP_RIGHT || type == BoundaryType.BOTTOM_RIGHT) {
				max.x = Mathf.Min(max.x, boundary.Location.x);
			}
		}
	}

	/// <summary>
	/// Remove a camera boundary.
	/// </summary>
	/// <param name="boundary">The boundary object.</param>
	public void RemoveBoundary(Boundary boundary) {
		Boundaries.Remove(boundary);
	}

	/// <summary>
	/// Add a camera boundary.
	/// </summary>
	/// <param name="boundary">The boundary object.</param>
	public void AddBoundary(Boundary boundary) {
		if (!Boundaries.Contains(boundary)) {
			Boundaries.AddLast(boundary);
		}
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Reposition the camera to within the boundaries.
	/// </summary>
	public void Reposition() {
		Vector2 rel_min = min + (SIZE / 2f);
		Vector2 rel_max = max - (SIZE / 2f);
		Vector2 pos = ((Vector2) transform.position).Clamp(rel_min, rel_max);

		transform.position = new Vector3(pos.x, pos.y, transform.position.z);
	}

	/// <summary>
	/// [DEBUG] Draw the camera boundaries.
	/// </summary>
	void DebugDrawBoundaries() {
		Debug.DrawLine(min, new Vector2(min.x, max.y), Color.magenta);
		Debug.DrawLine(min, new Vector2(max.x, min.y), Color.magenta);
		Debug.DrawLine(max, new Vector2(min.x, max.y), Color.magenta);
		Debug.DrawLine(max, new Vector2(max.x, min.y), Color.magenta);
	}

	/// <summary>
	/// [UNITY] Called every frame.
	/// </summary>
	void Update() {
		// Recalculate and reposition.
		Recalculate();
		Reposition();

		// Debug.
		if (DebugSettings.CAMERA_LIMITS) {
			DebugDrawBoundaries();
		}
	}
}