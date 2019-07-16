using UnityEngine;
using System;

namespace MA{
	public interface IMenuTransition
	{
		float length
		{
			get;
		}
		void closeAnimation(Action onFinished);
		void openAnimation(Action onFinished);
	}
}