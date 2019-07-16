using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Language and font drawer
    /// </summary>
    [CustomPropertyDrawer(typeof(LanguageAndFont))]
    public class LanguageAndFontDrawer : LeftAndRightPropertyDrawer
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
            this.OnGUILeftAndRight(0.4f, "systemLanguage", "font", position, property, label);
        }

    }

}
