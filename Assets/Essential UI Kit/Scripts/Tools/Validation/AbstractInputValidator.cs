using UnityEngine;
using System.Collections;

namespace MA{
	public abstract class AbstractInputValidator : MonoBehaviour {
		[Multiline]
		public string failurmessage;

		public virtual bool validateInput(string input)
		{
			throw new System.NotImplementedException();
		}
	}
}
