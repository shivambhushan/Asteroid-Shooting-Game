using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;

namespace MA{
	/// <summary>
	/// Replaces the text on a Text Component with random Numbers. 
	/// Then gradually reveals the text while replacing the
	/// numbers with the string Character after Character.
	/// </summary>
	public class TextReveal : MonoBehaviour {
		[GetOrAddComponent]
		[SerializeField]
		protected Text textComponent;
		[SerializeField]
		protected float revealSpeed = 0.1f;
		[SerializeField]
		protected bool revealOnStartup = true;
		private WaitForSeconds wait;
		protected StringBuilder builder;


		protected virtual void Awake () {
			RevealSpeed = revealSpeed;
			builder = new StringBuilder();
			if(revealOnStartup) replaceAndReveal(textComponent.text);
		}
		public void replaceAndReveal(string message)
		{
			StartCoroutine(reveal(message));
		}
		private IEnumerator reveal(string text)
		{
			string revealed, toReveal;

			for(int i = 0; i <= text.Length; i++){
				revealed = text.Substring(0,i);
				toReveal = stringReplacer(text.Substring(i,text.Length-i));
				textComponent.text = revealed + toReveal;
				yield return wait;
			}
		}
		//Simply show nothing at all 
		protected virtual string stringReplacer(string toReplace)
		{
			return "";
		}

		public float RevealSpeed {
			get {
				return revealSpeed;
			}
			set {
				revealSpeed = value;
				wait = new WaitForSeconds(value);
			}
		}
	}
}
