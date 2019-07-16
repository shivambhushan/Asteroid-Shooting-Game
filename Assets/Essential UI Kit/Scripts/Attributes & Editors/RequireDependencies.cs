using System;
using UnityEngine;
using System.Collections;
/// <summary>
/// Require dependencies. Displays a Warning
/// if the GameObject doesn't have all of the Components 
/// specified and lists which are missing.
/// </summary>
public class RequireDependencies : PropertyAttribute{
	public Type[] requiredDepencies;
	public bool lookInChildren;
	
	public RequireDependencies (bool lookInChildren, params Type[] requiredComponents) {
		this.requiredDepencies = requiredComponents;
		this.lookInChildren = lookInChildren;
	}
	public RequireDependencies (params Type[] requiredComponents) {
		this.requiredDepencies = requiredComponents;
		this.lookInChildren = false;
	}
}
