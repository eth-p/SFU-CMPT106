using UnityEngine;
using System;

/// <summary>
/// An abstract base for player weapons.
/// 
/// This abstracts away weapon warmup delays, weapon fire delays, weapon cooldown delays, etc.
/// </summary>
public abstract class AbstractWeapon : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	/// <summary>
	/// The rate of fire (in frames).
	/// </summary>
	public uint Rate = 20;

	/// <summary>
	/// The warmup time (in frames). 
	/// </summary>
	public uint Warmup = 0;

	/// <summary>
	/// The cooldown time (in frames).
	/// </summary>
	public uint Cooldown = 0;

	/// <summary>
	/// The input axis to use when firing the weapon.
	/// </summary>
	public string Button = "Shoot";

	/// <summary>
	/// Manual fire rate. (Require pressing the button again)
	/// </summary>
	public bool Manual = false;

	
	// -----------------------------------------------------------------------------------------------------------------
	// Enum:

	/// <summary>
	/// 	The state of the weapon.
	/// </summary>
	/// <remarks>
	/// 	This is used both internally, and for animators.<br/>
	/// 	<b>Do not modify the enum values or animations will break.</b>
	/// </remarks>
	protected enum WeaponState {
		READY = 1,
		INACTIVE = 0,
		WARMING = -1,
		COOLING = -2,
		DELAY = -3
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected uint delay;
	protected bool button_released;
	protected WeaponState state;
	protected Animator animator;


	// -----------------------------------------------------------------------------------------------------------------
	// Abstract:

	/// <summary>
	/// The method called when the weapon is used.
	/// This should be overridden by non-abstract subclasses.
	/// </summary>
	protected abstract void OnUse();

	/// <summary>
	/// The method called when the weapon is starting its warmup.
	/// </summary>
	protected virtual void OnStartWarmup() {
		if (animator != null) {
			animator.SetInteger("Weapon:State", (int) state);
		}
	}

	/// <summary>
	/// The method called when the weapon is finished its warmup.
	/// </summary>
	protected virtual void OnWarmup() {
		if (animator != null) {
			animator.SetInteger("Weapon:State", (int) state);
		}
	}

	/// <summary>
	/// The method called when the weapon is starting its cooldown.
	/// </summary>
	protected virtual void OnStartCooldown() {
		if (animator != null) {
			animator.SetInteger("Weapon:State", (int) state);
		}
	}

	/// <summary>
	/// The method called when the weapon is finished its cooldown.
	/// </summary>
	protected virtual void OnCooldown() {
		if (animator != null) {
			animator.SetInteger("Weapon:State", (int) state);
		}
	}

	/// <summary>
	/// The method called when the weapon is ready to fire.
	/// </summary>
	protected virtual void OnReady() {
		if (animator != null) {
			animator.SetInteger("Weapon:State", (int) state);
		}
	}

	/// <summary>
	/// The method called when the weapon is delayed due to the rate of fire.
	/// </summary>
	protected virtual void OnDelay() {
		if (animator != null) {
			animator.SetInteger("Weapon:State", (int) state);
		}
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Check if the weapon is ready to be used.
	/// </summary>
	/// <returns>True if the weapon is ready to be used.</returns>
	public bool Ready() {
		return state == WeaponState.READY;
	}

	/// <summary>
	/// Use the weapon.
	/// This will work regardless of the weapon state.
	/// </summary>
	public void Use() {
		OnUse();

		state = WeaponState.DELAY;
		delay = Rate;
		UpdateState();

		if (state == WeaponState.DELAY) {
			OnDelay();
		}
	}

	/// <summary>
	/// Warm up the weapon.
	/// </summary>
	public void Warm() {
		if (state == WeaponState.READY || state == WeaponState.WARMING || state == WeaponState.DELAY) return;

		if (state == WeaponState.COOLING) {
			float pCooled = Cooldown / (float) delay;
			delay = Warmup - ((uint) Math.Floor(pCooled * Warmup));
		} else {
			delay = Warmup;
		}
		
		state = WeaponState.WARMING;

		OnStartWarmup();
		UpdateState();
	}

	/// <summary>
	/// Cool down the weapon.
	/// </summary>
	public void Cool() {
		if (state == WeaponState.INACTIVE || state == WeaponState.COOLING) return;

		state = WeaponState.COOLING;
		delay = Math.Max(delay, Cooldown);

		OnStartCooldown();
		UpdateState();
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Methods:

	/// <summary>
	/// Update the weapon's state.
	/// </summary>
	protected void UpdateState() {
		if (delay < 1) {
			switch (state) {
				case WeaponState.COOLING:
					state = WeaponState.INACTIVE;
					OnCooldown();
					break;

				case WeaponState.WARMING:
					state = WeaponState.READY;
					OnWarmup();
					OnReady();
					break;

				case WeaponState.DELAY:
					state = WeaponState.READY;
					OnReady();
					break;
			}
		}
	}

	/// <summary>
	/// [UNITY] Called every tick.
	/// </summary>
	protected void FixedUpdate() {
		// Decrement delay and update state.
		if (delay > 0) {
			if (--delay < 1) {
				UpdateState();
			}
		}

		// Handle fire button.
		if (!string.IsNullOrEmpty(Button)) {
			bool pressed = Input.GetAxis(Button) > 0f;
			if (!pressed) {
				button_released = true;
			}

			// Start warmup/cooldown.
			if (pressed && state == WeaponState.INACTIVE) {
				Warm();
			} else if (!pressed && state != WeaponState.INACTIVE) {
				Cool();
			}
			
			// If pressed and ready, use.
			if (pressed && Ready() && (!Manual || button_released)) {
				button_released = false;
				Use();
			}
		}
	}
}