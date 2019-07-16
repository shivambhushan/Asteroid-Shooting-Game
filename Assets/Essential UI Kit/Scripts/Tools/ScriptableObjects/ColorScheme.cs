using UnityEngine;
using System.Collections;

public class ColorScheme : ScriptableObject {
	public MA.ColorWrapper[] colors;
}
namespace MA{
	[System.Serializable]
	public class ColorWrapper
	{
		public string m_ColorName;
		public Color32 m_color = Color.white;
	}
}
