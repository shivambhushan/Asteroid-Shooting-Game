using UnityEngine;
using System.Collections;
using MA.EaseNTween;

namespace MA{
	public class EasedExpandableListElement : ExpandableListElement {
		[SerializeField]
		private EasingTypes easing;
		[SerializeField]
		[Range(0.01f, 5f)]
		private float animationLength = 0.5f;

		protected override void Awake ()
		{
			base.Awake ();
		}

		protected override void expand ()
		{
			layout.EaseLayoutMinValues(expandedSize, animationLength, easing);
		}
		protected override void contract ()
		{
			layout.EaseLayoutMinValues(contractedSize, animationLength, easing);
		}
	}
}
