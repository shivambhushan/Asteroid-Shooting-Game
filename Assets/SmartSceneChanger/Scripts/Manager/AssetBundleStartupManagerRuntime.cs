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
        /// Queue for loading AssetBundle in runtime
        /// </summary>
        protected Queue<AbStartupContentsGroup> m_runtimeQueue = new Queue<AbStartupContentsGroup>();

        /// <summary>
        /// IEnumerator for runtime loading
        /// </summary>
        protected IEnumerator m_runtimeLoading = null;

        /// <summary>
        /// Add startup
        /// </summary>
        /// <param name="nameDotVariant">nameDotVariant</param>
        /// <param name="abs">AbStartupContents</param>
        // -------------------------------------------------------------------------------------------------------
        protected void addNewRuntimeAbStartupContents(string nameDotVariant, AbStartupContents abs)
        {

            if(SimpleReduxManager.Instance.SceneChangeStateWatcher.state().stateEnum != SceneChangeState.StateEnum.ScenePlaying)
            {
                return;
            }

            // -----------------

            // Enqueue
            {
                
                AbStartupContentsGroup group = new AbStartupContentsGroup(nameDotVariant);

                group.absList.Add(abs);

                this.m_runtimeQueue.Enqueue(group);

            }

            // StartCoroutine
            {
                if (this.m_runtimeLoading == null)
                {
                    StartCoroutine(this.m_runtimeLoading = this.loadAssetBundleInRuntimeIE());
                }
            }

        }

        /// <summary>
        /// Add AbStartupContents to runtime queue
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successAction">successAction</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntime(
            string assetBundleName,
            string variant,
            Action<AssetBundle> successAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction
            )
        {

            this.addNewRuntimeAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContents(successAction, failedAction, progressAction)
            );

        }

        /// <summary>
        /// Add AbStartupContents ro list
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successDetailAction">successDetailAction</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntime(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object> successDetailAction,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {
 
            this.addNewRuntimeAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContents(successDetailAction, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Add AbStartupContents ro list
        /// </summary>
        /// <param name="assetBundleName">assetBundleName</param>
        /// <param name="variant">variant</param>
        /// <param name="successDetailActionForAsync">successDetailActionForAsync</param>
        /// <param name="failedAction">failedAction</param>
        /// <param name="progressAction">progressAction</param>
        /// <param name="identifierForDetail">identifierForDetail</param>
        // -------------------------------------------------------------------------------------------------------
        public void loadAssetBundleInRuntime(
            string assetBundleName,
            string variant,
            Action<AssetBundle, System.Object, Action> successDetailActionForAsync,
            Action<WWW> failedAction,
            Action<WWW> progressAction,
            System.Object identifierForDetail
            )
        {

            this.addNewRuntimeAbStartupContents(
                this.createNameDotVariantString(assetBundleName, variant),
                new AbStartupContents(successDetailActionForAsync, failedAction, progressAction, identifierForDetail)
            );

        }

        /// <summary>
        /// Retry runtime
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void retryRuntime()
        {

            // m_runtimeLoading
            {

                if (this.m_runtimeLoading != null)
                {
                    StopCoroutine(this.m_runtimeLoading);
                }

                StartCoroutine(this.m_runtimeLoading = this.loadAssetBundleInRuntimeIE());

            }

        }

        /// <summary>
        /// Back to title because of error
        /// </summary>
        // -------------------------------------------------------------------------------------------------------
        protected void backToTileBecauseOfRuntimeError()
        {

            // removeLockFromBefore
            {
                SceneChangeManager.Instance.removeLockFromBefore(this);
            }

            // m_runtimeLoading
            {

                if(this.m_runtimeLoading != null)
                {
                    StopCoroutine(this.m_runtimeLoading);
                }

                this.m_runtimeLoading = null;

            }

            // backToTitleSceneWithOkDialog
            {
                SceneChangeManager.Instance.backToTitleSceneWithOkDialog();
            }
                              
        }

        /// <summary>
        /// Load AssetBundle in runtime
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator loadAssetBundleInRuntimeIE()
        {

            // addLockToBefore
            {
                SceneChangeManager.Instance.addLockToBefore(this);
            }

            // wait 1 frame
            {
                yield return null;
            }

            // setManifestFileAndFolderUrl
            {
                this.setManifestFileAndFolderUrl();
            }

            // loop
            {

                AbStartupContentsGroup group = null;

                while (this.m_runtimeQueue.Count > 0)
                {

                    // Peek
                    {
                        group = this.m_runtimeQueue.Peek();
                    }

                    // clearContents
                    {
                        this.clearContents(false);
                    }

                    // downloadManifest
                    {

                        if (!this.m_manifestInfo.manifest)
                        {
                            yield return this.downloadManifest();
                        }

                        if (this.hasError())
                        {
                            break;
                        }

                    }

                    // addAllDependencies
                    {
                        yield return this.addAllDependencies(group.nameDotVariant);
                    }

                    // m_dependencies
                    {

                        // startAbStartupSub
                        {
                            foreach(var depend in this.m_dependencies)
                            {
                                yield return this.loadAbStartupContents(depend.Value);
                            }
                        }

                        if (this.hasError())
                        {
                            break;
                        }

                    }

                    // group
                    {
                        
                        // startAbStartupSub
                        {
                            yield return this.loadAbStartupContents(group);
                            group.unloadAssetBundle(false);
                        }

                        if (this.hasError())
                        {
                            break;
                        }

                    }

                    // Dequeue
                    {
                        if(!this.hasError())
                        {
                            this.m_runtimeQueue.Dequeue();
                        }
                    }

                } // while (this.m_runtimeQueue.Count > 0)

            }

            // finish
            {
                
                if(this.hasError())
                {
                    DialogManager.Instance.showYesNoDialog(
                        this.createErrorMessage(),
                        this.retryRuntime,
                        this.backToTileBecauseOfRuntimeError
                    );
                }

                else
                {

                    // removeLockFromBefore
                    {
                        SceneChangeManager.Instance.removeLockFromBefore(this);
                    }

                    // m_runtimeLoading
                    {
                        this.m_runtimeLoading = null;
                    }

                }

            }

        }

    }

}
