using UnityEngine;
using System.Collections;
/// <summary>
/// Gets the declared Component or adds one if none is present on the GameObject
/// (or its children and parent when lookInChildren or lookInParent is set to true)
/// Only works if the property field is serialized (public or decorated with the SerializeField
/// Attribute)
/// </summary>
public class GetOrAddComponent : PropertyAttribute {
	public bool lookInChildren = false, lookInParent = false;

	public GetOrAddComponent()
	{

	}
	public GetOrAddComponent (bool lookInChildren, bool lookInParent) {
		this.lookInChildren = lookInChildren;
		this.lookInParent = lookInParent;
	}
}
