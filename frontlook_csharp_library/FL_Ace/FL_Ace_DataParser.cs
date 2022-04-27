using System;

namespace frontlook_csharp_library.FL_Ace
{
	public static class FL_Ace_DataParser
	{
		//public const string DBF_Empty_Value = "null";
		public static object FL_AceDataTableDataParser(this object value)
		{
			string DBF_Empty_Value = "null";
			Type type;
			if (value == null)
			{
				return DBF_Empty_Value;
			}
			else
			{
				type = value.GetType();
				if (type == typeof(string))
				{
					return $"'{value.ToString().Replace("'", "''")}'";
				}
				else if (type == typeof(Boolean))
				{
					var val = $"{(bool)value}";
					return val;
				}
				else if (type == typeof(double) || type == typeof(int) || type == typeof(long) || type == typeof(decimal))
				{
					return (double)value;
				}
				else if (type == typeof(DateTime))
				{
					var val = $"#{(DateTime)value:MM-dd-yyyy}#";
					return val;
				}
				else
				{
					return DBF_Empty_Value;
				}
			}

		}
	}
}
