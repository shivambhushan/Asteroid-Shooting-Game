using UnityEngine;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	public class EasedDraggableItem : DraggableItem  {
		[SerializeField]
		private EasingTypes easing;
		[SerializeField]
		private float animationLength = 0.5f;

		public override void returnToPrevious ()
		{
			//Make sure User can't grab the Element while Snapping
			canvasgroup.interactable = false;
			transform.MoveTo(previousPosition, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{ 
				canvasgroup.interactable = true;
				transform.SetParent(previousHolder, true);
				blocked = false;
			});
		}

	}
}
