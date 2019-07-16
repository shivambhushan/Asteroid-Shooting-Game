using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSC
{

    /// <summary>
    /// Change sprite by language
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ChangeSpriteByLanguageScript : MonoBehaviour
    {

        /// <summary>
        /// Reference to Image
        /// </summary>
        Image m_refImage = null;

        /// <summary>
        /// LanguageAndSprite list
        /// </summary>
        [SerializeField]
        [Tooltip("LanguageAndSprite list")]
        List<LanguageAndSprite> m_languageAndSpriteList = new List<LanguageAndSprite>();

        /// <summary>
        /// Start
        /// </summary>
        // ---------------------------------------------------------------------------------------
        private void Start()
        {

            // m_refImage
            {
                this.m_refImage = this.GetComponent<Image>();
            }

            // addLanguageSignalReceiver
            {
                SimpleReduxManager.Instance.addLanguageSignalReceiver(this.onLanguageSignal);
            }

            // changeSpriteByLanguage
            {
                this.changeSpriteByLanguage();
            }

#if UNITY_EDITOR

            // checkDuplicatesEditorOnly
            {
                Funcs.checkDuplicatesEditorOnly(this.m_languageAndSpriteList, val => val.systemLanguage, this.transform);
            }
            
            // empty sprite
            {

                var temp = this.m_languageAndSpriteList.FindAll(val => val.sprite == null);

                if(temp.Count > 0)
                {
                    Debug.LogWarningFormat("(#if UNITY_EDITOR) : {0} Empty sprites : {1}", temp.Count, Funcs.CreateHierarchyPath(this.transform));
                }

            }

            // supportedLanguagesEditorOnly
            {

                if (LanguageManager.isAvailable())
                {

                    // count
                    {
                        if (LanguageManager.Instance.supportedLanguagesEditorOnly.Count != this.m_languageAndSpriteList.Count)
                        {
                            Debug.LogWarning("(#if UNITY_EDITOR) : Different size in language setting : " + Funcs.CreateHierarchyPath(this.transform));
                        }
                    }

                    // isSupportedLanguageEditorOnly
                    {

                        foreach (var val in this.m_languageAndSpriteList)
                        {
                            if (!LanguageManager.Instance.isSupportedLanguageEditorOnly(val.systemLanguage))
                            {
                                Debug.LogWarningFormat(
                                    "(#if UNITY_EDITOR) : Not supported language : {0} : {1}",
                                    val.systemLanguage,
                                    Funcs.CreateHierarchyPath(this.transform)
                                    );
                            }
                        }

                    }

                }

            }

#endif

        }

        /// <summary>
        /// Change sprite
        /// </summary>
        // ---------------------------------------------------------------------------------------
        void changeSpriteByLanguage()
        {

            if (LanguageManager.isAvailable())
            {
                var temp = this.m_languageAndSpriteList.Find(val => val.systemLanguage == LanguageManager.Instance.CurrentSystemLanguage);
                this.m_refImage.sprite = (temp != null) ? temp.sprite : null;
            }

        }

        /// <summary>
        /// LanguageSignal receiver
        /// </summary>
        /// <param name="langSignal">LanguageSignal</param>
        // ---------------------------------------------------------------------------------------
        void onLanguageSignal(LanguageSignal langSignal)
        {

            this.changeSpriteByLanguage();

        }

    }

}
