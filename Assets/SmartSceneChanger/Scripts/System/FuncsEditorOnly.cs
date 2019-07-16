using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using System.Linq;

#endif

namespace SSC
{

#if UNITY_EDITOR

    /// <summary>
    /// Static Functions for EditorOnly
    /// </summary>
    public partial class Funcs
    {

        /// <summary>
        /// Check duplicates
        /// </summary>
        /// <typeparam name="T1">IEnumerable</typeparam>
        /// <typeparam name="T2">key</typeparam>
        /// <param name="source">source</param>
        /// <param name="keySelector">keySelector</param>
        /// <param name="transform">Transform</param>
        // ------------------------------------------------------------------------------------------------------------------------------
        public static void checkDuplicatesEditorOnly<T1, T2>(IEnumerable<T1> source, System.Func<T1,T2> keySelector, Transform transform)
        {

            var temp = source.GroupBy(keySelector).Where(val => val.Count() > 1);
            
            foreach(var val in temp)
            {
                Debug.LogWarningFormat(
                    "(#if UNITY_EDITOR) : Found duplicates : {0} : {1}",
                    val.Key,
                    Funcs.CreateHierarchyPath(transform)
                    );
            }

        }

    }

#endif

}
