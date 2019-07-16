using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace MA{
	/// <summary>
	/// Controller for Children Pages (implementing IPage). Switches Pages between an Active and two inactive(Off Screen) States.
	/// Can also be used to access the current page index and total amount of pages.
	/// 
	/// </summary>
	public class PageController : MonoBehaviour, IChangePage {
		private List<IPage> pages = new List<IPage>();
		public Transform leftOrDownPosition, rightOrUpPosition, activePositon;
		private IPage currentPage = null;
		private IStatusChanged[] controllerVisuals;
		[SerializeField]
		private bool BlockOnTransition;
		[Header("This is the Index. So Page 1 would have the index 0")]
		[SerializeField]
		private int pageOpenOnStart = 0;
		[HideInInspector]
		public bool Blocked;

		void Start () {
			buildPageList(pageOpenOnStart);
		}
		/// <summary>
		/// (Re)Builds the page list with all current Children implementing IPage. Sets the active Page to 
		/// the int parameter
		/// </summary>
		public void buildPageList(int openedPage)
		{
			pages.Clear();
			int counter = 0;
			foreach(GameObject go in gameObject.GetChildren())
			{
				IPage page = go.GetComponent<IPage>();
				if(page == null) continue;
				pages.Add(page);
				page.pageObject.transform.position = (counter < openedPage) ? leftOrDownPosition.position : rightOrUpPosition.position;
				counter++;
			}
			controllerVisuals = GetComponents<IStatusChanged>();
			if(openedPage < pages.Count)
			{
				currentPage = pages[openedPage];
				currentPage.pageObject.transform.position = activePositon.position;
				currentPage.onBecameActive();
			}
			foreach(IStatusChanged st in controllerVisuals)
			{
				st.initialize();
				if(openedPage < pages.Count) st.statusChanged(openedPage);
			}
		}

		public void changeToPage (int index)
		{
			if((index >= pages.Count) || index < 0)
			{
				currentPage.returnToMiddle();
				return;
			}
			if(BlockOnTransition && Blocked) return;
			foreach(IStatusChanged st in controllerVisuals)
			{
				st.statusChanged(index);
			}
			if(index > pages.IndexOf(currentPage))
			{
				currentPage.leaveDownOrLeft();
				currentPage.onBecameInactive();
				currentPage = pages[index];
				currentPage.onBecameActive();
				currentPage.enterUpOrRight();
			}
			else if (index < pages.IndexOf(currentPage))
			{
				currentPage.leaveUpOrRight();
				currentPage.onBecameInactive();
				currentPage = pages[index];
				currentPage.onBecameActive();
				currentPage.enterDownOrLeft();
			}else return;
		}

		public bool changeUp()
		{
			int newIndex = pages.IndexOf(currentPage) +1;
			if(newIndex >= pages.Count) return false;
			changeToPage(newIndex);
			return true;
		}
		public bool changeDown()
		{
			int newIndex = pages.IndexOf(currentPage) -1;
			if(newIndex < 0) return false;
			changeToPage(newIndex);
			return true;
		}

		public IPage CurrentPage {
			get {
				return currentPage;
			}
		}
		public int PageAmount{
			get{
				return pages.Count;
			}
		}
		public int CurrentPageIndex{
			get{
				if(currentPage != null) return pages.IndexOf(currentPage);
				else return -1;
			}
		}

		public int PageOpenOnStart {
			get {
				return pageOpenOnStart;
			}
			set {
				pageOpenOnStart = value;
				buildPageList(value);
			}
		}
	}
}

