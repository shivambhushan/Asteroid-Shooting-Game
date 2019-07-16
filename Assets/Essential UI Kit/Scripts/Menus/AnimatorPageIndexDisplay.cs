using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace MA{
	[RequireComponent(typeof(PageController))]
	public class AnimatorPageIndexDisplay : MonoBehaviour, IStatusChanged {
		private PageController controller;
		[SerializeField]
		private Transform indexContainer;
		[RequireDependencies(typeof(Animator), typeof(Button))]
		[SerializeField]
		private GameObject indexPrefab;
		private List<Animator> indexObjects = new List<Animator>();
		[SerializeField]
		private string animatorBoolAlias;
		private Animator currentAnim;
		private int boolID;

		#region IStatusChanged implementation
		public void initialize ()
		{
			if(indexPrefab == null) return;
			controller = GetComponent<PageController>();
			int difference = controller.PageAmount - indexObjects.Count;
			if(difference == 0) return;
			else if(difference > 0)
			{
				int startCount = indexObjects.Count;
				for(int i = 0; i< difference; i++)
				{
					GameObject go = Instantiate(indexPrefab);
					go.transform.SetParent(indexContainer, false);
					indexObjects.Add(go.GetComponent<Animator>());
					int iCopy = i + startCount;
					go.GetComponent<Button>().onClick.AddListener(()=>{
						controller.changeToPage(iCopy);
					});
				}
			}else{
				int numberToDelete = Mathf.Abs(difference);
				foreach(Animator anim in indexObjects.GetRange(indexObjects.Count- numberToDelete, numberToDelete))
				{
					Destroy(anim.gameObject);
				}
				indexObjects.RemoveRange(indexObjects.Count- numberToDelete, numberToDelete);
			}
			boolID = Animator.StringToHash(animatorBoolAlias);
			if(currentAnim != null) currentAnim.SetBool(boolID, false);
			if(controller.CurrentPageIndex < indexObjects.Count) {
				currentAnim = indexObjects[controller.CurrentPageIndex];
				currentAnim.SetBool(boolID, true);
			}
		}

		public void statusChanged (int newStatus)
		{
			currentAnim.SetBool(boolID, false);
			currentAnim = indexObjects[newStatus];
			currentAnim.SetBool(boolID, true);
		}
		#endregion
	}
}
