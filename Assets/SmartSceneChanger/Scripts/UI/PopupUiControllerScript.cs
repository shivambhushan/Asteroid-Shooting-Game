using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSC
{

    /// <summary>
    /// Popup UI controller
    /// </summary>
    public class PopupUiControllerScript : SimpleUiControllerScript
    {

        /// <summary>
        /// Reference to Text
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Text")]
        protected Text m_refText = null;

        /// <summary>
        /// Auto hide seconds
        /// </summary>
        [SerializeField]
        [Tooltip("Auto hide seconds")]
        protected float m_autoHideSeconds = 2.0f;

        /// <summary>
        /// Auto hide seconds
        /// </summary>
        public float autoHideSeconds { get { return this.m_autoHideSeconds; } set { this.m_autoHideSeconds = value; } }

        /// <summary>
        /// Set text
        /// </summary>
        /// <param name="text">text</param>
        // ----------------------------------------------------------------------------------
        public void setText(string text)
        {

            if(this.m_refText)
            {
                this.m_refText.text = text;
            }

        }

    }

}
