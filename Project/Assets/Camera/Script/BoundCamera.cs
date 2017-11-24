using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A camera restricted to a specific area in the world.
/// </summary>
public class BoundCamera : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Enum:

	/// <summary>
	/// 	A struct that contains vectors representing the minimum and maximum coordinates of the restricted area.
	/// </summary>
	/// <remarks>
	/// 	For a <b>very</b> good reason, we're not using UnityEngine.Bounds:<br/>
	/// 	<br/>
	/// 	We need to be able to have floating-point numbers with vastly different exponents.<br/>
	/// 	UnityEngine.Bounds needs to perform math operations to determine its min and max values.<br/>
	/// 	When you have (-15, -5) and (float.MaxValue, float.MaxValue), shit will break.<br/>
	/// </remarks>
	public struct Limits {
		public Vector2 min;
		public Vector2 max;
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The camera boundaries.
	/// </summary>
	public LinkedList<Boundary> Boundaries = new LinkedList<Boundary>();

	// -----------------------------------------------------------------------------------------------------------------
	// Public:

	/// <summary>
	/// The viewport size.
	/// </summary>
	public Vector2 Viewport {
		get { return viewport; }
	}

	/// <summary>
	/// The "tight" (smallest area possible) limits.
	/// </summary>
	public Limits Tight {
		get { return tight; }
	}
	
	/// <summary>
	/// The "loose" (largest area possible) limits.
	/// </summary>
	public Limits Loose {
		get { return loose; }
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected Limits loose;
	protected Limits tight;

	protected Camera cam;
	protected Vector2 viewport;


	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Recalculate the camera viewport.
	/// </summary>
	public void RecalculateViewport() {
		viewport = cam.Coverage();
	}

	/// <summary>
	/// Recalculate the tight and loose bounds.
	/// </summary>
	public void RecalculateBounds() {
		// Initialize min/max vectors
		Vector2 t_min, t_max, l_min, l_max;
		t_min = l_max = VectorHelper.minValue;
		t_max = l_min = VectorHelper.maxValue;

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

			if ((type & BoundaryType.TOP) > 0) {
				t_max.y = Mathf.Min(t_max.y, boundary.Location.y);
				l_max.y = Mathf.Max(l_max.y, boundary.Location.y);
			} else if ((type & BoundaryType.BOTTOM) > 0) {
				t_min.y = Mathf.Max(t_min.y, boundary.Location.y);
				l_min.y = Mathf.Min(l_min.y, boundary.Location.y);
			}

			if ((type & BoundaryType.LEFT) > 0) {
				t_min.x = Mathf.Max(t_min.x, boundary.Location.x);
				l_min.x = Mathf.Min(l_min.x, boundary.Location.x);
			} else if ((type & BoundaryType.RIGHT) > 0) {
				t_max.x = Mathf.Min(t_max.x, boundary.Location.x);
				l_max.x = Mathf.Max(l_max.x, boundary.Location.x);
			}
		}
		
		// Set bounds.
		tight.min = t_min;
		tight.max = t_max;
		
		loose.min = l_min;
		loose.max = l_max;
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

	/// <summary>
	/// Reposition the camera to within the boundaries.
	/// </summary>
	public void Reposition() {
		Vector2 rel_min = tight.min + (viewport / 2f);
		Vector2 rel_max = tight.max - (viewport / 2f);
		Vector2 pos = VectorHelper.Clamp(transform.position, rel_min, rel_max);

		transform.position = new Vector3(pos.x, pos.y, transform.position.z);
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// [DEBUG] Draw the camera boundaries.
	/// </summary>
	void DebugDrawBoundaries() {
		Vector2 min = tight.min;
		Vector2 max = tight.max;
		Debug.DrawLine(min, new Vector2(min.x, max.y), Color.magenta);
		Debug.DrawLine(min, new Vector2(max.x, min.y), Color.magenta);
		Debug.DrawLine(max, new Vector2(min.x, max.y), Color.magenta);
		Debug.DrawLine(max, new Vector2(max.x, min.y), Color.magenta);
	}
	
	/// <summary>
	/// [UNITY] Called every frame.
	/// </summary>
	protected void Update() {
		// Recalculate and reposition.
		RecalculateBounds();
		Reposition();

		// Debug.
		if (DebugSettings.CAMERA_LIMITS) {
			DebugDrawBoundaries();
		}
	}


	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		cam = GetComponent<Camera>();
		Assert.IsNotNull(cam);
		
		RecalculateViewport();
		RecalculateBounds();
	}
}