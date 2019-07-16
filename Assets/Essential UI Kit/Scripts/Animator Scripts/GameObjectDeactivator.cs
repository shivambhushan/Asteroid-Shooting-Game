using UnityEngine;
using System.Collections;
namespace MA{
	/// <summary>
	/// Game object deactivator. Deactivates a gameObject based on the Animator State it's Animator is in
	/// without constantly checking the Animator State or Transition Info. Used by the Animator Toast for Example
	/// </summary>
	public class GameObjectDeactivator : StateMachineBehaviour {
		[Header("Else it will Deactivate on Enter")]
		public bool ActivateOnEnter = false;
		public bool parent = false;
		 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			GameObject go = parent ? animator.transform.parent.gameObject : animator.gameObject;
			go.SetActive(ActivateOnEnter);
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//	}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			GameObject go = parent ? animator.transform.parent.gameObject : animator.gameObject;
			go.SetActive(!ActivateOnEnter);
		}

		// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
		//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
		//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}
	}
}
