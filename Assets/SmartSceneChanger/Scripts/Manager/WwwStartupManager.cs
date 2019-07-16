using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Class for WWW startup
    /// </summary>
    public class WwwStartupManager : SingletonMonoBehaviour<WwwStartupManager>
    {

        /// <summary>
        /// Class for loading WWW startup
        /// </summary>
        protected class WwwStartupContents : StartupContents
        {

            /// <summary>
            /// Url
            /// </summary>
            public string url = "";

            /// <summary>
            /// Success Action
            /// </summary>
            public Action<WWW> successAction = null;

            /// <summary>
            /// Failed Action
            /// </summary>
            public Action<WWW> failedAction = null;

            /// <summary>
            /// Progress Action
            /// </summary>
            public Action<WWW> progressAction = null;

            /// <summary>
            /// Progress value function
            /// </summary>
            public Func<float> progressValueFunc = null;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_url">url</param>
            /// <param name="_successAction">successAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            public WwwStartupContents(string _url, Action<WWW> _successAction, Action<WWW> _failedAction, Action<WWW> _progressAction)
            {
                this.url = _url;
                this.successAction = _successAction;
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

        }

        /// <summary>
        /// WwwStartupContents list
        /// </summary>
        protected List<WwwStartupContents> m_wwwsList = new List<WwwStartupContents>();

        /// <summary>
        /// The number of parallel loading coroutines
        /// </summary>
        [SerializeField]
        [Tooltip("The number of parallel loading coroutines")]
        protected int m_numberOfCo = 4;

        /// <summary>
        /// Ignore error
        /// </summary>
        [SerializeField]
        [Tooltip("Ignore error")]
        protected bool m_ignoreError = false;

        /// <summary>
        /// ThreadPriority
        /// </summary>
        [SerializeField]
        [Tooltip("ThreadPriority")]
        protected UnityEngine.ThreadPriority m_threadPriority = UnityEngine.ThreadPriority.Low;

        /// <summary>
        /// Error seconds for timeout
        /// </summary>
        [SerializeField]
        [Tooltip("Error seconds for timeout")]
        protected float m_noProgressTimeOutSeconds = 0.0f;

        /// <summary>
        /// DialogMessages
        /// </summary>
        protected DialogMessages m_messages = new DialogMessages();

        /// <summary>
        /// Current error
        /// </summary>
        protected StartupContents m_currentError = null;

        /// <summary>
        /// override
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {
            this.m_numberOfCo = Math.Max(1, this.m_numberOfCo);
        }

        /// <summary>
        /// Add startup data
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="success_func">success function</param>
        /// <param name="failed_func">failed function</param>
        /// <param name="progress_func">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupWww(string url, Action<WWW> success_func, Action<WWW> failed_func, Action<WWW> progress_func)
        {
            this.m_wwwsList.Add(new WwwStartupContents(url, success_func, failed_func, progress_func));
        }

        /// <summary>
        /// Message for timeout
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageTimeout()
        {
            return "Connection Timeout";
        }

        /// <summary>
        /// Denominator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public int progressDenominator()
        {
            return this.m_wwwsList.Count;
        }

        /// <summary>
        /// Numerator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public float progressNumerator()
        {

            float ret = 0.0f;

            foreach (var wwws in this.m_wwwsList)
            {

                if (wwws.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                {
                    ret += 1.0f;
                }

                else if (wwws.progressValueFunc != null)
                {
                    ret += wwws.progressValueFunc();
                }

            }

            return ret;

        }

        /// <summary>
        /// Has NotYet content
        /// </summary>
        /// <returns>detected</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool hasNotYetContent()
        {
            return this.m_wwwsList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NotYet) != null;
        }

        /// <summary>
        /// Create error message for dialog
        /// </summary>
        /// <returns>error message object</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual System.Object createErrorMessage()
        {

            this.m_messages.clear();

            this.m_messages.category = DialogMessages.MessageCategory.Error;

            string errorMessage = (this.m_currentError != null) ? this.m_currentError.errorMessage : "Unknown Error";
            string errorUrl = (this.m_currentError != null) ? this.m_currentError.urlIfNeeded : "Unknown Error";

            if (LanguageManager.isAvailable())
            {

                LanguageManager lm = LanguageManager.Instance;

                this.m_messages.title = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Title_Error);
                this.m_messages.mainMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Error_Www_Startup, errorMessage);
                this.m_messages.subMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Sub_Retry);
                this.m_messages.urlIfNeeded = errorUrl;

            }

            else
            {
                this.m_messages.title = "WWW Error";
                this.m_messages.mainMessage = errorMessage;
                this.m_messages.subMessage = "Retry ?";
                this.m_messages.urlIfNeeded = errorUrl;
            }

            return this.m_messages;

        }

        /// <summary>
        /// Clear contents
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public void clearContents()
        {

            this.m_wwwsList.Clear();

            this.clearErrorForRestart();

        }

        /// <summary>
        /// Start www loadings
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public IEnumerator startWwwStartup()
        {

            if (this.m_wwwsList.Count <= 0 || !this.hasNotYetContent())
            {
                yield break;
            }

            // ---------------

            // clearErrorForRestart
            {
                this.clearErrorForRestart();
            }

            // StartCoroutine
            {

                for (int i = 0; i < this.m_numberOfCo; i++)
                {
                    StartCoroutine(this.startWwwStartupSub());
                }

            }

            // wait 1 frame
            {
                yield return null;
            }

            // wait coroutines
            {

                WaitForSeconds wfs = new WaitForSeconds(0.5f);

                while (this.m_wwwsList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NowWorking) != null)
                {
                    yield return wfs;
                }

            }

        }

        /// <summary>
        /// Start www loadings sub
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator startWwwStartupSub()
        {

            int size = this.m_wwwsList.Count;

            WwwStartupContents wwws = null;

            // ----------------------

            for (int i = 0; i < size; i++)
            {

                // hasError break
                {
                    if (this.hasError())
                    {
                        break;
                    }
                }

                // ----------------------

                wwws = this.m_wwwsList[i];

                // ----------------------

                // continue if not NotYet
                {
                    if (wwws.currentWorkingState != StartupContents.WorkingState.NotYet)
                    {
                        continue;
                    }
                }

                // NowWorking
                {
                    wwws.currentWorkingState = StartupContents.WorkingState.NowWorking;
                }

                // loadWwwStartupContents
                {
                    yield return this.loadWwwStartupContents(wwws);
                }

                // DoneSuccessOrError
                {
                    wwws.currentWorkingState = StartupContents.WorkingState.DoneSuccessOrError;
                }

            }

        }

        /// <summary>
        /// Load WwwStartupContents
        /// </summary>
        /// <param name="wwws">WwwStartupContents</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadWwwStartupContents(WwwStartupContents wwws)
        {

            if(wwws == null)
            {
                yield break;
            }

            // ---------------------

            // WWW
            {

                float noProgressTimer = 0.0f;
                float previousProgress = 0.0f;

                using (WWW www = new WWW(wwws.url))
                {

#if !UNITY_WEBGL
                    www.threadPriority = this.m_threadPriority;
#endif

                    // set progressValueFunc
                    {
                        wwws.progressValueFunc = () =>
                        {
                            return (www != null) ? www.progress : 0.0f;
                        };
                    }

                    // set urlIfNeeded
                    {
                        wwws.urlIfNeeded = wwws.url;
                    }

                    // wait www done
                    {

                        while (!www.isDone)
                        {

                            if (wwws.progressAction != null)
                            {
                                wwws.progressAction(www);
                            }

                            // timeout
                            {

                                if (this.m_noProgressTimeOutSeconds > 0.0f)
                                {

                                    if (Mathf.Approximately(previousProgress, www.progress))
                                    {
                                        noProgressTimer += Time.deltaTime;
                                    }

                                    else
                                    {
                                        noProgressTimer = 0.0f;
                                    }

                                    previousProgress = www.progress;

                                    if (noProgressTimer >= this.m_noProgressTimeOutSeconds)
                                    {
                                        wwws.errorMessage = this.messageTimeout();
                                        break;
                                    }

                                }

                            }

                            yield return null;

                        } // while (!www.isDone)

                        if (wwws.progressAction != null)
                        {
                            wwws.progressAction(www);
                        }

                        yield return null;

                    }

                    // success or fail
                    {

                        // set errorMessage
                        {

                            if (string.IsNullOrEmpty(wwws.errorMessage))
                            {
                                wwws.errorMessage = www.error;
                            }

                        }

                        // success
                        if (string.IsNullOrEmpty(wwws.errorMessage))
                        {
                            if (wwws.successAction != null)
                            {
                                wwws.successAction(www);
                            }
                        }

                        // fail
                        else
                        {

                            if (wwws.failedAction != null)
                            {
                                wwws.failedAction(www);
                            }

                            // updateError
                            {
                                this.updateError(wwws);
                            }

                        }

                    }

                    // set progressValueFunc to null
                    {
                        wwws.progressValueFunc = null;
                    }

                } // using

            }

        }

        /// <summary>
        /// Clear error for restart
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void clearErrorForRestart()
        {

            // m_currentError
            {
                this.m_currentError = null;
            }

            // m_wwwsList
            {

                foreach (var val in this.m_wwwsList)
                {

                    if (!string.IsNullOrEmpty(val.errorMessage))
                    {
                        val.currentWorkingState = StartupContents.WorkingState.NotYet;
                    }

                    val.errorMessage = "";

                }

            }

        }

        /// <summary>
        /// Update error
        /// </summary>
        /// <param name="startupContents">error StartupContents</param>
        // -------------------------------------------------------------------------------------------------------
        protected void updateError(StartupContents startupContents)
        {

            if (this.m_ignoreError)
            {
                return;
            }

            // ----------

            if (startupContents != null)
            {
                this.m_currentError = startupContents;
            }

        }

        /// <summary>
        /// Has error
        /// </summary>
        /// <returns>has</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool hasError()
        {
            return this.m_currentError != null;
        }

    }

}
