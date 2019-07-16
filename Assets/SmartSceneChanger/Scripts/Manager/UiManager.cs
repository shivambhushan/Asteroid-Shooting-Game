using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using UiIdentifiers = System.Collections.Generic.List<string>;

namespace SSC
{

    /// <summary>
    /// Ui group and its default Selectable
    /// </summary>
    [Serializable]
    public class UiListAndDefaultSelectable
    {

        /// <summary>
        /// UI identifier
        /// </summary>
        [Tooltip("UI identifier")]
        public string identifier = "";

        /// <summary>
        /// UI list
        /// </summary>
        [Tooltip("UI list")]
        public List<UiControllerScript> uiList = new List<UiControllerScript>();

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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_identifier">identifier</param>
        public UiListAndDefaultSelectable(string _identifier)
        {
            this.identifier = _identifier;
        }

    }

    /// <summary>
    /// UI singleton manager
    /// </summary>
    public partial class UiManager : SingletonMonoBehaviour<UiManager>
    {

        [Obsolete("Obsolete", false)]
        public enum UiManagerType
        {
            Common,
            Scene,
        }

        /// <summary>
        /// Current showing ui id
        /// </summary>
        [SerializeField]
        [ReadOnly]
        protected UiIdentifiers m_currentShowingUi = new UiIdentifiers();

        /// <summary>
        /// Previous showing ui id
        /// </summary>
        [SerializeField]
        [ReadOnly]
        protected UiIdentifiers m_previousShowingUi = new UiIdentifiers();

#if UNITY_EDITOR

        /// <summary>
        /// Update m_uiGroups for debug purpose in Inspector
        /// </summary>
        [SerializeField]
        [Tooltip("Update m_uiGroups for debug purpose in Inspector")]
        protected bool m_updateUiGroupForDebug = false;

#endif

        /// <summary>
        /// UI group from inspector
        /// </summary>
        [Header("You can add a value to Ui Groups by UiControllerScript.")]
        [Header("So you don't have to add a value in this Inspector.")]
        [SerializeField]
        [Tooltip("UI Group")]
        protected List<UiListAndDefaultSelectable> m_uiGroups = new List<UiListAndDefaultSelectable>();

        /// <summary>
        /// UI group Dictionary
        /// </summary>
        protected Dictionary<string, UiListAndDefaultSelectable> m_uiDictionary = new Dictionary<string, UiListAndDefaultSelectable>();

        /// <summary>
        /// There are any UIs in showing transition 
        /// </summary>
        protected bool m_nowInShowingTransition = false;

        /// <summary>
        /// There are any UIs in hiding transition 
        /// </summary>
        protected bool m_nowInHidingTransition = false;

        /// <summary>
        /// Temp string list
        /// </summary>
        protected List<string> m_tempShowUi = new List<string>();

        /// <summary>
        /// Temp UiControllerScript list
        /// </summary>
        protected List<UiControllerScript> m_tempShowList = new List<UiControllerScript>();

        /// <summary>
        /// Temp UiControllerScript list
        /// </summary>
        protected List<UiControllerScript> m_tempHideList = new List<UiControllerScript>();

        // ----------------------------------------------------------------------------------------

        /// <summary>
        /// UI Dictionary Reference
        /// </summary>
        public Dictionary<string, UiListAndDefaultSelectable> uiDictionary { get { return this.m_uiDictionary; } }

        /// <summary>
        /// m_currentShowingUi AsReadOnly
        /// </summary>
        public IList<string> currentShowingUiAsReadOnly { get { return this.m_currentShowingUi.AsReadOnly(); } }

        /// <summary>
        /// m_currentShowingUi Copy
        /// </summary>
        public List<string> currentShowingUiCopy { get { return new List<string>(this.m_currentShowingUi); } }

        /// <summary>
        /// m_previousShowingUi AsReadOnly
        /// </summary>
        public IList<string> previousShowingUiAsReadOnly { get { return this.m_previousShowingUi.AsReadOnly(); } }

        /// <summary>
        /// m_previousShowingUi Copy
        /// </summary>
        public List<string> previousShowingUiCopy { get { return new List<string>(this.m_previousShowingUi); } }

        /// <summary>
        /// Are there any UIs in showing transition
        /// </summary>
        public bool nowInShowingTransition { get { return this.m_nowInShowingTransition; } }

