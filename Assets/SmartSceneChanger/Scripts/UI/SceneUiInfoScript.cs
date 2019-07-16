using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSC
{

    /// <summary>
    /// Current scene's UI manager
    /// </summary>
    public class SceneUiInfoScript : MonoBehaviour
    {

        /// <summary>
        /// Scene UI info to set the UI's default Selectable and pause signal
        /// </summary>
        [Serializable]
        private class SceneUiInfo
        {

            /// <summary>
            /// UI identifier
            /// </summary>
            [Tooltip("UI identifier")]
            public string identifier = "";

            /// <summary>
            /// Default Selectable when showing
            /// </summary>
            [Tooltip("Default Selectable when showing")]
            public Selectable defaultSelectable;

            /// <summary>
            /// Send pause signal when showing
            /// </summary>
            [Tooltip("Send pause signal when showing")]
            public bool sendPauseSignal = false;

        }

        /// <summary>
        /// Current scene's first UI after loading the scene
        /// </summary>
        [SerializeField]
        [Tooltip("Current scene's first UI after loading the scene")]
        string m_sceneFirstUi = "";

        /// <summary>
        /// If you want to set UI's default Selectable and pause signal at current scene starts, add an instance
        /// </summary>
        [SerializeField]
        [Tooltip("If you want to set UI's default Selectable and pause signal at current scene starts, add an instance")]
        List<SceneUiInfo> m_SceneUiInfoList = null;

        /// <summary>
        /// Start
        /// </summary>
        // --------------------------------------------------------------------------------
        void Start()
        {

            // setUiIdentifiersForNextSceneStart
            {
                SceneChangeManager.Instance.setUiIdentifiersForNextSceneStart(this.m_sceneFirstUi);
            }

            // setDefaultSelectable
            // setSendPauseSignal
            {

                foreach (var val in this.m_SceneUiInfoList)
                {
                    UiManager.Instance.setDefaultSelectable(val.identifier, val.defaultSelectable);
                    UiManager.Instance.setSendPauseSignal(val.identifier, val.sendPauseSignal);
                }
                
            }
            
        }

        /// <summary>
        /// Back UI
        /// </summary>
        /// <param name="updateHistory">update UI history</param>
        // --------------------------------------------------------------------------------
        public void backUi(bool updateHistory)
        {
            UiManager.Instance.back(updateHistory);
        }

    }

}
