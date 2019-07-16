using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	/// <summary>
	/// Level change button. Triggers the changeLevelTo function on a
	/// Component implementing ILevelChangeRequest higher in the 
	/// Hierarchy
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class LevelChangeButton : MonoBehaviour {
		[SerializeField]
		private string levelname;
		[SerializeField]
		private int levelID;
		private Button button;

		void Start () {
			button = GetComponent<Button>();
			button.onClick.AddListener(()=>{
				if(string.IsNullOrEmpty(levelname))
				{
					ExecuteEvents.ExecuteHierarchy<ILevelChangeRequest>(gameObject, null, (x, y)=>{x.changeLevelTo(levelID);});
				}else
				{
					ExecuteEvents.ExecuteHierarchy<ILevelChangeRequest>(gameObject, null, (x, y)=>{x.changeLevelTo(levelname);});
				}

			});
		}
	}
}
