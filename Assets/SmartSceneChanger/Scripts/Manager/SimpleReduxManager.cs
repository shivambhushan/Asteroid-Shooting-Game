using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SSC
{

    /// <summary>
    /// interface for onSceneLoaded
    /// </summary>
    [Obsolete("Use SReduxStateBase", true)]
    public interface IResetStateOnSceneLoaded
    {
        void resetOnSceneLevelLoaded();
    }

    /// <summary>
    /// SReduxStateBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SReduxStateBase<T> where T : SReduxStateBase<T>, new()
    {

        /// <summary>
        /// Reference to StateWatcher
        /// </summary>
        protected StateWatcher<T> m_refWatcher = null;

        /// <summary>
        /// Set StateWatcher
        /// </summary>
        /// <param name="watcher">StateWatcher</param>
        public void setStateWatcher(StateWatcher<T> watcher)
        {
            this.m_refWatcher = watcher;
        }

        /// <summary>
        /// Reset params on scene loaded
        /// </summary>
        public abstract void resetOnSceneLevelLoaded();

    }

    /// <summary>
    /// Class for simple redux pattern
    /// </summary>
    public class SimpleReduxManager : SingletonMonoBehaviour<SimpleReduxManager>
    {

        /// <summary>
        /// StateWatcher for SceneChangeState
        /// </summary>
        protected StateWatcher<SceneChangeState> m_sceneChangeStateWatcher = new StateWatcher<SceneChangeState>();

        /// <summary>
        /// StateWatcher for PauseState
        /// </summary>
        protected StateWatcher<PauseState> m_pauseStateWatcher = new StateWatcher<PauseState>();

        /// <summary>
        /// StateWatcher for LanguageSignal
        /// </summary>
        protected StateWatcher<LanguageSignal> m_languageSignalWatcher = new StateWatcher<LanguageSignal>();

        // ----------------------------------------------------------------------------------------------

        /// <summary>
        /// StateWatcher SceneChangeState  getter
        /// </summary>
        public StateWatcher<SceneChangeState> SceneChangeStateWatcher { get { return this.m_sceneChangeStateWatcher; } }

        /// <summary>
        /// StateWatcher PauseState getter
        /// </summary>
        public StateWatcher<PauseState> PauseStateWatcher { get { return this.m_pauseStateWatcher; } }

        /// <summary>
        /// StateWatcher LanguageSignal getter
        /// </summary>
        public StateWatcher<LanguageSignal> LanguageSignalWatcher { get { return this.m_languageSignalWatcher; } }

        /// <summary>
        /// Called in Awake
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

            SceneManager.sceneLoaded += this.resetOnSceneLoaded;

        }

        /// <summary>
        /// Reset states on scene loaded
        /// </summary>
        /// <param name="scene">Scene</param>
        /// <param name="mode">LoadSceneMode</param>
        // ----------------------------------------------------------------------------------------------
        protected virtual void resetOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            if(mode != LoadSceneMode.Single)
            {
                return;
            }

            // ---------------

            this.m_sceneChangeStateWatcher.state().resetOnSceneLevelLoaded();
            this.m_pauseStateWatcher.state().resetOnSceneLevelLoaded();
            this.m_languageSignalWatcher.state().resetOnSceneLevelLoaded();

        }


        /// <summary>
        /// Add scene change state receiver action
        /// </summary>
        /// <param name="action">PauseState</param>
        // ----------------------------------------------------------------------------------------------
        public void addSceneChangeStateReceiver(Action<SceneChangeState> action)
        {

            if (action != null)
            {
                this.m_sceneChangeStateWatcher.addAction(action);
            }

        }

        /// <summary>
        /// Add pause state receiver action
        /// </summary>
        /// <param name="action">PauseState</param>
        // ----------------------------------------------------------------------------------------------
        public void addPauseStateReceiver(Action<PauseState> action)
        {

            if(action != null)
            {
                this.m_pauseStateWatcher.addAction(action);
            }

        }

        /// <summary>
        /// Add language signal receiver action
        /// </summary>
        /// <param name="action">LanguageSignal</param>
        // ----------------------------------------------------------------------------------------------
        public void addLanguageSignalReceiver(Action<LanguageSignal> action)
        {

            if (action != null)
            {
                this.m_languageSignalWatcher.addAction(action);
            }

        }

    }

}
