#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using MA;

public class ColorAssetCreator
{
	[MenuItem("Assets/Create/Color Scheme")]
	public static void CreateAsset ()
	{
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == "") 
		{
			path = "Assets";
		} 
		else if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		ColorScheme scheme = ScriptableObject.CreateInstance<ColorScheme>();
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + "ColorScheme.asset");
		AssetDatabase.CreateAsset (scheme, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = scheme;
	}
}
#endif
