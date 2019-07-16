using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MA;

namespace MA.Examples{
	public class ModalTest : MonoBehaviour {
		[SerializeField]
		private string modalName;
		[SerializeField]
		private Text feedbackText;

		void Start()
		{
			IModal modal = ModalManager.instance.requestModal(modalName);
			if(modal != null && feedbackText != null)
			{
				modal.ModifyModal(()=>{feedbackText.text = "Canceled";}, false);
				modal.ModifyModal("A Modal is a prefab whose Text & Title as well as it's behaviour to User Choices can be modified at Runtime",
				            ()=>{
					feedbackText.text = "Accepted";
					ModalManager.instance.hideActiveModal();},
				()=>{
					feedbackText.text = "Refused";
					ModalManager.instance.hideActiveModal();},
				false);
			}
		}

		public void showModal()
		{
			ModalManager.instance.displayModal(modalName);
		}
	}
}
