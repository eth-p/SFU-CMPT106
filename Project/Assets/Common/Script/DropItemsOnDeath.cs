using UnityEngine;

/// <summary>
/// Drop items on death.
/// </summary>
public class DropItemsOnDeath : MonoBehaviour, DeathBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public int Quantity = 1;
	public int Rarity = 6; // Higher is worse.

	public GameObject[] ItemsGuaranteed = { };

	public GameObject[] ItemsOptional = { };

	// -----------------------------------------------------------------------------------------------------------------
	// Internal:

	protected Collider2D col;

	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	public void Start() {
		col = GetComponent<Collider2D>();
	}

	// -----------------------------------------------------------------------------------------------------------------
	// DeathBehaviour:

	public void OnDeath() {
		if (Random.Range(0, Rarity) != 0) {
			return; // Tough luck.
		}

		// Drop items.
		for (int i = 0; i < Quantity; i++) {
			Vector2 pos = gameObject.transform.position;

			// If the object has a collider, we can pick a random position inside it.
			if (col != null) {
				pos = new Vector2(
					Random.Range(col.bounds.min.x, col.bounds.max.x),
					Random.Range(col.bounds.min.y, col.bounds.max.y)
				);
			}

			// Spawn the item.
			if (i < ItemsGuaranteed.Length) {
				Instantiate(ItemsGuaranteed[i], pos, Quaternion.identity);
			} else {
				Instantiate(ItemsOptional[Random.Range(0, ItemsOptional.Length)], pos, Quaternion.identity);
			}
		}
	}
}