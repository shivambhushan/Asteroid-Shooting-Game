using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using MA;

[CustomPropertyDrawer(typeof(DisplayablePaletteColor))]
public class DisplayablePaletteColorDrawer : PropertyDrawer {
	private int indexChosen = 0;
	private Color chosenColor;
	private List<string> DisplayNames = new List<string>();
	private List<Color32> Values = new List<Color32>();

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

		EditorGUI.BeginProperty(position, label, property);
		
		// Don't make child fields be indented
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		
		Values.Clear();
		DisplayNames.Clear();
		
		string[] guids = AssetDatabase.FindAssets ("t:ColorScheme", null);
		foreach(string guid in guids)
		{
			ColorScheme scheme = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(ColorScheme)) as ColorScheme;
			string schemeName = scheme.name;
			foreach(ColorWrapper color in scheme.colors)
			{
				DisplayNames.Add(schemeName + " - " + color.m_ColorName);
				Values.Add(color.m_color);
			}
		}
		DisplayNames.Add("Supply Own Color or modify Chosen One");
		if(Values.Count < 1)
		{
			EditorGUI.LabelField (position, label.text, "No Color Schemes in your Assets Folder");
			return;
		}
		indexChosen = resolveIndexFromName(property.FindPropertyRelative("colorname").stringValue);
		Rect enumPosition = position;
		enumPosition.height = 17;
		Rect ColorFieldPosition = position;
		ColorFieldPosition.y += enumPosition.height + 3;
		ColorFieldPosition.height = 20;
		indexChosen = EditorGUI.Popup(enumPosition, label.text,  indexChosen, DisplayNames.ToArray());
		if(indexChosen != Values.Count)
		{
			chosenColor = Values[indexChosen];
		}else
		{
			chosenColor = property.FindPropertyRelative("color").colorValue;
		}
		chosenColor = EditorGUI.ColorField(ColorFieldPosition, chosenColor);
		property.FindPropertyRelative("colorname").stringValue = DisplayNames[indexChosen];
		property.FindPropertyRelative("color").colorValue = chosenColor;

		
		// Set indent back to what it was
		EditorGUI.indentLevel = indent;
		
		EditorGUI.EndProperty();
	}
	private int resolveIndexFromName(string name)
	{
		int lastindex = DisplayNames.IndexOf(name);
		if(lastindex == -1) return 0;
		else return lastindex;
	}
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 40;
	}
}
