using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace MA{
	/// <summary>
	/// Works with a ScrollRectController to center the ScrollRect viewport on this 
	/// GameObject when clicked on
	/// </summary>
	public class CenterOnClick : MonoBehaviour, IPointerClickHandler {
		private ScrollRectController controller;

		void Awake()
		{
			controller = GetComponentInParent<ScrollRectController>();
		}
		#region IPointerClickHandler implementation
		public void OnPointerClick (PointerEventData eventData)
		{
			if(controller != null) controller.centerOn(transform as RectTransform);
		}
		#endregion
	}
}