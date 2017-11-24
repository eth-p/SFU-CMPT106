using System;

/// <summary>
/// 	The type of boundary.
/// </summary>
/// <remarks>
/// 	This is also used as a bitfield.<br/>
/// 	<b>Do not modify the enum values or things will break.</b>
/// </remarks>
[Flags]
public enum BoundaryType {
	NONE = 0x00,
	TOP = 0x01,
	BOTTOM = 0x02,
	LEFT = 0x04,
	RIGHT = 0x08,
	TOP_LEFT = 0x05,
	TOP_RIGHT = 0x09,
	BOTTOM_LEFT = 0x06,
	BOTTOM_RIGHT = 0x0A
}