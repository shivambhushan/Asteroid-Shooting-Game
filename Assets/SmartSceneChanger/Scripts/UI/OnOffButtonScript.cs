using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SSC
{

    /// <summary>
    /// On Off switch button
    /// </summary>
    public class OnOffButtonScript : MonoBehaviour
    {

        /// <summary>
        /// Info
        /// </summary>
        [Serializable]
        public class OnOffInfo
        {

            /// <summary>
            /// Sprite
            /// </summary>
            public Sprite sprite = null;

            /// <summary>
            /// Text
            /// </summary>
            public string text = "";

            /// <summary>
            /// Sprite color
            /// </summary>
            public Color spriteColor = Color.white;

            /// <summary>
            /// Text color
            /// </summary>
            public Color textColor = Color.white;

            /// <summary>
            /// UnityEvent
            /// </summary>
            public UnityEvent anyEvent = null;

        }

        /// <summary>
        /// On true, off false
        /// </summary>
        [SerializeField]
        [Tooltip("On true, off false")]
        bool m_onTrueOffFalse = true;

        /// <summary>
        /// Reference to Image
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Image")]
        Image m_refImage = null;

        /// <summary>
        /// Reference to Text
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Text")]
        Text m_refText = null;

        /// <summary>
        /// Info for On
        /// </summary>
        [SerializeField]
        [Tooltip("Info for On")]
        OnOffInfo m_on = null;

        /// <summary>
        /// Info for Off
        /// </summary>
        [SerializeField]
        [Tooltip("Info for Off")]
        OnOffInfo m_off = null;

        // -----------------------------------------------------------------------------------

        /// <summary>
        /// On true, off false
        /// </summary>
        public bool isOnTrueOffFalse { get { return this.m_onTrueOffFalse; } }

        // -----------------------------------------------------------------------------------

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------------------
        void Start()
        {
            this.setOnOff(this.m_onTrueOffFalse, false);
        }

        /// <summary>
        /// Toggle
        /// </summary>
        /// <param name="invokeEvent">invoke event</param>
        // -----------------------------------------------------------------------------------
        public void toggleOnOff(bool invokeEvent)
        {
            this.setOnOff(!this.m_onTrueOffFalse, invokeEvent);
        }

        /// <summary>
        /// Set on off
        /// </summary>
        /// <param name="on">on</param>
        /// <param name="invokeEvent">invoke event</param>
        // -----------------------------------------------------------------------------------
        public void setOnOff(bool on, bool invokeEvent)
        {

            if(on)
            {
                this.setOn(invokeEvent);
            }

            else
            {
                this.setOff(invokeEvent);
            }

        }

        /// <summary>
        /// Set on
        /// </summary>
        /// <param name="invokeEvent">invoke event</param>
        // -----------------------------------------------------------------------------------
        public void setOn(bool invokeEvent)
        {
            this.m_onTrueOffFalse = true;
            this.setValues(this.m_on, invokeEvent);
        }

        /// <summary>
        /// Set off
        /// </summary>
        /// <param name="invokeEvent">invoke event</param>
        // -----------------------------------------------------------------------------------
        public void setOff(bool invokeEvent)
        {
            this.m_onTrueOffFalse = false;
            this.setValues(this.m_off, invokeEvent);
        }

        /// <summary>
        /// Set values
        /// </summary>
        /// <param name="info">OnOffInfo</param>
        /// <param name="invokeEvent">invoke event</param>
        // -----------------------------------------------------------------------------------
        void setValues(OnOffInfo info, bool invokeEvent)
        {

            if(info == null)
            {
                return;
            }

            // -----------------------

            if(this.m_refImage)
            {
                this.m_refImage.sprite = info.sprite;
                this.m_refImage.color = info.spriteColor;
            }

            if (this.m_refText)
            {

                this.m_refText.text = info.text;
                this.m_refText.color = info.textColor;

            }

            if (invokeEvent)
            {
                info.anyEvent.Invoke();
            }

        }

    }

}
