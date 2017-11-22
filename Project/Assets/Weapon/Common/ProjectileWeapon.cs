using System;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A weapon that shoots projectiles.
/// </summary>
public class ProjectileWeapon : AbstractWeapon {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The prefab bullet.
	/// </summary>
	public GameObject Bullet;


	// -----------------------------------------------------------------------------------------------------------------
	// Implement: AbstractWeapon

	/// <summary>
	/// Spawn the projectile.
	/// The projectile will inherit this object's transformation.
	/// </summary>
	protected override void OnUse() {
		Assert.IsNotNull(Bullet);
		
		// Calculate scale to world.
		Vector2 scale = transform.localScale;
		Transform trans = transform;
		while ((trans = trans.parent) != null) {
			scale.x *= trans.localScale.x;
			scale.y *= trans.localScale.y;
		}

		// Calculate angle (ignoring flips).
		float z = transform.rotation.eulerAngles.z + 360;
		
		if (scale.x < 0) {
			z += -180;
		}

		if (scale.y < 0) {
			z += -180;
		}
		
		// Spawn bullet.
		Instantiate(Bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, z)));
	}
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	void Start() {
		animator = GetComponent<Animator>();
	}
}
