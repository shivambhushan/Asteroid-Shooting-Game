using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using MA;

[CustomPropertyDrawer(typeof(StringOptions))]
public class StringOptionsDrawer : PropertyDrawer {
	private int indexChosen = 0;
	private string choice;
	private bool ChoiceAllowed = true;
	private List<string> DisplayNames = new List<string>();
	private List<string> Values = new List<string>();
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		
		EditorGUI.BeginProperty(position, label, property);
		
		// Don't make child fields be indented
		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		DisplayNames.Clear();
		Values.Clear();
		int optionnamessize = property.FindPropertyRelative("optionnames").arraySize;
		int valuessize = property.FindPropertyRelative("options").arraySize;
		if(optionnamessize != valuessize || valuessize <1)
		{
			EditorGUI.LabelField (position, label.text, "Supply at least one Option & Same amount Options and Values");
			return;
		}
		ChoiceAllowed = property.FindPropertyRelative("OwnChoiceAllowed").boolValue;

		for(int i = 0; i < optionnamessize; i++)
		{
			DisplayNames.Add(property.FindPropertyRelative("optionnames").GetArrayElementAtIndex(i).stringValue);
			Values.Add(property.FindPropertyRelative("options").GetArrayElementAtIndex(i).stringValue);
		}
		if(ChoiceAllowed)DisplayNames.Add("Supply your Own Choice");

		indexChosen = resolveIndexFromName(property.FindPropertyRelative("chosenoption").stringValue);

		Rect enumPosition = position;
		enumPosition.height = 17;
		Rect textFieldPosition = position;
		textFieldPosition.y += enumPosition.height + 3;
		textFieldPosition.height = 17;
		indexChosen = EditorGUI.Popup(enumPosition, label.text,  indexChosen, DisplayNames.ToArray());
		if(indexChosen != Values.Count)
		{
			choice = Values[indexChosen];
		}else
		{
			choice = property.FindPropertyRelative("choice").stringValue;
		}
		choice = EditorGUI.TextField(textFieldPosition, choice);
		property.FindPropertyRelative("choice").stringValue = choice;
		property.FindPropertyRelative("chosenoption").stringValue = DisplayNames[indexChosen];
		
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
