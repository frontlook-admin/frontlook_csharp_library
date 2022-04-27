using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace frontlook_csharp_library.FL_General.FL_string_helper
{
	public static class FL_StringHelper
	{
		public enum FL_StringType
		{
			NullOrEmpty = 1,
			String = 2,
			Double = 3,
			Int = 4,
			Date = 5
		}

		public static string FL_RepeatText([CanBeNull] this string text, int Count, bool withSpace = false)
		{
			if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
			{
				return "";
			}
			else
			{
				var stri = "";
				for (int i = 0; i < Count; i++)
				{
					stri += text + (withSpace ? " " : "");
				}
				return stri;
			}
		}

		public static int? FL_CheckInteger([CanBeNull] this string str)
		{
			if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
			{
				return null;
			}
			try
			{
				var f = int.Parse(str);
				if (str.Length == f.ToString().Length)
				{
					return f;
				}
				else
				{
					return null;
				}
			}
			catch
			{
				return null;
			}
		}

		public static double? FL_CheckDouble([CanBeNull] this string str)
		{
			if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
			{
				return null;
			}
			try
			{
				var f = double.Parse(str);
				if (str.Length == f.ToString(CultureInfo.CurrentCulture).Length)
				{
					return f;
				}

				return null;
			}
			catch
			{
				return null;
			}
		}

		public static FL_StringType FL_GetStringType(this string str)
		{
			return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) ? FL_StringType.NullOrEmpty :
				str.FL_CheckInteger() != null ? FL_StringType.Int :
				str.FL_CheckInteger() != null ? FL_StringType.Double :
				str.FL_ParseDateTime() != null ? FL_StringType.Date : FL_StringType.String;
		}

		public static object FL_GetObject(this string str)
		{
			return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str) ? "" :
				str.FL_CheckInteger() != null ? int.Parse(str) :
				str.FL_CheckInteger() != null ? double.Parse(str) :
				str.FL_ParseDateTime() != null ? (object)DateTime.ParseExact(str, FL_DateHelper.FL_DateParseFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal) : "";
		}

		public static object FL_SetICellType(this string str)
		{
			if (str.FL_GetStringType() == FL_StringType.String || str.FL_GetStringType() == FL_StringType.NullOrEmpty)
			{
				return str;
			}
			if (str.FL_GetStringType() == FL_StringType.Int)
			{
				return int.Parse(str);
			}
			if (str.FL_GetStringType() == FL_StringType.Double)
			{
				return double.Parse(str);
			}
			if (str.FL_GetStringType() == FL_StringType.Date)
			{
				return DateTime.ParseExact(str, FL_DateHelper.FL_DateParseFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
			}
			else
			{
				return str;
			}
		}

		public static string FL_StringModifierWithPattern([CanBeNull] this string Source, [CanBeNull] string Pattern, [CanBeNull] string ReplaceWith, [CanBeNull] string ToReplace = null)
		{
			if (string.IsNullOrEmpty(Source) || string.IsNullOrEmpty(ReplaceWith) || string.IsNullOrEmpty(Pattern)) return Source;
#if DEBUG
			Debug.WriteLine(Source);
			Console.WriteLine(Source);
#endif
			//string source = "The mountains are still there behind the clouds today.";

			// Use Regex.Replace for more flexibility.
			// Replace "the" or "The" with "many" or "Many".
			// using System.Text.RegularExpressions
			//string replaceWith = "many ";
			Source = Regex.Replace(Source,
				@Pattern, LocalReplaceMatchCase,
				RegexOptions.IgnoreCase);

			string LocalReplaceMatchCase(Match MatchExpression)
			{
				// Test whether the match is capitalized
				if (ToReplace != null)
				{
					if (!string.Equals(MatchExpression.Value, ToReplace, StringComparison.CurrentCultureIgnoreCase)) return MatchExpression.Value;
					if (!char.IsUpper(MatchExpression.Value[0])) return ReplaceWith;
					// Capitalize the replacement string
					var replacementBuilder = new StringBuilder(ReplaceWith);
					replacementBuilder[0] = char.ToUpper(replacementBuilder[0]);
					return replacementBuilder.ToString();
				}

				if (!char.IsUpper(MatchExpression.Value[0])) return ReplaceWith;
				{
					// Capitalize the replacement string
					var replacementBuilder = new StringBuilder(ReplaceWith);
					replacementBuilder[0] = char.ToUpper(replacementBuilder[0]);
					return replacementBuilder.ToString();
				}
			}

#if DEBUG
			return Source.FL_ConsoleWriteDebug();
#else
            return Source;
#endif
		}

		public static string FL_ToTitleCase(this string text)
		{
			CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
			TextInfo textInfo = cultureInfo.TextInfo;
			var line = textInfo.ToTitleCase(text.ToLower());
			return line;
		}

		public static string FL_StringModifierWithPattern([CanBeNull] this string Source, [CanBeNull] Regex Pattern, [CanBeNull] string ReplaceWith, [CanBeNull] string ToReplace = null)
		{
			return Pattern != null ? Source.FL_StringModifierWithPattern(Pattern.ToString(), ReplaceWith, ToReplace) : Source;
		}
	}
}