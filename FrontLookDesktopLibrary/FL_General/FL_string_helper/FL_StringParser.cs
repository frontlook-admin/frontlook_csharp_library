namespace frontlook_csharp_library.FL_General.FL_string_helper
{
	public static class FL_StringParser
	{
		public enum ObjectToFormat
		{
			Int, Long, Double, Float, Decimal
		}

		public static object FL_ParseObject(this object value, ObjectToFormat ToFormat)
		{
			switch (ToFormat)
			{
				case ObjectToFormat.Double:
					try
					{
						return double.Parse((string)value);
					}
					catch
					{
						return 0.00;
					}
				case ObjectToFormat.Decimal:
					try
					{
						return decimal.Parse((string)value);
					}
					catch
					{
						return 0.00;
					}

				case ObjectToFormat.Int:
					try
					{
						return int.Parse((string)value);
					}
					catch
					{
						return 0;
					}

				case ObjectToFormat.Long:
					try
					{
						return long.Parse((string)value);
					}
					catch
					{
						return 0;
					}

				case ObjectToFormat.Float:
					try
					{
						return float.Parse((string)value);
					}
					catch
					{
						return 0.00;
					}

				default:
					return value.ToString();
			}
		}
	}
}