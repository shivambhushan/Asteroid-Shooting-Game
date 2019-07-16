using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(LayoutElement))]
	public class ExpandableListElement : ListElement {
		[Space(10)]
		protected LayoutElement layout;
		[SerializeField] protected bool startExpanded = false;
		[SerializeField] protected Vector2 contractedSize, expandedSize;


		protected virtual void Awake()
		{
			layout = GetComponent<LayoutElement>();
			if(startExpanded) expand();
			else contract();
		}

		protected virtual void expand()
		{
			layout.minHeight = expandedSize.y;
			layout.minWidth = expandedSize.x;
		}
		protected virtual void contract()
		{
			layout.minHeight = contractedSize.y;
			layout.minWidth = contractedSize.x;
		}
		public override void ElementSelected(BaseEventData data)
		{
			base.ElementSelected(data);
			expand();

		}
		public override void OnDeselect(BaseEventData data)
		{
			base.OnDeselect(data);
			contract();
		}
	}
}
