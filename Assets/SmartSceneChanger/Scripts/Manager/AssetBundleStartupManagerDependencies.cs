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
        /// Add all dependencies
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator addAllDependencies()
        {
            
            if (!this.m_manifestInfo.manifest || this.hasError())
            {
                yield break;
            }

            // ----------------

            // add to m_dependencies
            {

                foreach (var group in this.m_absList)
                {
                    
                    foreach (var dependencyNameDotVariant in this.m_manifestInfo.manifest.GetAllDependencies(group.Value.nameDotVariant))
                    {

                        if(!this.m_dependencies.ContainsKey(dependencyNameDotVariant))
                        {
                            this.m_dependencies.Add(dependencyNameDotVariant, new AbStartupContentsGroup(dependencyNameDotVariant));
                            this.m_dependencies[dependencyNameDotVariant].absList.Add(new AbStartupContents());
                        }

                    }

                }

            }

        }

        /// <summary>
        /// Add all dependencies
        /// </summary>
        /// <returns>IEnumerator</returns>
        /// <param name="nameDotVariant">name.variant</param>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator addAllDependencies(string nameDotVariant)
        {

            if (!this.m_manifestInfo.manifest || this.hasError())
            {
                yield break;
            }

            // ----------------

            // add to m_dependencies
            {

                foreach (var dependencyNameDotVariant in this.m_manifestInfo.manifest.GetAllDependencies(nameDotVariant))
                {

                    if (!this.m_dependencies.ContainsKey(dependencyNameDotVariant))
                    {
                        this.m_dependencies.Add(dependencyNameDotVariant, new AbStartupContentsGroup(dependencyNameDotVariant));
                        this.m_dependencies[dependencyNameDotVariant].absList.Add(new AbStartupContents());
                    }

                }

            }

        }

    }

}
