using UnityEngine;
using System.Collections;

namespace MA{
	public class NumberReveal : TextReveal {
		[Header("Min is Inclusive, Max is Exclusive")]
		[Range(0,10)]
		[SerializeField]
		protected int minNumber = 0, maxNumber = 10;

		protected override string stringReplacer (string toReplace)
		{
			builder.Length = 0;
			foreach(char c in toReplace.ToCharArray())
			{
				if(c == ' ') builder.Append(" ");
				else builder.Append(Random.Range(minNumber, maxNumber));
			}
			return builder.ToString();
		}
	}
}
