using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class SampleSimpleSceneScript : MonoBehaviour
    {

        List<string> m_left = new List<string>() { "SimpleSceneUi_Left", "Dummy" };
        string m_up = "SimpleSceneUi_Up";

        //void Start()
        //{
        //    print("You can also use SceneUiInfo prefab (== SceneUiInfoScript.cs) instead");
        //    SceneChangeManager.Instance.setUiIdentifiersForNextSceneStart(this.m_up);
        //}

        public void switchUi()
        {

            if(UiManager.Instance.containsIdentifier(this.m_up))
            {
                UiManager.Instance.showUi(this.m_left, true, false);
            }

            else
            {
                UiManager.Instance.showUi(this.m_up, true, false);
            }

        }

    }

}
