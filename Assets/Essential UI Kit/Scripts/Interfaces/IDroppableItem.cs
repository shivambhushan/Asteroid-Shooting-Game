using UnityEngine;
using System.Collections;

namespace MA{
	public interface IDroppableItem{
		//return to a previously held position if drop didn't work
		void returnToPrevious();
		//needed to access components via the interface
		GameObject holdingObject
		{
			get;
		}
		//needed to notify the previous holder that the item is gone
		GameObject previousParent
		{
			get;
		}
		bool blocked{
			get;
			set;
		}
	}
}
