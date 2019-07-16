using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace MA{
	/// <summary>
	/// Access Point for showing tooltips. The tooltips can be accessed via the Singleton instance
	/// of this class.
	/// </summary>
	public class ToolTipManager : UnitySingleton<ToolTipManager>{
		private Dictionary<string, ToolTip> tooltips = new Dictionary<string, ToolTip>();

		public void registerToolTip(string tooltipname, ToolTip tooltip)
		{
			if(!tooltips.ContainsKey(name))
			{
				tooltips.Add(tooltipname, tooltip);
			}else Debug.Log("There is already a ToolTip with name " + tooltipname);
		}
		public ToolTip getToolTip(string tooltipname)
		{
			if(tooltips.ContainsKey(tooltipname))
			{
				return tooltips[tooltipname];
			}else return null;
		}
		public void showToolTip(string tooltipName, RectTransform rectTrans, string tooltipMessage, string tooltipHeader = null, Sprite toolTipImage = null)
		{
			ToolTip t = getToolTip(tooltipName);
			if(t != null)
			{
				t.setup(tooltipMessage, tooltipHeader, toolTipImage);
				t.show(rectTrans);
			}
		}
		public void hideToolTip(string tooltipName)
		{
			ToolTip t = getToolTip(tooltipName);
			if(t != null)
			{
				t.hide();
			}
		}

	}
}
