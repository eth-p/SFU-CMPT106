﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A helper class for working with Unity's Sprites.
/// </summary>
static public class SpriteHelper {
	// -----------------------------------------------------------------------------------------------------------------
	// Extension: Sprite

	/// <summary>
	/// Calculate the size of a sprite in world space units.
	/// </summary>
	/// <param name="sprite">The sprite.</param>
	/// <returns>The size of the sprite.</returns>
	static public Vector2 WorldSize(this Sprite sprite) {
		return new Vector2(
			sprite.bounds.extents.x * 2f,
			sprite.bounds.extents.y * 2f
		);
	}

	/// <summary>
	/// Calculate the size of a sprite in world space units with a transform applied.
	/// </summary>
	/// <param name="sprite">The sprite.</param>
	/// <param name="transform">The transform.</param>
	/// <returns>The size of the transformed sprite.</returns>
	static public Vector2 WorldSize(this Sprite sprite, Transform transform) {
		Vector2 scale = transform.lossyScale;
		return new Vector2(
			sprite.bounds.extents.x * 2f * scale.x,
			sprite.bounds.extents.y * 2f * scale.y
		);
	}


	// -----------------------------------------------------------------------------------------------------------------
	// Extension: SpriteRenderer

	/// <summary>
	/// Calculate the size of a sprite in world space units with transform and tiling applied.
	/// </summary>
	/// <param name="renderer">The sprite renderer.</param>
	/// <returns>The size of the transformed and tiled sprite.</returns>
	static public Vector2 WorldSize(this SpriteRenderer renderer) {
		Vector2 scale = renderer.transform.lossyScale;
		float swidth = renderer.sprite.bounds.extents.x * 2f;
		float sheight = renderer.sprite.bounds.extents.y * 2f;
		return new Vector2(
			swidth * scale.x * (renderer.size.x / swidth),
			sheight * scale.y * (renderer.size.y / sheight)
		);
	}

	static public void Expand(this SpriteRenderer renderer, Vector2 target) {
		
	}

	static public void ExpandEvenly(this SpriteRenderer renderer, Vector2 target) {
		Vector2 size = renderer.WorldSize();
		Vector2 sprite = renderer.sprite.WorldSize();
		Vector2 scale = renderer.gameObject.transform.lossyScale;

		float width = renderer.size.x;
		float height = renderer.size.y;
		
		// Horizontal.
		if (size.x < target.x) {
			width = Mathf.Ceil(target.x / (size.x * scale.x)) * sprite.x;
		}
		
		// Vertical.
		if (size.y < target.y) {
			height = Mathf.Ceil(target.y / (size.y * scale.y)) * sprite.y;
		}
		
		// Apply.
		renderer.size = new Vector2(width, height);
	}
}