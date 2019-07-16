using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MA{
	/// <summary>
	/// Base item menu. Backed up by a Dictionary of provided Type T (derived from Component).
	/// Keys are the Instance Ids of the Gameobjects associated with the Component T.
	/// The instance IDs will furthermore be accessible under the name of the GameObject
	/// they were registered with
	/// </summary>
	public abstract class BaseItemMenu<T> : MonoBehaviour where T : Component{
		protected Dictionary<int, T> items = new Dictionary<int, T>();
		protected Dictionary<string, List<int>> nameLookUp = new Dictionary<string, List<int>>();

		public bool itemExists(string name)
		{
			return nameLookUp.ContainsKey(name);
		}
		public bool itemExists(int id)
		{
			return items.ContainsKey(id);
		}
		public bool anyItemExists(params string[] names)
		{
			foreach(string name in names)
			{
				if(itemExists(name)) return true;
			}
			return false;
		}
		protected void registerChildrenAsItems()
		{
			foreach(T t in gameObject.GetComponentsInChildren<T>())
			{
				registerItem(t);
			}
		}
		public bool removeAllItemsWithName(string name, bool onlyDeactivate = true)
		{
			if(itemExists(name))
			{
				foreach(int id in nameLookUp[name])
				{
					T item = items[id];
					items.Remove(id);
					disposeOfItem(item);
				}
				nameLookUp.Remove(name);
				return true;
			}else return false;
		}
		public bool removeItem(int id)
		{
			if(itemExists(id))
			{
				T item = items[id];
				items.Remove(id);
				if(item != null)
				{
					List<int> ids = nameLookUp[item.name];
					ids.Remove(id);
					if(ids.Count > 0) nameLookUp[item.name] = ids;
					else nameLookUp.Remove(item.name);

					disposeOfItem(item);
				}
				return true;
			}else return false;
		}
		public T getItem(string name)
		{
			if(itemExists(name))
			{
				return items[nameLookUp[name][0]];
			}else return default(T);
		}
		public T getItem(int id)
		{
			if(itemExists(id))
			{
				return items[id];
			}else return null;
		}
		public List<T> getAllItems(string name)
		{
			if(itemExists(name))
			{
				List<T> matches = new List<T>();
				foreach(int id in nameLookUp[name])
				{
					matches.Add(items[id]);
				}
				return matches;
			}else return null;
		}
		protected virtual void disposeOfItem(T item)
		{
			
		}
		/// <summary>
		/// Registers the item with the Component instance ID
		/// </summary>
		/// <returns><c>true</c>, if item was registered, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		protected bool registerItem(T item)
		{
			int id = item.GetInstanceID();
			//if we already have this item Registered just exit
			if(itemExists(id))
			{
				return false;
			}else
			{
				//if we have a gameobject with the same name just add the id
				if(itemExists(item.name))
				{
					nameLookUp[item.name].Add(id);
				}else
				{
					//otherwise create a new nameLookUp entry
					nameLookUp.Add(item.name, new List<int>(){id});
				}
				items.Add(id, item);
				return true;
			}
		}
	}
}
