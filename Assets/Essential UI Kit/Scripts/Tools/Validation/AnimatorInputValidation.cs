using UnityEngine;
using System.Collections;

namespace MA{
	public class AnimatorInputValidation : InputValidation {
		[SerializeField]
		[GetOrAddComponent]
		protected Animator anim;
		[SerializeField]
		protected string AnimatorBoolAlias;
		private int boolID;

		protected override void Awake ()
		{
			base.Awake ();
			boolID = Animator.StringToHash(AnimatorBoolAlias);
		}

		protected override void handleInputFailure (string failuremessage)
		{
			base.handleInputFailure(failuremessage);
			anim.SetBool(boolID, false);
		}
		protected override void handleInputSuccess ()
		{
			base.handleInputSuccess();
			anim.SetBool(boolID, true);
		}
	}
}
