using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSCSample
{

    public class SamplePopupUiScript : MonoBehaviour
    {

        public enum SamplePopupEnum
        {
            Sample1,
        }

        public void showPopUp()
        {

            //string temp = SSC.LanguageManager.Instance.getFormattedString(SamplePopupEnum.Sample1);
            //SSC.UiManager.Instance.showPopup(temp, null);


            SSC.UiManager.Instance.showPopup("Popup Message 1", null);

        }

    }

}