        /// <summary>
        /// Are there any UIs in hiding transition
        /// </summary>
        public bool nowInHidengTransition { get { return this.m_nowInHidingTransition; } }

        /// <summary>
        /// Are there any UIs in showing or hiding transition
        /// </summary>
        public bool nowInShowingOrHidingTransition { get { return (this.m_nowInShowingTransition || this.m_nowInHidingTransition); } }

        // ----------------------------------------------------------------------------------------

        /// <summary>
        /// initOnAwake
        /// </summary>
        // ----------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

            this.m_currentShowingUi.Clear();
            this.m_previousShowingUi.Clear();

            this.initUiDictionary();

        }

        /// <summary>
        /// Init m_uiDictionary
        /// </summary>
        // ----------------------------------------------------------------------------------------
        protected void initUiDictionary()
        {

            this.m_uiDictionary.Clear();

            foreach (UiListAndDefaultSelectable val in this.m_uiGroups)
            {

                if (string.IsNullOrEmpty(val.identifier))
                {
                    Debug.LogError("Empty UI identifier not allowed");
                    continue;
                }

                if (this.m_uiDictionary.ContainsKey(val.identifier))
                {
                    Debug.LogError("m_uiDictionary already contains a key : " + val.identifier);
                    continue;
                }

                this.m_uiDictionary.Add(val.identifier, val);

            }

        }

        /// <summary>
        /// Contains identifier
        /// </summary>
        /// <param name="id">UI identifier</param>
        /// <returns>contains</returns>
        // ----------------------------------------------------------------------------------------
        public bool containsIdentifier(string id)
        {
            return this.m_currentShowingUi.Contains(id);
        }

        /// <summary>
        /// Add element without duplicated
        /// </summary>
        /// <param name="currentList">current</param>
        /// <param name="targetList">result target</param>
        // ----------------------------------------------------------------------------------------
        protected void addElementWithoutDuplicated(List<UiControllerScript> currentList, List<UiControllerScript> targetList)
        {

            int size = targetList.Count;

            for (int i = 0; i < size; i++)
            {
                if (!currentList.Contains(targetList[i]))
                {
                    currentList.Add(targetList[i]);
                }
            }

        }

        /// <summary>
        /// List by identifiers
        /// </summary>
        /// <param name="identifiers">UiIdentifiers</param>
        /// <param name="refList">list to return</param>
        /// <param name="removeElements">elements to remove</param>
        // ----------------------------------------------------------------------------------------
        protected virtual void listByIdentifier(
            UiIdentifiers identifiers,
            List<UiControllerScript> refList,
            List<UiControllerScript> removeElements = null
            )
        {

            refList.Clear();

            foreach (string id in identifiers)
            {

                if (this.m_uiDictionary.ContainsKey(id))
                {
                    this.addElementWithoutDuplicated(refList, this.m_uiDictionary[id].uiList);
                }

            }

            if (removeElements != null)
            {
                for (int i = refList.Count - 1; i >= 0; i--)
                {
                    if (removeElements.Contains(refList[i]))
                    {
                        refList.RemoveAt(i);
                    }
                }
            }

        }

        /// <summary>
        /// Show UIs by list
        /// </summary>
        /// <param name="list">UiControllerScript list</param>
        /// <param name="restartShowing">restart showing ui if already showing</param>
        /// <param name="autoHideInvoke">Invoke hide function</param>
        /// <param name="showAllDoneCallback">callback when all showing done</param>
        // ----------------------------------------------------------------------------------------
        protected void showUiByList(
            List<UiControllerScript> list,
            bool restartShowing,
            float autoHideInvoke,
            Action showAllDoneCallback
            )
        {

            if (list.Count <= 0)
            {

                if (showAllDoneCallback != null)
                {
                    showAllDoneCallback();
                }

                this.m_nowInShowingTransition = false;

                setSelectable();

                return;

            }

            this.m_nowInShowingTransition = true;

            int counter = 0;

            for (int i = list.Count - 1; i >= 0; i--)
            {

                if (!list[i])
                {
                    list.RemoveAt(i);
                    continue;
                }

                // -------------

                list[i].startShowing(restartShowing, autoHideInvoke, () =>
                {

                    counter++;

                    if (counter >= list.Count)
                    {

                        this.m_nowInShowingTransition = false;

                        setSelectable();

                        if (showAllDoneCallback != null)
                        {
                            showAllDoneCallback();
                        }

                    }

                });

            }

        }

