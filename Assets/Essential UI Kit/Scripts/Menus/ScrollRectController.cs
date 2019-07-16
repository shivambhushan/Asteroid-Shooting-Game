using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	/// <summary>
	/// Scroll rect controller. Can be used to bring something inside a ScrollRect
	/// into focus
	/// </summary>
	[RequireComponent(typeof(ScrollRect))]
	public class ScrollRectController : MonoBehaviour, IBeginDragHandler {
		[Header("Either block User Input during Movement or end the Movement on User Input")]
		[SerializeField]
		private bool blockUserInputDuringScroll = true;
		[SerializeField]
		private bool eased = true;
		[SerializeField]
		private EasingTypes easing;
		[SerializeField]
		private float animationLength = 1f;
		[HideInInspector]
		public ScrollRect scrollRect;
		private bool moving = false, vertical, horizontal;
		private Coroutine scrollRoutine = null;
		private RectTransform contentRect, viewRect;

		public virtual void Awake()
		{
			scrollRect = GetComponent<ScrollRect>();
			contentRect = scrollRect.content;
			viewRect = contentRect.parent as RectTransform;
		}

		public void jumpTo(float scrollPosition)
		{
			if(scrollRect.vertical)
			{
				if(eased)
				{
					if(moving)
					{
						TweenManager.instance.stopTween(scrollRoutine);
					}
					if(blockUserInputDuringScroll && !moving) blockScrollRectInput();
					moving = true;
					scrollRoutine = TweenManager.instance.playTween(new Tween((float f)=>{
						scrollRect.verticalNormalizedPosition = f;
					}, scrollRect.verticalNormalizedPosition, scrollPosition, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{
						moving = false;
						if(blockUserInputDuringScroll) resetScrollRectInput();
					}));
				}else scrollRect.verticalNormalizedPosition = scrollPosition;

			}else if(scrollRect.horizontal)
			{
				if(eased)
				{
					if(moving)
					{
						TweenManager.instance.stopTween(scrollRoutine);
					}
					if(blockUserInputDuringScroll && !moving) blockScrollRectInput();
					moving = true;
					scrollRoutine = TweenManager.instance.playTween(new Tween((float f)=>{
						scrollRect.horizontalNormalizedPosition = f;
					}, scrollRect.horizontalNormalizedPosition, scrollPosition, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{
						moving = false;
						if(blockUserInputDuringScroll) resetScrollRectInput();
					}));
				}else scrollRect.horizontalNormalizedPosition = scrollPosition;
			}
		}
		public void jumpTo(Vector2 scrollPosition)
		{
			if(eased)
			{
				if(moving)
				{
					TweenManager.instance.stopTween(scrollRoutine);
				}
				if(blockUserInputDuringScroll && !moving) blockScrollRectInput();
				moving = true;
				scrollRoutine = scrollRect.jump(scrollPosition, animationLength, easing, false, Tween.TweenRepeat.Once, ()=>{
					moving = false;
					if(blockUserInputDuringScroll) resetScrollRectInput();
				});
			}else{
				scrollRect.normalizedPosition = scrollPosition;
			}
		}
		public void centerOn(RectTransform centerTransform)
		{
			Vector3[] viewRectWorldCoordinates = new Vector3[4], contentRectWorldCoordinates = new Vector3[4];
			viewRect.GetWorldCorners(viewRectWorldCoordinates);
			contentRect.GetWorldCorners(contentRectWorldCoordinates);
			Vector3 centerPosition = Vector3.Lerp(viewRectWorldCoordinates[0], viewRectWorldCoordinates[2], 0.5f);
			Vector3 difference = centerPosition - centerTransform.position;
			Vector2 viewRectWorldSize = new Vector2(Vector3.Distance(viewRectWorldCoordinates[0], viewRectWorldCoordinates[3]), 
			                                        Vector3.Distance(viewRectWorldCoordinates[0], viewRectWorldCoordinates[1]));
			Vector2 contentRectWorldSize = new Vector2(Vector3.Distance(contentRectWorldCoordinates[0], contentRectWorldCoordinates[3]), 
			                                           Vector3.Distance(contentRectWorldCoordinates[0], contentRectWorldCoordinates[1]));
			Vector2 normalizedDifference = new Vector2( difference.x / (contentRectWorldSize.x - viewRectWorldSize.x),
			                                           difference.y / (contentRectWorldSize.y - viewRectWorldSize.y));
			
			Vector2 normalizedPosition = scrollRect.normalizedPosition - normalizedDifference;
			if (scrollRect.movementType != ScrollRect.MovementType.Unrestricted)
			{
				normalizedPosition.x = Mathf.Clamp01(normalizedPosition.x);
				normalizedPosition.y = Mathf.Clamp01(normalizedPosition.y);
			}
			jumpTo(normalizedPosition);
		}
		private void blockScrollRectInput()
		{
			horizontal = scrollRect.horizontal;
			vertical = scrollRect.vertical;
			scrollRect.vertical = scrollRect.horizontal = false;
		}
		private void resetScrollRectInput()
		{
			scrollRect.horizontal = horizontal;
			scrollRect.vertical = vertical;
		}
		public void abortScrollMovement()
		{
			if(!moving) return;
			TweenManager.instance.stopTween(scrollRoutine);
		}

		#region IBeginDragHandler implementation
		public void OnBeginDrag (PointerEventData eventData)
		{
			if(!blockUserInputDuringScroll) abortScrollMovement();
		}
		#endregion
	}
}