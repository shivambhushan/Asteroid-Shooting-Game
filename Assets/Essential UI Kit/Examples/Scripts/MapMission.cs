using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA.Examples{
	[RequireComponent(typeof(ExpandableListElement))]
	public class MapMission : MonoBehaviour {
		private ExpandableListElement listelement;
		public Typer MissionTyper;
		[Multiline]
		public string missionDescription;
		public MapLocation missionLocation;
		public ScrollRectController mapController;

		void Start () {
			listelement = GetComponent<ExpandableListElement>();
			listelement.onSelected.AddListener(()=>{
				MissionTyper.ReplaceText(missionDescription);
				mapController.centerOn(missionLocation.transform as RectTransform);
				missionLocation.showLocationInfo();
			});
			listelement.onDeselected.AddListener(()=>{
				MissionTyper.StopTyping();
				MissionTyper.textComponent.text = "";
			});
		}
		

	}
}