        /// <summary>
        /// Hide UIs by list
        /// </summary>
        /// <param name="list">UiControllerScript list</param>
        /// <param name="hideAllDoneCallback">callback when all hiding done</param>
        // ----------------------------------------------------------------------------------------
        protected void hideUiByList(List<UiControllerScript> list, Action hideAllDoneCallback)
        {

            if (list.Count <= 0)
            {

                this.m_nowInHidingTransition = false;

                if (hideAllDoneCallback != null)
                {
                    hideAllDoneCallback();
                }

                return;

            }

            this.m_nowInHidingTransition = true;

            int counter = 0;

            for (int i = list.Count - 1; i >= 0; i--)
            {

                if (!list[i])
                {
                    list.RemoveAt(i);
                    continue;
                }

                // -------------

                list[i].startHiding(() =>
                {

                    counter++;

                    if (counter >= list.Count)
                    {

                        this.m_nowInHidingTransition = false;

                        if (hideAllDoneCallback != null)
                        {
                            hideAllDoneCallback();
                        }

                    }

                });

            }

        }

        /// <summary>
        /// Back button function
        /// </summary>
        // ----------------------------------------------------------------------------------------
        public void back(bool updateHistory)
        {
            this.showUi(this.previousShowingUiCopy, updateHistory, false);
        }

        /// <summary>
        /// Show Ui button function
        /// </summary>
        /// <param name="identifier">ui identifier</param>
        [Obsolete("Obsolete", false)]
        // ----------------------------------------------------------------------------------------
        public void showUiOnButtonClick(string identifier)
        {
            this.showUi(identifier, true, false);
        }

        /// <summary>
        /// Show Ui button function without updating previous ui identifier
        /// </summary>
        /// <param name="identifier">ui identifier</param>
        [Obsolete("Obsolete", false)]
        // ----------------------------------------------------------------------------------------
        public void showUiOnButtonClickWithoutUpdatingHistory(string identifier)
        {
            this.showUi(identifier, false, false);
        }

        /// <summary>
        /// Hide UI
        /// </summary>
        [Obsolete("Use showUi(\"\", ---)", true)]
        // ----------------------------------------------------------------------------------------
        public void hideUi()
        {

        }

        /// <summary>
        /// Show UI
        /// </summary>
        /// <param name="identifier">Ui identifier</param>
        // ----------------------------------------------------------------------------------------
        public void showUi(string identifier)
        {
            this.showUi(identifier, true, false);
        }

        /// <summary>
        /// Show UI
        /// </summary>
        /// <param name="identifier">Ui identifier</param>
        /// <param name="updateUiHistory">update m_previousShowingUi</param>
        /// <param name="restartShowing">restart showing ui if already showing</param>
        /// <param name="autoHideInvoke">Invoke hide function</param>
        /// <param name="showAllDoneCallback">callback when all showing done</param>
        /// <param name="hideAllDoneCallback">callback when all hiding done</param>
        // ----------------------------------------------------------------------------------------
        public void showUi(
            string identifier,
            bool updateUiHistory,
            bool restartShowing,
            float autoHideInvoke = 0.0f,
            Action showAllDoneCallback = null,
            Action hideAllDoneCallback = null
            )
        {

            this.m_tempShowUi.Clear();
            this.m_tempShowUi.Add(identifier);

            this.showUi(
                this.m_tempShowUi,
                updateUiHistory,
                restartShowing,
                autoHideInvoke,
                showAllDoneCallback,
                hideAllDoneCallback
                );

        }

        /// <summary>
        /// Show UI
        /// </summary>
        /// <param name="identifiers">UiIdentifiers</param>
        /// <param name="updateUiHistory">update m_previousShowingUi</param>
        /// <param name="restartShowing">restart showing ui if already showing</param>
        /// <param name="autoHideInvoke">Invoke hide function</param>
        /// <param name="showAllDoneCallback">callback when all showing done</param>
        /// <param name="hideAllDoneCallback">callback when all hiding done</param>
            // ----------------------------------------------------------------------------------------
        public void showUi(
            UiIdentifiers identifiers,
            bool updateUiHistory,
            bool restartShowing,
            float autoHideInvoke = 0.0f,
            Action showAllDoneCallback = null,
            Action hideAllDoneCallback = null
            )
        {

            this.showUiInternal(
                identifiers,
                updateUiHistory,
                restartShowing,
                autoHideInvoke,
                showAllDoneCallback,
                hideAllDoneCallback
                );

        }

