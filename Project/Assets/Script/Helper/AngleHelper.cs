using UnityEngine;

/// <summary>
/// Extension functions that assist in dealing with angles.  
/// </summary>
static public class AngleHelper {
	// -----------------------------------------------------------------------------------------------------------------
	// EXTENSIONS: Vector2
	
	/// <summary>
	/// Get the angle (in radians) between two points. 
	/// </summary>
	/// <param name="self">The origin point.</param>
	/// <param name="point">The other point</param>
	/// <returns>The angle in radians.</returns>
	static public float RadiansBetween(this Vector2 self, Vector2 point) {
		return Mathf.Atan2(
			point.x - self.x,
			point.y - self.y
		);
	}

	/// <summary>
	/// Get a vector for the angle between two points.
	/// </summary>
	/// <param name="self">The origin point.</param>
	/// <param name="point">The other point.</param>
	/// <returns>A vector.</returns>
	static public Vector2 VectorBetween(this Vector2 self, Vector2 point) {
		float angle = RadiansBetween(self, point);
		return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
	}
}