using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Startup contents
    /// </summary>
    public class StartupContents
    {

        /// <summary>
        /// Working state
        /// </summary>
        public enum WorkingState
        {
            NotYet,
            NowWorking,
            DoneSuccessOrError,
        }

        /// <summary>
        /// Current WorkingState
        /// </summary>
        public WorkingState currentWorkingState = WorkingState.NotYet;

        /// <summary>
        /// Error message
        /// </summary>
        public string errorMessage = "";

        /// <summary>
        /// URL if needed
        /// </summary>
        public string urlIfNeeded = "";

    }

}
