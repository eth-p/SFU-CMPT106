using UnityEngine;

/// <summary>
/// A component that can be added to a GameObject to set a static camera boundary to its location.
/// </summary>
public class StaticBoundary : AbstractBoundary {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The boundary type.
	/// </summary>
	public BoundaryType Type = BoundaryType.NONE;


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected Vector2 extents;


	// -----------------------------------------------------------------------------------------------------------------
	// Implement: AbstractBoundary

	/// <inheritdoc cref="AbstractBoundary.ApplyBoundary"/>
	protected override void ApplyBoundary(BoundCamera camera) {
		Boundary b = new Boundary(Type, new Vector2());
		UpdateBoundary(b);
		camera.AddBoundary(b);
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Using the SpriteRenderer, calculate the extents of the GameObject.
	/// </summary>
	protected void CalculateExtents() {
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr != null) {
			extents = sr.sprite.WorldSize() * 0.5f;
		}
	}

	/// <summary>
	/// Using this GameObject's position and extents, update a boundary location.
	/// </summary>
	/// <param name="boundary">The boundary object.</param>
	protected void UpdateBoundary(Boundary boundary) {
		BoundaryType type = boundary.Type;

		if ((type & BoundaryType.TOP) > 0) {
			boundary.Location.y = transform.position.y - extents.y; // Bottom of sprite.
		} else if ((type & BoundaryType.BOTTOM) > 0) {
			boundary.Location.y = transform.position.y + extents.y; // Top of sprite.
		}

		if ((type & BoundaryType.LEFT) > 0) {
			boundary.Location.x = transform.position.x + extents.x; // Right of sprite.
		} else if ((type & BoundaryType.RIGHT) > 0) {
			boundary.Location.x = transform.position.x - extents.x; // Left of sprite.
		}
	}

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		CalculateExtents();
		base.Start();
	}
}