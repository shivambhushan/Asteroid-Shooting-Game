using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSCSample
{

    public class SampleUITestScript : MonoBehaviour
    {

        void Start()
        {
            SSC.SimpleReduxManager.Instance.addSceneChangeStateReceiver(this.onSceneChangeState);
            SSC.SceneChangeManager.Instance.setUiIdentifiersForNextSceneStart("SampleSceneUi");
        }

        void onSceneChangeState(SSC.SceneChangeState scState)
        {

        }

        void Update()
        {

            if(Input.GetMouseButtonDown(0))
            {

                List<string> temp = SSC.UiManager.Instance.currentShowingUiCopy;

                if (temp.Contains("Pause"))
                {
                    temp.Remove("Pause");
                    SSC.UiManager.Instance.showUi(temp, true, false);
                }

            }

            else if (Input.GetMouseButtonDown(1))
            {

                if(!SSC.SimpleReduxManager.Instance.PauseStateWatcher.state().pause)
                {

                    var list = SSC.UiManager.Instance.currentShowingUiAsReadOnly;

                    if (list.Count > 0)
                    {
                        SSC.UiManager.Instance.showUi("", true, false);
                    }

                    else
                    {
                        SSC.UiManager.Instance.showUi("SampleSceneUi", true, false);
                    }

                }

            }

        }

        public void onClickPauseButton()
        {
            
            List<string> temp = SSC.UiManager.Instance.currentShowingUiCopy;

            if (!temp.Contains("Pause"))
            {
                temp.Add("Pause");
                SSC.UiManager.Instance.showUi(temp, true, false);
            }

        }

        public void onClickReloadButton()
        {
            SSC.SceneChangeManager.Instance.loadNextScene(SSC.SceneChangeManager.Instance.nowLoadingSceneName, true);
        }

    }

}
