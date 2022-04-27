using System;
using System.Linq;

namespace frontlook_csharp_library.FL_General.FL_string_helper
{
	public static class FL_NumberStrings
	{
		public static string[] currencyDenotionIndian => new[] { "Rupees", "Paise" };

		public static string FL_NumberToWordsFull(this long number)
		{
			if (number == 0) { return "zero"; }
			if (number < 0) { return "minus " + FL_NumberToWordsFull(Math.Abs(number)); }
			string words = "";
			if ((number / 10000000) > 0) { words += FL_NumberToWordsFull(number / 10000000) + " Crore "; number %= 10000000; }
			if ((number / 100000) > 0) { words += FL_NumberToWordsFull(number / 100000) + " Lakh "; number %= 100000; }
			if ((number / 1000) > 0) { words += FL_NumberToWordsFull(number / 1000) + " Thousand "; number %= 1000; }
			if ((number / 100) > 0) { words += FL_NumberToWordsFull(number / 100) + " Hundred "; number %= 100; }

			if (number <= 0) return words;
			if (words != "") { words += "and "; }
			var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
			var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "seventy", "Eighty", "Ninety" };
			if (number < 20) { words += unitsMap[number]; }
			else { words += tensMap[number / 10]; if ((number % 10) > 0) { words += "-" + unitsMap[number % 10]; } }
			return words;
		}

		public static string FL_NumberToWordsFullAfterPoint(string number, bool IfCurrency)
		{
			var word = string.Empty;
			if (long.Parse(number) <= 0) return word;
			if (IfCurrency)
			{
				if (number.Length == 1)
				{
					number = number + "0";
				}
				word = FL_NumberToWordsFull(long.Parse(number));
			}
			else
			{
				var pt = number.ToCharArray();
				word = pt.Aggregate(word,
					(current, n) => current + (FL_NumberToWordsFull(long.Parse(n.ToString())) + " "));
			}


			return word;
		}

		public static string FL_NumberToWordsFull(this double number, bool IfCurrency, bool showRupees = false)
		{

			var num = number.ToString().Split('.');
			var words1 = FL_NumberToWordsFull(long.Parse(num[0]));

			var word2 = string.Empty;
			if (num.Length > 1 && long.Parse(num[1]) > 0)
			{

				word2 = FL_NumberToWordsFullAfterPoint(num[1], IfCurrency);
			}

			if (IfCurrency)
			{
				if (showRupees)
				{
					return num.Length > 1 && long.Parse(num[1]) > 0 ? $"{currencyDenotionIndian[0]} {words1.Replace(" and ", " ")} And {word2} {currencyDenotionIndian[1]} Only"
						: $"{currencyDenotionIndian[0]} {words1} Only";
				}
				return num.Length > 1 && long.Parse(num[1]) > 1 ? $"{words1.Replace(" and ", " ")} And {word2} {currencyDenotionIndian[1]} Only"
					: $"{words1} Only";
			}

			return num.Length > 1 && long.Parse(num[1]) > 0 ? $"{words1} Point {word2}"
				: $"{words1}";

		}



		public static string FL_NumberToWordsMinimised(long number)
		{
			string words = "";
			string unit = "";
			int divider = 100;
			if (number > 99 && number < 999)
			{
				divider = 100;
				unit = "Hundred";
			}
			else if (number >= 999 && number < 99999)
			{
				divider = 1000;
				unit = "Thousand";
			}
			else if (number >= 99999 && number < 9999999)
			{
				divider = 100000;
				unit = "Lakh";
			}
			else if (number >= 9999999)
			{
				divider = 10000000;
				unit = "Crore";
			}
			decimal no = Convert.ToDecimal(Convert.ToDecimal(number) / divider);
			if (Math.Floor(no) != no)
			{
				words = no.ToString("F1") + " " + unit;
			}
			else
			{
				words = no + " " + unit;
			}
			return words;
		}
	}

}
