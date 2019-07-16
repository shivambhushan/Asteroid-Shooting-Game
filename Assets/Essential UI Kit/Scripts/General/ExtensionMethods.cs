using UnityEngine;
using System.Collections;

namespace MA{
	public static class ExtensionMethods{

		//returns an Array with the Child Gameobjects (in transform order)
		public static GameObject[] GetChildren(this GameObject go)
		{
			GameObject[] children = new GameObject[go.transform.childCount];
			for(int i = 0; i< go.transform.childCount; i++)
			{
				children[i] = go.transform.GetChild(i).gameObject;
			}
			return children;
		}
	}
}
