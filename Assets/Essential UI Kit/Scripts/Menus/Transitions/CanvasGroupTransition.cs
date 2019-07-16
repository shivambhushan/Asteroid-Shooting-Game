using UnityEngine;
using System;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	[RequireComponent(typeof(CanvasGroup))]
	public class CanvasGroupTransition : MonoBehaviour, IMenuTransition {
		[SerializeField]
		private EasingTypes easing;
		public float animationLength = 1f;
		[SerializeField]
		private bool unscaled;
		[SerializeField]
		private bool onlyInteractableAndRayCast;
		private CanvasGroup cgroup;

		void Awake () {
			cgroup = GetComponent<CanvasGroup>();
		}

		#region IAnimatable implementation

		public void openAnimation(Action onFinished)
		{
			cgroup.blocksRaycasts = true;
			cgroup.interactable = true;
			if(!onlyInteractableAndRayCast) animate(onFinished, 1f);
			else onFinished();
		}
		public void closeAnimation(Action onFinished)
		{
			cgroup.blocksRaycasts = false;
			cgroup.interactable = false;
			if(!onlyInteractableAndRayCast) animate(onFinished, 0f);
			else onFinished();
		}
		
		#endregion

		private void animate(Action OnFinished, float alphatarget)
		{
			cgroup.FadeTo(alphatarget, length, easing, unscaled, Tween.TweenRepeat.Once, OnFinished);
		}
		public float length {
			get {
				return animationLength;
			}
			set {
				animationLength = value;
			}
		}
	}
}
