using UnityEngine;

/// <summary>
/// Standard death behaviour.
/// </summary>
public class Death : MonoBehaviour, DeathBehaviour {
	
	// -----------------------------------------------------------------------------------------------------------------
	// Implement: DeathBehaviour

	/// <inheritdoc cref="DeathBehaviour.OnDeath"/>
	public void OnDeath() {
		gameObject.SetActive(false);
	}
	
}