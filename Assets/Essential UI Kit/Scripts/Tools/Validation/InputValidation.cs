using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace MA{
	[RequireComponent(typeof(InputField))]
	public class InputValidation : MonoBehaviour {
		[SerializeField]
		protected bool onlyCheckOnEndEdit = true;
		protected AbstractInputValidator[] validators = new AbstractInputValidator[0];
		protected bool isValidInput;
		[Space(5)]
		public InputField.SubmitEvent onInputValidated;
		public UnityEvent onValidationFailure;
		protected InputField inputfield;
		[HideInInspector]
		public string validatedInput = "";

		protected virtual void Awake()
		{
			validators = GetComponents<AbstractInputValidator>();
			inputfield = GetComponent<InputField>();
			if(onlyCheckOnEndEdit) 
			{
				inputfield.onEndEdit.AddListener(validateInputAgainstAllValidators);
			}else
			{
				inputfield.onValueChange.AddListener(validateInputAgainstAllValidators);
			}
		}

		protected void validateInputAgainstAllValidators(string input)
		{
			foreach(AbstractInputValidator validator in validators)
			{
				if(!validator.validateInput(input))
				{
					handleInputFailure(validator.failurmessage);
					onValidationFailure.Invoke();
					return;
				}
			}
			isValidInput = true;
			validatedInput = input;
			handleInputSuccess();
			onInputValidated.Invoke(input);
		}
		protected virtual void handleInputFailure(string failuremessage)
		{
			Debug.Log("Invalid");
		}
		protected virtual void handleInputSuccess()
		{
			Debug.Log("Valid");
		}

		public bool IsValidInput {
			get {
				return isValidInput;
			}
		}

		public string ValidatedInput {
			get {
				return validatedInput;
			}
		}
	}
}
