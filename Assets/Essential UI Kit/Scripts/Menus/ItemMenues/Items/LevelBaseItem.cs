using UnityEngine;
using System.Collections;

namespace MA{
	public abstract class LevelBaseItem : MonoBehaviour {
		[SerializeField] protected string levelname;
		[SerializeField] protected int levelID;

		public virtual void initializeLevelItem(string name, int id)
		{
			this.levelname = name;
			this.levelID = id;
		}
	}
}
