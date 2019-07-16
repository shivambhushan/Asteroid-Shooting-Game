using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;


namespace MA{
	public class Slot : MonoBehaviour, IDropHandler, IItemDropTarget {

		public GameObject Item {
			get {
				if(transform.childCount>0) return transform.GetChild(0).gameObject;
				else return null;
			}
			private set	{
				IDroppableItem droppable = value.GetComponent<IDroppableItem>();
				if(droppable != null)
				{
					droppable.blocked = true;
					ExecuteEvents.ExecuteHierarchy<IgainedItem>(gameObject, null, (x, y) =>{ x.gainedItem(droppable, this);});
				}

			}
		}
		public T ItemComponent<T>() where T : Component {
			if(Item != null)
			{
					return Item.GetComponent<T>();
			}else return null;
		}

		#region IDropHandler implementation

		public void OnDrop (PointerEventData eventData)
		{
			if(Item == null)
			{
				Item = eventData.pointerDrag;
			}
		}

		#endregion

		#region IItemDropTarget implementation

		public virtual void MoveToSlot (IDroppableItem drop)
		{
			drop.holdingObject.transform.SetParent(transform, true);
			drop.holdingObject.transform.localPosition = Vector3.zero;
			drop.blocked = false;
		}

		#endregion
	}
}

