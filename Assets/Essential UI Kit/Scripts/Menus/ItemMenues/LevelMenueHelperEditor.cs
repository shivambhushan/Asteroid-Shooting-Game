#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using MA;

[CustomEditor(typeof(LevelMenuHelper))]
public class LevelMenueHelperEditor : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		LevelMenuHelper script = (LevelMenuHelper)target;
		if(GUILayout.Button("Update Scene Names"))
		{
			script.scenes = getSceneNames();
		}
		#region Warnings
		if((script.LevelItemPrefab == null)) 
		{
			EditorGUILayout.HelpBox("No LevelItemPrefab", MessageType.Warning);
			return;
		}
		#endregion

		#region Getting SceneNames
		if(GUILayout.Button("Generate LevelItems under ItemMenu"))
		{
			script.scenes = getSceneNames();
			for (int i = 0; i < script.scenes.Length; i++)
			{
				foreach(GameObject g in script.gameObject.GetChildren())
				{
					if(g.name == script.scenes[i]) continue;
				}
				GameObject newlevelItem = Instantiate(script.LevelItemPrefab);

				newlevelItem.transform.SetParent(script.transform, false);
				newlevelItem.GetComponent<LevelBaseItem>().initializeLevelItem(script.scenes[i], i);
			}
		}
		#endregion
	}
	public string[] getSceneNames()
	{
		string[] names = new string[EditorBuildSettings.scenes.Length];
		for (int i = 0; i < names.Length; i++)
		{
			EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
			string name = scene.path.Substring(scene.path.LastIndexOf('/')+1);
			names[i] = name.Substring(0,name.Length-6);
		}
		return names;
	}
}
#endif
