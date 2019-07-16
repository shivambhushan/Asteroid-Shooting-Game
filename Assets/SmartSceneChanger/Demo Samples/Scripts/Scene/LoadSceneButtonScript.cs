using System.Collections;
using System.Collections.Generic;
using SSC;
using UnityEngine;

namespace SSCSample
{

    public class LoadSceneButtonScript : MonoBehaviour
    {

#if UNITY_EDITOR

        [SerializeField]
        UnityEngine.Object m_scene;


#endif

        [HideInInspector]
        [SerializeField]
        string m_sceneName = "";

        public void loadScene()
        {
            Debug.LogError("load scene");
            SceneChangeManager.Instance.loadNextScene(this.m_sceneName, "NowLoading1", true);
        }

        void OnValidate()
        {

#if UNITY_EDITOR

            this.m_sceneName = (this.m_scene && !string.IsNullOrEmpty(this.m_scene.name)) ? this.m_scene.name : "";

#endif

        }

    }

}
