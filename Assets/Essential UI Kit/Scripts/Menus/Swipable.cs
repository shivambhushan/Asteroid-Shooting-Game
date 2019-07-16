using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	public class Swipable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
		protected Vector2 previousPosition, pointerOffset;
		[SerializeField]
		protected bool vertical = false;
		

		#region IBeginDragHandler implementation
		public virtual void OnBeginDrag (PointerEventData eventData)
		{
			previousPosition = transform.position;
			pointerOffset = eventData.position - previousPosition;
		}
		#endregion

		#region IDragHandler implementation

		public virtual void OnDrag (PointerEventData eventData)
		{
			transform.position = new Vector2(vertical ? transform.position.x : eventData.position.x - pointerOffset.x, vertical ? eventData.position.y - pointerOffset.y : transform.position.y);
			visualizeDrag();
		}

		#endregion

		#region IEndDragHandler implementation

		public virtual void OnEndDrag (PointerEventData eventData)
		{
			if(boundsCheck()) disposeOf();
			else returnToPrevious(previousPosition);
		}

		#endregion

		//override this for a fancier return Animation in a base Class
		protected virtual void returnToPrevious(Vector2 oldPosition)
		{
			transform.position = previousPosition;
		}
		//override this with a custom implementation for when the object has left the bounds
		protected virtual bool boundsCheck()
		{
			return false;
		}
		//override this for a fancy deactivation
		protected virtual void disposeOf()
		{
			gameObject.SetActive(false);
		}
		protected virtual void visualizeDrag()
		{

		}
	}
}
