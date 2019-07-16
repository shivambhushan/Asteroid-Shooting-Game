using System;

namespace SSC
{

    /// <summary>
    /// PauseState class
    /// </summary>
    public class PauseState : SReduxStateBase<PauseState>
    {

        /// <summary>
        /// Pause state
        /// </summary>
        public bool pause = false;

        /// <summary>
        /// Set state
        /// </summary>
        /// <param name="_pause">pause</param>
        // ----------------------------------------------------------------------------------------------
        public void setState(bool _pause)
        {
            this.pause = _pause;

            this.m_refWatcher.sendState();
        }

        /// <summary>
        /// Set state
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="_pause">pause</param>
        [Obsolete("Use setState(bool)", false)]
        // ----------------------------------------------------------------------------------------------
        public void setState(StateWatcher<PauseState> watcher, bool _pause)
        {
            this.pause = _pause;

            watcher.sendState();
        }

        /// <summary>
        /// Reset params on scene loaded
        /// </summary>
        // ----------------------------------------------------------------------------------------------
        public override void resetOnSceneLevelLoaded()
        {
            this.pause = false;
        }

    }

}
