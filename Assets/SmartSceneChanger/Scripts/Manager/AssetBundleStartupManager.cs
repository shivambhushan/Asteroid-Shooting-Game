#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SSC
{

    /// <summary>
    /// Class for AssetBundle startup
    /// </summary>
    public partial class AssetBundleStartupManager : SingletonMonoBehaviour<AssetBundleStartupManager>
    {


        /// <summary>
        /// Class for AssetBundle startup
        /// </summary>
        protected class AbStartupContents : StartupContents
        {

            /// <summary>
            /// Success action 
            /// </summary>
            public Action<AssetBundle> successAction = null;

            /// <summary>
            /// Success detail action 
            /// </summary>
            public Action<AssetBundle, System.Object> successDetailAction = null;

            /// <summary>
            /// Success detail action for asunc
            /// </summary>
            public Action<AssetBundle, System.Object, Action> successDetailActionForAsync = null;

            /// <summary>
            /// Failed action
            /// </summary>
            public Action<WWW> failedAction = null;

            /// <summary>
            /// Progress action
            /// </summary>
            public Action<WWW> progressAction = null;

            /// <summary>
            /// Identifier for detail
            /// </summary>
            public System.Object identifierForDetail = null;

            /// <summary>
            /// Progress value function
            /// </summary>
            public Func<float> progressValueFunc = null;

            /// <summary>
            /// Constructor
            /// </summary>
            public AbStartupContents()
            {

            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successAction">successAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            public AbStartupContents(
                Action<AssetBundle> _successAction,
                Action<WWW> _failedAction,
                Action<WWW> _progressAction
                )
            {
                this.successAction = _successAction;
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successDetailAction">successDetailAction</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            /// <param name="_identifierForDetail">identifierForDetail</param>
            public AbStartupContents(
                Action<AssetBundle, System.Object> _successDetailAction,
                Action<WWW> _failedAction,
                Action<WWW> _progressAction,
                System.Object _identifierForDetail
                )
            {
                this.successDetailAction = _successDetailAction;
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
                this.identifierForDetail = _identifierForDetail;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_successDetailActionForAsync">successDetailActionForAsync</param>
            /// <param name="_failedAction">failedAction</param>
            /// <param name="_progressAction">progressAction</param>
            /// <param name="_identifierForDetail">identifierForDetail</param>
            public AbStartupContents(
                Action<AssetBundle, System.Object, Action> _successDetailActionForAsync,
                Action<WWW> _failedAction,
                Action<WWW> _progressAction,
                System.Object _identifierForDetail
                )
            {
                this.successDetailActionForAsync = _successDetailActionForAsync;
                this.failedAction = _failedAction;
                this.progressAction = _progressAction;
                this.identifierForDetail = _identifierForDetail;
            }

        }


        /// <summary>
        /// Class for AssetBundle startup
        /// </summary>
        protected class AbStartupContentsGroup
        {

            /// <summary>
            /// AssetBundle name
            /// </summary>
            public string nameDotVariant = "";

            /// <summary>
            /// AssetBundle
            /// </summary>
            public AssetBundle assetBundle = null;

            /// <summary>
            /// AbStartupContents list
            /// </summary>
            public List<AbStartupContents> absList = new List<AbStartupContents>();

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="_nameDotVariant">nameDotVariant</param>
            public AbStartupContentsGroup(string _nameDotVariant)
            {
                this.nameDotVariant = _nameDotVariant;
            }

            /// <summary>
            /// Unload AssetBundle
            /// </summary>
            /// <param name="unloadAllLoadedObjects">unloadAllLoadedObjects</param>
            public void unloadAssetBundle(bool unloadAllLoadedObjects)
            {

                if (this.assetBundle)
                {
                    this.assetBundle.Unload(unloadAllLoadedObjects);
                }

                this.assetBundle = null;

            }

        }

        /// <summary>
        /// AbStartupContents dictionary
        /// </summary>
        protected Dictionary<string, AbStartupContentsGroup> m_absList = new Dictionary<string, AbStartupContentsGroup>();

        /// <summary>
        /// New detected AbStartupContents dictionary
        /// </summary>
        protected Dictionary<string, AbStartupContentsGroup> m_newDetected = new Dictionary<string, AbStartupContentsGroup>();

        /// <summary>
        ///Dependencies AbStartupContents dictionary
        /// </summary>
        protected Dictionary<string, AbStartupContentsGroup> m_dependencies = new Dictionary<string, AbStartupContentsGroup>();

        /// <summary>
        /// The number of parallel loading coroutines
        /// </summary>
        [SerializeField]
        [Tooltip("The number of parallel loading coroutines")]
        protected int m_numberOfCo = 4;

        /// <summary>
        /// Ignore error except manifest
        /// </summary>
        [SerializeField]
        [Tooltip("Ignore error except manifest")]
        protected bool m_ignoreErrorExceptManifest = false;

        /// <summary>
        /// After loading done, redownload manifest and compare new and old.
        /// if deference detected, reload unity scene
        /// </summary>
        [SerializeField]
        [Tooltip("After loading done, redownload a manifest and compare new one with old one, if some differences detected, reload unity scene silently")]
        protected bool m_checkManifestAfterLoading = false;

        /// <summary>
        /// Use decryption
        /// </summary>
        [SerializeField]
        [Tooltip("Use decryption. If you changed this, don't forget to clear cache.")]
        protected bool m_useDecryption = false;

        /// <summary>
        /// Crypto version when m_useDecryption is true
        /// </summary>
        [SerializeField]
        [Tooltip("Crypto version when m_useDecryption is true")]
        protected CryptoVersion m_cryptoVersion = CryptoVersion.Ver1_depricated;

        /// <summary>
        /// ThreadPriority
        /// </summary>
        [SerializeField]
        [Tooltip("ThreadPriority")]
        protected UnityEngine.ThreadPriority m_threadPriority = UnityEngine.ThreadPriority.Low;

        /// <summary>
        /// Error seconds for timeout
        /// </summary>
        [SerializeField]
        [Tooltip("Error seconds for timeout")]
        protected float m_noProgressTimeOutSeconds = 0.0f;

        /// <summary>
        /// AbStartupContents list for runtime
        /// </summary>
        protected List<AbStartupContents> m_absListRuntime = new List<AbStartupContents>();

        /// <summary>
        /// DialogMessages
        /// </summary>
        protected DialogMessages m_messages = new DialogMessages();

        /// <summary>
        /// Current error
        /// </summary>
        protected StartupContents m_currentError = null;

        /// <summary>
        /// Additive scene progress
        /// </summary>
        protected Dictionary<int, float> m_additiveSceneProgressDict = new Dictionary<int, float>();

        /// <summary>
        /// WaitForSeconds for loading
        /// </summary>
        WaitForSeconds m_defaultWaitForSeconds = new WaitForSeconds(0.5f);

        /// <summary>
        /// override
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

            this.m_numberOfCo = Math.Max(1, this.m_numberOfCo);

#if UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)

            if(!SystemInfo.graphicsDeviceType.ToString().ToLower().Contains("opengl"))
            {
                Debug.LogWarning("(#if UNITY_EDITOR) Use OpenGLES, or you will see pink shader, perhaps.");
            }

#endif

#if UNITY_EDITOR

            if (this.m_useDecryption)
            {

                if (this.m_cryptoVersion == CryptoVersion.Ver1_depricated)
                {
                    Debug.LogWarning(
                        "(#if UNITY_EDITOR) : CryptoVersion.Ver1_depricated uses old crypto function, please use newer instead : " +
                        Funcs.CreateHierarchyPath(this.transform));
                }

                else
                {
                    Debug.Log("(#if UNITY_EDITOR) : AssetBundleStartupManager's CryptoVersion == " + this.m_cryptoVersion.ToString());
                }

            }

#endif

        }

        /// <summary>
        /// Add startup data
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundle(
            string assetBundleName,
            string variant,
            Action<AssetBundle> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction
        )
        {

            this.addNewDetectedAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContents(successAction, failedAction, progressAction)
            );

        }

        /// <summary>
        /// Add startup data
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundle(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewDetectedAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContents(successAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add startup data
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">success function</param>
        /// <param name="failedAction">failed function</param>
        /// <param name="progressAction">progress function</param>
        // -------------------------------------------------------------------------------------------------------
        public void addSceneStartupAssetBundle(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object, Action> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewDetectedAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContents(successAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Create name.variant string
        /// </summary>
        /// <returns>name.variant</returns>
        /// <param name="assetBundleName">AssetBundle name</param>
        /// <param name="variant">variant</param>
        // -------------------------------------------------------------------------------------------------------
        protected string createNameDotVariantString(string assetBundleName, string variant)
        {
            return string.IsNullOrEmpty(variant) ? assetBundleName : assetBundleName + "." + variant;
        }

        /// <summary>
        /// Add startup
        /// </summary>
        /// <param name="nameDotVariant">nameDotVariant</param>
        /// <param name="abs">AbStartupContents</param>
        // -------------------------------------------------------------------------------------------------------
        protected void addNewDetectedAbStartupContents(string nameDotVariant, AbStartupContents abs)
        {

            if (SimpleReduxManager.Instance.SceneChangeStateWatcher.state().stateEnum == SceneChangeState.StateEnum.NowLoadingMain)
            {

                if (!this.m_newDetected.ContainsKey(nameDotVariant))
                {
                    this.m_newDetected.Add(nameDotVariant, new AbStartupContentsGroup(nameDotVariant));
                }

                this.m_newDetected[nameDotVariant].absList.Add(abs);

            }

            else
            {
                this.addNewRuntimeAbStartupContents(nameDotVariant, abs);
            }

        }

        /// <summary>
        /// Message for timeout
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageTimeout()
        {
            return "Connection Timeout";
        }

        /// <summary>
        /// Message for failed to get manifest
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageFailedToGetAssetBundleManifest()
        {
            return "Failed to get AssetBundleManifest";
        }

        /// <summary>
        /// Message for failed to decrypt
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageFailedToDecryptAssetBundle()
        {
            return "Failed to load AssetBundle";
        }

        /// <summary>
        /// Message for AssetBundle not found
        /// </summary>
        /// <returns>message</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string messageAssetBundleNotFound()
        {
            return "AssetBundle not found";
        }

        /// <summary>
        /// Denominator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public int progressDenominator()
        {

            int ret = 0;

            foreach (var group in this.m_absList)
            {
                ret += group.Value.absList.Count;
            }

            ret += this.m_dependencies.Count;

            ret += this.m_additiveSceneProgressDict.Count;

            return ret;

        }

        /// <summary>
        /// Numerator of progress
        /// </summary>
        /// <returns>value</returns>
        // -------------------------------------------------------------------------------------------------------
        public float progressNumerator()
        {

            float ret = 0.0f;

            foreach (var group in this.m_absList)
            {

                foreach (var abs in group.Value.absList)
                {

                    if (abs.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                    {
                        ret += 1.0f;
                    }

                    else if (abs.progressValueFunc != null)
                    {
                        ret += abs.progressValueFunc();
                    }

                }

            }

            foreach (var group in this.m_dependencies)
            {

                foreach (var abs in group.Value.absList)
                {

                    if (abs.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                    {
                        ret += 1.0f;
                    }

                    else if (abs.progressValueFunc != null)
                    {
                        ret += abs.progressValueFunc();
                    }

                }

            }

            foreach (var val in this.m_additiveSceneProgressDict)
            {
                ret += val.Value;
            }

            return ret;

        }

        /// <summary>
        /// Has NotYet content
        /// </summary>
        /// <returns>detected</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool hasNotYetContent()
        {

            if (this.m_newDetected.Count > 0)
            {
                return true;
            }

            foreach (var val in this.m_absList)
            {
                if (val.Value.absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NotYet) != null)
                {
                    return true;
                }
            }

            return false;

        }

        /// <summary>
        /// Create error message for dialog
        /// </summary>
        /// <returns>error message object</returns>
        // -------------------------------------------------------------------------------------------------------
        public virtual System.Object createErrorMessage()
        {

            this.m_messages.clear();

            this.m_messages.category = DialogMessages.MessageCategory.Error;

            string errorMessage = (this.m_currentError != null) ? this.m_currentError.errorMessage : "Unknown Error";
            string errorUrl = (this.m_currentError != null) ? this.m_currentError.urlIfNeeded : "Unknown Error";

            if (LanguageManager.isAvailable())
            {

                LanguageManager lm = LanguageManager.Instance;

                this.m_messages.title = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Title_Error);
                this.m_messages.mainMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Error_AssetBundle_Startup, errorMessage);
                this.m_messages.subMessage = lm.getFormattedString(LanguageManager.SSCLanguageKeys.Dialog_Sub_Retry);
                this.m_messages.urlIfNeeded = errorUrl;

            }

            else
            {
                this.m_messages.title = "AssetBundle Error";
                this.m_messages.mainMessage = errorMessage;
                this.m_messages.subMessage = "Retry ?";
                this.m_messages.urlIfNeeded = errorUrl;
            }

            return this.m_messages;

        }

        /// <summary>
        /// Clear contents
        /// </summary>
        /// <param name="unloadManifest">unload manifest</param>
        // -------------------------------------------------------------------------------------------------------
        public void clearContents(bool unloadManifest)
        {

            // m_absList
            {

                foreach (var val in this.m_absList)
                {
                    val.Value.unloadAssetBundle(false);
                }

                this.m_absList.Clear();

            }

            // m_dependencies
            {

                foreach (var val in this.m_dependencies)
                {
                    val.Value.unloadAssetBundle(false);
                }

                this.m_dependencies.Clear();

            }

            // m_runtimeQueue
            {

                //foreach (var val in this.m_runtimeQueue)
                //{
                //    val.unloadAssetBundle(false);
                //}

                //this.m_runtimeQueue.Clear();

            }

            // m_manifestInfo
            {
                this.m_manifestInfo.clear(unloadManifest);
            }

            // m_additiveSceneProgressDict
            {
                this.m_additiveSceneProgressDict.Clear();
            }

            // clearErrorForRestart
            {
                this.clearErrorForRestart();
            }

        }

        /// <summary>
        /// Merge dictionaries
        /// </summary>
        /// <param name="into">into</param>
        /// <param name="merge">maege this into</param>
        // -------------------------------------------------------------------------------------------------------
        protected void mergeDictionaries(Dictionary<string, AbStartupContentsGroup> into, Dictionary<string, AbStartupContentsGroup> merge)
        {

            foreach (var mergeKv in merge)
            {

                if (into.ContainsKey(mergeKv.Key))
                {
                    into[mergeKv.Key].absList.AddRange(mergeKv.Value.absList);
                }

                else
                {
                    into.Add(mergeKv.Key, mergeKv.Value);
                }

            }

        }

        /// <summary>
        /// Check if need to reload scene
        /// </summary>
        /// <returns>yes</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool checkIfNeedToReloadScene()
        {

            if (!this.m_checkManifestAfterLoading)
            {
                return false;
            }

            // ------------------

            return this.m_manifestInfo.hasDifferenceBetweenNewAndOld();

        }

        /// <summary>
        /// Check if new manifest detected
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public IEnumerator checkNewManifestIfNeeded()
        {

            if (!this.m_checkManifestAfterLoading)
            {
                yield break;
            }

            // ---------------

            // m_oldManifestKeyHashSet
            {
                this.setManifestKeyHashSet(this.m_manifestInfo.oldManifestKeyHashSet);
            }

            // clear
            {
                this.m_manifestInfo.clear(true);
            }

            // downloadManifest
            {
                yield return this.downloadManifest();
            }

            // newManifestKeyHashSet
            {
                this.setManifestKeyHashSet(this.m_manifestInfo.newManifestKeyHashSet);
            }

        }

        /// <summary>
        /// Start AB loadings
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        public IEnumerator startAbStartup()
        {

            if (!this.hasNotYetContent())
            {
                yield break;
            }

            // ----------------

            // clearErrorForRestart
            {
                this.clearErrorForRestart();
            }

            // mergeDictionaries
            {
                this.mergeDictionaries(this.m_absList, this.m_newDetected);
                this.m_newDetected.Clear();
            }

            // setManifestFileAndFolderUrl
            {
                this.setManifestFileAndFolderUrl();
            }

            // downloadManifest
            {

                if (!this.m_manifestInfo.manifest)
                {
                    yield return this.downloadManifest();
                }

                if (this.hasError())
                {
                    yield break;
                }

            }

            // addAllDependencies
            {
                yield return this.addAllDependencies();
            }

            // -----------------

            // m_dependencies
            {

                for (int i = 0; i < this.m_numberOfCo; i++)
                {
                    StartCoroutine(this.startAbStartupSub(this.m_dependencies));
                }

                // wait 1 frame
                {
                    yield return null;
                }

                // wait coroutines
                {

                    foreach (var key in this.m_dependencies.Keys)
                    {
                        while (this.m_dependencies[key].absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NowWorking) != null)
                        {
                            yield return this.m_defaultWaitForSeconds;
                        }
                    }

                }

                if (this.hasError())
                {
                    yield break;
                }

            }

            // m_absList
            {

                for (int i = 0; i < this.m_numberOfCo; i++)
                {
                    StartCoroutine(this.startAbStartupSub(this.m_absList));
                }

                // wait 1 frame
                {
                    yield return null;
                }

                // wait coroutines
                {

                    foreach (var key in this.m_absList.Keys)
                    {
                        while (this.m_absList[key].absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NowWorking) != null)
                        {
                            yield return this.m_defaultWaitForSeconds;
                        }
                    }

                }

                if (this.hasError())
                {
                    yield break;
                }

            }

        }

        /// <summary>
        /// Create AssetBundle url
        /// </summary>
        /// <param name="nameDotVariant">AssetBundle name</param>
        /// <returns>url</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual string createAssetBundleUrl(string nameDotVariant)
        {
            return this.m_manifestInfo.manifestFolderUrl + nameDotVariant;
        }

        /// <summary>
        /// Decrypt binary data
        /// </summary>
        /// <param name="textAsset">binary data</param>
        /// <returns>decrypted binary data</returns>
        // -------------------------------------------------------------------------------------------------------
        protected virtual byte[] decryptBinaryData(TextAsset textAsset)
        {

#if UNITY_EDITOR

            Debug.LogWarning("(#if UNITY_EDITOR) : You must override [decryptBinaryData] function.");

#endif

            if (!textAsset)
            {
                return new byte[] { };
            }

            if (this.m_cryptoVersion == CryptoVersion.Ver1_depricated)
            {
                return Funcs.DecryptBinaryData(textAsset.bytes, "PassworDPassworD");
            }

            else if (this.m_cryptoVersion == CryptoVersion.Ver2)
            {
                return Funcs.DecryptBinaryData2(textAsset.bytes, "PassworDPassworD_123");
            }

#if UNITY_EDITOR
            Debug.LogError("Developer Implementation Error in AssetBundleStartupManager.decryptBinaryData");
#endif

            return Funcs.DecryptBinaryData(textAsset.bytes, "PassworDPassworD");

        }

        /// <summary>
        /// Decrypt AssetBundle if needed
        /// </summary>
        /// <param name="assetBundle">AssetBundle</param>
        /// <param name="ret">return function</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator decryptAssetBundleIfNeeded(AssetBundle assetBundle, Action<AssetBundle> ret)
        {

            yield return null;

            if (!assetBundle)
            {
                yield break;
            }

            // ------------------

            if (this.m_useDecryption)
            {

                string[] assetNames = assetBundle.GetAllAssetNames();
                string assetName = (assetNames.Length > 0) ? assetNames[0] : "";

                if (string.IsNullOrEmpty(assetName))
                {
                    yield break;
                }

                byte[] decrypted = this.decryptBinaryData(assetBundle.LoadAsset<TextAsset>(assetName));

                yield return null;

                if (decrypted != null && decrypted.Length > 0)
                {

                    AssetBundleCreateRequest abcr = AssetBundle.LoadFromMemoryAsync(decrypted);

                    if (abcr != null)
                    {
                        yield return abcr;
                        ret(abcr.assetBundle);
                        assetBundle.Unload(true);
                    }

                }

                else
                {
                    assetBundle.Unload(true);
                }

            }

            else
            {
                ret(assetBundle);
            }

        }

        /// <summary>
        /// Start AB loadings sub
        /// </summary>
        /// <param name="groupDict">group dictionary</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator startAbStartupSub(Dictionary<string, AbStartupContentsGroup> groupDict)
        {

            foreach (var key in groupDict.Keys)
            {

                // hasError break
                {
                    if (this.hasError())
                    {
                        break;
                    }
                }

                // ----------------------

                // continue if not NotYet
                {

                    if (groupDict[key].absList.Find(x => x.currentWorkingState == StartupContents.WorkingState.NotYet) == null)
                    {
                        continue;
                    }

                }

                // NowWorking
                {
                    foreach (var abs in groupDict[key].absList)
                    {
                        if (abs.currentWorkingState == StartupContents.WorkingState.NotYet)
                        {
                            abs.currentWorkingState = StartupContents.WorkingState.NowWorking;
                        }
                    }
                }

                // loadAbStartupContents
                {
                    yield return this.loadAbStartupContents(groupDict[key]);
                }

                // DoneSuccessOrError
                {
                    foreach (var abs in groupDict[key].absList)
                    {
                        abs.currentWorkingState = StartupContents.WorkingState.DoneSuccessOrError;
                    }
                }

            }

        }

        /// <summary>
        /// Load additive scene
        /// </summary>
        /// <param name="assetBundle">AssetBundle</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadAdditiveSceneIfNeeded(AssetBundle assetBundle)
        {

            if (!assetBundle)
            {
                yield break;
            }

            // ----------------

            int progressId = 0;

            foreach (string str in assetBundle.GetAllScenePaths())
            {

                var aoForAdditive = SceneManager.LoadSceneAsync(Path.GetFileNameWithoutExtension(str), LoadSceneMode.Additive);

                progressId = this.m_additiveSceneProgressDict.Count;

                if (!this.m_additiveSceneProgressDict.ContainsKey(progressId))
                {
                    this.m_additiveSceneProgressDict.Add(progressId, 0.0f);
                }

                if (aoForAdditive != null)
                {

                    while (!aoForAdditive.isDone)
                    {
                        this.m_additiveSceneProgressDict[progressId] = aoForAdditive.progress;
                        yield return null;
                    }

                    this.m_additiveSceneProgressDict[progressId] = 1.0f;

                }

            }

        }

        /// <summary>
        /// Load AbStartupContentsGroup
        /// </summary>
        /// <param name="absGroup">AbStartupContentsGroup</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadAbStartupContents(AbStartupContentsGroup absGroup)
        {

            if (absGroup == null)
            {
                yield break;
            }

            if (absGroup.absList.Count <= 0)
            {
#if UNITY_EDITOR

                Debug.LogError("(#if UNITY_EDITOR) : Implementation Error in loadAbStartupContents");
#endif
                yield break;
            }

            // ---------------------

            // Caching
            {
                while (!Caching.ready)
                {
                    yield return null;
                }
            }

            // ---------------------

            if (absGroup.assetBundle)
            {

                foreach (var abs in absGroup.absList)
                {

                    if (abs.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                    {
                        continue;
                    }

                    // --------------

                    // TODO?
                    yield return this.loadAdditiveSceneIfNeeded(absGroup.assetBundle);

                    if (abs.successAction != null)
                    {
                        abs.successAction(absGroup.assetBundle);
                    }

                }

            }

            else if (
                this.m_dependencies.ContainsKey(absGroup.nameDotVariant) &&
                this.m_dependencies[absGroup.nameDotVariant].assetBundle
                )
            {

                absGroup.assetBundle = this.m_dependencies[absGroup.nameDotVariant].assetBundle;

                foreach (var abs in absGroup.absList)
                {

                    if (abs.currentWorkingState == StartupContents.WorkingState.DoneSuccessOrError)
                    {
                        continue;
                    }

                    // --------------

                    // TODO?
                    yield return this.loadAdditiveSceneIfNeeded(absGroup.assetBundle);

                    if (abs.successAction != null)
                    {
                        abs.successAction(absGroup.assetBundle);
                    }

                }

            }

            else
            {

                float noProgressTimer = 0.0f;
                float previousProgress = 0.0f;

                using (WWW www =
                    WWW.LoadFromCacheOrDownload(
                        this.createAssetBundleUrl(absGroup.nameDotVariant),
                        this.m_manifestInfo.manifest.GetAssetBundleHash(absGroup.nameDotVariant)
                        ))
                {

#if !UNITY_WEBGL
                    www.threadPriority = this.m_threadPriority;
#endif

                    // set progressValueFunc
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.progressValueFunc = () =>
                            {
                                return (www != null) ? www.progress : 0.0f;
                            };
                        }
                    }

                    // set urlIfNeeded
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.urlIfNeeded = www.url;
                        }
                    }

                    // wait www done
                    {

                        while (!www.isDone)
                        {

                            foreach (var abs in absGroup.absList)
                            {
                                if (abs.progressAction != null)
                                {
                                    abs.progressAction(www);
                                }
                            }
                            
                            // timeout
                            {

                                if (this.m_noProgressTimeOutSeconds > 0.0f)
                                {

                                    if (Mathf.Approximately(previousProgress, www.progress))
                                    {
                                        noProgressTimer += Time.deltaTime;
                                    }

                                    else
                                    {
                                        noProgressTimer = 0.0f;
                                    }

                                    previousProgress = www.progress;

                                    if (noProgressTimer >= this.m_noProgressTimeOutSeconds)
                                    {

                                        foreach (var abs in absGroup.absList)
                                        {
                                            abs.errorMessage = this.messageTimeout();
                                        }

                                        break;

                                    }

                                }

                            }

                            yield return null;

                        } // while (!www.isDone)

                        foreach (var abs in absGroup.absList)
                        {
                            if (abs.progressAction != null)
                            {
                                abs.progressAction(www);
                            }
                        }

                        yield return null;

                    }

                    // success or fail
                    {

                        // set errorMessage
                        {

                            foreach (var abs in absGroup.absList)
                            {
                                if (string.IsNullOrEmpty(abs.errorMessage))
                                {
                                    abs.errorMessage = www.error;
                                }
                            }

                        }

                        // success
                        if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                        {

                            if (www.assetBundle)
                            {

                                yield return this.decryptAssetBundleIfNeeded(www.assetBundle, (ab) =>
                                {
                                    absGroup.assetBundle = ab;
                                });

                                if (absGroup.assetBundle)
                                {

                                    yield return this.loadAdditiveSceneIfNeeded(absGroup.assetBundle);

                                    // success action
                                    {

                                        foreach (var abs in absGroup.absList)
                                        {

                                            if (abs.successAction != null)
                                            {
                                                abs.successAction(absGroup.assetBundle);
                                            }

                                            else if (abs.successDetailAction != null)
                                            {
                                                abs.successDetailAction(absGroup.assetBundle, abs.identifierForDetail);
                                            }

                                            else if (abs.successDetailActionForAsync != null)
                                            {

                                                bool finished = false;

                                                abs.successDetailActionForAsync(absGroup.assetBundle, abs.identifierForDetail, () =>
                                                {
                                                    finished = true;
                                                });

                                                while (!finished)
                                                {
                                                    yield return null;
                                                }

                                            }

                                        }

                                    }

                                    // Don't fo here
                                    {
                                        // absGroup.unloadAssetBundle(false);
                                    }

                                } // if (this.m_manifestInfo.manifestAssetBundle)

                                else
                                {
                                    absGroup.absList[0].errorMessage = this.messageFailedToDecryptAssetBundle();
                                    this.updateError(absGroup.absList[0]);
                                }

                            } // if (www.assetBundle)

                            else
                            {
                                absGroup.absList[0].errorMessage = this.messageAssetBundleNotFound();
                                this.updateError(absGroup.absList[0]);
                            }

                        } // if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))

                        // fail
                        else
                        {

                            foreach (var abs in absGroup.absList)
                            {
                                if (abs.failedAction != null)
                                {
                                    abs.failedAction(www);
                                }
                            }

                            // updateError
                            {
                                this.updateError(absGroup.absList[0]);
                            }

                        }

                    } // success or fail

                    // set progressValueFunc to null
                    {
                        foreach (var abs in absGroup.absList)
                        {
                            abs.progressValueFunc = null;
                        }
                    }

                } // using

            }

        }

        /// <summary>
        /// Clear error for restart
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void clearErrorForRestart()
        {

            // m_currentError
            {
                this.m_currentError = null;
            }

            // m_manifestInfo
            {
                this.m_manifestInfo.clear(false);
            }

            // m_absList
            {

                foreach (var absGroup in this.m_absList)
                {

                    foreach (var abs in absGroup.Value.absList)
                    {

                        if (!string.IsNullOrEmpty(abs.errorMessage))
                        {
                            abs.currentWorkingState = StartupContents.WorkingState.NotYet;
                        }

                        abs.errorMessage = "";

                    }

                }

            }

            // m_dependencies
            {

                foreach (var absGroup in this.m_dependencies)
                {

                    foreach (var abs in absGroup.Value.absList)
                    {

                        if (!string.IsNullOrEmpty(abs.errorMessage))
                        {
                            abs.currentWorkingState = StartupContents.WorkingState.NotYet;
                        }

                        abs.errorMessage = "";

                    }

                }

            }

        }

        /// <summary>
        /// Update error
        /// </summary>
        /// <param name="startupContents">error StartupContents</param>
        // -------------------------------------------------------------------------------------------------------
        protected void updateError(StartupContents startupContents)
        {

            if (startupContents != this.m_manifestInfo.dummyStartup && this.m_ignoreErrorExceptManifest)
            {
                return;
            }

            // ----------

            if (startupContents != null)
            {
                this.m_currentError = startupContents;
            }

        }

        /// <summary>
        /// Has error
        /// </summary>
        /// <returns>has</returns>
        // -------------------------------------------------------------------------------------------------------
        public bool hasError()
        {
            return this.m_currentError != null;
        }

    }

}
