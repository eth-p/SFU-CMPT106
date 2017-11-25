using UnityEngine;

/// <summary>
/// A camera that shifts depending on the mouse relative to a GameObject.
/// </summary>
public class MouseCamera : MonoBehaviour {

	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The relative target. If this is null, it will be the center of the screen.
	/// </summary>
	public GameObject Target;

	/// <summary>
	/// The shift amount.
	/// </summary>
	public Vector2 Amount = new Vector2(1.25f, 0.5f);

	/// <summary>
	/// The dampening speed.
	/// </summary>
	public float Speed = 2f;


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected Vector2 shift;
	protected Vector2 dampened;
	protected BoundCamera[] bound;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Recalculate the shift amount.
	/// </summary>d
	public void Recalculate() {
		// Get screen size and cursor screen-position.
		Vector2 screen = new Vector2(Screen.width, Screen.height);
		Vector2 cursor = Input.mousePosition;
		
		// Get target screen-position.
		Vector2 target;
		if (Target == null) {
			target.x = Screen.width / 2f;
			target.y = Screen.height / 2f;
		} else {
			target = Camera.main.WorldToScreenPoint(Target.transform.position);
			target = target.Clamp(new Vector2(0.01f, 0.01f), screen);
		}

		// Calculate relative percentages.
		Vector2 rel = cursor - target;
		Vector2 max = screen - target;
		Vector2 min = Vector2.zero - target;
		
		float mx = rel.x < 0 ? -(rel.x / min.x) : (rel.x / max.x);
		float my = rel.y < 0 ? -(rel.y / min.y) : (rel.y / max.y);
		
		mx = Mathf.Clamp(mx, -1f, 1f);
		my = Mathf.Clamp(my, -1f, 1f);
		
		// Calculate shift.
		shift = new Vector2(mx * Amount.x, my * Amount.y);
	}

	/// <summary>
	/// Dampen the camera.
	/// </summary>
	/// <param name="deltaTime">The time delta.</param>
	public void Dampen(float deltaTime) {
		if (Speed > 0f) {
			dampened = Vector2.Lerp(dampened, shift, Speed * deltaTime);
		} else {
			dampened = shift;
		}
	}

	/// <summary>
	/// Shift the camera.
	/// </summary>
	public void Reposition() {
		// Apply.
		transform.position = new Vector3(
			transform.position.x + dampened.x,
			transform.position.y + dampened.y,
			transform.position.z
		);
		
		// Bound (if applicable).
		foreach (BoundCamera cam in bound) {
			if (cam.enabled) {
				cam.Reposition();
			}
		}
	}
	
	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	void Start() {
		// Get bound camera script.
		bound = GetComponents<BoundCamera>();
	}

	/// <summary>
	/// [UNITY] Called every frame.
	/// </summary>
	void Update() {
		
		// Recalculate and reposition.
		Recalculate();
		Dampen(Time.deltaTime);
		Reposition();
	}
}