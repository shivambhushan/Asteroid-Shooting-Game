using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MA;

namespace MA.Examples{
	public class ToastTest : MonoBehaviour {
		public string toastname = "SimpleNotification";
		public InputField TitleInput, TimeInput;


		public void showToast()
		{
			int time = 2;
			int.TryParse(TimeInput.text, out time);
			NotificationManager.instance.displayOrAddToast(toastname, "This Notification will disappear in "+time.ToString()+" seconds.", time , false, TitleInput.text);
		}
	}
}
