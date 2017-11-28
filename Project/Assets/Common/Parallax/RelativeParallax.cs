using UnityEngine;

/// <summary>
/// A script that parallaxes a sprite relative to the camera boundaries.
/// 
/// Due to the nature of this parallax type, it requires that the camera have a BoundaryHolder.
/// </summary>
public class RelativeParallax : AbstractParallax {
	
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:
	
	/// <summary>
	/// The parallax repeat.
	/// </summary>
	public ParallaxRepeat Repeat = ParallaxRepeat.NONE;

	/// <summary>
	/// The parallax offset (in world space units).
	/// </summary>
	public Vector2 Offset;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected BoundaryHolder boundaries;

	protected Vector2 min;
	protected Vector2 max;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Implement: AbstractParallax
	
	/// <inheritdoc cref="AbstractParallax.ChangeCamera"/>
	public override void ChangeCamera(Camera cam) {
		Camera = cam;
		boundaries = cam.gameObject.GetInterface<BoundaryHolder>();
	}
	
	/// <inheritdoc cref="AbstractParallax.RecalculateBounds"/>
	public override void RecalculateBounds() {
		BoundaryArea loose = boundaries.Loose;
		min = loose.min;
		max = loose.max;
	}

	/// <inheritdoc cref="AbstractParallax.Resize"/>
	public override void Resize() {
		bool vertical = (Repeat & ParallaxRepeat.VERTICAL) > 0;
		bool horizontal = (Repeat & ParallaxRepeat.HORIZONTAL) > 0;

		if (!vertical && !horizontal) {
			return;
		}

		sr.ExpandEvenly(new Vector2(
			horizontal ? viewport.x * 2f : 0f,
			vertical ? viewport.y * 2f : 0f
		));
	}

	/// <inheritdoc cref="AbstractParallax.Reposition"/>
	public override void Reposition() {
		Vector2 campos = Camera.transform.position;
		Vector2 rel_min = min + (viewport / 2f);
		Vector2 rel_max = max - (viewport / 2f);

		float x = transform.position.x;
		float y = transform.position.y;

		// Horizontal parallax.
		if ((Style & ParallaxStyle.HORIZONTAL) > 0) {
			float percent = (campos.x - rel_min.x) / (rel_max.x - rel_min.x);
			float vp_extent = viewport.x / 2f;

			// Reverse.
			if (Reverse) {
				percent = 1 - percent;
			}
			
			// Calculate translation.
			percent -= 0.5f;
			percent *= 1f / Depth;

			float translate = (size.x - vp_extent - vp_extent) * percent;
			
			// Move.
			x = campos.x; // Center.
			x += Offset.x; // Offset.
			x -= translate; // Translate.
		}
		
		// Vertical parallax.
		if ((Style & ParallaxStyle.VERTICAL) > 0) {
			float percent = (campos.y - rel_min.y) / (rel_max.y - rel_min.y);
			float vp_extent = viewport.y / 2f;

			// Reverse.
			if (Reverse) {
				percent = 1 - percent;
			}
			
			// Calculate translation.
			percent -= 0.5f;
			percent *= 1f / Depth;

			float translate = (size.y - vp_extent - vp_extent) * percent;
			
			// Move.
			y = campos.y; // Center.
			y += Offset.y; // Offset.
			y -= translate; // Translate.
		}

		// Apply
		transform.position = new Vector3(x, y, transform.position.z);
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Recalculate the bounds of the background.
	/// 
	/// This is dangerous.
	/// It will cause an obvious position shift.
	/// </summary>
	/// <param name="changed">The variable set to whether or not the bounds have changed.</param>
	public void RecalculateBounds(out bool changed) {
		BoundaryArea loose = boundaries.Loose;
		Vector2 nmin = loose.min;
		Vector2 nmax = loose.max;

		changed = min != nmin || max != nmax;
		if (changed) {
			min = nmin;
			max = nmax;
		}
	}
	
}