using UnityEngine;
using System.Collections;

namespace MA{
	/// <summary>
	/// Uses the Animator for UI Animations
	/// </summary>
	public class AnimatorTransition : MonoBehaviour, IMenuTransition {
		[SerializeField]
		[GetOrAddComponent]
		private Animator anim;
		[SerializeField]
		private string AnimatorBoolAlias = "open";
		private int boolID;
		[Header("After this time has elapsed the Next Menu will be called")]
		[SerializeField]
		private float animationLength = 3f;

		void Awake()
		{
			boolID = Animator.StringToHash(AnimatorBoolAlias);
		}
		
		#region IAnimatable implementation

		public void closeAnimation (System.Action onFinished)
		{
			anim.SetBool(boolID, false);
			Scheduler.createTask(onFinished, animationLength, 1);
		}

		public void openAnimation (System.Action onFinished)
		{
			anim.SetBool(boolID, true);
			Scheduler.createTask(onFinished, animationLength, 1);
		}

		public float length {
			get {
				return animationLength;
			}
		}
		#endregion
	}
}
