using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSC
{

    /// <summary>
    /// Send text by clicking button
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class TextAndSendButtonScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Text
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to Text")]
        Text m_refText = null;

        /// <summary>
        /// Send Text value
        /// </summary>
        [SerializeField]
        [Tooltip("Send Text value")]
        StringUnityEvent m_sendEvent = null;

        /// <summary>
        /// Start
        /// </summary>
        // -----------------------------------------------------------------------------
        void Start()
        {

#if UNITY_EDITOR

            if(!this.m_refText)
            {
                Debug.LogWarning("(#if UNITY_EDITOR) : m_refText == null : " + Funcs.CreateHierarchyPath(this.transform));
            }

            if (this.m_sendEvent.GetPersistentEventCount() <= 0)
            {
                Debug.LogWarning("(#if UNITY_EDITOR) : m_sendEvent.GetPersistentEventCount() <= 0 : " + Funcs.CreateHierarchyPath(this.transform));
            }
#endif

        }

        /// <summary>
        /// Send Text value
        /// </summary>
        // -----------------------------------------------------------------------------
        public void sendTextValue()
        {

            if(this.m_refText)
            {
                this.m_sendEvent.Invoke(this.m_refText.text);
            }
            
        }

    }

}
