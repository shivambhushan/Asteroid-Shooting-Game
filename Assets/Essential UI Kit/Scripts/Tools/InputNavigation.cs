using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(Selectable))]
	public class InputNavigation : MonoBehaviour, ISelectHandler, IDeselectHandler {
		private Selectable selectable;
		private KeyCode[] switchKeysDown = new KeyCode[]{KeyCode.Tab, KeyCode.DownArrow};
		private KeyCode[] switchKeysUp = new KeyCode[]{ KeyCode.UpArrow};
		private bool active = false;

		void Start () {
			selectable = GetComponent<Selectable>();
		}

		void Update()
		{
			if(!active) return;
			foreach(KeyCode k in switchKeysDown)
			{
				if(Input.GetKeyDown(k))
				{
					switchSelectedDown();
					return;
				}
			}
			foreach(KeyCode k in switchKeysUp)
			{
				if(Input.GetKeyDown(k))
				{
					switchSelectedUp();
					return;
				}
			}

		}
		#region ISelectHandler implementation

		public void OnSelect (BaseEventData eventData)
		{
			active = true;
		}

		#endregion

		#region IDeselectHandler implementation

		public void OnDeselect (BaseEventData eventData)
		{
			active = false;
		}

		#endregion
		private void switchSelectedUp()
		{
			Selectable next = selectable.FindSelectableOnUp();
			if(next != null)
			{
				EventSystem.current.SetSelectedGameObject(next.gameObject);
			}
		}
		private void switchSelectedDown()
		{
			Selectable next = selectable.FindSelectableOnDown();
			if(next != null) 
			{
				EventSystem.current.SetSelectedGameObject(next.gameObject);
			}
		}
	}
}
