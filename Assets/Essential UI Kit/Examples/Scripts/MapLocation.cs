using UnityEngine;
using System.Collections;

namespace MA.Examples{
	public class MapLocation : MonoBehaviour {
		public string toastName;
		public string locationName;
		[Multiline]
		public string locationDescription;

		public void showLocationInfo()
		{
			NotificationManager.instance.displayOrChangeToast(toastName, locationDescription, 4f, locationName);
		}
	}
}
