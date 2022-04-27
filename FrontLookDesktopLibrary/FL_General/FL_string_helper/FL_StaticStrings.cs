namespace frontlook_csharp_library.FL_General.FL_string_helper
{
	public static class FL_StaticStrings
	{
		public static string FL_BankDetailsAuthCode => "DY-X-Authorization: cf33ce6c8c0b75abe44a2b0a925948db5bf060c0";
		public static string FL_Correct => "✅";
		public static string FL_Wrong => "❌";

		public static string FL_Correct_Wrong(this bool BooleanValue)
		{
			return BooleanValue ? FL_Correct : FL_Wrong;
		}

		public static string FL_INR => "₹";

		public static string[] FL_Indian_States => new string[]
		{
			"Andhra Pradesh", "Assam", "Arunachal Pradesh", "Bihar", "Goa", "Gujarat", "Jharkhand", "West Bengal", "Karnataka", "Kerala", "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland", "Orissa", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Tripura", "Uttarakhand (Uttaranchal)", "Uttar Pradesh", "Haryana", "Himachal Pradesh", "Chhattisgarh"
		};

		public static string[] FL_Indian_UnionTerritories => new string[]
		{
			"Andaman and Nicobar", "Pondicherry", "Dadra and Nagar Haveli", "Daman and Diu", "Delhi", "Chandigarh", "Jammu and Kashmir", "Ladakh", "Lakshadweep"
		};

		public static string[] FL_Indian_States_And_Uts => new string[]
		{
			"Andhra Pradesh", "Assam", "Arunachal Pradesh", "Bihar", "Goa", "Gujarat", "Jharkhand", "West Bengal", "Karnataka", "Kerala", "Madhya Pradesh", "Maharashtra", "Manipur", "Meghalaya", "Mizoram", "Nagaland", "Orissa", "Punjab", "Rajasthan", "Sikkim", "Tamil Nadu", "Tripura", "Uttarakhand (Uttaranchal)", "Uttar Pradesh", "Haryana", "Himachal Pradesh", "Chhattisgarh",
			"Andaman and Nicobar", "Pondicherry", "Dadra and Nagar Haveli", "Daman and Diu", "Delhi", "Chandigarh", "Jammu and Kashmir", "Ladakh", "Lakshadweep"
		};

		public static string FL_UnSpaced(this string s)
		{
			return s.Replace(" ", string.Empty);
		}

		public enum FL_MonthCombination
		{
			Monthly = 1,
			BiMonthly = 2,
			Quarterly = 3,
			HalfYearly = 6,
			Yearly = 12
		}
	}
}