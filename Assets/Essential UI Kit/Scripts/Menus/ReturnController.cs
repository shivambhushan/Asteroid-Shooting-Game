using UnityEngine;
using System.Collections;

namespace MA{
	/// <summary>
	/// Using Keys to return to the next higher Menu from the current Menu
	/// </summary>
	public class ReturnController : MonoBehaviour {
		[SerializeField]
		private KeyCode returnKey = KeyCode.Escape;
		
		void Update () {
			if(Input.GetKeyDown(returnKey)) UIMenu.returnToHigherMenu();	
		}
	}
}