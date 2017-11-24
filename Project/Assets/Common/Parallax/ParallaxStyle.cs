using System;

/// <summary>
/// The parallax style options.
/// </summary>
[Flags]
public enum ParallaxStyle {
		
	/// <summary>
	/// Horizontal parallax.
	/// </summary>
	HORIZONTAL = 0x01,
		
	/// <summary>
	/// Vertical parallax.
	/// </summary>
	VERTICAL = 0x02,
		
	/// <summary>
	/// Both horizontal and vertical parallax.
	/// </summary>
	BOTH = 0x03
}