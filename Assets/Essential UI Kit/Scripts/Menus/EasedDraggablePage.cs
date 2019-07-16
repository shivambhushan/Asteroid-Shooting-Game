using UnityEngine;
using MA.EaseNTween;
using System.Collections;

namespace MA{
	public class EasedDraggablePage : DraggablePage {
		[SerializeField]
		private EasingTypes easing = EasingTypes.BounceIn;
		[GetOrAddComponent]
		[SerializeField]
		private CanvasGroup cgroup;
		[SerializeField]
		private float animationLength = 0.7f;

		public override void leaveDownOrLeft ()
		{
			transform.MoveTo(controller.leftOrDownPosition.position, animationLength, easing);
		}
		public override void leaveUpOrRight ()
		{
			transform.MoveTo(controller.rightOrUpPosition.position, animationLength, easing);
		}
		public override void enterDownOrLeft ()
		{
			transform.WarpAndMoveTo(controller.leftOrDownPosition.position, controller.activePositon.position, animationLength, easing);
		}
		public override void enterUpOrRight ()
		{
			transform.WarpAndMoveTo(controller.rightOrUpPosition.position, controller.activePositon.position, animationLength, easing);
		}
		public override void returnToMiddle ()
		{
			returnToPrevious(previousPosition);
		}
		protected override void returnToPrevious(Vector2 oldPosition)
		{
			//Making sure the Player can't grab the Object while it's animating
			cgroup.interactable = false;
			transform.MoveTo(controller.activePositon.position, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{cgroup.interactable = true;});
		}
	}
}
