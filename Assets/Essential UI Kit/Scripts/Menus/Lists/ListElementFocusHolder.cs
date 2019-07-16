using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	/// <summary>
	/// List element focus holder. Propagates Select and Deselect Elements of an Selectable inside (as a Child)
	/// an List Element upwards so that a selected List Element doesn't lose Focus
	/// </summary>
	public class ListElementFocusHolder : MonoBehaviour, ISelectHandler, IDeselectHandler {
		private ListElement listelement;

		void Start () {
			listelement = GetComponentInParent<ListElement>();
		}
		
		
		#region ISelectHandler implementation
		public void OnSelect (BaseEventData eventData)
		{
			listelement.ElementSelected(eventData);
		}
		#endregion

		#region IDeselectHandler implementation

		public void OnDeselect (BaseEventData eventData)
		{
			listelement.OnDeselect(eventData);
		}

		#endregion
	}
}
