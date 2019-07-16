using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	public class LevelItem : LevelBaseItem {
		[GetOrAddComponent(true, false)]
		[SerializeField] private Button button;
		[GetOrAddComponent(true, false)]
		[SerializeField] private Text LevelNameText;
		[SerializeField] private Text NumberText;

		void Awake () {
			button.onClick.AddListener(()=>{
				ExecuteEvents.ExecuteHierarchy<ILevelChangeRequest>(gameObject, null, (x, y)=>{x.changeLevelTo(levelID);});
			});
		}
		public override void initializeLevelItem (string name, int id)
		{
			base.initializeLevelItem (name, id);
			LevelNameText.text = name;
			gameObject.name = levelname;
			if(NumberText != null) NumberText.text = (id+1).ToString();
		}

	}
}
