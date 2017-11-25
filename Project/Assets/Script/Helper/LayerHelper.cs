using UnityEngine;
using System.Collections.Generic;

public static class LayerHelper {
	/// <summary>
	/// Check if a layer is inside a LayerMask.
	/// 
	/// Credit: http://answers.unity3d.com/answers/1332280/view.html
	/// </summary>
	/// 
	/// <param name="mask">The layer mask.</param>
	/// <param name="layer">The layer.</param>
	/// <returns>True if the layer is inside a LayerMask.</returns>
	public static bool Contains(this LayerMask mask, int layer) {
		return mask == (mask | (1 << layer));
	}

	/// <summary>
	/// Get the layer numbers from a layer mask.
	/// </summary>
	/// <param name="mask">The layer mask.</param>
	/// <returns>The layer numbers.</returns>
	public static int[] ToLayers(this LayerMask mask) {
		List<int> layers = new List<int>();
		int layer = mask.value;
		int num = 0;
		while (layer > 0) {
			if ((layer & 0x01) == 1) {
				layers.Add(num);
			}

			num++;
			layer = layer >> 1;
		}

		return layers.ToArray();
	}

	/// <summary>
	/// Merge an array layer masks into a single layer mask.
	/// </summary>
	/// <param name="masks">The array of layer masks.</param>
	/// <returns>The merged layer mask.</returns>
	public static LayerMask Merge(this LayerMask[] masks) {
		LayerMask result = new LayerMask();
		foreach (LayerMask mask in masks) {
			result.value = result.value | mask.value;
		}
		
		return result;
	}

	/// <summary>
	/// Get the layer names for an array of layers.
	/// </summary>
	/// <param name="layers">The layer numbers.</param>
	/// <returns>The layer names.</returns>
	public static string[] LayersToNames(int[] layers) {
		string[] layernames = new string[layers.Length];
		for (int i = 0; i < layers.Length; i++) {
			layernames[i] = LayerMask.LayerToName(layers[i]);
		}

		return layernames;
	}
}