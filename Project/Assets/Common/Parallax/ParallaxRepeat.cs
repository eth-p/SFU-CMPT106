using System;

/// <summary>
/// The parallax repeating options.
/// </summary>
[Flags]
public enum ParallaxRepeat {
	/// <summary>
	/// No repeating.
	/// </summary>
	NONE = 0x00,

	/// <summary>
	/// Horizontal repeating.
	/// </summary>
	HORIZONTAL = 0x01,

	/// <summary>
	/// Vertical repeating.
	/// </summary>
	VERTICAL = 0x02,

	/// <summary>
	/// Both horizontal and vertical repeating.
	/// </summary>
	BOTH = 0x03
}