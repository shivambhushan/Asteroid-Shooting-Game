using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA.Examples{
	public class AnimationExample : MonoBehaviour {
		public Animator rectAnimator, maskAnimator;
		public Image maskImage;
		public Mask mask;
		public Text currentAnimationText;
		public string[] rectTriggerAliases, maskTriggerAliases;
		private int counter = 0;

		public void showNextAnimation()
		{
			string trigger;
			if( counter < rectTriggerAliases.Length)
			{
				maskImage.enabled = false;
				mask.enabled = false;
				trigger = rectTriggerAliases[counter];
				rectAnimator.SetTrigger(trigger);
				currentAnimationText.text = trigger;
				counter++;
			}
			else if(counter < (rectTriggerAliases.Length + maskTriggerAliases.Length))
			{
				mask.enabled = true;
				maskImage.enabled = true;
				trigger = maskTriggerAliases[counter- rectTriggerAliases.Length];
				maskAnimator.SetTrigger(trigger);
				currentAnimationText.text = trigger;
				counter++;
			}else
			{
				counter = 0;
				showNextAnimation();
			}
		}
	}
}
