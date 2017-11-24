using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class containing a list of boundaries.
/// 
/// This class can calculate tight or loose boundary areas from its contained boundaries.
/// </summary>
public class BoundaryList {
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected LinkedList<Boundary> list = new LinkedList<Boundary>();
	protected bool stale = false;

	
	// -----------------------------------------------------------------------------------------------------------------
	// Public:

	/// <summary>
	/// Whether or not the list contains a Boundary with a dead (GC'd) manipulator.
	/// </summary>
	public bool Stale {
		get { return stale; }
	}

	/// <summary>
	/// The number of items in the list.
	/// </summary>
	public int Count {
		get { return list.Count; }
	}

	
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	/// <summary>
	/// Add a boundary to the list.
	/// </summary>
	/// <param name="boundary"></param>
	public void Add(Boundary boundary) {
		if (!list.Contains(boundary)) {
			list.AddLast(boundary);
		}
	}

	/// <summary>
	/// Remove a boundary from the list.
	/// </summary>
	/// <param name="boundary">The item to remove.</param>
	public void Remove(Boundary boundary) {
		list.Remove(boundary);
	}

	/// <summary>
	/// Get the boundary list as an array.
	/// </summary>
	/// <returns></returns>
	public Boundary[] ToArray() {
		Boundary[] arr = new Boundary[list.Count];
		
		int i = 0;
		foreach (Boundary b in list) {
			arr[i++] = b;
		}
		
		return arr;
	}

	/// <summary>
	/// Update all the dynamic boundaries.
	/// </summary>
	public void Update() {
		LinkedListNode<Boundary> node = list.First;
		while (node != null) {
			Boundary boundary = node.Value;
			node = node.Next;

			// Ignore dead boundaries.
			if (boundary.Dead) {
				stale = true;
				continue;
			}

			// Ignore disabled boundaries.
			if (!boundary.Enabled) {
				continue;
			}

			// Update dynamic boundaries.
			if (boundary.Dynamic) {
				boundary.Update();
			}
		}
	}

	/// <summary>
	/// Calculate the loose (largest possible) boundary area.
	/// </summary>
	/// <returns>The largest boundary area.</returns>
	public BoundaryArea CalculateLoose() {
		// Initialize min/max vectors
		Vector2 l_max = VectorHelper.minValue;
		Vector2 l_min = VectorHelper.maxValue;

		// Iterate through boundaries.
		LinkedListNode<Boundary> node = list.First;
		while (node != null) {
			Boundary boundary = node.Value;
			node = node.Next;


			// Ignore dead boundaries.
			if (boundary.Dead) {
				stale = true;
				continue;
			}

			// Ignore disabled boundaries.
			if (!boundary.Enabled) {
				continue;
			}

			// Apply boundary.
			BoundaryType type = boundary.Type;

			if ((type & BoundaryType.TOP) > 0) {
				l_max.y = Mathf.Max(l_max.y, boundary.Location.y);
			} else if ((type & BoundaryType.BOTTOM) > 0) {
				l_min.y = Mathf.Min(l_min.y, boundary.Location.y);
			}

			if ((type & BoundaryType.LEFT) > 0) {
				l_min.x = Mathf.Min(l_min.x, boundary.Location.x);
			} else if ((type & BoundaryType.RIGHT) > 0) {
				l_max.x = Mathf.Max(l_max.x, boundary.Location.x);
			}
		}

		// Return result.
		return new BoundaryArea(l_min, l_max);
	}

	/// <summary>
	/// Calculate the tight (smallest possible) boundary area.
	/// </summary>
	/// <returns>The tight boundary area.</returns>
	public BoundaryArea CalculateTight() {
		// Initialize min/max vectors.
		Vector2 t_min = VectorHelper.minValue;
		Vector2 t_max = VectorHelper.maxValue;

		// Iterate through boundaries.
		LinkedListNode<Boundary> node = list.First;
		while (node != null) {
			Boundary boundary = node.Value;
			node = node.Next;

			// Ignore dead boundaries.
			if (boundary.Dead) {
				stale = true;
				continue;
			}

			// Ignore disabled boundaries.
			if (!boundary.Enabled) {
				continue;
			}

			// Apply boundary.
			BoundaryType type = boundary.Type;

			if ((type & BoundaryType.TOP) > 0) {
				t_max.y = Mathf.Min(t_max.y, boundary.Location.y);
			} else if ((type & BoundaryType.BOTTOM) > 0) {
				t_min.y = Mathf.Max(t_min.y, boundary.Location.y);
			}

			if ((type & BoundaryType.LEFT) > 0) {
				t_min.x = Mathf.Max(t_min.x, boundary.Location.x);
			} else if ((type & BoundaryType.RIGHT) > 0) {
				t_max.x = Mathf.Min(t_max.x, boundary.Location.x);
			}
		}

		// Return result.
		return new BoundaryArea(t_min, t_max);
	}

	/// <summary>
	/// Remove all boundaries with a dead (GC'd) manipulator.
	/// </summary>
	public void Prune() {
		LinkedListNode<Boundary> node = list.First;
		LinkedListNode<Boundary> last;
		while (node != null) {
			last = node;
			node = node.Next;
			
			if (last.Value.Dead) {
				list.Remove(last);
			}
		}
	}
}