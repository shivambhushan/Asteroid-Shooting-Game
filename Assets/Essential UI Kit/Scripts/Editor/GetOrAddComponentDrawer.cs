using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

namespace MA.Editor{
[CustomPropertyDrawer (typeof (GetOrAddComponent))]
	public class GetOrAddComponentDrawer : PropertyDrawer {
		
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
			
			//get the attribute
			GetOrAddComponent propertyAttribute = attribute as GetOrAddComponent;
			
			
			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				EditorGUI.ObjectField (position, property, label);
				//if we already have this assigned then there is no need to bother; 
				if(property.objectReferenceValue != null) return;
				Type scriptType = property.serializedObject.targetObject.GetType();
				BindingFlags flag = BindingFlags.FlattenHierarchy| BindingFlags.NonPublic 
					| BindingFlags.Public | BindingFlags.Instance;
				//get the type of the field the attribute has been placed on
				FieldInfo info = scriptType.GetField(property.name, flag);
				//if finding the field didn't work go recursively up the inheritance chain (for private inherited fields with SerializeField Attribute)
				while(info == null)
				{
					Type parentType = scriptType.BaseType;
					scriptType = parentType;
					info = scriptType.GetField(property.name, flag);
					//if we have gone all the way to the top and still no field, then return
					if (parentType == null && info == null) return;
				}
				Type propertyType = info.FieldType;

				//if it's not derived from Component we also don't bother with it;
				if(!propertyType.IsSubclassOf(typeof(Component))) return;


				Component invokingComponent = property.serializedObject.targetObject as Component;

				//look on the GameObject itself and children or parents if specified
				Component desiredComponent = invokingComponent.GetComponent(propertyType);
				if(desiredComponent == null && propertyAttribute.lookInChildren) desiredComponent = invokingComponent.GetComponentInChildren(propertyType);
				else if(desiredComponent == null && propertyAttribute.lookInParent) desiredComponent = invokingComponent.GetComponentInParent(propertyType);

				//if there isn't a component present already add one
				//and assign the component reference
				if(desiredComponent != null) property.objectReferenceValue = desiredComponent;
				else property.objectReferenceValue = invokingComponent.gameObject.AddComponent(propertyType);
				
			}
			else 
				EditorGUI.LabelField (position, label.text, "Use GetOrAddComponent with a Component", EditorStyles.boldLabel);
		}
	}
}