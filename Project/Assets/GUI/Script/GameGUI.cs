using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class GameGUI : MonoBehaviour {
	// -----------------------------------------------------------------------------------------------------------------
	// Constants:

	protected const float COORD_X = 0;
	protected const float COORD_Y = 0;

	protected Rect TEX_BAR_FULL = new Rect(0f, 0.5f, 0.5f, 0.5f);
	protected Rect TEX_BAR_EMPTY = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
	protected Rect TEX_BAR_LEFT_CAP = new Rect(0f, 0f, 0.5f, 0.5f);
	protected Rect TEX_BAR_RIGHT_CAP = new Rect(0.5f, 0f, 0.5f, 0.5f);


	// -----------------------------------------------------------------------------------------------------------------
	// Configurable:

	public GameObject Player;


	// -----------------------------------------------------------------------------------------------------------------
	// Textures:

	protected Texture2D tex_health;


	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected RectTransform rtransform;
	protected Health player_health;


	// -----------------------------------------------------------------------------------------------------------------
	// Unity:

	void Start() {
		// Get textures.
		tex_health = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/GUI/Texture/Health.png");
		Assert.IsNotNull(tex_health);

		// Get components.
		rtransform = GetComponent<RectTransform>();
		player_health = Player.GetComponent<Health>();
	}

	void OnGUI() {
		Rect screen = rtransform.rect;
		
		RenderBar(player_health.Value, player_health.Maximum, tex_health, 4, 22, 2f);
	}


	// -----------------------------------------------------------------------------------------------------------------
	// GUI:

	void RenderBar(float cur, float max, Texture2D tex, float x, float y, float scale) {
		float sw = 8f * scale;
		float xi = 0;
		
		// The bar itself.
		for (int i = 0; i < max; i++) {
			float parts = cur - i;
			
			if (parts >= 1) {
				GUI.DrawTextureWithTexCoords(new Rect(x + xi, y, sw, sw), tex, TEX_BAR_FULL);
			} else if (parts <= 0) {
				GUI.DrawTextureWithTexCoords(new Rect(x + xi, y, sw, sw), tex, TEX_BAR_EMPTY);
			} else {
				Debug.Log(parts);
				GUI.DrawTextureWithTexCoords(new Rect(x + xi, y, sw * parts, sw), tex, TEX_BAR_FULL);
				GUI.DrawTextureWithTexCoords(new Rect(x + xi + (sw * parts), y, sw * (1f - parts), sw), tex, TEX_BAR_EMPTY);
			}

			xi += sw;
		}
		
		// The beginning and end of the bar.
		GUI.DrawTextureWithTexCoords(new Rect(x, y, sw, sw), tex, TEX_BAR_LEFT_CAP);
		GUI.DrawTextureWithTexCoords(new Rect(x + xi - sw, y, sw, sw), tex, TEX_BAR_RIGHT_CAP);
	}
	
}