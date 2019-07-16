using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SSC
{

    /// <summary>
    /// Language Manager
    /// </summary>
    public class LanguageManager : SingletonMonoBehaviour<LanguageManager>
    {

        /// <summary>
        /// Language keys for SSC
        /// </summary>
        public enum SSCLanguageKeys
        {

            Confirmation_Back_To_Title,

            Dialog_Title_Confirmation,
            Dialog_Title_Error,
            Dialog_Sub_Retry,

            Error_AssetBundle_Startup,
            Error_Connection_Timeout,
            Error_IEnumerator_Startup,
            Error_Www_Startup,

        }

        /// <summary>
        /// Current SystemLanguage
        /// </summary>
        [SerializeField]
        [Tooltip("Current SystemLanguage")]
        protected SystemLanguage m_currentSystemLanguage = SystemLanguage.English;

        /// <summary>
        /// Language csv for system text
        /// </summary>
        [SerializeField]
        [Tooltip("Language csv for system text")]
        [FormerlySerializedAs("m_languageCsv")]
        protected TextAsset m_languageCsvForSystemText = null;

        /// <summary>
        /// LanguageAndFont list
        /// </summary>
        [SerializeField]
        [Tooltip("LanguageAndFont list")]
        protected List<LanguageAndFont> m_languageAndFontList = new List<LanguageAndFont>();

        /// <summary>
        /// Language format dictionary
        /// </summary>
        protected Dictionary<SystemLanguage, Dictionary<string, string>> m_languageDict = new Dictionary<SystemLanguage, Dictionary<string, string>>();

        /// <summary>
        /// Current Font
        /// </summary>
        protected Font m_currentFont = null;

#if UNITY_EDITOR

        [Space(30.0f)]

        /// <summary>
        /// Supported language list (EditorOnly)
        /// </summary>
        [SerializeField]
        [Tooltip("Supported language list (EditorOnly)")]
        protected List<SystemLanguage> m_supportedLanguagesEditorOnly = new List<SystemLanguage>();

#endif

        // -------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Current SystemLanguage
        /// </summary>
        public SystemLanguage CurrentSystemLanguage { get { return this.m_currentSystemLanguage; } }

        /// <summary>
        /// Current Font
        /// </summary>
        public Font CurrentFont { get { return this.m_currentFont; } }

#if UNITY_EDITOR

        /// <summary>
        /// Supported language list (EditorOnly)
        /// </summary>
        public List<SystemLanguage> supportedLanguagesEditorOnly { get { return this.m_supportedLanguagesEditorOnly; } }

        /// <summary>
        /// Is supported language
        /// </summary>
        /// <param name="systemLanguage">SystemLanguage</param>
        /// <returns>supported</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool isSupportedLanguageEditorOnly(SystemLanguage systemLanguage)
        {
            return this.m_supportedLanguagesEditorOnly.Contains(systemLanguage);
        }

#endif

        /// <summary>
        /// Called in Awake
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

#if UNITY_EDITOR

            if (!this.m_languageCsvForSystemText)
            {
                Debug.LogWarning("(#if UNITY_EDITOR) : m_languageCsvForSystemText is null : " + Funcs.CreateHierarchyPath(this.transform));
            }

            // m_languageAndFontList
            {

                foreach (var val in this.m_languageAndFontList)
                {

                    if (!this.isSupportedLanguageEditorOnly(val.systemLanguage))
                    {
                        Debug.LogWarning("(#if UNITY_EDITOR) : Not supported language : " + val.systemLanguage.ToString() + " : " + Funcs.CreateHierarchyPath(this.transform));
                    }

                    if (!val.font)
                    {
                        Debug.LogWarning("(#if UNITY_EDITOR) : font is null : " + val.systemLanguage.ToString() + " : " + Funcs.CreateHierarchyPath(this.transform));
                    }

                }

            }

#endif

            // initCsvForSystemText
            {
                this.initCsvForSystemText();
            }

            // m_currentFont
            {
                var temp = this.m_languageAndFontList.Find(val => val.systemLanguage == this.m_currentSystemLanguage);
                this.m_currentFont = (temp != null) ? temp.font : null;
            }

        }

        /// <summary>
        /// Init
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void initCsvForSystemText()
        {

            if (!this.m_languageCsvForSystemText)
            {
                return;
            }

            // ----------------------

            // clear
            {
                this.m_languageDict.Clear();
            }

            // ----------------------

            List<List<string>> csvRowColList = CsvParser.parse(this.m_languageCsvForSystemText.text);

            List<string> languageNames = new List<string>(Enum.GetNames(typeof(SystemLanguage)));

            // ----------------------

            // init
            {

                int rowSize = csvRowColList.Count;
                int colSize = (rowSize > 0) ? csvRowColList[0].Count : 0;

                string languageInRow0 = "";
                SystemLanguage systemLanguage = SystemLanguage.Unknown;
                string keyInCol0 = "";

                for (int row = 1; row < rowSize; row++)
                {

                    colSize = csvRowColList[row].Count;

                    keyInCol0 = csvRowColList[row][0].Split('\n')[0];

                    for (int col = 3; col < colSize; col++)
                    {

                        languageInRow0 = csvRowColList[0][col];

                        if (col == 3 && !languageNames.Contains(languageInRow0))
                        {
#if UNITY_EDITOR
                            Debug.LogWarning("(#if UNITY_EDITOR) Unknown language : " + languageInRow0);
#endif
                            continue;
                        }

                        // ----------------

                        systemLanguage = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), languageInRow0);

                        if (!this.m_languageDict.ContainsKey(systemLanguage))
                        {
                            this.m_languageDict.Add(systemLanguage, new Dictionary<string, string>());
                        }

                        // -------------------

                        if (!string.IsNullOrEmpty(keyInCol0) && !this.m_languageDict[systemLanguage].ContainsKey(keyInCol0))
                        {
                            this.m_languageDict[systemLanguage].Add(keyInCol0, csvRowColList[row][col]);
                        }

#if UNITY_EDITOR

                        else
                        {
                            Debug.LogWarning("(#if UNITY_EDITOR) Found empty or duplicated key : " + keyInCol0);
                        }

#endif


                    }

                }

            }

            // sanity check
            {

#if UNITY_EDITOR
                this.sanityCheck();
#endif
            }

        }

