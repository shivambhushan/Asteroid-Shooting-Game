using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	public class ListElement : MonoBehaviour, IDeselectHandler, IPointerClickHandler, ISelectHandler, IMoveHandler{
		public UnityEvent onSelected;
		public UnityEvent onDeselected;
		
		public virtual void ElementSelected (BaseEventData eventData)
		{
			ExecuteEvents.ExecuteHierarchy<IListElementSelected>(gameObject, eventData, (x, y) =>{ x.elementSelected(this);});
			if(EventSystem.current.currentSelectedGameObject != gameObject) EventSystem.current.SetSelectedGameObject(gameObject);
			onSelected.Invoke();
		}

		#region IPointerClickHandler implementation
		public void OnPointerClick (PointerEventData eventData)
		{
			ElementSelected(eventData);
		}
		#endregion

		#region ISelectHandler implementation

		public void OnSelect (BaseEventData eventData)
		{
			ElementSelected(eventData);
		}

		#endregion

		#region IDeselectHandler implementation

		public virtual void OnDeselect (BaseEventData eventData)
		{
			onDeselected.Invoke();
		}

		#endregion

		#region IMoveHandler implementation

		public void OnMove (AxisEventData eventData)
		{
			int index = transform.GetSiblingIndex();
			switch (eventData.moveDir) {
			case MoveDirection.Left:
				if(index == 0) return;
				else EventSystem.current.SetSelectedGameObject(transform.parent.GetChild(index-1).gameObject);
				break;
			case MoveDirection.Up:
				goto case MoveDirection.Left;
			case MoveDirection.Right:
				if(index >= transform.parent.childCount -1) return;
				else EventSystem.current.SetSelectedGameObject(transform.parent.GetChild(index+1).gameObject);
				break;
			case MoveDirection.Down:
				goto case MoveDirection.Right;
			case MoveDirection.None:
				break;
			default:
				throw new System.ArgumentOutOfRangeException ();
			}
		}

		#endregion
	}
}
