using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MA.EaseNTween{

	public sealed class TweenManager : UnitySingleton<TweenManager> {
		private WaitForEndOfFrame wait = new WaitForEndOfFrame();

		#region Public Tween Starters

		/// <summary>
		/// Plays a tween and returns a Coroutine which you can cache 
		/// to abort the Tween later
		/// </summary>
		/// <param name="t">Tween.</param>
		public Coroutine playTween(Tween t)
		{
			return StartCoroutine(animate(t));
		}
		/// <summary>
		/// Chains the tweens for sequential execution
		/// </summary>
		/// <param name="tweens">Tweens.</param>
		public void chainTweens(params Tween[] tweens)
		{
			List<IEnumerator> tmp = new List<IEnumerator>();
			foreach(Tween t in tweens)
			{
				tmp.Add(animate(t));
			}
			StartCoroutine(chain(tmp.ToArray()));
		}
		#endregion

		#region Animate
		private IEnumerator animate(Tween t)
		{
			//store the easeCall in this Action depending on Tweentype
			Action easeCall = ()=>{};
			switch (t.type) {
				case Tween.TweenType.delay:
				yield return new WaitForSeconds(t.originaltime);
					yield break;
				case Tween.TweenType.f:
				easeCall = () =>
				{
					t.Value = t.easeFunc(t.from, t.to, t.progress);
				};
					break;
				case Tween.TweenType.v2:
				easeCall = () =>
				{
					t.ValueV2 = Vector2.zero.ease(t.easeFunc,t.fromV2, t.toV2, t.progress);
				};
					break;
				case Tween.TweenType.v3:
				easeCall = () =>
				{
					t.ValueV3 = Vector3.zero.ease(t.easeFunc,t.fromV3, t.toV3, t.progress);
				};
					break;
				case Tween.TweenType.c:
				easeCall = () =>
				{
					t.ValueC = Color.black.ease(t.easeFunc,t.fromC, t.toC, t.progress);
				};
					break;
				default:
					throw new ArgumentOutOfRangeException ();
			}
			//the actual easing over time
			while(t.resttime > 0)
			{
				t.resttime -= t.time();
				t.progress = 1f - t.resttime/t.originaltime;
				easeCall();
				yield return wait;
			}
			if(t.OnComplete != null) t.OnComplete();
			checkRepeat(t);
		}
		#endregion

		#region Convenience Functions
		private IEnumerator chain(params IEnumerator[] enumerators)
		{
			foreach(IEnumerator i in enumerators)
			{
				yield return StartCoroutine(i);
			}
		}
		public void stopAllTweens(){
			this.StopAllCoroutines();
		}
		public void stopTween(Coroutine cor){
			this.StopCoroutine(cor);
		}
		public static void Dispose()
		{
			if(HasInstance) 
			{
				instance.stopAllTweens();
				Destroy(instance);
			}
		}
		private void checkRepeat(Tween t)
		{
			t.resttime = t.originaltime;
			if(t.repeat == Tween.TweenRepeat.PingPong) t.SwitchTargets();
			if(t.repeat == Tween.TweenRepeat.PingPong || t.repeat == Tween.TweenRepeat.Loop) StartCoroutine(animate (t));
		}

		#endregion

	}
	public class Tween {
		public float from, to, resttime, originaltime, progress;
		public Vector2 fromV2, toV2;
		public Vector3 fromV3, toV3;
		public Color fromC, toC;
		public Func<float, float, float, float> easeFunc;
		private Action<float> valSet;
		private Action<Vector2> valSetv2;
		private Action<Vector3> valSetv3;
		private Action<Color> valSetC;
		public Func<float> time;
		public Action OnComplete;
		public const Action doNothing = null;
		public TweenType type = TweenType.f;
		public TweenRepeat repeat = TweenRepeat.Once;

		public enum TweenType
		{
			delay, f, v2, v3, c 
		}
		public enum TweenRepeat
		{
			Once, PingPong, Loop
		}
		
		#region Tween Constructors
		/// <summary>
		/// Use this for animating float values. The first parameter has to a setter for the float
		/// </summary>
		/// <param name="valueSetter">Value setter.</param>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="length">Length.</param>
		/// <param name="easeType">Ease type.</param>
		/// <param name="unscaled">If set to <c>true</c> unscaled.</param>
		/// <param name="repeat">Repeat.</param>
		/// <param name="OnComplete">On complete.</param>
		public Tween(Action<float> valueSetter, float from, float to, float length, EasingTypes easeType = EasingTypes.Linear,bool unscaled = false, TweenRepeat repeat = TweenRepeat.Once ,Action OnComplete = doNothing)
		{
			this.valSet = valueSetter;
			this.from = from;
			this.to = to;
			this.easeFunc = Easing.Function(easeType);
			this.resttime = this.originaltime = length;
			this.OnComplete = OnComplete;
			this.type = TweenType.f;
			this.repeat = repeat;
			this.time = timeFunc(unscaled);
		}
		/// <summary>
		/// Use this for animating a Vector2
		/// </summary>
		/// <param name="valueSetter">Value setter.</param>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="length">Length.</param>
		/// <param name="easeType">Ease type.</param>
		/// <param name="unscaled">If set to <c>true</c> unscaled.</param>
		/// <param name="repeat">Repeat.</param>
		/// <param name="OnComplete">On complete.</param>
		public Tween(Action<Vector2> valueSetter, Vector2 from, Vector2 to, float length, EasingTypes easeType = EasingTypes.Linear,bool unscaled = false, TweenRepeat repeat = TweenRepeat.Once, Action OnComplete= doNothing)
		{
			this.valSetv2 = valueSetter;
			this.fromV2 = from;
			this.toV2 = to;
			this.easeFunc = Easing.Function(easeType);
			this.resttime = this.originaltime = length;
			this.OnComplete = OnComplete;
			this.type = TweenType.v2;
			this.repeat = repeat;
			this.time = timeFunc(unscaled);
		}
		/// <summary>
		/// Use this for animating a Vector3
		/// </summary>
		/// <param name="valueSetter">Value setter.</param>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="length">Length.</param>
		/// <param name="easeType">Ease type.</param>
		/// <param name="unscaled">If set to <c>true</c> unscaled.</param>
		/// <param name="repeat">Repeat.</param>
		/// <param name="OnComplete">On complete.</param>
		public Tween(Action<Vector3> valueSetter, Vector3 from, Vector3 to, float length, EasingTypes easeType = EasingTypes.Linear,bool unscaled = false, TweenRepeat repeat = TweenRepeat.Once, Action OnComplete= doNothing)
		{
			this.valSetv3 = valueSetter;
			this.fromV3 = from;
			this.toV3 = to;
			this.easeFunc = Easing.Function(easeType);
			this.resttime = this.originaltime = length;
			this.OnComplete = OnComplete;
			this.type = TweenType.v3;
			this.repeat = repeat;
			this.time = timeFunc(unscaled);
		}
		/// <summary>
		/// Use this for animating colours
		/// </summary>
		/// <param name="valueSetter">Value setter.</param>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		/// <param name="length">Length.</param>
		/// <param name="easeType">Ease type.</param>
		/// <param name="unscaled">If set to <c>true</c> unscaled.</param>
		/// <param name="repeat">Repeat.</param>
		/// <param name="OnComplete">On complete.</param>
		public Tween(Action<Color> valueSetter, Color from, Color to, float length, EasingTypes easeType = EasingTypes.Linear,bool unscaled = false, TweenRepeat repeat = TweenRepeat.Once, Action OnComplete= doNothing)
		{
			this.valSetC = valueSetter;
			this.fromC = from;
			this.toC = to;
			this.easeFunc = Easing.Function(easeType);
			this.resttime = this.originaltime = length;
			this.OnComplete = OnComplete;
			this.type = TweenType.c;
			this.repeat = repeat;
			this.time = timeFunc(unscaled);
		}
		public Tween(float seconds)
		{
			this.originaltime = seconds;
			this.type = TweenType.delay;
		}
		#endregion
		
		#region Tween Value Getters
		public Vector2 ValueV2
		{
			set {
				this.valSetv2(value);
			}
		}
		public Vector3 ValueV3
		{
			set {
				this.valSetv3(value);
			}
		}
		public Color ValueC
		{
			set {
				this.valSetC(value);
			}
		}
		public float Value 
		{
			set {
				this.valSet(value);
			}
		}
		#endregion
		#region Tween Helper Functions
		public void SwitchTargets()
		{
			swap<float>(ref from, ref to);
			swap<Vector2>(ref fromV2, ref toV2);
			swap<Vector3>(ref fromV3, ref toV3);
			swap<Color>(ref fromC, ref toC);
		}
		public static void swap<T>(ref T param1, ref T param2)
		{
			T tmp = param1;
			param1 = param2;
			param2 = tmp;
		}
		public static Func<float> timeFunc(bool unscaled)
		{
			return unscaled ? (Func<float>)(()=>Time.unscaledDeltaTime) : (Func<float>)(()=> Time.deltaTime);
		}
		#endregion
	}
}