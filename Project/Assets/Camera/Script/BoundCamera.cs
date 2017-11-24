using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A camera restricted to a specific area in the world.
/// </summary>
public class BoundCamera : MonoBehaviour, BoundaryHolder {
	// -----------------------------------------------------------------------------------------------------------------
	// Public:

	/// <summary>
	/// The viewport size.
	/// </summary>
	public Vector2 Viewport {
		get { return viewport; }
	}
	

	// -----------------------------------------------------------------------------------------------------------------
	// Variables:
	
	protected BoundaryList boundaries = new BoundaryList();

	protected bool cache_loose_stale;
	protected BoundaryArea cache_loose;
	protected BoundaryArea cache_tight;

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
	/// Recalculate the tight bounds.
	/// </summary>
	public void RecalculateBounds() {
		// Update bounds.
		boundaries.Update();
		if (boundaries.Stale) {
			boundaries.Prune();
		}

		// Calculate tight bounds and invalidate loose bounds.
		cache_tight = boundaries.CalculateTight();
		cache_loose_stale = true;
	}

	/// <summary>
	/// Reposition the camera to within the boundaries.
	/// </summary>
	public void Reposition() {
		Vector2 rel_min = cache_tight.min + (viewport / 2f);
		Vector2 rel_max = cache_tight.max - (viewport / 2f);
		Vector2 pos = VectorHelper.Clamp(transform.position, rel_min, rel_max);

		transform.position = new Vector3(pos.x, pos.y, transform.position.z);
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Implement: BoundaryHolder

	/// <inheritdoc cref="BoundaryHolder.Boundaries"/>
	public BoundaryList Boundaries {
		get { return boundaries; }
	}

	/// <inheritdoc cref="BoundaryHolder.Tight"/>
	public BoundaryArea Tight {
		get { return cache_tight; }
	}

	/// <inheritdoc cref="BoundaryHolder.Loose"/>
	public BoundaryArea Loose {
		get {
			if (cache_loose_stale) {
				cache_loose_stale = false;
				cache_loose = Boundaries.CalculateLoose();
			}

			return cache_loose;
		}
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// [DEBUG] Draw the camera boundaries.
	/// </summary>
	void DebugDrawBoundaries() {
		Vector2 min = cache_tight.min;
		Vector2 max = cache_tight.max;
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