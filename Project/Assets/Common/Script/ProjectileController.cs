using UnityEngine;

/// <summary>
/// Controller for a projectile.
/// 
/// This will need to be extended into something useful.
/// </summary>
public abstract class ProjectileController : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float Speed = 2f;
	public int Despawn = 120;

	public LayerMask CollideLayers;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	private float angle;
	private Vector2 angle_vec;
	private int despawnIn;
	
	// -----------------------------------------------------------------------------------------------------------------
	// ProjectileController:

	/// <summary>
	/// Recalculate the projectile's angle vector.
	/// </summary>
	void RecalculateAngle() {
		angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
		angle_vec = new Vector2(
			Mathf.Cos(angle),
			Mathf.Sin(angle)
		);
	}
	
	/// <summary>
	/// Move the projectile forwards.
	/// </summary>
	void Move() {
		transform.position = ((Vector2) transform.position) + (angle_vec * Speed);
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	void Start() {
		despawnIn = Despawn;
		RecalculateAngle();
	}
	
	void FixedUpdate() {
		if (--despawnIn < 1) {
			gameObject.SetActive(false);
			Destroy(gameObject);
			return;
		}
		
		// Raycast to check for collision.
		// If it collides with something, run OnCollide and destroy the projectile.
		// If it doesn't, move the projectile forwards.
		RaycastHit2D ray = Physics2D.Raycast(transform.position, angle_vec, Speed, CollideLayers);
		if (ray.collider == null) {
			Move();
		} else {
			OnCollide(ray.collider);
			gameObject.SetActive(false);
			Destroy(gameObject);
		}
	}
	
	// -----------------------------------------------------------------------------------------------------------------
	// Abstract:
	
	/// <summary>
	/// Called when the projectile collides with something.
	/// </summary>
	/// <param name="obj">The collider of the "something".</param>
	public abstract void OnCollide(Collider2D col);

}