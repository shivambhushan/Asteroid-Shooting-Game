using UnityEngine;
using System;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	public class TransformTransition : MonoBehaviour, IMenuTransition {
		[SerializeField]
		private Transform openTarget;
		[SerializeField]
		private Transform closeTarget;
		[SerializeField]
		private TransformProperty propertyToAnimate;
		[SerializeField]
		private EasingTypes easing;
		public float animationLength = 1f;
		[SerializeField]
		private bool unscaled;


		public void openAnimation(Action onFinished )
		{
			animate(onFinished, openTarget);
		}
		public void closeAnimation(Action onFinished)
		{
			animate(onFinished, closeTarget);
		}
		private void animate(Action onFinished, Transform target)
		{
			switch (propertyToAnimate) {
			case TransformProperty.Position:
				transform.MoveTo(target.position, length, easing, unscaled, Tween.TweenRepeat.Once, onFinished);
				break;
			case TransformProperty.Scale:
				transform.ScaleTo(target.localScale, length, easing, unscaled, Tween.TweenRepeat.Once, onFinished);
				break;
			case TransformProperty.Rotation:
				transform.RotateTo(target.localEulerAngles, length, easing, unscaled, Tween.TweenRepeat.Once, onFinished);
				break;
			default:
				throw new ArgumentOutOfRangeException ();
			}
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
	public enum TransformProperty
	{
		Position, Scale, Rotation
	}
}
