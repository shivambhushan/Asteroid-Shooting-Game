using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA{
	/// <summary>
	/// Animator toast. Hides an Displays a Toast using the Animator.
	/// </summary>
	public class AnimatorToast : AnimatorModal, IToast {
		[SerializeField] protected Image toastImage;
		protected Coroutine hideRoutine;
		protected float hideTime;
		protected bool showing = false;

		#region IToast implementation
		public void changeModalText (Sprite image)
		{
			toastImage.sprite = image;
		}

		public void hide (float seconds)
		{
			hideTime = seconds;
			//Assigning the hide Routine here gives Toast Control over when and if it should be hidden
			//for Example to make it interactable
			if(hideRoutine != null) Scheduler.endTask(hideRoutine);
			hideRoutine = Scheduler.createTask(()=>{this.hide();}, hideTime, 1);
		}

		public bool IsShowing {
			get {
				return showing;
			}
		}
		#endregion
		public override void display ()
		{
			showing = true;
			base.display ();
		}
		public override void hide ()
		{
			showing = false;
			base.hide ();
		}
	}
}
