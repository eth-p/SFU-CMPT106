using UnityEngine;

/// <summary>
/// A script that parallaxes a sprite at a constant rate.
/// 
/// This will automatically repeat the sprite to fit the viewport.
/// </summary>
public class FixedParallax : AbstractParallax {
	
	// -----------------------------------------------------------------------------------------------------------------
	// Implement: AbstractParallax

	/// <inheritdoc cref="AbstractParallax.Resize"/>
	public override void Resize() {
		bool vertical = (Style & ParallaxStyle.VERTICAL) > 0;
		bool horizontal = (Style & ParallaxStyle.HORIZONTAL) > 0;

		if (!vertical && !horizontal) {
			return;
		}

		sr.ExpandEvenly(new Vector2(
			horizontal ? viewport.x * 3f : 0f,
			vertical ? viewport.y * 3f : 0f
		));
	}

	/// <inheritdoc cref="AbstractParallax.Reposition"/>
	public override void Reposition() {
		Vector2 campos = Camera.transform.position;

		float x = transform.position.x;
		float y = transform.position.y;

		// Horizontal parallax.
		if ((Style & ParallaxStyle.HORIZONTAL) > 0) {
			float percent = campos.x % Depth / Depth;
			
			// Fix sign.
			if (percent < 0f) {
				percent = 1f + percent;
			}
			
			// Calculate translation.
			float translate = size.x * (percent - 0.5f);
			
			// Reverse.
			if (Reverse) {
				translate *= -1;
			}

			// Move.
			x = campos.x; // <-- Center.
			x -= translate; // <-- Translate.
		}
		
		// Vertical parallax.
		if ((Style & ParallaxStyle.VERTICAL) > 0) {
			float percent = campos.y % Depth / Depth;

			// Fix sign.
			if (percent < 0f) {
				percent = 1f + percent;
			}
			
			// Calculate translation.
			float translate = size.y * (percent - 0.5f);
			
			// Reverse.
			if (Reverse) {
				translate *= -1;
			}
			
			// Move.
			y = campos.y; // <-- Center.
			y -= translate; // <-- Translate.
		}
		
		// Apply
		transform.position = new Vector3(x, y, transform.position.z);
	}

}