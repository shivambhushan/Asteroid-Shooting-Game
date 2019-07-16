using UnityEngine;
using System.Collections;

namespace MA{
	public interface MBoundsCheck {}
	public static class MBoundsCheckImpl
	{
		public static bool checkOutsideBounds(this MBoundsCheck mixinImplementer, RectTransform rect, RectTransform containerRect, Camera cam = null )
		{
			if(cam == null) cam = Camera.main;
			Vector3[] cornersArray = new Vector3[4];
			rect.GetWorldCorners(cornersArray);
			foreach(Vector3 point in cornersArray)
			{
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam ,point);
				if(!RectTransformUtility.RectangleContainsScreenPoint(containerRect, screenPoint, cam)) return true;
			}
			return false;
		}
	}
}
