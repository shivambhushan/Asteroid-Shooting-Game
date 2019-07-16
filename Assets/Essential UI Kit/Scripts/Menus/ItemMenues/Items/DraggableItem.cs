using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(CanvasGroup))]
	public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDroppableItem {
		protected Vector2 previousPosition, pointerOffset;
		protected Canvas canvas;
		protected CanvasGroup canvasgroup;
		protected Transform previousHolder;
		private bool block = false;

		protected void Awake()
		{
			canvasgroup = GetComponent<CanvasGroup>();
			canvas = GetComponentInParent<Canvas>();
		}

		#region IBeginDragHandler implementation
		public void OnBeginDrag (PointerEventData eventData)
		{
			previousPosition = transform.position;
			pointerOffset = eventData.position - previousPosition;
			//Ensures that the dragged items is in front of all other objects in this canvas
			previousHolder = transform.parent;
			transform.SetParent(canvas.transform, false);
			transform.SetAsLastSibling();
			canvasgroup.blocksRaycasts = false;
		}
		#endregion

		#region IDragHandler implementation

		public void OnDrag (PointerEventData eventData)
		{
			//Move the Element with the Pointer and adjust for the Point where the Element was grabbed
			transform.position = eventData.position - pointerOffset;
		}

		#endregion

		#region IEndDragHandler implementation

		public void OnEndDrag (PointerEventData eventData)
		{
			canvasgroup.blocksRaycasts = true;
			if(transform.parent == canvas.transform && !block) returnToPrevious();
		}

		#endregion

		#region IDroppableItem implementation
		public virtual void returnToPrevious()
		{
			transform.position = previousPosition;
			transform.SetParent(previousHolder, true);
		}


		public GameObject holdingObject {
			get {
				return gameObject;
			}
		}

		public GameObject previousParent {
			get {
				if(previousHolder != null) return previousHolder.gameObject;
				else return null;
			}
		}
		public bool blocked {
			get {
				return block;
			}
			set{
				block = value;
			}
		}
		#endregion
	}
}
