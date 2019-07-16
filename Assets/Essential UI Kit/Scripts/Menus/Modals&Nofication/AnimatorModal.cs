using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA{
	public class AnimatorModal : MonoBehaviour, IModal {
		[Header("The Name of the Animator bool")]
		[SerializeField] private string boolAlias = "show";
		[SerializeField] private Animator anim;
		[SerializeField] private Text modaltext;
		[SerializeField] private Text headertext;
		public Button acceptButton, refuseButton, cancelButton; 
		private GameObject modal;
		private int boolID;

		void Awake()
		{
			boolID = Animator.StringToHash(boolAlias);
			if(cancelButton != null) cancelButton.onClick.AddListener(()=>{ ModalManager.instance.hideActiveModal();});
		}
		#region IModal implementation

		public virtual void display ()
		{
			anim.SetBool(boolID, true);
		}
		public void ModifyModal (string text)
		{
			modaltext.text = text;
		}

		public void ModifyModal (string text, System.Action onAccept, bool overwriteCallbacks = false)
		{
			ModifyModal(text);
			if(overwriteCallbacks) acceptButton.onClick.RemoveAllListeners();
			acceptButton.onClick.AddListener(()=>{ onAccept();});
		}
		public void ModifyModal (string question, System.Action onAccept, System.Action onRefuse, bool overwriteCallbacks = false)
		{
			if(overwriteCallbacks) refuseButton.onClick.RemoveAllListeners();
			refuseButton.onClick.AddListener(()=>{ onRefuse();});
			ModifyModal(question, onAccept, overwriteCallbacks);
		}
		public void ModifyModal (System.Action onCancel, bool overwriteCallbacks = false)
		{
			if(overwriteCallbacks) cancelButton.onClick.RemoveAllListeners();
			cancelButton.onClick.AddListener(()=>{onCancel();});
		}
		/*
		 * in this case we'll use this implementation for changing the Header Text
		 * not really useful but this function is really more for unusual modals
		 */
		public void ModifyModal (string[] texts, bool overwriteCallbacks, params System.Action[] actions)
		{
			if(texts.Length != 1 || actions.Length != 0) return;
			headertext.text = texts[0];
		}
		public void setupHeader(string headline)
		{
			ModifyModal(new string[]{headline}, false);
		}


		public void ModifyModalImage (Sprite image)
		{
			throw new System.NotImplementedException ();
		}

		public virtual void hide ()
		{
			anim.SetBool(boolID, false);
		}

		public GameObject modalObject {
			get {
				return modal;
			}
			set {
				modal = value;
			}
		}
		#endregion
	}
}
