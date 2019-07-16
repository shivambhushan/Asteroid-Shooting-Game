using UnityEngine;
using System.Collections;

namespace MA{
	public interface IItemDropTarget{
		void MoveToSlot(IDroppableItem droppable);
	}
}
