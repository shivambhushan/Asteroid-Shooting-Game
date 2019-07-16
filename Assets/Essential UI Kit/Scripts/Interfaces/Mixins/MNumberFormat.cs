using UnityEngine;
using System.Collections;

namespace MA{
	public interface MNumberFormat {}
	public static class MNumberFormatImpl
	{
		public static string formatNumbersBehindComma(this MNumberFormat mixinImplementer, int amount)
		{
			if(amount == 0) return "{0:0}";
			else
			{
				char[] buffer = new char[amount];
				for (int i = 0; i < amount; i++)
				{
					buffer[i] = '0';
				}
				return "{0:0." +new string(buffer)+ "}";
			}
		}
	}
}