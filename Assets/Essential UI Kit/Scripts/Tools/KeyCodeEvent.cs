using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace MA{
	public class KeyCodeEvent : MonoBehaviour {
		[SerializeField]
		private KeyCode[] keys;
		[SerializeField]
		private UnityEvent keyPressEvent;

		void Update () {
			foreach(KeyCode k in keys)
			{
				if(Input.GetKeyDown(k)) 
				{
					keyPressEvent.Invoke();
					return;
				}
			}
		}
	}
}