#if UNITY_EDITOR

        /// <summary>
        /// Sanity check
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void sanityCheck()
        {

            if(this.m_languageDict.ContainsKey(this.m_currentSystemLanguage))
            {

                List<string> sscLanguageKeys = new List<string>(Enum.GetNames(typeof(SSCLanguageKeys)));
                List<string> keys = new List<string>(this.m_languageDict[this.m_currentSystemLanguage].Keys);

                foreach (var key in sscLanguageKeys)
                {
                    
                    if(!keys.Contains(key))
                    {
                        Debug.LogWarning("(#if UNITY_EDITOR) Not found key in csv : " + key);
                    }

                }

            }

            foreach (var val in this.m_languageDict)
            {
                if (!this.isSupportedLanguageEditorOnly(val.Key))
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) Csv contains not supported language : " + val.Key);
                }
            }

            foreach (var val in this.m_supportedLanguagesEditorOnly)
            {
                if(!this.m_languageDict.ContainsKey(val))
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) Csv doesn't contain : " + val);
                }
            }

        }

#endif

        /// <summary>
        /// Get formatted string by current language
        /// </summary>
        /// <param name="keyInCsv">Enum</param>
        /// <param name="args">args</param>
        /// <returns>formatted string</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getFormattedString(Enum keyInCsv, params string[] args)
        {
            return this.getFormattedString(keyInCsv, this.m_currentSystemLanguage, args);
        }

        /// <summary>
        /// Get formatted string by current language
        /// </summary>
        /// <param name="keyInCsv">Enum</param>
        /// <param name="systemLanguage">SystemLanguage</param>
        /// <param name="args">args</param>
        /// <returns>formatted string</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getFormattedString(Enum keyInCsv, SystemLanguage systemLanguage, params string[] args)
        {
            return this.getFormattedString(Enum.GetName(keyInCsv.GetType(), keyInCsv), systemLanguage, args);
        }

        /// <summary>
        /// Get formatted string by current language
        /// </summary>
        /// <param name="keyInCsv">Enum</param>
        /// <param name="args">args</param>
        /// <returns>formatted string</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getFormattedString(string keyInCsv, params string[] args)
        {
            return this.getFormattedString(keyInCsv, this.m_currentSystemLanguage, args);
        }

        /// <summary>
        /// Get formatted string by current language
        /// </summary>
        /// <param name="keyInCsv">Enum</param>
        /// <param name="systemLanguage">SystemLanguage</param>
        /// <param name="args">args</param>
        /// <returns>formatted string</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getFormattedString(string keyInCsv, SystemLanguage systemLanguage, params string[] args)
        {

            string ret = "";

            try
            {

                ret = string.Format(
                    this.getStringFormat(keyInCsv, systemLanguage),
                    args
                    );

            }

#if UNITY_EDITOR

            catch (Exception e)
            {
                Debug.LogError(e.Message + " : " + keyInCsv);
            }
#else

            catch
            {
            
            }

#endif
            return ret;

        }

        /// <summary>
        /// Get string format by current language
        /// </summary>
        /// <param name="keyInCsv">Enum</param>
        /// <returns>string format</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getStringFormat(Enum keyInCsv)
        {
            return this.getStringFormat(Enum.GetName(keyInCsv.GetType(), keyInCsv), this.m_currentSystemLanguage);
        }

        /// <summary>
        /// Get string format by current language
        /// </summary>
        /// <param name="keyInCsv">Enum</param>
        /// <param name="systemLanguage">SystemLanguage</param>
        /// <returns>string format</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getStringFormat(Enum keyInCsv, SystemLanguage systemLanguage)
        {
            return this.getStringFormat(Enum.GetName(keyInCsv.GetType(), keyInCsv), systemLanguage);
        }

        /// <summary>
        /// Get string format by current language
        /// </summary>
        /// <param name="keyInCsv"></param>
        /// <returns>string format</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getStringFormat(string keyInCsv)
        {
            return this.getStringFormat(keyInCsv, this.m_currentSystemLanguage);
        }

        /// <summary>
        /// Get string format
        /// </summary>
        /// <param name="keyInCsv">key in csv</param>
        /// <param name="systemLanguage">SystemLanguage</param>
        /// <returns>string format</returns>
        // -------------------------------------------------------------------------------------------------------
        public string getStringFormat(string keyInCsv, SystemLanguage systemLanguage)
        {

            if (this.m_languageDict.ContainsKey(systemLanguage))
            {

                if (this.m_languageDict[systemLanguage].ContainsKey(keyInCsv))
                {
                    return this.m_languageDict[systemLanguage][keyInCsv];
                }

#if UNITY_EDITOR

                else
                {
                    Debug.LogWarning("(#if UNITY_EDITOR) Not found key : " + keyInCsv);
                }

#endif

            }

#if UNITY_EDITOR

            else
            {
                Debug.LogWarning("(#if UNITY_EDITOR) Not found language : " + systemLanguage);
            }

#endif

            return "";

        }

        /// <summary>
        ///  Set current SystemLanguage
        /// </summary>
        /// <param name="lang">SystemLanguage</param>
        // -------------------------------------------------------------------------------------------------------
        public void setCurrentSystemLanguage(SystemLanguage lang)
        {

            // m_currentSystemLanguage
            {
                this.m_currentSystemLanguage = lang;
            }

            // m_currentFont
            {

                var temp = this.m_languageAndFontList.Find(val => val.systemLanguage == lang);

                if(temp != null)
                {
                    this.m_currentFont = temp.font;
                }

            }

            SimpleReduxManager.Instance.LanguageSignalWatcher.state().setState();

        }

    }

}
