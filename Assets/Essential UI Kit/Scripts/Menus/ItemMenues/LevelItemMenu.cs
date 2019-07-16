using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace MA{
	public class LevelItemMenu : BaseItemMenu<LevelItem> {

		void Awake () {
			registerChildrenAsItems();

		}

		public bool addItem(GameObject prefab, string itemname = "", bool insertAsFirst = false)
		{
			if(prefab.GetComponent<LevelItem>() == null) return false;
			GameObject go = Instantiate(prefab) as GameObject;
			go.transform.SetParent(transform, false);
			if(insertAsFirst) go.transform.SetAsFirstSibling();
			return registerItem(go.GetComponent<LevelItem>());
		}
	}
}

