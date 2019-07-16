using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	public class ImageFillEaser : MonoBehaviour {
		[SerializeField]
		[GetOrAddComponent]
		private Image image;
		[SerializeField]
		private EasingTypes easing;
		private Coroutine fillRoutine;

		public void animateFill(float target, float animationLength = 0.5f)
		{
			Mathf.Clamp01(target);
			if(fillRoutine != null) TweenManager.instance.stopTween(fillRoutine);
			fillRoutine = image.EaseFill(target, animationLength, easing);
		}

	}
}
