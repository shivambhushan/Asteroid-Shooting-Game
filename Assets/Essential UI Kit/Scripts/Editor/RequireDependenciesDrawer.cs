using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MA.Editor{
	[CustomPropertyDrawer (typeof (RequireDependencies))]
	public class RequireDependenciesDrawer : PropertyDrawer {
		private bool missingDepencies = false;

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			
			//get the types specified in the Attribute
			RequireDependencies depencies = attribute as RequireDependencies;
			

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				Rect objectPosition = position;
				objectPosition.height = 17;
				Rect warningPosition = position;
				warningPosition.y += objectPosition.height + 3;
				warningPosition.height = 30;
				List<string> missingDepenciesNames = new List<string>();
				EditorGUI.ObjectField (objectPosition, property, label);
				GameObject go = property.objectReferenceValue as GameObject;
				if(go == null)
				{
					missingDepencies = false;
					return;
				}
				foreach(Type t in depencies.requiredDepencies)
				{
					if(go.GetComponent(t) != null) continue;
					if(depencies.lookInChildren && go.GetComponentInChildren(t) != null) continue;
					missingDepenciesNames.Add(t.Name);
				}
				if(missingDepenciesNames.Count > 0)
				{
					EditorGUI.HelpBox(warningPosition, "The Gameobject is missing the following Components: \n " + string.Join(", ", missingDepenciesNames.ToArray()), MessageType.Error);
					missingDepencies = true;
				}else missingDepencies = false;

			}
			else 
				EditorGUI.LabelField (position, label.text, "Use RequireDependencies with a GameObject");
		}
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return missingDepencies ? 50 : 18;
		}
	}
}