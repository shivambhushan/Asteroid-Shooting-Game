using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace MA{
	/// <summary>
	/// Typing effect for UI Text Component
	/// </summary>
	[RequireComponent(typeof(Text))]
	public class Typer : MonoBehaviour {
		[Header("If empty nothing will be shown on Startup")]
		[Multiline]
		[SerializeField]
		private string message = "Replace me with some Text," +
			"I'll be flying in";
		[SerializeField]
		private float typeDelay = 0.03f;
		[SerializeField]
		[Header("Text (Or Character) that will be shown just " +
			"after the typed in Text")]
		private string appendedText = "";
		[Header("This will be replaced with a new Line")]
		[SerializeField]
		private string newLinePlaceHolder = "___";
		private WaitForSeconds wait;
		[HideInInspector]
		public Text textComponent;
		private Coroutine typeRoutine;

		void Awake(){
			textComponent = GetComponent<Text>();
			TypeDelay = typeDelay;
		}
		void Start()
		{
			if(!string.IsNullOrEmpty(message)) AddText(message);
		}


		public void StopTyping()
		{
			if(typeRoutine != null) StopCoroutine(typeRoutine);
		}

		public void ReplaceText(string text) {
			StopTyping();
			text = text.Replace(newLinePlaceHolder, System.Environment.NewLine);
			typeRoutine = StartCoroutine(TypeIn(text));
		}
		public void AddText(string text) {
			StopTyping();
			text = text.Replace(newLinePlaceHolder, System.Environment.NewLine);
			typeRoutine = StartCoroutine(AddedTypeIn(text));
		}
		public void HideText() {
			StopTyping();
			typeRoutine = StartCoroutine(TypeOff());
		}

		private IEnumerator TypeIn(string text){

			for(int i = 0; i <= text.Length; i++){
				textComponent.text = (string.IsNullOrEmpty(appendedText) || i >= text.Length-1) ? text.Substring(0,i) : text.Substring(0, i) + appendedText;
				yield return wait;
			}
		}
		private IEnumerator AddedTypeIn(string text){
			string oldmessage = textComponent.text;
			for(int i = 0; i <= text.Length; i++){
				textComponent.text = oldmessage + ((string.IsNullOrEmpty(appendedText) || i >= text.Length-1) ? text.Substring(0,i) : text.Substring(0, i) + appendedText);
				yield return wait;
			}
		}
		private IEnumerator TypeOff(){

			int length = textComponent.text.Length;
			string oldmessage = textComponent.text;
			for(int i = length; i >= 0; i--){
				textComponent.text = (string.IsNullOrEmpty(appendedText) || i == 0) ? oldmessage.Substring(0,i) : oldmessage.Substring(0, i) + appendedText;
				yield return wait;
			}
		}

		public float TypeDelay {
			get {
				return typeDelay;
			}
			set {
				typeDelay = value;
				wait = new WaitForSeconds(value);
			}
		}
	}
}
