/// <summary>
/// An interface for when an entity is damaged.
/// </summary>
public interface HurtBehaviour {

	/// <summary>
	/// Function called when the entity gets damaged.
	/// </summary>
	/// <param name="amount">The damage amount.</param>
	void OnHurt(float amount);

	/// <summary>
	/// Function called when the entity becomes vulnerable again.
	/// </summary>
	void OnVulnerable();

}
