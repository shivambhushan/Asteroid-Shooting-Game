using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	/// <summary>
	/// Base inventory. Manages Inventory Items of type T (Storing References, Accepting or Refusing new Items,
	/// Connecting Slots (Implementing IItemDropTarget) with Items (with a Component implementing IDroppableItem)).
	/// The Slots should be children of the Inventory.
	/// </summary>
	public abstract class BaseInventory<T> :  BaseItemMenu<T>, IgainedItem, IlostItem where T : Component{

		protected virtual void Awake () {
			registerChildrenAsItems();
		}

		#region IgainedItem implementation

		public void gainedItem (IDroppableItem item, IItemDropTarget slotTarget)
		{
			//See wheter the item has our Component. Accept if it does otherwise return it to it's old Position
			T inventoryComponent = item.holdingObject.GetComponent<T>();
			if(inventoryComponent !=null){ 
				//check if this item is a new Item or already registered in the inventory and just changed Slots
				if(!itemExists(inventoryComponent.GetInstanceID()))
				{
					if(item.previousParent != null) ExecuteEvents.ExecuteHierarchy<IlostItem>(item.previousParent, null, (x, y) =>{ x.lostItem(item);});
					registerItem(inventoryComponent);
					addedNewItem(inventoryComponent);
				}
				slotTarget.MoveToSlot(item);
			}
			else item.returnToPrevious();
		}
		#endregion

		#region IlostItem implementation

		public void lostItem (IDroppableItem item)
		{
			T inventoryComponent = item.holdingObject.GetComponent<T>();
			if(inventoryComponent != null)
			{
				removeItem(inventoryComponent.GetInstanceID());
				lostItem();
			}
		}
		#endregion
		/// <summary>
		/// Overrides of this function will be called whenever there is a new item added to the Inventory
		/// </summary>
		/// <param name="newItem">New item.</param>
		protected virtual void addedNewItem(T newItem )
		{

		}
		/// <summary>
		/// Overrides of this function will be called whenever an item is lost
		/// </summary>
		protected virtual void lostItem()
		{

		}
	}
}
