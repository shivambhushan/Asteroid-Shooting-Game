using System;

namespace SSC
{

    /// <summary>
    /// SceneChangeState class
    /// </summary>
    public class SceneChangeState : SReduxStateBase<SceneChangeState>
    {

        /// <summary>
        /// enum state
        /// </summary>
        public enum StateEnum
        {
            ScenePlaying,
            NowLoadingIntro,
            NowLoadingMain,
            AllStartupsDonePrev,
            AllStartupsDone,
            AllStartupsDoneNext,
            NowLoadingOutro,

            [Obsolete("Obsoleted value", true)]
            InnerChange,
        }

        /// <summary>
        /// Current state
        /// </summary>
        public StateEnum stateEnum = StateEnum.ScenePlaying;

        /// <summary>
        /// Next scene name
        /// </summary>
        [Obsolete("Use SceneChangeManager.Instance.nowLoadingSceneName", true)]
        public string nextSceneName = "";

        /// <summary>
        /// Set state
        /// </summary>
        /// <param name="_stateEnum">stateEnum</param>
        // ----------------------------------------------------------------------------------------------
        public void setState(StateEnum _stateEnum)
        {
            this.stateEnum = _stateEnum;
            
            this.m_refWatcher.sendState();
        }

        /// <summary>
        /// Set state
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="_stateEnum">stateEnum</param>
        [Obsolete("Use setState(StateEnum)", false)]
        // ----------------------------------------------------------------------------------------------
        public void setState(StateWatcher<SceneChangeState> watcher, StateEnum _stateEnum)
        {
            this.stateEnum = _stateEnum;

            watcher.sendState();
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
