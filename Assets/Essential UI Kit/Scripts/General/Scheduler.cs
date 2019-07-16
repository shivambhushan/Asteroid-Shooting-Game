using UnityEngine;
using System;
using System.Collections;

namespace MA{
	/// <summary>
	/// Scheduler. A class for managing recurring Tasks in arbitrary
	///invervals or scheduling future Actions
	/// </summary>
	public sealed class Scheduler : UnitySingleton<Scheduler> {
		
		/// <summary>
		/// Creates the task. If number of Executions is negative(default), then the task will execute indefinitely at the given interval rate
		/// </summary>
		/// <returns>The Coroutine for the task.</returns>
		/// <param name="task">Task.</param>
		/// <param name="interval">Interval.</param>
		/// <param name="NumberOfExecutions">Number of executions.</param>
		public static Coroutine createTask(Action task, float interval = 1f, int NumberOfExecutions = -1)
		{
			if(NumberOfExecutions == 0) return null;
			return instance.StartCoroutine(instance.Task(task, new WaitForSeconds(interval), NumberOfExecutions));
		}
		/// <summary>
		/// Creates a watch task. The task will execute when the supplied condition is true (at the time of checking), by default this will 
		///	stop checking once the condition has been met once
		/// </summary>
		/// <returns>The Coroutine for the watch task.</returns>
		/// <param name="condition">Condition.</param>
		/// <param name="callbackWhenTrue">Callback when true.</param>
		/// <param name="interval">Interval.</param>
		/// <param name="endWhenTrue">If set to <c>true</c> end when true.</param>
		public static Coroutine createWatchTask(Func<bool> condition, Action callbackWhenTrue,float interval = 1f, bool endWhenTrue = true )
		{
			return instance.StartCoroutine(instance.WatchTask(condition, callbackWhenTrue, new WaitForSeconds(interval), endWhenTrue));
		}
		public static void endTask(Coroutine cor)
		{
			instance.StopCoroutine(cor);
		}
		/// <summary>
		/// Executes a Task every other frame.
		/// </summary>
		/// <returns>The Coroutine.</returns>
		/// <param name="Task">Task.</param>
		/// <param name="frameAmount">Frame amount.</param>
		public static Coroutine everyOtherFrame(Action Task, int frameAmount)
		{
			return instance.StartCoroutine(instance.Task(Task, frameAmount, frameAmount));
		}
		private IEnumerator Task(Action task, int frameAmount, int currentFrame)
		{
			yield return null;
			currentFrame--;
			if(currentFrame == 0) 
			{
				task();
				currentFrame = frameAmount;
			}
			StartCoroutine(Task(task, frameAmount, currentFrame));

		}
		private IEnumerator Task(Action task, WaitForSeconds wait, int nrExecutions)
		{
			yield return wait;
			task();
			nrExecutions--;
			if(nrExecutions != 0) StartCoroutine(Task(task, wait, nrExecutions));
		}
		private IEnumerator WatchTask(Func<bool> condition, Action callbackWhenTrue, WaitForSeconds wait, bool endWhenTrue = true)
		{
			yield return wait;
			if(condition())
			{
				callbackWhenTrue();
				if(endWhenTrue) yield break;
			}else 
				StartCoroutine(WatchTask(condition, callbackWhenTrue, wait, endWhenTrue));
		}
		private IEnumerator WatchTask(Func<bool> condition, Action callbackWhenTrue, bool endWhenTrue = true)
		{
			while(!condition())
			{
				yield return null;
			}
			callbackWhenTrue();
			if(endWhenTrue) yield break;
			else StartCoroutine(WatchTask(condition, callbackWhenTrue, endWhenTrue));
		}
	}
}