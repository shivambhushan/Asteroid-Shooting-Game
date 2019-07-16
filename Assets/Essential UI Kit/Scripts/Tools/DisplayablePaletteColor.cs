using UnityEngine;
using System.Collections;

namespace MA{
	/// <summary>
	/// Displayable palette color. A class with a Custom Drawer to Choose from
	/// a Color from all Color Palettes in the asset folder. Implicitly convertible
	/// to Color
	/// </summary>
	[System.Serializable]
	public class DisplayablePaletteColor{
		public string colorname = "";
		public Color32 color = Color.white;
		
		public static implicit operator Color(DisplayablePaletteColor d)
		{
			return d.color;
		}
	}
}
