using UnityEngine;
using System.Collections;

namespace MA{
	public class LengthValidator : AbstractInputValidator {
		[Header("Only accepts input that is longer than min and shorter than max (Both Exclusive)")]
		public int min = 0;
		public int max = 16;

		public override bool validateInput(string input)
		{
			return input.Length < max && input.Length > min;
		}
	}
}
