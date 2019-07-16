using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MA{
	/// <summary>
	/// Resize panel. Taken from Unity UI Samples almost unmodified except for the
	/// Scroll direction adjustment according to the Pivot of the Rect. Resizing
	/// is always away from the Pivot
	/// </summary>
	public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler {
		[SerializeField]
		private Vector2 minSize = new Vector2 (100, 100);
		[SerializeField]
		private Vector2 maxSize = new Vector2 (400, 400);
		
		private RectTransform panelRectTransform;
		private Vector2 originalLocalPointerPosition;
		private Vector2 originalSizeDelta;
		private float xSign;
		private float ySign;
		
		void Awake () {
			panelRectTransform = transform.parent.transform as RectTransform;
			xSign = panelRectTransform.pivot.x <= 0.5f ? 1f : -1f;
			ySign = panelRectTransform.pivot.y <= 0.5f ? 1f : -1f;
		}
		
		public void OnPointerDown (PointerEventData data) {
			originalSizeDelta = panelRectTransform.sizeDelta;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out originalLocalPointerPosition);
		}
		
		public void OnDrag (PointerEventData data) {
			if (panelRectTransform == null)
				return;
			
			Vector2 localPointerPosition;
			RectTransformUtility.ScreenPointToLocalPointInRectangle (panelRectTransform, data.position, data.pressEventCamera, out localPointerPosition);
			Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;

			Vector2 sizeDelta = originalSizeDelta + new Vector2 (xSign * offsetToOriginal.x, ySign * offsetToOriginal.y);
			sizeDelta = new Vector2 (
				Mathf.Clamp (sizeDelta.x, minSize.x, maxSize.x),
				Mathf.Clamp (sizeDelta.y, minSize.y, maxSize.y)
			);
			
			panelRectTransform.sizeDelta = sizeDelta;
		}
	}
}