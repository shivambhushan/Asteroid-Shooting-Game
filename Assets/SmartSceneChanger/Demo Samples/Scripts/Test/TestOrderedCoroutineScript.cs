using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;

namespace SSCSample
{

    public class TestOrderedCoroutineScript : MonoBehaviour
    {

        int m_val0 = 51;

        int m_val1 = 1102;

        // ----------------------------------------------------------------------------------------------------
        void Start()
        {
            SSC.SimpleReduxManager.Instance.addSceneChangeStateReceiver(this.onSceneChangeState);

        }

        // ----------------------------------------------------------------------------------------------------
        void onSceneChangeState(SSC.SceneChangeState scState)
        {
            
            if(scState.stateEnum == SceneChangeState.StateEnum.ScenePlaying)
            {
                Invoke("addTest0", 1.0f);
                Invoke("addTest0", 1.3f);
                Invoke("addTest0", 3.4f);
                Invoke("addTest1", 2.2f);
                Invoke("addTest1", 4.7f);
                Invoke("addTest1", 5.8f);
            }

        }

        // ----------------------------------------------------------------------------------------------------
        void addTest0()
        {
            int groupId = 0;
            CoroutineManager.Instance.addOrderedCoroutine(this.testIe0(groupId), this, groupId);
        }

        // ----------------------------------------------------------------------------------------------------
        void addTest1()
        {
            int groupId = 1;
            CoroutineManager.Instance.addOrderedCoroutine(this.testIe1(groupId), this, groupId);
        }

        // ----------------------------------------------------------------------------------------------------
        IEnumerator testIe0(int groupId)
        {

            for(int i = 0; i < 5; i++)
            {
                this.m_val0++;
                print(groupId + " : val = " + this.m_val0);
                yield return new WaitForSeconds(1);
            }

        }

        // ----------------------------------------------------------------------------------------------------
        IEnumerator testIe1(int groupId)
        {

            for (int i = 0; i < 5; i++)
            {
                this.m_val1++;
                print(groupId + " : val = " + this.m_val1);
                yield return new WaitForSeconds(1);
            }

        }

    }

}
