using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSC
{

    /// <summary>
    /// Change system text by language
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ChangeSystemTextByLanguageScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Text
        /// </summary>
        Text m_refText = null;

        /// <summary>
        /// Key in csv
        /// </summary>
        [SerializeField]
        [Tooltip("Key in csv")]
        string m_keyInCsv = "";

        /// <summary>
        /// Start
        /// </summary>
        // ---------------------------------------------------------------------------------------
        private void Start()
        {

            // m_refText
            {
                this.m_refText = this.GetComponent<Text>();
            }

            // addLanguageSignalReceiver
            {
                SimpleReduxManager.Instance.addLanguageSignalReceiver(this.onLanguageSignal);
            }

            // changeTextAndFontByLanguage
            {
                this.changeTextAndFontByLanguage();
            }

        }

        /// <summary>
        /// Change text and font
        /// </summary>
        // ---------------------------------------------------------------------------------------
        void changeTextAndFontByLanguage()
        {

            if (LanguageManager.isAvailable())
            {

                if (LanguageManager.Instance.CurrentFont)
                {
                    this.m_refText.font = LanguageManager.Instance.CurrentFont;
                }

                this.m_refText.text = LanguageManager.Instance.getFormattedString(this.m_keyInCsv);

            }

        }

        /// <summary>
        /// LanguageSignal receiver
        /// </summary>
        /// <param name="langSignal">LanguageSignal</param>
        // ---------------------------------------------------------------------------------------
        void onLanguageSignal(LanguageSignal langSignal)
        {

            this.changeTextAndFontByLanguage();

        }

    }

}
