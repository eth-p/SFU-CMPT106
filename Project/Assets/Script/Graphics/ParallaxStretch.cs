using System;

/// <summary>
/// The parallax stretch/resizing options.
/// </summary>
[Flags]
public enum ParallaxStretch {
	/// <summary>
	/// No stretching (or tiling).
	/// </summary>
	NONE = 0x00,

	/// <summary>
	/// Stretch (or tile) horizontally.
	/// </summary>
	HORIZONTAL = 0x01,

	/// <summary>
	/// Stretch (or tile) vertically.
	/// </summary>
	VERTICAL = 0x02,

	/// <summary>
	/// Stretch (or tile) both horizontally and vertically.
	/// </summary>
	BOTH = 0x03
}