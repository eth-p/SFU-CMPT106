using UnityEngine;

/// <summary>
/// An abstract class for implementing parallax.
/// </summary>
public abstract class AbstractParallax : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The camera to attach to.
	/// This will attach to the main camera if null.
	/// </summary>
	public Camera Camera = null;

	/// <summary>
	/// The parallax depth.
	/// </summary>
	public float Depth = 10f;

	/// <summary>
	/// Reverse the parallax direction.
	/// </summary>
	public bool Reverse = false;

	/// <summary>
	/// The parallax style.
	/// </summary>
	public ParallaxStyle Style = ParallaxStyle.HORIZONTAL;


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected SpriteRenderer sr;

	protected Vector2 size;

	protected Vector2 viewport;


	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Change the camera that the parallax is adjusting itself to.
	/// </summary>
	/// <param name="cam">The target camera.</param>
	public virtual void ChangeCamera(Camera cam) {
		Camera = cam;
	}

	/// <summary>
	/// Recalculate the camera viewport.
	/// 
	/// This may cause a position shift.
	/// </summary>
	public virtual void RecalculateViewport() {
		viewport = Camera.Coverage();
	}

	/// <summary>
	/// Recalculate the size of the sprite.
	/// 
	/// This may cause a position shift.
	/// </summary>
	public virtual void RecalculateSize() {
		size = sr.sprite.bounds.size;
	}
	
	/// <summary>
	/// Recalculate the bounds of the parallax.
	/// 
	/// This may cause a position shift.
	/// </summary>
	public virtual void RecalculateBounds() {
		// This is only really needed for relative parallax.
	}



	// -----------------------------------------------------------------------------------------------------------------
	// API / Abstract:


	/// <summary>
	/// Resize the sprite to fit the viewport.
	/// </summary>
	public abstract void Resize();
	
	/// <summary>
	/// Reposition the parallax.
	/// </summary>
	public abstract void Reposition();


	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// [UNITY] Called every frame.
	/// </summary>
	protected virtual void Update() {
		Reposition();
	}

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected virtual void Start() {
		sr = GetComponent<SpriteRenderer>();

		ChangeCamera(Camera == null ? UnityEngine.Camera.main : Camera);
		RecalculateViewport();
		RecalculateBounds();
		Resize();
		RecalculateSize();
	}
}