using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Language and sprite drawer
    /// </summary>
    [CustomPropertyDrawer(typeof(LanguageAndSprite))]
    public class LanguageAndSpriteDrawer : LeftAndRightPropertyDrawer
    {

        /// <summary>
        /// OnGUI
        /// </summary>
        /// <param name="position">Rect</param>
        /// <param name="property">SerializedProperty</param>
        /// <param name="label">GUIContent</param>
        // ------------------------------------------------------------------------------------------------------
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.OnGUILeftAndRight(0.4f, "systemLanguage", "sprite", position, property, label);
        }

    }

}
