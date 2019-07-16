using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MA{
	public class NotificationManager : UnitySingleton<NotificationManager> {
		[Header("The Toasts (only unique Names)")]
		[SerializeField] private Toast[] toasts;
		private IToast activeToast = null;
		private Dictionary<string, IToast> toastLookup = new Dictionary<string, IToast>();

		void Awake()
		{
			toastSetup();
		}
		private void toastSetup()
		{
			foreach(Toast t in toasts)
			{
				if(toastLookup.ContainsKey(t.toastname))
				{
					Debug.Log("There are two Modals with this name: "+ t.toastname +". The second one will be ignored");
					continue;
				}
				IToast toast = instantiateToast(t.toastPrefab);
				toastLookup.Add(t.toastname, toast);
			}
		}
		private IToast instantiateToast(GameObject prefab)
		{
			GameObject go = Instantiate(prefab) as GameObject;
			go.SetActive(false);
			go.transform.SetParent(transform, false);
			IToast toast = go.GetComponent<IToast>();
			toast.modalObject = go;
			return toast;
		}
		public bool displayOrChangeToast(string toastName, string toastText,  float displayTime = 2f, string toastHeader = null, Sprite toastSprite = null)
		{
			IToast toastToDisplay = requestToast(toastName);
			if(toastToDisplay == null) return false;
			if(activeToast != null)
			{
				//check if this is a new Type of toast and if so hide the old type of toast 
				if(activeToast != toastToDisplay)
				{
					activeToast.hide();
				}
				toastToDisplay.modalObject.SetActive(true);
			}
			else toastToDisplay.modalObject.SetActive(true);

			activeToast = toastToDisplay;
			activeToast.ModifyModal(toastText);
			if(toastHeader != null) activeToast.setupHeader(toastHeader);
			if(toastSprite != null) activeToast.ModifyModalImage(toastSprite);
			activeToast.modalObject.transform.SetAsFirstSibling();
			activeToast.display();
			activeToast.hide(displayTime);
			return true;
		}
		public bool displayOrAddToast(string toastName, string toastText, float displayTime = 2f, bool addOnTop = true, string toastHeader = null, Sprite toastSprite = null)
		{
			IToast toastToDisplay = requestToast(toastName);
			if(toastToDisplay == null) return false;
			//if the Toast is already showing we'll clone it
			if(toastToDisplay.IsShowing)
			{
				toastToDisplay = instantiateToast(toastToDisplay.modalObject);
			}
			toastToDisplay.modalObject.SetActive(true);
			toastToDisplay.ModifyModal(toastText);
			if(toastHeader != null) toastToDisplay.setupHeader(toastHeader);
			if(toastSprite != null) toastToDisplay.ModifyModalImage(toastSprite);
			if(addOnTop) toastToDisplay.modalObject.transform.SetAsFirstSibling();
			toastToDisplay.display();
			toastToDisplay.hide(displayTime);
			return true;
		}
		public void hideActiveToast()
		{
			if(activeToast == null) return;
			activeToast.hide();
		}

		IToast requestToast (string toastName)
		{
			if(toastLookup.ContainsKey(toastName))
			{
				return toastLookup[toastName];
			}else return null;
		}
	}
	[System.Serializable]
	public class Toast
	{
		public string toastname;
		[RequireDependencies(typeof(IToast))]
		public GameObject toastPrefab;
	}
}