//using UnityEngine;
//
///// <summary>
///// A script that parallaxes a background.
///// 
///// This requires that the camera have a component with a BoundaryHolder attached to it.
///// </summary>
//public class RelativeParallaxOld : AbstractParallax {
//	// -----------------------------------------------------------------------------------------------------------------
//	// Configurable:
//
//	/// <summary>
//	/// The parallax style.
//	/// </summary>
//	public ParallaxStyle Style = ParallaxStyle.BOTH;
//
//	/// <summary>
//	/// The parallax stretch.
//	/// </summary>
//	public ParallaxRepeat Repeat = ParallaxRepeat.BOTH;
//
//	/// <summary>
//	/// The camera to attach to.
//	/// This will attach to the main camera if null.
//	/// </summary>
//	public Camera Camera = null;
//
//
//	// -----------------------------------------------------------------------------------------------------------------
//	// Variables:
//
//	protected SpriteRenderer sr;
//
//	protected Vector2 size;
//
//	protected Vector2 min;
//	protected Vector2 max;
//
//	protected Vector2 viewport;
//	protected BoundaryHolder holder;
//
//	// -----------------------------------------------------------------------------------------------------------------
//	// API:
//
//	/// <summary>
//	/// Change the camera that the parallax is adjusting itself to.
//	/// </summary>
//	/// <param name="cam"></param>
//	public void ChangeCamera(Camera cam) {
//		Camera = cam;
//		holder = cam.gameObject.GetInterface<BoundaryHolder>();
//	}
//
//	/// <summary>
//	/// Resize the background to fit within bounds.
//	/// </summary>
//	public void Resize() {
//		if (Repeat == ParallaxRepeat.NONE) {
//			return;
//		}
//
//		Vector2 wanted = viewport * 2f * Mathf.Max(Amount, 1f);
//		Vector2 wsize = sr.WorldSize();
//
//		Vector2 ssize = sr.sprite.WorldSize(); // <-- sprite size
//		Vector2 scale = transform.lossyScale;
//
//		float width = sr.size.x;
//		float height = sr.size.y;
//
//		// Stretch horizontally.
//		if ((Repeat & ParallaxRepeat.HORIZONTAL) > 0 && wsize.x < wanted.x) {
//			width = ssize.x * wanted.x / (ssize.x * scale.x);
//		}
//
//		// Stretch vertically.
//		if ((Repeat & ParallaxRepeat.VERTICAL) > 0 && wsize.y < wanted.y) {
//			height = ssize.y * wanted.y / (ssize.y * scale.y);
//		}
//
//		// Apply
//		sr.size = new Vector2(width, height);
//	}
//
//	/// <summary>
//	/// Recalculate the camera viewport.
//	/// </summary>
//	public void RecalculateViewport() {
//		viewport = Camera.Coverage();
//	}
//
//	/// <summary>
//	/// Recalculate the size of the background.
//	/// 
//	/// This is dangerous.
//	/// It will cause an obvious position shift.
//	/// </summary>
//	public void RecalculateSize() {
//		size = sr.WorldSize();
//	}
//
//	/// <summary>
//	/// Recalculate the bounds of the background.
//	/// 
//	/// This is dangerous.
//	/// It will cause an obvious position shift.
//	/// </summary>
//	public void RecalculateBounds() {
//		BoundaryArea loose = holder.Loose;
//		min = loose.min;
//		max = loose.max;
//	}
//
//	/// <summary>
//	/// Recalculate the bounds of the background.
//	/// 
//	/// This is dangerous.
//	/// It will cause an obvious position shift.
//	/// </summary>
//	/// <param name="changed">The variable set to whether or not the bounds have changed.</param>
//	public void RecalculateBounds(out bool changed) {
//		BoundaryArea loose = holder.Loose;
//		Vector2 nmin = loose.min;
//		Vector2 nmax = loose.max;
//
//		changed = min != nmin || max != nmax;
//		if (changed) {
//			min = nmin;
//			max = nmax;
//		}
//	}
//
//	// -----------------------------------------------------------------------------------------------------------------
//	// Methods:
//
//	/// <summary>
//	/// Reposition the parallax.
//	/// </summary>
//	public void Reposition() {
//		Vector2 campos = Camera.transform.position;
//		Vector2 rel_min = min + (viewport / 2f);
//		Vector2 rel_max = max - (viewport / 2f);
//		Debug.DrawLine(rel_min, rel_max, Color.cyan);
//
//		float x = transform.position.x;
//		float y = transform.position.y;
//
//		// Horizontal parallax.
//		if ((Style & ParallaxStyle.HORIZONTAL) > 0) {
//			float percent = (campos.x - rel_min.x) / (rel_max.x - rel_min.x);
//			float bg_extent = size.x / 2f;
//			float vp_extent = viewport.x / 2f;
//
//			if (Reverse) {
//				percent = 1 - percent;
//			}
//
//			if (Amount < 1f) {
//				percent = percent * Amount + 0.5f; // Slower parallax.
//			}
//
//			x = campos.x + (bg_extent - vp_extent); // Left of sprite -> left of viewport.
//			x -= (size.x - vp_extent - vp_extent) * percent; // Move according to parallax.
//		}
//		
//		// Vertical parallax.
//		if ((Style & ParallaxStyle.VERTICAL) > 0) {
//			float percent = (campos.y - rel_min.y) / (rel_max.y - rel_min.y);
//			float bg_extent = size.y / 2f;
//			float vp_extent = viewport.y / 2f;
//
//			if (Reverse) {
//				percent = 1 - percent;
//			}
//
//			if (Amount < 1f) {
//				percent = percent * Amount + 0.5f; // Slower parallax.
//			}
//
//			y = campos.y + (bg_extent - vp_extent); // Top of sprite -> top of viewport.
//			y -= (size.y - vp_extent - vp_extent) * percent; // Move according to parallax.
//		}
//
//		// Apply
//		transform.position = new Vector3(x, y, transform.position.z);
//	}
//
//	/// <summary>
//	/// [UNITY] Called every frame.
//	/// </summary>
//	protected void Update() {
//		Reposition();
//	}
//
//	/// <summary>
//	/// [UNITY] Called when the object is instantiated.
//	/// </summary>
//	protected void Start() {
//		sr = GetComponent<SpriteRenderer>();
//
//		ChangeCamera(Camera == null ? UnityEngine.Camera.main : Camera);
//		RecalculateViewport();
//		RecalculateBounds();
//		Resize();
//		RecalculateSize();
//	}
//}