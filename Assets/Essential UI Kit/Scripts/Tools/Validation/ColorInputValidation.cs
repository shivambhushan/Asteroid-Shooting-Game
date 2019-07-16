using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA{
	public class ColorInputValidation : InputValidation {
		[SerializeField]
		protected Image imageToColor;
		[SerializeField]
		protected Color failureColor = Color.red, validatedColor = Color.green;

		protected override void Awake ()
		{
			base.Awake ();
			if(imageToColor == null) imageToColor = inputfield.image;
		}

		protected override void handleInputFailure (string failuremessage)
		{
			imageToColor.color = failureColor;
		}
		protected override void handleInputSuccess ()
		{
			imageToColor.color = validatedColor;
		}
	}
}
