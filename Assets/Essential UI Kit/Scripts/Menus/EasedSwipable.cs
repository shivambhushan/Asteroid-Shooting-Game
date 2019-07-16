using UnityEngine;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	[RequireComponent(typeof(CanvasGroup))]
	public class EasedSwipable : Swipable {
		[SerializeField]
		protected EasingTypes easing = EasingTypes.ElasticOut;
		[SerializeField]
		[Header("Relative to Screen Size")]
		[Range(0,1f)]
		protected float swipeDistanceThreshold = 0.5f;
		[SerializeField]
		UnityEngine.Events.UnityEvent onSwipedEvent;
		[SerializeField]
		[GetOrAddComponent]
		private CanvasGroup cgroup;
		
		protected override void returnToPrevious(Vector2 oldPosition)
		{
			//Making sure the Player can't grab the Object while it's animating
			cgroup.interactable = false;
			TweenManager.instance.playTween(new Tween((Vector3 pos)=>{
				transform.position = pos;
			}
			, transform.position, oldPosition, 0.6f, easing, false, Tween.TweenRepeat.Once, 
			()=>{cgroup.interactable = true;}));

		}
		//See if we're out of bounds based on the distance to the original position + give visual feedback
		protected override bool boundsCheck()
		{
			float relativeDistance = Vector3.Distance(previousPosition, transform.position)/ (vertical ? Screen.height : Screen.width);
			return relativeDistance > swipeDistanceThreshold;
		}
		protected bool boundsCheck(out float relativeDistance)
		{
			relativeDistance = Vector3.Distance(previousPosition, transform.position)/ (vertical ? Screen.height : Screen.width);
			return relativeDistance > swipeDistanceThreshold;
		}
		protected override void disposeOf()
		{
			TweenManager.instance.playTween(new Tween((float alpha)=>{
				cgroup.alpha = alpha;
			}, cgroup.alpha, 0f, 0.6f, EasingTypes.CubicIn, false, Tween.TweenRepeat.Once, onSwipedEvent.Invoke));
		}
		protected override void visualizeDrag ()
		{
			float relativeDistance;
			if(boundsCheck(out relativeDistance)) cgroup.alpha = Mathf.Clamp01(1f - (relativeDistance - swipeDistanceThreshold) * 3f);
			else cgroup.alpha = 1f;
		}
	}
}
