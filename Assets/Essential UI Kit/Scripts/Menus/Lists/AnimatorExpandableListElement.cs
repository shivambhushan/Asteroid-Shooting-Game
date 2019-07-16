using UnityEngine;
using System.Collections;

namespace MA{
	public class AnimatorExpandableListElement : ExpandableListElement {
		[SerializeField]
		[GetOrAddComponent]
		private Animator anim;
		[Header("False when contracting, true when expanding")]
		[SerializeField]
		private string AnimatorBoolAlias;
		protected int boolID;

		protected override void Awake ()
		{
			base.Awake ();
			boolID = Animator.StringToHash(AnimatorBoolAlias);
		}

		protected override void contract ()
		{
			onDeselected.Invoke();
			anim.SetBool(AnimatorBoolAlias, false);
		}
		protected override void expand ()
		{
			onSelected.Invoke();
			anim.SetBool(AnimatorBoolAlias, true);
		}
	}
}
