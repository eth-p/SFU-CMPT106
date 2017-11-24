using UnityEngine;

/// <summary>
/// A script that parallaxes a background.
/// 
/// This requires that the camera have a BoundCamera component attached to it.
/// 
/// TODO: Change it to work with anything which implements an interface that specifies its bounds.
/// NOTE: This won't work properly with a camera that doesn't have full bounds.
/// </summary>
public class Parallax : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The parallax style.
	/// </summary>
	public ParallaxStyle Style = ParallaxStyle.BOTH;

	/// <summary>
	/// The parallax stretch.
	/// </summary>
	public ParallaxStretch Stretch = ParallaxStretch.BOTH;

	/// <summary>
	/// The parallax amount.
	/// </summary>
	public float Amount = 1f;

	/// <summary>
	/// Reverse the parallax direction.
	/// </summary>
	public bool Reverse = false;

	/// <summary>
	/// The camera to attach to.
	/// This will attach to the main camera if null.
	/// </summary>
	public Camera Camera = null;


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected SpriteRenderer sr;

	protected Vector2 size;

	protected Vector2 min;
	protected Vector2 max;

	protected Vector2 viewport;
	protected BoundCamera bound;

	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Change the camera that the parallax is adjusting itself to.
	/// </summary>
	/// <param name="cam"></param>
	public void ChangeCamera(Camera cam) {
		Camera = cam;
		bound = cam.GetComponent<BoundCamera>();
	}

	/// <summary>
	/// Resize the background to fit within bounds.
	/// </summary>
	public void Resize() {
		if (Stretch == ParallaxStretch.NONE) {
			return;
		}

		Vector2 wanted = viewport * 2f * Mathf.Max(Amount, 1f);
		Vector2 wsize = sr.WorldSize();
		Debug.Log(wsize);

		Vector2 ssize = sr.sprite.WorldSize(); // <-- sprite size
		Vector2 scale = transform.lossyScale;

		float width = sr.size.x;
		float height = sr.size.y;

		// Stretch horizontally.
		if ((Stretch & ParallaxStretch.HORIZONTAL) > 0 && wsize.x < wanted.x) {
			width = ssize.x * wanted.x / (ssize.x * scale.x);
		}

		// Stretch vertically.
		if ((Stretch & ParallaxStretch.HORIZONTAL) > 0 && wsize.y < wanted.y) {
			height = ssize.y * wanted.y / (ssize.y * scale.y);
		}

		// Apply
		sr.size = new Vector2(width, height);
	}

	/// <summary>
	/// Recalculate the camera viewport.
	/// </summary>
	public void RecalculateViewport() {
		if (bound != null) {
			viewport = bound.Viewport;
		} else {
			viewport = Camera.Coverage();
		}
	}

	/// <summary>
	/// Recalculate the size of the background.
	/// 
	/// This is dangerous.
	/// It will cause an obvious position shift.
	/// </summary>
	public void RecalculateSize() {
		size = sr.WorldSize();
	}

	/// <summary>
	/// Recalculate the bounds of the background.
	/// 
	/// This is dangerous.
	/// It will cause an obvious position shift.
	/// </summary>
	public void RecalculateBounds() {
		min = bound.Loose.min;
		max = bound.Loose.max;
	}

	/// <summary>
	/// Recalculate the bounds of the background.
	/// 
	/// This is dangerous.
	/// It will cause an obvious position shift.
	/// </summary>
	/// <param name="changed">The variable set to whether or not the bounds have changed.</param>
	public void RecalculateBounds(out bool changed) {
		Vector2 nmin = bound.Loose.min;
		Vector2 nmax = bound.Loose.max;

		changed = min != nmin || max != nmax;
		if (changed) {
			min = nmin;
			max = nmax;
		}
	}

	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Reposition the parallax.
	/// </summary>
	public void Reposition() {
		Vector2 campos = Camera.transform.position;
		Vector2 rel_min = min + (viewport / 2f);
		Vector2 rel_max = max - (viewport / 2f);
		Debug.DrawLine(rel_min, rel_max, Color.cyan);

		float x = campos.x;
		float y = campos.y;

		// Horizontal parallax.
		if ((Style & ParallaxStyle.HORIZONTAL) > 0) {
			float percent = (campos.x - rel_min.x) / (rel_max.x - rel_min.x);
			float bg_extent = size.x / 2f;
			float vp_extent = viewport.x / 2f;

			if (Reverse) {
				percent = 1 - percent;
			}

			if (Amount < 1f) {
				percent = percent * Amount + 0.5f; // Slower parallax.
			}

			x = campos.x + (bg_extent - vp_extent);				// Left of sprite -> left of viewport.
			x -= (size.x - vp_extent - vp_extent) * percent;	// Move according to parallax.
		}

		// Apply
		transform.position = new Vector3(
			x,
			y,
			transform.position.z
		);
	}

	/// <summary>
	/// [UNITY] Called every frame.
	/// </summary>
	protected void Update() {
		Reposition();
	}

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	protected void Start() {
		sr = GetComponent<SpriteRenderer>();

		ChangeCamera(Camera == null ? UnityEngine.Camera.main : Camera);
		RecalculateViewport();
		RecalculateBounds();
		Resize();
		RecalculateSize();
	}
}