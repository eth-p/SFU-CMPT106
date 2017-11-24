using System;
using UnityEngine;

/// <summary>
/// A boundary.
/// </summary>
public class Boundary {
	// -----------------------------------------------------------------------------------------------------------------
	// Variables:

	protected WeakReference reference = null;
	protected bool dead = false;


	// -----------------------------------------------------------------------------------------------------------------
	// Public:

	/// <summary>
	/// The boundary type.
	/// </summary>
	public BoundaryType Type = BoundaryType.NONE;

	/// <summary>
	/// The boundary location.
	/// </summary>
	public Vector2 Location = new Vector2();

	/// <summary>
	/// Whether or not the boundary is enabled.
	/// </summary>
	public bool Enabled = true;

	/// <summary>
	/// Whether or not the boundary is dynamic.
	/// </summary>
	public bool Dynamic {
		get { return reference != null; }
	}

	/// <summary>
	/// Whether or not the dynamic boundary reference is dead. 
	/// </summary>
	public bool Dead {
		get { return dead; }
	}


	// -----------------------------------------------------------------------------------------------------------------
	// API:

	/// <summary>
	/// Create a new static boundary.
	/// </summary>
	/// <param name="type">The boundary type.</param>
	/// <param name="location">The boundary location.</param>
	public Boundary(BoundaryType type, Vector2 location) {
		Type = type;
		Location = location;
	}

	/// <summary>
	/// Create a new dynamic boundary.
	/// </summary>
	/// <param name="manipulator">The boundary manipulator.</param>
	public Boundary(BoundaryManipulator manipulator) {
		reference = new WeakReference(manipulator);
	}

	/// <summary>
	/// Update the boundary (if dynamic).
	/// </summary>
	public void Update() {
		if (reference != null) {
			object manipulator = reference.Target;
			if (manipulator == null) {
				dead = true;
				return;
			}
			
			(manipulator as BoundaryManipulator).ManipulateBoundary(this);
		}
	}
}