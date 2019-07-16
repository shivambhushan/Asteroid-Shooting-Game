using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace MA{
	/// <summary>
	/// List controller. A List can have one of it's Children selected and hold's a
	/// Reference to the last selected Element.
	/// </summary>
	public class ListController : MonoBehaviour, IListElementSelected {
		protected ListElement currentlySelected = null;
		protected List<ListElement> elements = new List<ListElement>();
		
		protected void Start () {
			buildList();
		}

		public void buildList()
		{
			elements.Clear();
			foreach(GameObject go in gameObject.GetChildren())
			{
				ListElement element = go.GetComponent<ListElement>();
				if(element == null) continue;
				elements.Add(element);
			}
		}

		public ListElement CurrentlySelected {
			get {
				return currentlySelected;
			}
		}
		public T CurrentlySelectedComponent<T>()where T : Component{
			if(currentlySelected != null) return currentlySelected.GetComponent<T>();
			else return null;
		}

		#region IListElementSelected implementation

		public virtual void elementSelected (ListElement elem)
		{
			currentlySelected = elem;
		}

		#endregion
		public int index{
			get{
				if(currentlySelected != null ) return elements.IndexOf(currentlySelected);
				else return -1;
			}
		}
	}
}
