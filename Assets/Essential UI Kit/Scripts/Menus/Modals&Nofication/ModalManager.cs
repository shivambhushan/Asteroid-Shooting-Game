using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MA{
	/// <summary>
	/// Modal manager. Add to Scene and add Modal Prefabs. The Names (which should be unique)
	/// can then be used to display or modify the Modal at runtime later.
	/// </summary>
	public class ModalManager : UnitySingleton<ModalManager> {
		[Header("The Modals (only unique Names)")]
		[SerializeField] private Modal[] modals;
		private IModal activeModal = null;
		private Dictionary<string, IModal> modalLookup = new Dictionary<string, IModal>();

		void Awake()
		{
			modalSetup();
		}
		private void modalSetup()
		{
			foreach(Modal m in modals)
			{
				if(modalLookup.ContainsKey(m.modalname))
				{
					Debug.Log("There are two Modals with this name: "+ m.modalname +". The second one will be ignored");
					continue;
				}
				GameObject go = Instantiate(m.modalprefab) as GameObject;
				go.transform.SetParent(transform, false);
				IModal modal = go.GetComponent<IModal>();
				modal.modalObject = go;
				modalLookup.Add(m.modalname, modal);
			}
		}
		public bool displayModalIfExists(string modalName)
		{
			IModal modalToDisplay = requestModal(modalName);
			if(modalToDisplay == null) return false;
			if(activeModal != null) activeModal.hide();
			activeModal = modalToDisplay;
			activeModal.display();
			return true;
		}
		public void displayModal(string modalName)
		{
			displayModalIfExists(modalName);
		}
		public void hideActiveModal()
		{
			if(activeModal != null) activeModal.hide();
			activeModal = null;
		}
		public IModal requestModal(string modalName)
		{
			if(modalLookup.ContainsKey(modalName))
			{
				return modalLookup[modalName];
			}else return null;
		}

		public IModal ActiveModal {
			get {
				return activeModal;
			}
		}
	}
	[System.Serializable]
	public class Modal
	{
		public string modalname;
		[RequireDependencies(typeof(IModal))]
		public GameObject modalprefab;
	}
}
