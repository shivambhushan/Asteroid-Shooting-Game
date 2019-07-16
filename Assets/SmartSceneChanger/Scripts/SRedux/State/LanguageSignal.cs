using System;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// LanguageSignal class
    /// </summary>
    public class LanguageSignal : SReduxStateBase<LanguageSignal>
    {

#if UNITY_EDITOR

        /// <summary>
        /// Unused (Use LanguageManager.Instance.CurrentSystemLanguage)
        /// </summary>
        [Obsolete("Use LanguageManager.Instance.CurrentSystemLanguage", true)]
        public SystemLanguage currentSystemLanguage = SystemLanguage.Unknown;

#endif

        /// <summary>
        /// Set state
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public void setState()
        {
            this.m_refWatcher.sendState();
        }

        /// <summary>
        /// Reset params on scene loaded
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public override void resetOnSceneLevelLoaded()
        {
            
        }

    }

}
