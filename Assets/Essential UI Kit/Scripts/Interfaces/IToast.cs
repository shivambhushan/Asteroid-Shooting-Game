using UnityEngine;

namespace MA{
	public interface IToast : IModal{

		void hide(float seconds);

		bool IsShowing{
			get;
		}
	}
}
