using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Left and right PropertyDrawer
    /// </summary>
    public class LeftAndRightPropertyDrawer : PropertyDrawer
    {

        /// <summary>
        /// OnGUI for left and right
        /// </summary>
        /// <param name="leftPercentage01">left width percentage</param>
        /// <param name="leftRelativePropertyPath">left property</param>
        /// <param name="rightRelativePropertyPath">right property</param>
        /// <param name="position">OnGUI position</param>
        /// <param name="property">OnGUI property</param>
        /// <param name="label">OnGUI label</param>
        // -----------------------------------------------------------------------------------------------
        protected virtual void OnGUILeftAndRight(
            float leftWidthPercentage01,
            string leftRelativePropertyPath,
            string rightRelativePropertyPath,
            Rect position,
            SerializedProperty property,
            GUIContent label
            )
        {

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("*"));

            position.xMin += 20.0f;

            float leftX = position.xMin;
            float leftWidth = position.width * leftWidthPercentage01;
            float rightX = leftX + leftWidth;
            float rightWidth = position.width - leftWidth;

            var leftRect = new Rect(leftX, position.y, leftWidth, position.height);
            var rightRect = new Rect(rightX, position.y, rightWidth, position.height);

            EditorGUI.PropertyField(leftRect, property.FindPropertyRelative(leftRelativePropertyPath), GUIContent.none);
            EditorGUI.PropertyField(rightRect, property.FindPropertyRelative(rightRelativePropertyPath), GUIContent.none);

            EditorGUI.EndProperty();

        }

    }

}
