using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Coroutine manager
    /// </summary>
    public class CoroutineManager : SingletonMonoBehaviour<CoroutineManager>
    {

        protected class LinkedIEnumerator
        {
            public bool useLinked = false;
            public System.Object obj = null;
            public IEnumerator ie = null;
        }

        /// <summary>
        /// IEnumerator queue
        /// </summary>
        //Queue<IEnumerator> m_ieQueue = new Queue<IEnumerator>();

        protected Dictionary<int, Queue<LinkedIEnumerator>> m_ieQueueDict = new Dictionary<int, Queue<LinkedIEnumerator>>();

        /// <summary>
        /// Called in Awake
        /// </summary>
        // ----------------------------------------------------------------------------------------
        protected override void initOnAwake()
        {

        }

        /// <summary>
        /// Start Coroutine for non-MonoBehaviour
        /// </summary>
        /// <param name="coroutine">Coroutine</param>
        /// <returns>Coroutine</returns>
        // ----------------------------------------------------------------------------------------
        public Coroutine startCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        /// <summary>
        /// Start watching ordered IEnumerator queue
        /// </summary>
        /// <param name="groupId">group id</param>
        /// <returns>IEnumerator</returns>
        // ----------------------------------------------------------------------------------------
        protected IEnumerator startIeQueue(int groupId)
        {

            if (!this.m_ieQueueDict.ContainsKey(groupId))
            {
                yield break;
            }

            // ------------------

            Queue<LinkedIEnumerator> queue = this.m_ieQueueDict[groupId];

            LinkedIEnumerator li = null;

            while (queue.Count > 0)
            {

                li = queue.Peek();

                if(li != null)
                {

                    if(li.useLinked && li.ie != null)
                    {

                        while (li.ie.MoveNext() && li.obj != null && !li.obj.Equals(null))
                        {
                            yield return li.ie.Current;
                        }

                    }

                    else
                    {
                        yield return li.ie;
                    }
                    
                }
                
                queue.Dequeue();

            }

            // remove
            {
                this.m_ieQueueDict.Remove(groupId);
            }

        }

        /// <summary>
        /// Add ordered IEnumerator
        /// </summary>
        /// <param name="ie">IEnumerator</param>
        /// <param name="groupId">group id to belong</param>
        // ----------------------------------------------------------------------------------------
        public void addOrderedCoroutine(IEnumerator ie, int groupId = 0)
        {
           
            if(!this.m_ieQueueDict.ContainsKey(groupId))
            {
                this.m_ieQueueDict.Add(groupId, new Queue<LinkedIEnumerator>());
            }

            // add
            {

                LinkedIEnumerator li = new LinkedIEnumerator();

                li.ie = ie;
                li.obj = null;
                li.useLinked = false;

                this.m_ieQueueDict[groupId].Enqueue(li);

            }

            // StartCoroutine
            {

                if (this.m_ieQueueDict[groupId].Count == 1)
                {
                    StartCoroutine(this.startIeQueue(groupId));
                }

            }

        }

        /// <summary>
        /// Add ordered IEnumerator
        /// </summary>
        /// <param name="ie">IEnumerator</param>
        /// <param name="linkedObj">linked object to live with</param>
        /// <param name="groupId">group id to belong</param>
        // ----------------------------------------------------------------------------------------
        public void addOrderedCoroutine(IEnumerator ie, System.Object linkedObj, int groupId = 0)
        {

            if (!this.m_ieQueueDict.ContainsKey(groupId))
            {
                this.m_ieQueueDict.Add(groupId, new Queue<LinkedIEnumerator>());
            }

            // add
            {

                LinkedIEnumerator li = new LinkedIEnumerator();

                li.ie = ie;
                li.obj = linkedObj;
                li.useLinked = true;

                this.m_ieQueueDict[groupId].Enqueue(li);

            }

            // StartCoroutine
            {

                if (this.m_ieQueueDict[groupId].Count == 1)
                {
                    StartCoroutine(this.startIeQueue(groupId));
                }

            }

        }

    }

}
