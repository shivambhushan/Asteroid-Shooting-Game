#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using MA;
/// <summary>
/// Scale the Mesh to the Parent Rect from within the Editor
/// </summary>
[CustomEditor(typeof(CanvasElement3D))]
public class CanvasElement3DEditor : Editor
{
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		CanvasElement3D script = (CanvasElement3D)target;
		if(GUILayout.Button("Scale to Rect"))
		{
			bool previousState = script.eased;
			script.eased = false;
			script.initialize();
			script.eased = previousState;
		}
	}
}
#endif