using UnityEngine;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(LevelItemMenu))]
	public class LevelMenuHelper : MonoBehaviour {
		public string[] scenes;
		[RequireDependencies(typeof(LevelBaseItem))]
		public GameObject LevelItemPrefab;

	}
}
