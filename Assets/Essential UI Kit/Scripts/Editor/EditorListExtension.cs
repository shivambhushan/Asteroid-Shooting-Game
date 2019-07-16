#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;

public static class EditorListExtension{

	public static string[] GetNameArray<T>(this List<T> list, Func<T, string> nameFunction)
	{
		string[] names = new string[list.Count];

		for(var i = 0; i < list.Count; i++)
		{
			names[i] = nameFunction(list[i]);
		}
		return names;
	}
}
#endif
