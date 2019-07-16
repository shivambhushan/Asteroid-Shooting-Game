using UnityEngine;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	public class SimpleFader : UnitySingleton<SimpleFader> {
		private CanvasGroup cgroup;
		private Canvas canvas;
		private RectTransform rectTransform;
		private bool initialized = false;
		public float animationLength = 0.7f;
		public EasingTypes easing = EasingTypes.Linear;

		void Start () {
			if(!initialized) initialize();
		}
		void initialize()
		{
			cgroup = gameObject.AddComponent<CanvasGroup>();
			gameObject.AddComponent<UnityEngine.UI.Image>().color = new Color(0,0,0,1f);
			cgroup.interactable = false;
			cgroup.blocksRaycasts = false;
			cgroup.alpha = 0f;
			canvas = GetComponentInParent<Canvas>();
			if(canvas == null) canvas = GameObject.FindObjectOfType<Canvas>();
			rectTransform = transform as RectTransform;
			rectTransform.SetParent(canvas.transform, false);
			rectTransform.SetAsLastSibling();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.anchoredPosition3D = new Vector3(0,0,-100);
			initialized = true;
		}
		public float FadeValue{
			get{
				if(!initialized) initialize();
				return cgroup.alpha;
			}
			set{
				if(!initialized) initialize();
				cgroup.FadeTo(Mathf.Clamp01(value), animationLength, easing, true, Tween.TweenRepeat.Once, blockingCheck);
			}
		}
		public Coroutine fade(float target, float length, EasingTypes fadingEasing, System.Action onComplete = null)
		{
			if(!initialized) initialize();
			return cgroup.FadeTo(Mathf.Clamp01(target), length, easing, true, Tween.TweenRepeat.Once, ()=>{
				blockingCheck();
				if(onComplete!= null) onComplete();
			});
		}
		private void blockingCheck()
		{
			if(Mathf.Approximately(cgroup.alpha, 1f)) cgroup.blocksRaycasts = true;
		}

	}
}
