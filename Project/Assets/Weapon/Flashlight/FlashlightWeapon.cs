using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// A weapon that uses light.
/// </summary>
public class FlashlightWeapon : AbstractWeapon {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public float FlashIntensity = 12f;
	public float FlashAngle = 40f;
	public uint FlashDuration = 20;


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected Light[] lights;
	protected float[] lightscache_intensity;
	protected float[] lightscache_angle;
	protected uint duration;
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Implement: AbstractWeapon

	/// <summary>
	/// Spawn the projectile.
	/// The projectile will inherit this object's transformation.
	/// </summary>
	protected override void OnUse() {
		Flash();
		
		// Attack.
		Debug.Log("TODO: Flashlight attack.");
	}
	
	
	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Super-flash the flashlight.
	/// </summary>
	void Flash() {
		animator.SetBool("Weapon:Flash", true);
		for (uint i = 0; i < lights.Length; i++) {
			Light light = lights[i];

			if (duration < 1) {
				lightscache_angle[i] = light.spotAngle;
				lightscache_intensity[i] = light.intensity;
			}

			light.spotAngle = FlashAngle;
			light.intensity = FlashIntensity;
		}
		
		duration = FlashDuration;
	}

	/// <summary>
	/// Revert the flash.
	/// </summary>
	void Revert() {
		animator.SetBool("Weapon:Flash", false);
		duration = 0;
		for (uint i = 0; i < lights.Length; i++) {
			Light light = lights[i];

			light.spotAngle = lightscache_angle[i];
			light.intensity = lightscache_intensity[i];
		}
	}

	/// <summary>
	/// [UNITY] Called when the object is instantiated.
	/// </summary>
	void Start() {
		animator = GetComponent<Animator>();
		lights = GetComponentsInChildren<Light>();
		lightscache_intensity = new float[lights.Length];
		lightscache_angle = new float[lights.Length];
	}

	/// <summary>
	/// [UNITY] Called every tick.
	/// </summary>
	void FixedUpdate() {
		base.FixedUpdate();

		if (duration > 0) {
			if (--duration < 1) {
				Revert();
			}
		}
		
	}
	
}
