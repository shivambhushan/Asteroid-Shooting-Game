using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	/// <summary>
	/// Tool tip target. Put this script on any UI Element that you want to show
	/// a Tooltip. The tooltipPosition transform will determine the placement of the Tooltip
	/// with it's position and pivot (which will allow for finer control of the relative placement of the Tooltip)
	/// </summary>
	public class ToolTipTarget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
		public string ToolTipName;
		public string ToolTipHeader;
		[Multiline]
		public string ToolTipMessage;
		public Sprite ToolTipImage;
		[Header("The pivot of this Transform will also be used to determine the Placement of the Tooltip")]
		public RectTransform tooltipPosition;

		#region IPointerEnterHandler implementation
		public void OnPointerEnter (PointerEventData eventData)
		{
			ToolTipManager.instance.showToolTip(ToolTipName, tooltipPosition, ToolTipMessage, ToolTipHeader, ToolTipImage);
		}
		#endregion

		#region IPointerExitHandler implementation

		public void OnPointerExit (PointerEventData eventData)
		{
			ToolTipManager.instance.hideToolTip(ToolTipName);
		}

		#endregion
	}
}
