using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA.Examples{
	[RequireComponent(typeof(Text))]
	public class ValidatorExample : MonoBehaviour {
		private Text textComponent;
		private string validatedYear = "Last valid Year: ", validateEmail = "Last valid E-mail: ";
		[HideInInspector]
		public bool yearValid, emailValid;

		void Awake () {
			textComponent = GetComponent<Text>();
		}

		
		public string ValidatedYear {
			get {
				return validatedYear;
			}
			set {
				validatedYear = "Last valid Year: " + value;
				yearValid = true;
				updateOutput();
			}
		}

		public string ValidatedDomain {
			get {
				return validateEmail;
			}
			set {
				validateEmail = "Last valid E-mail: " + value;
				emailValid = true;
				updateOutput();
			}
		}
		public void updateOutput(){
			string output = validatedYear + System.Environment.NewLine + "Currently ";
			output += yearValid ? "<color='green'>Valid</color>" : "<color='red'>Invalid</color>";
			output += System.Environment.NewLine + ValidatedDomain + System.Environment.NewLine + "Currently ";
			output += emailValid ? "<color='green'>Valid</color>" : "<color='red'>Invalid</color>";
			textComponent.text = output;
		}

		public bool YearValid {
			get {
				return yearValid;
			}
			set {
				yearValid = value;
				updateOutput();
			}
		}

		public bool EmailValid {
			get {
				return emailValid;
			}
			set {
				emailValid = value;
				updateOutput();
			}
		}
	}
}
