using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace MA{
	public class TextDial : Dial, MNumberFormat {
		[SerializeField]
		[GetOrAddComponent(true, false)]
		protected Text text;
		[Range(0, 20f)]
		[SerializeField]
		protected int numbersBehindComma = 3;
		[SerializeField]
		private string append = "%";
		private string format;

		void Awake()
		{
			format = this.formatNumbersBehindComma(numbersBehindComma);
			text.text = string.Format(format, value);
			displayProgress();
		}

		public override void displayProgress ()
		{
			base.displayProgress ();
			text.text = string.Format(format, value);
			if(!string.IsNullOrEmpty(append)) text.text += append;
		}
	}
}
