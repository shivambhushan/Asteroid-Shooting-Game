using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class SampleSceneCommonScript : MonoBehaviour
    {

        public string m_nowLoadingUiIdentifier = "NowLoading1";

        StateWatcher<PauseState> m_refPauseStateWatcher = null;

        void Start()
        {
            this.m_refPauseStateWatcher = SimpleReduxManager.Instance.PauseStateWatcher;
        }

        void Update()
        {

            if(Input.GetMouseButtonDown(0))
            {

                {
                    List<string> temp = SSC.UiManager.Instance.currentShowingUiCopy;

                    if (temp.Contains("Pause"))
                    {
                        temp.Remove("Pause");
                        UiManager.Instance.showUi(temp, true, false);
                    }

                }

                {

                    //PauseState pState = this.m_refPauseStateWatcher.state();

                    //if (pState.pause)
                    //{
                    //    pState.setState(this.m_refPauseStateWatcher, false);
                    //}

                }

            }

        }

        public void reloadCurrentScene()
        {
            SceneChangeManager.Instance.loadNextScene(SceneChangeManager.Instance.nowLoadingSceneName, this.m_nowLoadingUiIdentifier, true);
        }
        
        public void backToTitleScene()
        {
            SceneChangeManager.Instance.loadNextScene(SceneChangeManager.Instance.titleSceneName, this.m_nowLoadingUiIdentifier, true);
        }

        public void pause()
        {

            PauseState pState = this.m_refPauseStateWatcher.state();

            if (!pState.pause)
            {
                UiManager.Instance.showUi("Pause", true, false);
                //pState.setState(this.m_refPauseStateWatcher, true);
            }

        }

    }

}
