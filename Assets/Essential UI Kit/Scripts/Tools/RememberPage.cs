using UnityEngine;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(PageController))]
	public class RememberPage : MonoBehaviour, IStatusChanged {
		private int lastPage;
		[SerializeField]
		private string PrefKey = "OverviewPageControllerKey";
		private PageController controller;

		void Awake()
		{
			controller = GetComponent<PageController>();
			lastPage = PlayerPrefs.GetInt(PrefKey, -1);
			if(lastPage < 0) lastPage = controller.PageOpenOnStart;
			else controller.PageOpenOnStart = lastPage;
		}
		void OnDisable()
		{
			PlayerPrefs.SetInt(PrefKey, lastPage);
		}

		#region IStatusChanged implementation

		public void statusChanged (int newStatus)
		{
			lastPage = newStatus;
		}

		public void initialize ()
		{

		}

		#endregion
	}
}
