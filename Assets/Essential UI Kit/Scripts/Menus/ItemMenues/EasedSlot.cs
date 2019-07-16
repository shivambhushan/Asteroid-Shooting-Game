using UnityEngine;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	public class EasedSlot : Slot {
		[SerializeField]
		private EasingTypes easing;
		[SerializeField]
		private float animationLength = 0.5f;


		public override void MoveToSlot(IDroppableItem drop)
		{
			drop.holdingObject.transform.MoveTo(transform.position, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{
				drop.holdingObject.transform.SetParent(transform, true);
				drop.blocked = false;
			});
		}
	}
}
