using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Start IEnumerator at each start of scene
    /// </summary>
    public class IEnumeratorStartupManager : SingletonMonoBehaviour<IEnumeratorStartupManager>
    {

        /// <summary>
        /// Before any startups, After any startups
        /// </summary>
        public enum BeforeAfter
        {
            Before,
            After
        }

        /// <summary>
        /// Class for IEnumerator startup
        /// </summary>
        protected class IeStartupContents : StartupContents
        {

            /// <summary>
            /// IEnumeratorStartupScript reference 
            /// </summary>
            public IEnumerator startup = null;

            /// <summary>
            /// Progress value function
            /// </summary>
            public Func<float> progressValueFunc = null;

            /// <summary>
            /// Error function
            /// </summary>
            public Func<string> errorMessageFunc = null;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_startup">startup</param>
            /// <param name="_progressValueFunc">progressValueFunc</param>
            /// <param name="_errorMessageFunc">errorMessageFunc</param>
            public IeStartupContents(IEnumerator _startup, Func<float> _progressValueFunc, Func<string> _errorMessageFunc)
            {
                this.startup = _startup;
                this.progressValueFunc = _progressValueFunc;
                this.errorMessageFunc = _errorMessageFunc;
            }

        }

        /// <summary>
        /// IeStartupContents list for before
        /// </summary>
        protected List<IeStartupContents> m_iesListBefore = new List<IeStartupContents>();

        /// <summary>
        /// IeStartupContents list for After
        /// </summary>
        protected List<IeStartupContents> m_iesListAfter = new List<IeStartupContents>();

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
        /// <param name="startup">startup</param>
        /// <param name="progress">progress</param>
        /// <param name="ba">before or after</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupIEnumerator(IEnumerator startup, Func<float> progress, Func<string> error, BeforeAfter ba)
        {

            if(startup == null)
            {

#if UNITY_EDITOR

                Debug.LogWarning("startup == null");

#endif

                return;

            }

            // -------------------------------

            if(ba == BeforeAfter.Before)
            {
                this.m_iesListBefore.Add(new IeStartupContents(startup, progress, error));
            }

            else
            {
                this.m_iesListAfter.Add(new IeStartupContents(startup, progress, error));
            }
           
        }

        /// <summary>
        /// Denominator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public int progressDenominator()
        {
            return this.m_iesListBefore.Count + this.m_iesListAfter.Count;
        }

        /// <summary>
        /// Numerator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public float progressNumerator()
        {

            float ret = 0.0f;

            foreach(var ies in this.m_iesListBefore)
            {

                if(ies.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                {
                    ret += 1.0f;
                }

                else if(ies.progressValueFunc != null)
                {
                    ret += ies.progressValueFunc();
                }
                
            }

            foreach (var ies in this.m_iesListAfter)
            {

                if (ies.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                {
                    ret += 1.0f;
                }

                else if (ies.progressValueFunc != null)
                {
                    ret += ies.progressValueFunc();
                }

            }

            return ret;

        }

        /// <summary>
        /// Get proper list
        /// </summary>
        /// <param name="ba">BeforeAfter</param>
        /// <returns>list</returns>
        // -------------------------------------------------------------------------------------------------------
        protected List<IeStartupContents> properList(BeforeAfter ba)
        {
            return (ba == BeforeAfter.Before) ? this.m_iesListBefore : this.m_iesListAfter;
        }

        /// <summary>
        /// Has NotYet content
        /// </summary>
        /// <returns>detected</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool hasNotYetContent(BeforeAfter ba)
        {
            return this.properList(ba).Find(x => x.currentWorkingState == StartupContents.WorkingState.NotYet) != null;
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

            if(LanguageManager.isAvailable())
            {

                LanguageManager lm = LanguageManager.Instance;

                this.m_messages.title = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Title_Error);
                this.m_messages.mainMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Error_IEnumerator_Startup, errorMessage);
                this.m_messages.subMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Sub_Retry);
                
            }

            else
            {
                this.m_messages.title = "IEnumerator Error";
                this.m_messages.mainMessage = errorMessage;
                this.m_messages.subMessage = "Retry ?";
            }

            return this.m_messages;

        }

        /// <summary>
        /// Clear contents
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        public void clearContents()
        {

            this.m_iesListBefore.Clear();
            this.m_iesListAfter.Clear();

            this.clearErrorForRestart();

        }

        /// <summary>
        /// Start IEnumerator startups
        /// </summary>
        /// <param name="ba">BeforeAfter</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public IEnumerator startIEnumerator(BeforeAfter ba)
        {
            
            var list = (ba == BeforeAfter.Before) ? this.m_iesListBefore : this.m_iesListAfter;

            if(list.Count <= 0 || !this.hasNotYetContent(ba))
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
                    StartCoroutine(this.startIEnumeratorSub(ba));
                }

            }

            // wait 1 frame
            {
                yield return null;
            }

            // wait coroutines
            {

                WaitForSeconds wfs = new WaitForSeconds(0.1f);

                while (list.Find(x => x.currentWorkingState == StartupContents.WorkingState.NowWorking) != null)
                {
                    yield return wfs;
                }

            }

        }

        /// <summary>
        /// Start IEnumerator startups sub
        /// </summary>
        /// <param name="ba">BeforeAfter</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator startIEnumeratorSub(BeforeAfter ba)
        {

            var list = (ba == BeforeAfter.Before) ? this.m_iesListBefore : this.m_iesListAfter;

            int size = list.Count;

            IeStartupContents ies = null;

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

                ies = list[i];

                // ----------------------

                // continue if not NotYet
                {
                    if (ies.currentWorkingState != StartupContents.WorkingState.NotYet)
                    {
                        continue;
                    }
                }

                // ----------------------

                // NowWorking
                {
                    ies.currentWorkingState = StartupContents.WorkingState.NowWorking;
                }

                // startup
                {
                    yield return ies.startup;
                }

                // updateError
                {

                    ies.errorMessage = (ies.errorMessageFunc != null) ? ies.errorMessageFunc() : "";

                    if(!string.IsNullOrEmpty(ies.errorMessage))
                    {
                        this.updateError(ies);
                    }

                }

                // DoneSuccessOrError
                {
                    ies.currentWorkingState = StartupContents.WorkingState.DoneSuccessOrError;
                }

            }

        }

        /// <summary>
        /// Clear error for restart
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void clearErrorForRestart()
        {

            // m_currentYoungestError
            {
                this.m_currentError = null;
            }

            // m_iesListBefore
            {

                foreach (var val in this.m_iesListBefore)
                {

                    if (!string.IsNullOrEmpty(val.errorMessage))
                    {
                        val.currentWorkingState = StartupContents.WorkingState.NotYet;
                    }

                    val.errorMessage = "";

                }

            }

            // m_iesListAfter
            {

                foreach (var val in this.m_iesListAfter)
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
            
            if(this.m_ignoreError)
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