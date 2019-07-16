using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MA{
	/// <summary>
	/// A Menu which transitions to another UI Menu using the ITransitionMenu Components
	/// on the Gameobject. Events will be called when the Menu opens or closes.
	/// </summary>
	public class UIMenu : MonoBehaviour{
		[SerializeField]
		[Header("Open new Menu only after being done closing this one?")]
		private bool sequential = true;
		[Header("Things that should be done after this is opened")]
		public UnityEvent OnOpen;
		[Header("Things that should be done after this is closed")]
		public UnityEvent OnClose;
		private List<IMenuTransition> animatables = new List<IMenuTransition>();
		private IMenuTransition animatableToCall;
		[Header("Simply leave empty if there should be no returning from the current Menu")]
		[SerializeField]
		private UIMenu higerMenu = null;
		private static UIMenu currentMenu = null;

		void Awake () {
			animatables = new List<IMenuTransition>(GetComponents<IMenuTransition>());
			//if there are no animatables on this gameobject disable Component
			if(animatables.Count == 0){
				Debug.Log("No Transition Component on GameObject "+gameObject.name);
				this.enabled = false;
				return;
			}
			animatables.Sort((IMenuTransition x, IMenuTransition y)=>{
				if(x.length == y.length) return 0;
				else return (x.length < y.length) ? -1 : 1;
			});
			animatableToCall = animatables[animatables.Count-1];
			animatables.Remove(animatableToCall);
		}
		public void openNext(UIMenu next)
		{
			if(sequential){
				//call the other menu's open Menu after this menu is done closing
				this.close(()=>{
					next.open(()=>{
						next.OnOpen.Invoke();
					});
					OnClose.Invoke();
				});
			}else
			{
				//just call the events after closing
				this.close(()=>{
					OnClose.Invoke();
				});
				//and open the other menu right away
				next.open(()=>{
					next.OnOpen.Invoke();
				});
			}
			currentMenu = next;
		}
		public static void returnToHigherMenu()
		{
			if(currentMenu != null && currentMenu.higerMenu != null) currentMenu.openNext(currentMenu.higerMenu);
		}
		public void returnToHigherMenuInstanceMethod()
		{
			returnToHigherMenu();
		}
		public void close (Action onFinished)
		{
			foreach(IMenuTransition animatable in animatables)
			{
				animatable.closeAnimation(()=>{});
			}
			AnimatableToCall.closeAnimation(onFinished);
		}
		public void open (Action onFinished)
		{
			foreach(IMenuTransition animatable in animatables)
			{
				animatable.openAnimation(()=>{});
			}
			AnimatableToCall.openAnimation(onFinished);
		}
		public IMenuTransition AnimatableToCall {
			get {
				return animatableToCall;
			}
		}
	}
}
