using UnityEngine;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(PageController))]
	public class PageSwitcher : MonoBehaviour {
		[SerializeField]
		private KeyCode[] leftKeys = new KeyCode[1]{KeyCode.LeftArrow};
		[SerializeField]
		private KeyCode[] rightKeys = new KeyCode[1]{KeyCode.RightArrow};
		private PageController controller;

		void Awake()
		{
			controller = GetComponent<PageController>();
		}

		void Update () {
			foreach(KeyCode k in leftKeys)
			{
				if(Input.GetKeyDown(k))
				{
					controller.changeDown();
					return;
				}
			}
			foreach(KeyCode k in rightKeys)
			{
				if(Input.GetKeyDown(k))
				{
					controller.changeUp();
					return;
				}
			}
		}
	}
}
