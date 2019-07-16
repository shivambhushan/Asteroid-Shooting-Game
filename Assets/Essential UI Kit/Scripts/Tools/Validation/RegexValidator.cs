using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace MA{
	public class RegexValidator : AbstractInputValidator {
		/*
		 * Useful Regex Examples from Stack Overflow: http://stackoverflow.com/questions/1449817/what-are-some-of-the-most-useful-regular-expressions-for-programmers
		Decimals input

		Positive Integers  ^\d+$

		Negative Integers  ^-\d+$

		Integer ^-?\d+$

		Positive Number ^\d*\.?\d+$

		Negative Number  ^-\d*\.?\d+$

		Positive Number or Negative Number  ^-?\d*\.?\d+$

		Phone number ^\+?[\d\s]{3,}$

		Phone with code  ^\+?[\d\s]+\(?[\d\s]{10,}$

		Year 1900-2099  ^(19|20)\d{2}$

		Date (dd mm yyyy, d/m/yyyy, etc.)

		^([1-9]|0[1-9]|[12][0-9]|3[01])\D([1-9]|0[1-9]|1[012])\D(19[0-9][0-9]|20[0-9][0-9])$ 
		
		IP v4 --- ^(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]).(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]){3}$

		Alphabetic input

		Personal Name ^[\w.']{2,}(\s[\w.']{2,})+$
		Username  ^[\w\d_.]{4,}$
		Password at least 6 symbols  ^.{6,}$
		Password or empty input  ^.{6,}$|^$
		email ^[_]*([a-z0-9]+(\.|_*)?)+@([a-z][a-z0-9-]+(\.|-*\.))+[a-z]{2,6}$
		domain ^([a-z][a-z0-9-]+(\.|-*\.))+[a-z]{2,6}$
		Other regular expressions - Match no input  ^$ - Match blank input ^\s\t*$ - Match New line [\r\n]|$ - Match white Space  ^\s+$ - Match Url = ^http\:\/\/[a-zA-Z0-9.-]+\.[a-zA-Z]{2,3}$
		 */
		public StringOptions regexToMatch = new StringOptions(new string[5]{"Year between 1900-2099", "Domain", "URL", "Date", "E-Mail"}, 
		new string[5]{@"^(19|20)\d{2}$", @"^([a-z][a-z0-9-]+(\.|-*\.))+[a-z]{2,6}$", @"^http\:\/\/[a-zA-Z0-9.-]+\.[a-zA-Z]{2,3}$", 
			@"^([1-9]|0[1-9]|[12][0-9]|3[01])\D([1-9]|0[1-9]|1[012])\D(19[0-9][0-9]|20[0-9][0-9])$", @"^[_]*([a-z0-9]+(\.|_*)?)+@([a-z][a-z0-9-]+(\.|-*\.))+[a-z]{2,6}$"});
		private Regex regex;

		void Awake()
		{
			regex = new Regex(regexToMatch);
		}

		public override bool validateInput (string input)
		{
			return regex.IsMatch(input);
		}
	}
}
