using UnityEngine;
using System.Collections;

namespace MA{
	/// <summary>
	/// Calculates the appropriate Scrollposition for a 
	/// list inside a ScrollRect. Can be used with a ScrollRectController
	/// to bring the selected Element smoothly into focus.
	/// </summary>
	public class ScrollListController : ListController, MBoundsCheck{
		[SerializeField]
		private bool onlyScrollWhenOutOfBounds = true;
		[SerializeField]
		[GetOrAddComponent(false, true)]
		private ScrollRectController scrollController;

		public override void elementSelected (ListElement elem)
		{
			base.elementSelected (elem);
			if(onlyScrollWhenOutOfBounds)
			{
				if(activeElementOutsideBounds()) scrollController.centerOn(elem.transform as RectTransform);
			}else scrollController.centerOn(elem.transform as RectTransform);

		}
		private bool activeElementOutsideBounds()
		{
			return this.checkOutsideBounds(currentlySelected.transform as RectTransform, scrollController.scrollRect.transform as RectTransform);
		}
	}
}
