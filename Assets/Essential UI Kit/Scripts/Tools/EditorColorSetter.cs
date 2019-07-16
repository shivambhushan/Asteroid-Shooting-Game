#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA{
	/// <summary>
	/// Helper Script for picking the Color from a Color Palette in the Editor (for Image, Camera, Sprite etc.)
	/// </summary>
	[ExecuteInEditMode]
	public class EditorColorSetter : MonoBehaviour {
		[SerializeField]
		private DisplayablePaletteColor paletteColor = new DisplayablePaletteColor();
		[Header("This must be the name of the Property in the Shader")]
		[SerializeField]
		private string MaterialColorProperty = "";

		void Update () {
			Image image = GetComponent<Image>();
			if(image != null) image.color = paletteColor;
			Camera camera = GetComponent<Camera>();
			if(camera != null) camera.backgroundColor = paletteColor;
			SpriteRenderer sprite = GetComponent<SpriteRenderer>();
			if(sprite != null) sprite.color = paletteColor;
			MeshRenderer renderer = GetComponent<MeshRenderer>();
			if(renderer != null)
			{
				if(string.IsNullOrEmpty(MaterialColorProperty)) renderer.sharedMaterial.color = paletteColor;
				else renderer.sharedMaterial.SetColor(MaterialColorProperty, paletteColor);
			}
		}
	}
}
#endif