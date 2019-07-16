using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA{
	/// <summary>
	/// Tool tip. Simple class for displaying tooltips. Registers with TooltipManager Singleton
	/// and can be acessed via it's toolTipName Property from there.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class ToolTip : MonoBehaviour {
		public string toolTipName;
		public Text toolTipText, toolTipHeader;
		public Image toolTipImage;
		private CanvasGroup cgroup;

		protected virtual void Awake () {
			cgroup = GetComponent<CanvasGroup>();
			ToolTipManager.instance.registerToolTip(toolTipName, this);
		}
		/// <summary>
		///Shows the tooltip at the targetPosition with the pivot of the target. Override
		/// for a Tooltip Pop-Up with effects
		/// </summary>
		/// <param name="target">Target.</param>
		public virtual void show(RectTransform target)
		{
			transform.position = target.position;
			RectTransform rectTransform = transform as RectTransform;
			rectTransform.pivot = target.pivot;
			cgroup.alpha = 1f;
		}
		public virtual void hide()
		{
			cgroup.alpha = 0f;
		}
		public virtual void setup(string toolTipMessage, string toolTipHeadline = null, Sprite toolTipSprite = null)
		{
			toolTipText.text = toolTipMessage;
			if(toolTipHeadline != null && toolTipHeader != null) toolTipHeader.text = toolTipHeadline;
			if(toolTipSprite != null && toolTipImage != null) toolTipImage.sprite = toolTipSprite;
		}
	}
}
