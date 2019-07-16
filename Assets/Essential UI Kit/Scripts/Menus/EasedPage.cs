using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	public class EasedPage : MonoBehaviour, IPage {
		[SerializeField]
		[GetOrAddComponent(false, true)]
		protected PageController controller;
		[SerializeField]
		protected float animationLength = 0.5f;
		[SerializeField]
		protected EasingTypes easing;
		public UnityEvent OnActivation, OnDeactivation;

		#region IPage implementation

		public void leaveUpOrRight ()
		{
			controller.Blocked = true;
			transform.WarpAndMoveTo(controller.activePositon.position, controller.rightOrUpPosition.position, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{controller.Blocked = false;});
		}

		public void enterUpOrRight ()
		{
			transform.WarpAndMoveTo(controller.rightOrUpPosition.position, controller.activePositon.position, animationLength, easing);
		}

		public void leaveDownOrLeft ()
		{
			controller.Blocked = true;
			transform.WarpAndMoveTo(controller.activePositon.position, controller.leftOrDownPosition.position, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{controller.Blocked = false;});
		}

		public void enterDownOrLeft ()
		{
			transform.WarpAndMoveTo(controller.leftOrDownPosition.position, controller.activePositon.position, animationLength, easing);
		}

		public void returnToMiddle ()
		{

		}

		public GameObject pageObject {
			get {
				return gameObject;
			}
		}

		public void onBecameActive ()
		{
			OnActivation.Invoke();
		}

		public void onBecameInactive ()
		{
			OnDeactivation.Invoke();
		}

		#endregion
	}
}