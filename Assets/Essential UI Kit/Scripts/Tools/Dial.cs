using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	/// <summary>
	/// Dial. Showing progress between a min and a max value via the fillAmount of an Image
	/// </summary>
	public class Dial : MonoBehaviour {
		[Header("Image Type should be filled")]
		[SerializeField]
		[GetOrAddComponent]
		protected Image image;
		public float value, minValue, maxValue = 1f;
		[SerializeField]
		protected bool eased = false;
		[SerializeField]
		protected EasingTypes easing = EasingTypes.QuarticIn;
		[SerializeField]
		protected float animationLength = 0.2f;
		private Coroutine easeRoutine;


		protected virtual void Start()
		{
			Value = value;
		}
		
		public virtual void displayProgress()
		{
			if(eased)
			{
				if(easeRoutine != null) TweenManager.instance.StopCoroutine(easeRoutine);
				easeRoutine = image.EaseFill(Factor, animationLength, easing);
			}
			else image.fillAmount = Factor;
		}
		public float Factor{
			get{
				return (value-minValue)/(maxValue - minValue);
			}
		}
		public float Value
		{
			get{
				return value;
			}
			set {
				this.value = Mathf.Clamp(value, minValue, maxValue);
				displayProgress();
			}
		}
	}
}
