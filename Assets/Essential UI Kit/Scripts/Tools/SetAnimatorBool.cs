using UnityEngine;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(Animator))]
	public class SetAnimatorBool : MonoBehaviour {
		private Animator anim;

		void Start()
		{
			anim = GetComponent<Animator>();
		}
		public void setBoolTrue(string name)
		{
			anim.SetBool(name, true);
		}
		public void setBoolFalse(string name)
		{
			anim.SetBool(name, false);
		}
		public void toggleBool(string name)
		{
			bool currentState = anim.GetBool(name);
			anim.SetBool(name, !currentState);
		}
	}
}
