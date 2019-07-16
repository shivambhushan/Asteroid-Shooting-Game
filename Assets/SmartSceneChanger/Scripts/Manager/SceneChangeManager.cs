using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SSC
{

    /// <summary>
    /// Singleton class for changing scene
    /// </summary>
    public partial class SceneChangeManager : SingletonMonoBehaviour<SceneChangeManager>
    {

#if UNITY_EDITOR

        /// <summary>
        /// Title scene object for failure
        /// </summary>
        [SerializeField]
        [Tooltip("Title scene object for failure")]
        protected UnityEngine.Object m_titleScene;

#endif

        /// <summary>
        /// Title scene name of m_titleScene
        /// </summary>
        [HideInInspector]
        [SerializeField]
        protected string m_titleSceneName = "";

        /// <summary>
        /// Current UI identifier for now loading
        /// </summary>
        [SerializeField]
        [Tooltip("Current UI identifier for now loading")]
        protected string m_currentNowLoadingUiIdentifier = "NowLoading";

        /// <summary>
        /// string for next scene
        /// </summary>
        protected string m_nowLoadingSceneName = "";

        /// <summary>
        /// Previous scene name
        /// </summary>
        protected string m_previousSceneName = "";

        /// <summary>
        /// UI identifiers for next scene start
        /// </summary>
        protected List<string> m_uiIdentifiersForNextSceneStart = new List<string>();

        /// <summary>
        /// Progresses of loading scene
        /// </summary>
        protected float m_loadingSceneProgress = 0.0f;

        /// <summary>
        /// Lock statements before loadings
        /// </summary>
        protected List<MonoBehaviour> m_lockBeforeLoadings = new List<MonoBehaviour>();

        /// <summary>
        /// Lock statements after loadings
        /// </summary>
        protected List<MonoBehaviour> m_lockAfterLoadings = new List<MonoBehaviour>();

        /// <summary>
        /// DialogMessages for back to title
        /// </summary>
        protected DialogMessages m_messagesForBackToTitle = new DialogMessages();

        /// <summary>
        /// Messages object for error
        /// </summary>
        protected System.Object m_messagesForError = null;

        // -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// string for next scene
        /// </summary>
        public string nowLoadingSceneName { get { return this.m_nowLoadingSceneName; } }

        /// <summary>
        /// Previous scene name
        /// </summary>
        public string previousSceneName { get { return this.m_previousSceneName; } }

        /// <summary>
        /// UI identifier for now loading
        /// </summary>
        public string currentNowLoadingUiIdentifier { get { return this.m_currentNowLoadingUiIdentifier; } set {this.m_currentNowLoadingUiIdentifier = value; } }

        /// <summary>
        /// Title scene name
        /// </summary>
        public string titleSceneName { get { return this.m_titleSceneName; } }

        // -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// override
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

            SceneManager.sceneLoaded += this.onSceneLoaded;

            this.m_nowLoadingSceneName = SceneManager.GetActiveScene().name;
            this.m_previousSceneName = this.m_nowLoadingSceneName;

        }

        /// <summary>
        /// on scene loaded
        /// </summary>
        /// <param name="scene">Scene</param>
        /// <param name="mode">LoadSceneMode</param>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void onSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            if(mode != LoadSceneMode.Single)
            {
                return;
            }

            // --------------------

            if(scene.name != this.m_nowLoadingSceneName)
            {
                this.m_previousSceneName = this.m_nowLoadingSceneName;
                this.m_nowLoadingSceneName = scene.name;
            }

        }

        /// <summary>
        /// Set UI identifiers for next scene start
        /// </summary>
        /// <param name="uiIdentifier">UI identifier</param>
        // -------------------------------------------------------------------------------------------------------
        public void setUiIdentifiersForNextSceneStart(string uiIdentifier)
        {
            this.m_uiIdentifiersForNextSceneStart.Clear();
            this.m_uiIdentifiersForNextSceneStart.Add(uiIdentifier);
        }

        /// <summary>
        /// Set UI identifiers for next scene start
        /// </summary>
        /// <param name="uiIdentifiers">UI identifier list</param>
        [Obsolete("Use single identifier", false)]
        // -------------------------------------------------------------------------------------------------------
        public void setUiIdentifiersForNextSceneStart(List<string> uiIdentifiers)
        {
            this.m_uiIdentifiersForNextSceneStart.Clear();
            this.m_uiIdentifiersForNextSceneStart.AddRange(uiIdentifiers);
        }

        /// <summary>
        /// Load next scene
        /// </summary>
        /// <param name="sceneName">scene name</param>
        // -------------------------------------------------------------------------------------------------------
        public virtual void loadNextScene(string sceneName)
        {
            this.loadNextScene(sceneName, this.m_currentNowLoadingUiIdentifier, true);
        }

        /// <summary>
        /// Load next scene
        /// </summary>
        /// <param name="sceneName">scene name</param>
        /// <param name="updateHistory">update history</param>
        // -------------------------------------------------------------------------------------------------------
        public virtual void loadNextScene(string sceneName, bool updateHistory)
        {
            this.loadNextScene(sceneName, this.m_currentNowLoadingUiIdentifier, updateHistory);
        }

        /// <summary>
        /// Load next scene
        /// </summary>
        /// <param name="sceneName">scene name</param>
        /// <param name="nowLoadingUiIdentifier">UI identifier for now loading</param>
        /// <param name="updateHistory">update history</param>
        // -------------------------------------------------------------------------------------------------------
        public virtual void loadNextScene(string sceneName, string nowLoadingUiIdentifier, bool updateHistory)
        {

            var scState = SimpleReduxManager.Instance.SceneChangeStateWatcher.state();

            if (scState.stateEnum != SceneChangeState.StateEnum.ScenePlaying)
            {
#if UNITY_EDITOR
                Debug.LogWarning("(#if UNITY_EDITOR) Discard loadNextScene : " + sceneName);
#endif
                return;
            }

            // -----------------------

            // m_currentNowLoadingUiIdentifier
            {
                this.m_currentNowLoadingUiIdentifier = nowLoadingUiIdentifier;
            }

            //// clear
            //{
            //    this.clearContentsBeforeLoadingScene();
            //}
            
            // set
            {

                if (updateHistory)
                {
                    this.m_previousSceneName = this.m_nowLoadingSceneName;
                }

                this.m_nowLoadingSceneName = sceneName;

            }

            // SceneChangeStateWatcher
            {
                scState.setState(SceneChangeState.StateEnum.NowLoadingIntro);
            }

            // show now loading ui
            {
                UiManager.Instance.showUi(this.m_currentNowLoadingUiIdentifier, false, false, 0.0f, this.callbackForStartingNowLoading);
            }

        }

        /// <summary>
        /// Callback after showing ui done
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void callbackForStartingNowLoading()
        {
            this.clearContentsBeforeLoadingScene();
            StartCoroutine(this.startNowLoadings(false));
        }

        /// <summary>
        /// Set error
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void setError(System.Object error)
        {
            this.m_messagesForError = error;
        }

        /// <summary>
        /// Has error
        /// </summary>
        /// <returns>has error</returns>
        // -------------------------------------------------------------------------------------------------------
        protected bool hasError()
        {
            return (this.m_messagesForError != null);
        }

        /// <summary>
        /// Clear contents before loading scene
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void clearContentsCommon()
        {

            this.m_loadingSceneProgress = 0.0f;

            this.m_uiIdentifiersForNextSceneStart.Clear();

            this.m_messagesForError = null;

            IEnumeratorStartupManager.Instance.clearContents();
            WwwStartupManager.Instance.clearContents();

        }

        /// <summary>
        /// Clear contents before loading scene
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void clearContentsBeforeLoadingScene()
        {
            this.clearContentsCommon();
            AssetBundleStartupManager.Instance.clearContents(true);
        }

        /// <summary>
        /// Clear contents after loading scene
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void clearContentsAfterLoadingScene()
        {
            this.clearContentsCommon();
            AssetBundleStartupManager.Instance.clearContents(false);
        }

        /// <summary>
        /// Retrt from resume pint
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void retry()
        {
            StartCoroutine(this.startNowLoadings(true));
        }

        /// <summary>
        /// Back to title
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public virtual void backToTitleScene()
        {
            this.loadNextScene(this.m_titleSceneName);
        }

        /// <summary>
        /// Back to title with showing dialog
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public void backToTitleSceneWithOkDialog()
        {
            DialogManager.Instance.showOkDialog(this.backToTitleMessages(), this.backToTitleScene);
        }

        /// <summary>
        /// Back to title messages
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual System.Object backToTitleMessages()
        {

            this.m_messagesForBackToTitle.clear();

            this.m_messagesForBackToTitle.category = DialogMessages.MessageCategory.Confirmation;

            if (LanguageManager.isAvailable())
            {

                LanguageManager lm = LanguageManager.Instance;

                this.m_messagesForBackToTitle.title = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Title_Confirmation);
                this.m_messagesForBackToTitle.mainMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Confirmation_Back_To_Title);

            }

            else
            {
                this.m_messagesForBackToTitle.title = "Confirmation";
                this.m_messagesForBackToTitle.mainMessage = "Back to Title Scene";
            }

            return this.m_messagesForBackToTitle;

        }

        /// <summary>
        /// Show back to title dialog
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public void showBackToTitleOkDialog()
        {

            this.m_nowLoadingSceneName = this.m_titleSceneName;

            this.clearContentsBeforeLoadingScene();

            DialogManager.Instance.showOkDialog(this.backToTitleMessages(), this.callbackForStartingNowLoading);

        }

        /// <summary>
        /// Load next scene base
        /// </summary>
        /// <param name="progress01">progress 01 function</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual IEnumerator loadSceneBase(Action<float> progress01)
        {

            AsyncOperation ao = SceneManager.LoadSceneAsync(this.m_nowLoadingSceneName);

            if (ao == null)
            {
                yield break;
            }

            while (!ao.isDone)
            {

                if(progress01 != null)
                {
                    progress01(ao.progress);
                }

                yield return null;

            }

        }

        /// <summary>
        /// Set scene change state enum
        /// </summary>
        /// <param name="stateEnum">state enum</param>
        // -------------------------------------------------------------------------------------------------------
        protected void setSceneChangeState(SceneChangeState.StateEnum stateEnum)
        {
            SimpleReduxManager.Instance.SceneChangeStateWatcher.state().setState(stateEnum);
        }

        /// <summary>
        /// Start loading next scene
        /// </summary>
        /// <param name="isRestrat">is restart</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator startNowLoadings(bool isRestrat)
        {

            yield return null;

            // clear error
            {
                this.setError(null);
            }

            // wait by lock
            {

                int i = 0;

                while(this.m_lockBeforeLoadings.Count > 0)
                {

                    yield return null;

                    for(i = this.m_lockBeforeLoadings.Count - 1; i >= 0; i--)
                    {
                        if(!this.m_lockBeforeLoadings[i])
                        {
                            this.m_lockBeforeLoadings.RemoveAt(i);
                        }
                    }

                }

            }

            // unload
            if(!isRestrat)
            {

                AsyncOperation ao = Resources.UnloadUnusedAssets();

                while (!ao.isDone)
                {
                    yield return null;
                }

                System.GC.Collect();

            }

            // setSceneChangeState
            {
                if (!isRestrat)
                {
                    this.setSceneChangeState(SceneChangeState.StateEnum.NowLoadingMain);
                }
            }

            // CanStreamedLevelBeLoaded
            {

                if (!Application.CanStreamedLevelBeLoaded(this.m_nowLoadingSceneName))
                {
#if UNITY_EDITOR
                    Debug.LogWarning("(#if UNITY_EDITOR) Not found scene in BuildSettings : " + this.m_nowLoadingSceneName);
#endif
                    Invoke("showBackToTitleOkDialog", 0.1f);
                    yield break;

                }
                
            }

            // load scene
            {
                
                if (!isRestrat)
                {

                    // loadSceneBase
                    {
                        yield return this.loadSceneBase(progress => {
                            this.m_loadingSceneProgress = progress;
                        });
                    }

                    this.m_loadingSceneProgress = 1.0f;

                }

            }

            // main
            {
             
                if(!this.hasError())
                {

                    yield return IEnumeratorStartupManager.Instance.startIEnumerator(IEnumeratorStartupManager.BeforeAfter.Before);

                    if (IEnumeratorStartupManager.Instance.hasError())
                    {
                        this.setError(IEnumeratorStartupManager.Instance.createErrorMessage());
                    }

                }

                if (!this.hasError())
                {

                    yield return AssetBundleStartupManager.Instance.startAbStartup();

                    if (AssetBundleStartupManager.Instance.hasError())
                    {
                        this.setError(AssetBundleStartupManager.Instance.createErrorMessage());
                    }

                }

                if (!this.hasError())
                {

                    yield return WwwStartupManager.Instance.startWwwStartup();

                    if (WwwStartupManager.Instance.hasError())
                    {
                        this.setError(WwwStartupManager.Instance.createErrorMessage());
                    }

                }

                if (!this.hasError())
                {

                    yield return IEnumeratorStartupManager.Instance.startIEnumerator(IEnumeratorStartupManager.BeforeAfter.After);

                    if (IEnumeratorStartupManager.Instance.hasError())
                    {
                        this.setError(IEnumeratorStartupManager.Instance.createErrorMessage());
                    }

                }

            }

            // hasError
            {
                
                if(this.hasError())
                {
                    DialogManager.Instance.showYesNoDialog(this.m_messagesForError, this.retry, this.showBackToTitleOkDialog);
                    yield break;
                }

            }

            // retry
            {
                
                if (IEnumeratorStartupManager.Instance.hasNotYetContent(IEnumeratorStartupManager.BeforeAfter.Before))
                {
                    Invoke("retry", 0.1f);
                    yield break;
                }

                if (AssetBundleStartupManager.Instance.hasNotYetContent())
                {
                    Invoke("retry", 0.1f);
                    yield break;
                }

                if (WwwStartupManager.Instance.hasNotYetContent())
                {
                    Invoke("retry", 0.1f);
                    yield break;
                }

                if (IEnumeratorStartupManager.Instance.hasNotYetContent(IEnumeratorStartupManager.BeforeAfter.After))
                {
                    Invoke("retry", 0.1f);
                    yield break;
                }

            }

            // checkIfNeedToReloadScene
            {
                if(AssetBundleStartupManager.Instance.checkIfNeedToReloadScene())
                {
                    Invoke("callbackForStartingNowLoading", 0.1f);
                    yield break;
                }
            }

            // SceneChangeStateWatcher
            {

                yield return null;

                this.setSceneChangeState(SceneChangeState.StateEnum.AllStartupsDonePrev);
                yield return null;

                this.setSceneChangeState(SceneChangeState.StateEnum.AllStartupsDone);
                yield return null;

                this.setSceneChangeState(SceneChangeState.StateEnum.AllStartupsDoneNext);
                yield return null;

            }

            // wait by lock
            {

                int i = 0;

                while (this.m_lockAfterLoadings.Count > 0)
                {

                    yield return null;

                    for (i = this.m_lockAfterLoadings.Count - 1; i >= 0; i--)
                    {
                        if (!this.m_lockAfterLoadings[i])
                        {
                            this.m_lockAfterLoadings.RemoveAt(i);
                        }
                    }

                }

            }

            // setSceneChangeState
            {
                this.setSceneChangeState(SceneChangeState.StateEnum.NowLoadingOutro);
            }

            // show ui
            {
                UiManager.Instance.showUi(this.m_uiIdentifiersForNextSceneStart, true, false, 0, this.sendNowLoadingDoneSignal, null);
            }

            // clear
            {
                this.clearContentsAfterLoadingScene();
            }
            
        }

        /// <summary>
        /// Send now loading donw signal
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void sendNowLoadingDoneSignal()
        {
            var scState = SimpleReduxManager.Instance.SceneChangeStateWatcher.state();
            scState.setState(SceneChangeState.StateEnum.ScenePlaying);
        }

        /// <summary>
        /// All progress denominator
        /// </summary>
        /// <returns>progress denominator</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual int progressDenominator()
        {
            return this.progressDenominator(true, true, true, true);
        }

        /// <summary>
        /// Denominator of total progresses (be careful with zero)
        /// </summary>
        /// <param name="includeAssetBundle">AssetBundle startup progress</param>
        /// <param name="includeWww">WWW startup progress</param>
        /// <param name="includeIEnumerator">IEnumerator startup progress</param>
        /// <returns>progress</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual int progressDenominator(bool includeSceneProgress, bool includeAssetBundle, bool includeWww, bool includeIEnumerator)
        {
            
            int ret = 0;

            if (includeSceneProgress)
            {
                ret += 1;
            }

            if (includeAssetBundle)
            {
                ret += AssetBundleStartupManager.Instance.progressDenominator();
            }

            if (includeWww)
            {
                ret += WwwStartupManager.Instance.progressDenominator();
            }

            if (includeIEnumerator)
            {
                ret += IEnumeratorStartupManager.Instance.progressDenominator();
            }

            return ret;

        }

        /// <summary>
        /// All progress numerator
        /// </summary>
        /// <returns>progress numerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual float progressNumerator()
        {
            return this.progressNumerator(true, true, true, true);
        }

        /// <summary>
        /// Numerator of total progresses
        /// </summary>
        /// <param name="includeAssetBundle">AssetBundle startup progress</param>
        /// <param name="includeWww">WWW startup progress</param>
        /// <param name="includeIEnumerator">IEnumerator startup progress</param>
        /// <returns>progress</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual float progressNumerator(bool includeSceneProgress, bool includeAssetBundle, bool includeWww, bool includeIEnumerator)
        {

            float ret = 0.0f;

            if (includeSceneProgress)
            {
                ret += this.m_loadingSceneProgress;
            }

            if (includeAssetBundle)
            {
                ret += AssetBundleStartupManager.Instance.progressNumerator();
            }

            if (includeWww)
            {
                ret += WwwStartupManager.Instance.progressNumerator();
            }

            if (includeIEnumerator)
            {
                ret += IEnumeratorStartupManager.Instance.progressNumerator();
            }

            return ret;

        }

        /// <summary>
        /// Add lock obj to before
        /// </summary>
        /// <param name="mb">obj</param>
        // -------------------------------------------------------------------------------------------------------
        public void addLockToBefore(MonoBehaviour mb)
        {

            if(mb && !this.m_lockBeforeLoadings.Contains(mb))
            {
                this.m_lockBeforeLoadings.Add(mb);
            }

        }

        /// <summary>
        /// Add lock obj to after
        /// </summary>
        /// <param name="mb">obj</param>
        // -------------------------------------------------------------------------------------------------------
        public void addLockToAfter(MonoBehaviour mb)
        {

            if (mb && !this.m_lockAfterLoadings.Contains(mb))
            {
                this.m_lockAfterLoadings.Add(mb);
            }

        }

        /// <summary>
        /// Remove lock obj from before
        /// </summary>
        /// <param name="mb">obj</param>
        // -------------------------------------------------------------------------------------------------------
        public void removeLockFromBefore(MonoBehaviour mb)
        {

#if UNITY_EDITOR

            if(!this.m_lockBeforeLoadings.Remove(mb))
            {
                Debug.LogWarning("(#if UNITY_EDITOR) Failed removeLockFromBefore");
            }

#else

            this.m_lockBeforeLoadings.Remove(mb);

#endif


        }

        /// <summary>
        /// Remove lock obj from after
        /// </summary>
        /// <param name="mb">obj</param>
        // -------------------------------------------------------------------------------------------------------
        public void removeLockFromAfter(MonoBehaviour mb)
        {

#if UNITY_EDITOR

            if (!this.m_lockAfterLoadings.Remove(mb))
            {
                Debug.LogWarning("(#if UNITY_EDITOR) Failed removeLockFromAfter");
            }

#else

            this.m_lockAfterLoadings.Remove(mb);

#endif

        }

        /// <summary>
        /// OnValidate
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void OnValidate()
        {

#if UNITY_EDITOR

            if (this.m_titleScene && !string.IsNullOrEmpty(this.m_titleScene.name))
            {
                this.m_titleSceneName = this.m_titleScene.name;
            }

            else
            {
                this.m_titleSceneName = "";
            }
#endif

        }

    }

}
