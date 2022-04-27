using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace frontlook_csharp_library.FL_General
{
	public static class FL_FileManager
	{
		public static Stream FL_GetFileStream(this IFormFile file, string fullPath)
		{
			if (string.IsNullOrEmpty(fullPath))
			{
				var mainstream = file.OpenReadStream();
				return mainstream;
			}
			else
			{
				var mainstream = new FileStream(fullPath, FileMode.Create);
				//await using var v = mainstream;
				//await file.CopyToAsync(mainstream);
				return mainstream;
			}
		}

		public static Stream FL_GetFileStream(this IFormFile file)
		{
			var mainstream = file.OpenReadStream();
			return mainstream;
		}

		private static readonly string[] Size_Suffixes =
			{ "Bytes", "KB", "MB", "GB", "TB", "PB" };

		public static string FL_FormatSize(Int64 bytes)
		{
			var counter = 0;
			var number = (decimal)bytes;

			while (Math.Round(number / 1024) >= 1 && counter <= 5)
			{
				number = number / 1024;
				counter++;
			}
			return $"{number:n1}{Size_Suffixes[counter]}";
		}

		public static double FL_FormatSizeInMb(this Int64 bytes)
		{
			var number = (double)bytes;

			var i = number / (1024 * 1024);
			return i;
		}

		public static double FL_FormatSizeInMb(this string FileName)
		{
			var f = new FileInfo(FileName);
			if (!f.Exists || f.Length <= 0) return 0;
			var number = f.Length;
			var i = number / (1024 * 1024);
			return i;
		}
	}
}