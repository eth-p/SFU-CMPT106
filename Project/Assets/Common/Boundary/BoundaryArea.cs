using UnityEngine;

/// <summary>
/// 	A struct that contains vectors representing the minimum and maximum coordinates of a calculated boundary area.
/// </summary>
/// <remarks>
/// 	For a <b>very</b> good reason, we're not using UnityEngine.Bounds:<br/>
/// 	<br/>
/// 	We need to be able to have floating-point numbers with vastly different exponents.<br/>
/// 	UnityEngine.Bounds needs to perform math operations to determine its min and max values.<br/>
/// 	When you have (-15, -5) and (float.MaxValue, float.MaxValue), shit will break.<br/>
/// </remarks>
public struct BoundaryArea {
	// -----------------------------------------------------------------------------------------------------------------
	// Public:

	public Vector2 min;
	public Vector2 max;


	// -----------------------------------------------------------------------------------------------------------------
	// Constructors:
	
	/// <summary>
	/// Create a new BoundaryArea.
	/// </summary>
	/// <param name="min">The area minimum.</param>
	/// <param name="max">The area maximum.</param>
	public BoundaryArea(Vector2 min, Vector2 max) {
		this.min = min;
		this.max = max;
	}
}