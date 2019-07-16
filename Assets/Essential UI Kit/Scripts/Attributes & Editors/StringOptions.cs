using UnityEngine;
using System.Collections;

namespace MA{
	/// <summary>
	/// String options. Showns an enum Field with the displaynames and sets the choice to the
	/// corresponding value. Can be implicitly converted to the chosen string. If ownChoice
	/// is set to true
	/// </summary>
	[System.Serializable]
	public class StringOptions{
		public string[] optionnames;
		public string[] options;
		public string chosenoption = "";
		public string choice;
		public bool OwnChoiceAllowed = true;

		public StringOptions(string[] displaynames, string[] values, bool ChoiceAllowed = true)
		{
			this.optionnames = displaynames;
			this.options = values;
			this.OwnChoiceAllowed = ChoiceAllowed;
			if(values.Length > 0) choice = values[0];
		}

		public static implicit operator string(StringOptions o)
		{
			return o.choice;
		}
	}
}
