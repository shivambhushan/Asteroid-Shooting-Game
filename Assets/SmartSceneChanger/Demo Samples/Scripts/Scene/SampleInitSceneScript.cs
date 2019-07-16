using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSCSample
{

    public class SampleInitSceneScript : MonoBehaviour
    {

#if UNITY_EDITOR

        [SerializeField]
        UnityEngine.Object m_nextScene;

#endif

        [HideInInspector]
        [SerializeField]
        string m_nextSceneName = "";

        IEnumerator Start()
        {

            // wait for other scripts to finish their Start function
            yield return null;

            SSC.SceneChangeManager.Instance.loadNextScene(this.m_nextSceneName, "NowLoadingSimpleBlack", true);

        }

        void OnValidate()
        {

#if UNITY_EDITOR

            if (this.m_nextScene && !string.IsNullOrEmpty(this.m_nextScene.name))
            {
                this.m_nextSceneName = this.m_nextScene.name;
            }

            else
            {
                this.m_nextSceneName = "";
            }
#endif

        }

    }

}
