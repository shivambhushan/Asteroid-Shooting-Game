using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(RectTransform))]
	public class DraggablePage : Swipable, IPage, MBoundsCheck {
		[SerializeField]
		[GetOrAddComponent(false, true)]
		protected PageController controller;
		private RectTransform rectTransform, controllerRect;
		private Camera cam;
		public UnityEvent OnActivation, OnDeactivation;

		protected virtual void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
			controllerRect = controller.GetComponent<RectTransform>();
			//get the Canvas Camera and if the Canvas doesnt't have one take the Main Camera
			cam = GetComponentInParent<Canvas>().worldCamera;
			cam = (cam == null) ? Camera.main : cam;
		}

		//See if we're out of bounds based on the parent Container. True if any Corner is out of Bounds
		protected override bool boundsCheck()
		{
			return this.checkOutsideBounds(rectTransform, controllerRect, cam);
		}
		public override void OnEndDrag (PointerEventData eventData)
		{
			if(boundsCheck())
			{
				if(vertical)
				{
					if(previousPosition.y < transform.position.y) signalRightOrUp();
					else signalLeftOrDown();
				}else
				{
					if(previousPosition.x < transform.position.x) signalRightOrUp();
					else signalLeftOrDown();
				}
			}
			else returnToPrevious(previousPosition);
		}
		private void signalLeftOrDown()
		{
			ExecuteEvents.ExecuteHierarchy<IChangePage>(gameObject,null,(x, y)=>{
				if(!x.changeDown()) returnToMiddle();
			});
		}
		private void signalRightOrUp()
		{
			ExecuteEvents.ExecuteHierarchy<IChangePage>(gameObject,null,(x, y)=>{
				if(!x.changeUp()) returnToMiddle();
			});
		}

		#region IPage implementation
		public virtual void leaveUpOrRight ()
		{
			transform.position = controller.rightOrUpPosition.position;
		}
		public virtual void leaveDownOrLeft ()
		{
			transform.position = controller.leftOrDownPosition.position;
		}
		public virtual void enterUpOrRight ()
		{
			transform.position = controller.activePositon.position;
		}
		public virtual void enterDownOrLeft ()
		{
			transform.position = controller.activePositon.position;
		}
		public virtual void returnToMiddle ()
		{
			transform.position = controller.activePositon.position;
		}
		public GameObject pageObject {
			get {
				return gameObject;
			}
		}
		
		public virtual void onBecameActive ()
		{
			OnActivation.Invoke();
		}
		
		public virtual void onBecameInactive ()
		{
			OnDeactivation.Invoke();
		}

		#endregion
	}
}
