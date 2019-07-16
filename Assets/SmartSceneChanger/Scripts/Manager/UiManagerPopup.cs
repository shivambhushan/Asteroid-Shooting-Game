using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// UI singleton manager
    /// </summary>
    public partial class UiManager : SingletonMonoBehaviour<UiManager>
    {

        /// <summary>
        /// Temp popup info
        /// </summary>
        protected class TempPopupInfo
        {

            public string message = "";
            public Action showDoneCallback = null;

            public TempPopupInfo(string _message, Action _showDoneCallback)
            {
                this.message = _message;
                this.showDoneCallback = _showDoneCallback;
            }

        }

        [Space(30.0f)]

        /// <summary>
        /// Reference to PopupUiControllerScript
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to PopupUiControllerScript")]
        protected PopupUiControllerScript m_popupUiInfo = null;

        /// <summary>
        /// showPopupIE IEnumerator
        /// </summary>
        protected IEnumerator m_showPopupIE = null;

        /// <summary>
        /// TempPopupInfo Queue
        /// </summary>
        protected Queue<TempPopupInfo> m_tempPopupInfoList = new Queue<TempPopupInfo>();

        /// <summary>
        /// Show popup
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="showDoneCallback">callback when showing done</param>
        // ----------------------------------------------------------------------------------------
        public void showPopup(string message, Action showDoneCallback)
        {

            if(this.m_popupUiInfo)
            {

                // Enqueue
                {
                    this.m_tempPopupInfoList.Enqueue(new TempPopupInfo(message, showDoneCallback));
                }

                // StartCoroutine
                {
                    if (this.m_showPopupIE == null)
                    {
                        StartCoroutine(this.m_showPopupIE = this.showPopupIE());
                    }
                }

            }

#if UNITY_EDITOR

            else
            {
                Debug.LogWarning("(#if UNITY_EDITOR) : m_popupUiInfo == null : " + Funcs.CreateHierarchyPath(this.transform));
            }

#endif

        }

        /// <summary>
        /// Show popup UI IEnumerator
        /// </summary>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------------------------
        protected IEnumerator showPopupIE()
        {

            if (!this.m_popupUiInfo)
            {
                this.m_showPopupIE = null;
                yield break;
            }

            // -------------------


            // wait and show
            {

                TempPopupInfo temp = null;
                while (this.m_tempPopupInfoList.Count > 0)
                {

                    temp = this.m_tempPopupInfoList.Dequeue();

                    // wait
                    {
                        while (this.m_popupUiInfo.currentShowHideState != UiControllerScript.ShowHideState.NowHiding)
                        {
                            yield return null;
                        }
                    }

                    // show
                    {
                        this.m_popupUiInfo.setText(temp.message);
                        this.m_popupUiInfo.startShowing(true, this.m_popupUiInfo.autoHideSeconds, temp.showDoneCallback);
                    }

                }

            }

            // m_showPopupIE
            {
                this.m_showPopupIE = null;
            }

        }

    }

}
