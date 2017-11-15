using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension functions that assist in retreiving Unity Components which implement a specific interface.  
/// </summary>
static public class InterfaceHelper {
	
	// -----------------------------------------------------------------------------------------------------------------
	// EXTENSIONS: GameObject
	/// <summary>
	/// Get the first Component that implements a specific interface. 
	/// </summary>
	/// <param name="self">The GameObject.</param>
	/// <typeparam name="T">The interface.</typeparam>
	/// <returns>The component, or null.</returns>
	static public T GetInterface<T>(this GameObject self) where T : class {
		foreach (object comp in self.GetComponents<Component>()) {
			if (comp is T) {
				return (T) comp;
			}
		}

		return null;
	}

	/// <summary>
	/// Get an array of Components that implement a specific interface. 
	/// </summary>
	/// <param name="self">The GameObject.</param>
	/// <typeparam name="T">The interface.</typeparam>
	/// <returns>The component, or null.</returns>
	static public T[] GetInterfaces<T>(this GameObject self) where T : class {
		List<T> comps = new List<T>();
		foreach (object comp in self.GetComponents<Component>()) {
			if (comp is T) {
				comps.Add((T) comp);
			}
		}
		
		return comps.ToArray();
	}

	// -----------------------------------------------------------------------------------------------------------------
	// EXTENSIONS: MonoBehaviour
	/// <summary>
	/// Get the first Component that implements a specific interface. 
	/// </summary>
	/// <param name="self">The MonoBehaviour.</param>
	/// <typeparam name="T">The interface.</typeparam>
	/// <returns>The component, or null.</returns>
	static public T GetInterface<T>(this MonoBehaviour self) where T : class {
		return GetInterface<T>(self.gameObject);
	}

	/// <summary>
	/// Get an array of Components that implement a specific interface. 
	/// </summary>
	/// <param name="self">The MonoBehaviour.</param>
	/// <typeparam name="T">The interface.</typeparam>
	/// <returns>The component, or null.</returns>
	static public T[] GetInterfaces<T>(this MonoBehaviour self) where T : class {
		return GetInterfaces<T>(self.gameObject);
	}
}