        /// <summary>
        /// Should send pause state
        /// </summary>
        /// <returns>send or not</returns>
        // -------------------------------------------------------------------------------------
        public virtual bool shouldSendPauseState()
        {

            foreach (string id in this.m_currentShowingUi)
            {
                if (this.m_uiDictionary.ContainsKey(id))
                {
                    if (this.m_uiDictionary[id].sendPauseSignal)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

        /// <summary>
        /// Send pause state if needed
        /// </summary>
        // -------------------------------------------------------------------------------------
        protected void sendPauseSignalIfNeeded()
        {

            bool temp = this.shouldSendPauseState();

            var pState = SimpleReduxManager.Instance.PauseStateWatcher.state();

            if (temp != pState.pause)
            {
                pState.setState(temp);
            }

        }

        /// <summary>
        /// Show UI internal
        /// </summary>
        /// <param name="identifiers">UiIdentifiers</param>
        /// <param name="updateUiHistory">update UI history</param>
        /// <param name="restartShowing">restart showing</param>
        /// <param name="autoHideInvoke">auto hide invoke</param>
        /// <param name="showAllDoneCallback">callback when showing done</param>
        /// <param name="hideAllDoneCallback">callback when hiding done</param>
        // -------------------------------------------------------------------------------------
        protected void showUiInternal(
            UiIdentifiers identifiers,
            bool updateUiHistory,
            bool restartShowing,
            float autoHideInvoke,
            Action showAllDoneCallback,
            Action hideAllDoneCallback
            )
        {

            for (int i = identifiers.Count - 1; i >= 0; i--)
            {

                if (string.IsNullOrEmpty(identifiers[i]))
                {
                    identifiers.RemoveAt(i);
                }

            }

#if UNITY_EDITOR

            foreach (string id in identifiers)
            {

                if (!this.m_uiDictionary.ContainsKey(id))
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) m_uiDictionary not contain : " + id);
                }

            }

#endif

            // check the same
            {

                bool same = true;

                if (identifiers.Count == this.m_currentShowingUi.Count)
                {

                    int count = identifiers.Count;

                    for (int i = 0; i < count; i++)
                    {

                        if (!identifiers.Contains(this.m_currentShowingUi[i]))
                        {
                            same = false;
                            break;
                        }

                    }

                }

                else
                {
                    same = false;
                }

                if (same)
                {
                    return;
                }

            }

            this.listByIdentifier(identifiers, this.m_tempShowList);
            this.listByIdentifier(this.m_currentShowingUi, this.m_tempHideList, this.m_tempShowList);

            if (updateUiHistory)
            {
                this.m_previousShowingUi.Clear();
                this.m_previousShowingUi.AddRange(this.m_currentShowingUi);
            }

            // set m_currentShowingUi
            {
                this.m_currentShowingUi.Clear();
                this.m_currentShowingUi.AddRange(identifiers);
            }

            // sendPauseSignalIfNeeded
            {
                sendPauseSignalIfNeeded();
            }

            // show hide
            {
                this.hideUiByList(this.m_tempHideList, hideAllDoneCallback);
                this.showUiByList(this.m_tempShowList, restartShowing, autoHideInvoke, showAllDoneCallback);
            }

        }

        /// <summary>
        /// Get default Selectable from current showing ui
        /// </summary>
        /// <returns>Selectable</returns>
        // -------------------------------------------------------------------------------------
        public Selectable getDefaultSelectableFromCurrentShowingUi()
        {

            if (this.m_currentShowingUi.Count > 0 && this.m_uiDictionary.ContainsKey(this.m_currentShowingUi[0]))
            {
                return this.m_uiDictionary[this.m_currentShowingUi[0]].defaultSelectable;
            }

            return null;

        }

        /// <summary>
        /// Set Selectable
        /// </summary>
        // -------------------------------------------------------------------------------------
        public void setSelectable()
        {

            if (!EventSystem.current)
            {
                return;
            }

#if UNITY_IOS || UNITY_ANDROID

            EventSystem.current.SetSelectedGameObject(null);

#else

            // -----------------

            Selectable selectable = this.getDefaultSelectableFromCurrentShowingUi();
            GameObject currentGameObject = (EventSystem.current) ? EventSystem.current.currentSelectedGameObject : null;

            if (this.nowInShowingOrHidingTransition)
            {
                return;
            }

            // --------------------

            {

                if (selectable)
                {

                    if (currentGameObject != selectable.gameObject)
                    {
                        selectable.Select();
                    }

                }

                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }

            }

#endif

        }

