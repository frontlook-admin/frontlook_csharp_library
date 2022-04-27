using System;
using System.Diagnostics;
using System.Numerics;

namespace frontlook_csharp_library.FL_General
{
	public static class FL_ConsoleManager
	{
		public static void FL_ConsoleWrite(this string gString)
		{
			Console.WriteLine(gString);
		}

		public static string FL_ConsoleWriteDebug(this string gString)
		{
			Console.WriteLine(gString);
			Debug.WriteLine(gString);
			return gString;
		}

		public static int FL_ConsoleWriteDebug(this int gString)
		{
			Console.WriteLine(gString);
			Debug.WriteLine(gString);
			return gString;
		}

		public static double FL_ConsoleWriteDebug(this double gString)
		{
			Console.WriteLine(gString);
			Debug.WriteLine(gString);
			return gString;
		}

		public static string FL_WriteDebug(this string gString)
		{
			Debug.WriteLine(gString);
			return gString;
		}

		public static int FL_WriteDebug(this int gString)
		{
			Debug.WriteLine(gString);
			return gString;
		}

		public static double FL_WriteDebug(this double gString)
		{
			Debug.WriteLine(gString);
			return gString;
		}

		public static void FL_ConsoleBeep(this Object any)
		{
			Console.Beep();
			Debug.WriteLine(any.ToString());
		}

		public static Object FL_ConsoleBeepDebug(this Object any)
		{
			Console.Beep();
			return any;
		}

		public static BigInteger FL_ConsoleWriteDebug(this BigInteger gString)
		{
			Console.WriteLine(gString);
			Debug.WriteLine(gString);
			return gString;
		}

		public static BigInteger FL_WriteDebug(this BigInteger gString)
		{
			Debug.WriteLine(gString);
			return gString;
		}

		/*public static Object FL_ConsolePingGoogleServer()
        {
            var result = Cli.Wrap("")
                .WithArguments("ping 8.8.8.8 -t")
                .ExecuteBufferedAsync(Encoding.ASCII, Encoding.UTF8);
            var v = result.Select(e => e.StandardOutput);
            v.Select(e => e.ToString()).ToString().FL_ConsoleWriteDebug();
            return result.Select(e => e.StandardOutput).ToString().FL_ConsoleBeepDebug()
                .ToString().FL_ConsoleWriteDebug();
        }*/
	}
}