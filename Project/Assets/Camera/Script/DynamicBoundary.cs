/// <summary>
/// A component that can be added to a GameObject to set a camera boundary which follows its location.
/// </summary>
public class DynamicBoundary : StaticBoundary, BoundaryManipulator {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// Automatically disable the boundary when this component is disabled.
	/// </summary>
	public bool AutoDisable = true;

	/// <summary>
	/// Automatically enable the boundary when this component is enabled.
	/// </summary>
	public bool AutoEnabled = true;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Variables

	private Boundary bound = null;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Implement: AbstractBoundary

	/// <inheritdoc cref="AbstractBoundary.ApplyBoundary"/>
	protected override void ApplyBoundary(BoundCamera camera) {
		camera.AddBoundary(bound);
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Implement: BoundaryManipulator

	/// <inheritdoc cref="BoundaryManipulator.ManipulateBoundary"/>
	public void ManipulateBoundary(Boundary boundary) {
		UpdateBoundary(boundary);
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Methods
	
	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void OnStart() {
		bound = new Boundary(this);
		bound.Type = this.Type;
	}

	/// <summary>
	/// [UNITY] Called when the component is disabled.
	/// </summary>
	protected void OnDisable() {
		if (AutoDisable) {
			bound.Enabled = false;
		}
	}

	/// <summary>
	/// [UNITY] Called when the component is enabled.
	/// </summary>
	protected void OnEnable() {
		if (AutoEnabled) {
			bound.Enabled = false;
		}
	}
}