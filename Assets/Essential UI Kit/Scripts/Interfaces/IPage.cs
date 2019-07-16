using UnityEngine;
using System.Collections;

namespace MA{
	public interface IPage {

		GameObject pageObject
		{
			get;
		}
		void leaveUpOrRight();
		void enterUpOrRight();
		void leaveDownOrLeft();
		void enterDownOrLeft();
		void returnToMiddle();
		void onBecameActive();
		void onBecameInactive();
	}
}
