using UnityEngine;
using System.Collections;

namespace MA{
	/// <summary>
	/// Animator tool tip, handles the showing and hiding of the toolTip via the Animator
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class AnimatorToolTip : ToolTip {
		[SerializeField]
		protected string animatorBoolAlias;
		protected Animator animator;
		private int boolID;

		protected override void Awake()
		{
			boolID = Animator.StringToHash(animatorBoolAlias);
			animator = GetComponent<Animator>();
			base.Awake();
		}

		public override void show (RectTransform target)
		{
			transform.position = target.position;
			RectTransform rectTransform = transform as RectTransform;
			rectTransform.pivot = target.pivot;
			animator.SetBool(boolID, true);
		}
		public override void hide ()
		{
			animator.SetBool(boolID, false);
		}
	}
}
