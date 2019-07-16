using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSCSample
{

    public class SampleLanguageScript : MonoBehaviour
    {

        public void changeLanguage()
        {

            if(!SSC.LanguageManager.isAvailable())
            {
                Debug.LogWarning("Add LanguageManager.cs to your init scene.");
                return;
            }

            // -------------------------

            if(SSC.LanguageManager.Instance.CurrentSystemLanguage == SystemLanguage.English)
            {
                SSC.LanguageManager.Instance.setCurrentSystemLanguage(SystemLanguage.Japanese);
            }

            else
            {
                SSC.LanguageManager.Instance.setCurrentSystemLanguage(SystemLanguage.English);
            }

        }

    }

}