        /// <summary>
        /// Add new UiListAndDefaultSelectable
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <returns>success</returns>
        // -------------------------------------------------------------------------------------
        protected bool addNewUiListAndDefaultSelectable(string identifier)
        {

            if(string.IsNullOrEmpty(identifier))
            {
                Debug.LogError("Empty identifier");
                return false;
            }

            // ------------------

            if (!this.m_uiDictionary.ContainsKey(identifier))
            {
                this.m_uiDictionary.Add(identifier, new UiListAndDefaultSelectable(identifier));
            }

            return true;

        }

        /// <summary>
        /// Add UiControllerScript
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <param name="uiController">UiControllerScript</param>
        // -------------------------------------------------------------------------------------
        public void addUiControllerScript(string identifier, UiControllerScript uiController)
        {

            if(!this.addNewUiListAndDefaultSelectable(identifier))
            {
                return;
            }

            // --------------

            // if (this.m_uiDictionary.ContainsKey(identifier))
            {

                if(!this.m_uiDictionary[identifier].uiList.Contains(uiController))
                {

                    this.m_uiDictionary[identifier].uiList.Add(uiController);


#if UNITY_EDITOR

                    if (this.m_updateUiGroupForDebug)
                    {
                        this.m_uiGroups = new List<UiListAndDefaultSelectable>(this.m_uiDictionary.Values);
                    }

#endif

                }

            }

        }

        /// <summary>
        /// Remove UiControllerScript
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <param name="uiController">UiControllerScript</param>
        // -------------------------------------------------------------------------------------
        public void removeUiControllerScript(string identifier, UiControllerScript uiController)
        {

            if (!this.addNewUiListAndDefaultSelectable(identifier))
            {
                return;
            }

            // --------------

            if (this.m_uiDictionary.ContainsKey(identifier) && uiController)
            {
                
                if (this.m_uiDictionary[identifier].uiList.Contains(uiController))
                {

                    this.m_uiDictionary[identifier].uiList.Remove(uiController);

                    if (this.m_uiDictionary[identifier].uiList.Count <= 0 && !this.m_uiDictionary[identifier].sendPauseSignal)
                    {
                        this.m_uiDictionary.Remove(identifier);
                    }

#if UNITY_EDITOR

                    if (this.m_updateUiGroupForDebug)
                    {
                        this.m_uiGroups = new List<UiListAndDefaultSelectable>(this.m_uiDictionary.Values);
                    }

#endif
                }

            }

        }

        /// <summary>
        /// Set default Selectable
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <param name="selectable">Selectable</param>
        // -------------------------------------------------------------------------------------
        public void setDefaultSelectable(string identifier, Selectable selectable)
        {

            if (!this.addNewUiListAndDefaultSelectable(identifier))
            {
                return;
            }

            // --------------

            // if (this.m_uiDictionary.ContainsKey(identifier))
            {
                this.m_uiDictionary[identifier].defaultSelectable = selectable;
            }

        }

        /// <summary>
        /// Set sendPauseSignal
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <param name="sendPauseSignal">sendPauseSignal</param>
        // -------------------------------------------------------------------------------------
        public void setSendPauseSignal(string identifier, bool sendPauseSignal)
        {

            if (!this.addNewUiListAndDefaultSelectable(identifier))
            {
                return;
            }

            // --------------

            // if (this.m_uiDictionary.ContainsKey(identifier))
            {
                this.m_uiDictionary[identifier].sendPauseSignal = sendPauseSignal;
            }

        }

    }

}
