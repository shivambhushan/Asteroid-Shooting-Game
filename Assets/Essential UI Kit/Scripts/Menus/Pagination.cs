using UnityEngine;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(Animator))]
	public class Pagination : MonoBehaviour {
		[SerializeField]
		private PageController controller;
		private Animator anim;
		[SerializeField]
		private string clickedTriggerAlias;
		private int clickedBoolID;

		void Awake()
		{
			anim = GetComponent<Animator>();
			clickedBoolID = Animator.StringToHash(clickedTriggerAlias);
		}

		public void changeUp()
		{
			if(controller.changeUp()) anim.SetTrigger(clickedBoolID);
		}
		public void changeDown()
		{
			if(controller.changeDown()) anim.SetTrigger(clickedBoolID);
		}

	}
}
