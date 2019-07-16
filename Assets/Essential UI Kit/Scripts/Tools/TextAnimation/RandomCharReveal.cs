using UnityEngine;
using System.Collections;

namespace MA{
	public class RandomCharReveal : TextReveal {
		[SerializeField]
		protected char[] replacementChars;

		protected override string stringReplacer (string toReplace)
		{
			builder.Length = 0;
			foreach(char c in toReplace.ToCharArray())
			{
				if(c == ' ') builder.Append(" ");
				else builder.Append(replacementChars[Random.Range(0, replacementChars.Length-1)]);
			}
			return builder.ToString();
		}
	}
}
