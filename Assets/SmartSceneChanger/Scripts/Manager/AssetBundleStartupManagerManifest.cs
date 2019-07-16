using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Class for AssetBundle startup
    /// </summary>
    public partial class AssetBundleStartupManager : SingletonMonoBehaviour<AssetBundleStartupManager>
    {

        /// <summary>
        /// Manifest info
        /// </summary>
        protected class ManifestInfo
        {

            /// <summary>
            /// Url for manifest file
            /// </summary>
            public string manifestFileUrl = "";

            /// <summary>
            /// Url for base url
            /// </summary>
            public string manifestFolderUrl = "";

            /// <summary>
            /// AssetBundle that has manifest
            /// </summary>
            public AssetBundle manifestAssetBundle = null;

            /// <summary>
            /// Current AssetBundleManifest
            /// </summary>
            public AssetBundleManifest manifest = null;

            /// <summary>
            /// Dummy startup for error
            /// </summary>
            public StartupContents dummyStartup = new StartupContents();

            /// <summary>
            /// New manifest hash dictionary
            /// </summary>
            public Dictionary<string, Hash128> newManifestKeyHashSet = new Dictionary<string, Hash128>();

            /// <summary>
            /// Old manifest hash dictionary
            /// </summary>
            public Dictionary<string, Hash128> oldManifestKeyHashSet = new Dictionary<string, Hash128>();

            /// <summary>
            /// Clear
            /// </summary>
            /// <param name="unloadManifest">unload manifest</param>
            public void clear(bool unloadManifest)
            {

                if (unloadManifest && this.manifestAssetBundle)
                {
                    this.manifestAssetBundle.Unload(true);
                    this.manifestAssetBundle = null;
                    this.manifest = null;
                }

                this.dummyStartup.currentWorkingState = StartupContents.WorkingState.NotYet;
                this.dummyStartup.errorMessage = "";
                this.dummyStartup.urlIfNeeded = "";

            }

            /// <summary>
            /// Has difference
            /// </summary>
            /// <returns>difference detected</returns>
            public bool hasDifferenceBetweenNewAndOld()
            {

                if(this.newManifestKeyHashSet.Count != this.oldManifestKeyHashSet.Count)
                {
                    return false;
                }

                foreach(var newVal in this.newManifestKeyHashSet)
                {
                    
                    if(!this.oldManifestKeyHashSet.ContainsKey(newVal.Key))
                    {
                        return true;
                    }

                    else if(this.oldManifestKeyHashSet[newVal.Key] != newVal.Value)
                    {
                        return true;
                    }

                }

                return false;

            }

        }

        /// <summary>
        /// Manifest info
        /// </summary>
        protected ManifestInfo m_manifestInfo = new ManifestInfo();

        /// <summary>
        /// Set manifest file and base folder url
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void setManifestFileAndFolderUrl()
        {

            this.setManifestFileAndFolderUrl(
                ref this.m_manifestInfo.manifestFolderUrl,
                ref this.m_manifestInfo.manifestFileUrl
                );

        }

        /// <summary>
        /// Set manifest file and base folder url
        /// </summary>
        /// <param name="manifestFolderUrl">manifest folder url</param>
        /// <param name="manifestFileUrl">manifest file url</param>
        // -------------------------------------------------------------------------------------------------------
        protected virtual void setManifestFileAndFolderUrl(ref string manifestFolderUrl, ref string manifestFileUrl)
        {

#if UNITY_EDITOR

            Debug.LogWarning("(#if UNITY_EDITOR) : You must override [setManifestFileAndFolderUrl(ref, ref)] function.");

#endif
            // sample
            {

                string manifestName = "";

#if UNITY_ANDROID
                
                manifestName = (this.m_useDecryption) ? "android.encrypted.unity3d" : "android.unity3d";

#elif UNITY_IOS
                
                manifestName = (this.m_useDecryption) ? "ios.encrypted.unity3d" : "ios.unity3d";
                
#else

                manifestName = (this.m_useDecryption) ? "windows.encrypted.unity3d" : "windows.unity3d";

#endif

                // endsWith slash
                manifestFolderUrl =
                    "http://localhost:50002/" +
                    manifestName +
                    ((this.m_useDecryption) ? "/encrypted/" : "/")
                    ;

                manifestFileUrl = manifestFolderUrl + manifestName;

            }

        }

        /// <summary>
        /// Set manifest hash
        /// </summary>
        /// <param name="target">target</param>
        // -------------------------------------------------------------------------------------------------------
        protected void setManifestKeyHashSet(Dictionary<string, Hash128> target)
        {

            target.Clear();

            if (this.m_manifestInfo.manifest)
            {
                foreach (string str in this.m_manifestInfo.manifest.GetAllAssetBundles())
                {
                    if (!target.ContainsKey(str))
                    {
                        target.Add(str, this.m_manifestInfo.manifest.GetAssetBundleHash(str));
                    }
                }
            }

        }

        /// <summary>
        /// Download manifest
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator downloadManifest()
        {

            if(string.IsNullOrEmpty(this.m_manifestInfo.manifestFileUrl))
            {
#if UNITY_EDITOR
                Debug.LogWarning("(#if UNITY_EDITOR) : manifestFileUrl is empty");
#endif
                yield break;
            }

            // -----------------

            float noProgressTimer = 0.0f;
            float previousProgress = 0.0f;

            // -----------------

            using (WWW www = new WWW(this.m_manifestInfo.manifestFileUrl))
            {

#if !UNITY_WEBGL
                www.threadPriority = this.m_threadPriority;
#endif

                // set dummyStartup
                {
                    this.m_manifestInfo.dummyStartup.currentWorkingState = StartupContents.WorkingState.NowWorking; // (meaningless)
                    this.m_manifestInfo.dummyStartup.urlIfNeeded = www.url;
                }

                // wait www done
                {

                    while (!www.isDone)
                    {

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
                                    this.m_manifestInfo.dummyStartup.errorMessage = this.messageTimeout();
                                    break;
                                }

                            }

                        }

                        yield return null;

                    } // while (!www.isDone)

                    yield return null;

                }

                // success or fail
                {

                    // set errorMessage
                    {
                        if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                        {
                            this.m_manifestInfo.dummyStartup.errorMessage = www.error;
                        }
                    }

                    // success
                    if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))
                    {

                        if (www.assetBundle)
                        {

                            yield return this.decryptAssetBundleIfNeeded(www.assetBundle, (ab) =>
                            {
                                this.m_manifestInfo.manifestAssetBundle = ab;
                            });

                            if (this.m_manifestInfo.manifestAssetBundle)
                            {

                                AssetBundleRequest request =
                                    this.m_manifestInfo.manifestAssetBundle.LoadAssetAsync("AssetBundleManifest", typeof(AssetBundleManifest));

                                if (request != null)
                                {
                                    
                                    yield return request;

                                    this.m_manifestInfo.manifest = request.asset as AssetBundleManifest;

                                    if (!this.m_manifestInfo.manifest)
                                    {
                                        this.m_manifestInfo.dummyStartup.errorMessage = "AssetBundleRequest.asset isn't AssetBundleManifest.";
                                        this.updateError(this.m_manifestInfo.dummyStartup);
                                    }

                                }

                                else
                                {
                                    this.m_manifestInfo.dummyStartup.errorMessage = this.messageFailedToGetAssetBundleManifest();
                                    this.updateError(this.m_manifestInfo.dummyStartup);
                                }

                            } // if (this.m_manifestInfo.manifestAssetBundle)

                            else
                            {
                                this.m_manifestInfo.dummyStartup.errorMessage = this.messageFailedToDecryptAssetBundle();
                                this.updateError(this.m_manifestInfo.dummyStartup);
                            }

                        } // if (www.assetBundle)

                        else
                        {
                            this.m_manifestInfo.dummyStartup.errorMessage = this.messageAssetBundleNotFound();
                            this.updateError(this.m_manifestInfo.dummyStartup);
                        }

                    } // if (string.IsNullOrEmpty(this.m_manifestInfo.dummyStartup.errorMessage))

                    // fail
                    else
                    {
                        this.updateError(this.m_manifestInfo.dummyStartup);
                    }

                }

                // set dummyStartup
                {
                    this.m_manifestInfo.dummyStartup.currentWorkingState = StartupContents.WorkingState.DoneSuccessOrError; // (meaningless)
                }

            } // using (WWW www = new WWW(this.m_manifestInfo.manifestFileUrl))

        }

    }

}
