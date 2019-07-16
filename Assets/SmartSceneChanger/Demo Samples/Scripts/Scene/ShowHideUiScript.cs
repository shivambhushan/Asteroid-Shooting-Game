using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    [RequireComponent(typeof(UiControllerScript))]
    public class ShowHideUiScript : MonoBehaviour
    {

        UiControllerScript m_refUiController = null;

        void Start()
        {
            this.m_refUiController = this.GetComponent<UiControllerScript>();
        }

        public void showOrHide()
        {

            if(this.m_refUiController.currentShowHideState == UiControllerScript.ShowHideState.NowHiding)
            {
                this.m_refUiController.startShowing();
            }

            else if (this.m_refUiController.currentShowHideState == UiControllerScript.ShowHideState.NowShowing)
            {
                this.m_refUiController.startHiding();
            }

        }

    }

}