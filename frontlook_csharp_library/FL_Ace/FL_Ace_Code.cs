using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using frontlook_csharp_library.FL_Dbf_Helper;
using frontlook_csharp_library.FL_General;
using NPOI.SS.Formula.Functions;
using dh = frontlook_csharp_library.FL_Dbf_Helper.FL_Dbf_Manager;

namespace frontlook_csharp_library.FL_Ace
{
    public static class FL_Ace_Code
    {
        private const int paddChar = 2;

        public static string FL_SetNewCode(this string TableName, string FieldName, string DbfFolderPath,  bool UseDirectoryPath = true, bool DebugMode = false)
        {
            if (DebugMode)
            {
                var LastCode = TableName.FL_GetLastCode(FieldName, DbfFolderPath, UseDirectoryPath);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"\nLast Code: {LastCode}");
                var Z_Asc = FL_Z_Asc(LastCode);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val : {Z_Asc}");
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val (Long): {(long)Z_Asc}");
                var Z_Chr = FL_Z_Chr((long)Z_Asc);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Code In Char : {Z_Chr}\n");
                return Z_Chr;
            }
            else
            {
                return FL_Z_Chr((long)FL_Z_Asc(TableName.FL_GetLastCode(FieldName, DbfFolderPath, UseDirectoryPath)));
            }
        }

        public static string FL_SetNewCode(this string code, bool DebugMode = false)
        {
            if (DebugMode)
            {
                var LastCode = code;
                FL_ConsoleManager.FL_ConsoleWriteDebug($"\nLast Code: {LastCode}");
                var Z_Asc = FL_Z_Asc(LastCode, DebugMode);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val : {Z_Asc}");
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val (Long): {(long)Z_Asc}");
                var Z_Chr = FL_Z_Chr((long)Z_Asc, DebugMode);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Code In Char : {Z_Chr}\n");
                return Z_Chr;
            }
            else
            {
                return FL_Z_Chr((long)FL_Z_Asc(code));
            }
        }

        public static string FL_GetLastCode(this string TableName, string FieldName, string DbfFolderPath, bool UseDirectoryPath = true)
        {
            //var dbfConstring1 = DbfFolderPath.FL_GetDbfConnectionString(UseDirectoryPath);
            var query = $"select top 1 {FieldName} as valFld from {TableName} order by cast({FieldName} as varbinary) desc";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = dt.Rows[0]["valFld"].ToString().PadRight(paddChar, ' ');
            }
            else
            {
                code = dt.Rows[0]["valFld"].ToString().PadRight(paddChar, ' ');
            }
            return code;
        }

        public static double FL_Z_Asc(string LastCode, bool DebugMode = false)
        {
            double pg = 0;
            var i = LastCode.Length;
            try
            {
                while (i != 0)
                {
                    if (DebugMode) $"Generating Ascii: {pg}".FL_ConsoleWriteDebug();
                    if (DebugMode) $"Generating Ascii => i: {i}".FL_ConsoleWriteDebug();
                    pg += (char.ConvertToUtf32(LastCode.Substring(i - 1, 1), 0) - 31) * Math.Pow(91, (LastCode.Length - i));
                    
                    i -= 1;
                }
                return pg;
            }
            catch
            {
                return 0;
            }
        }

        public static string FL_Z_Chr(long Z_Asc, bool DebugMode = false)
        {
            var f_str = "";
            long i = 1;
            long pg;

            try
            {
                while (i != 0)
                {
                    if (DebugMode) $"Generating char: {f_str}".FL_ConsoleWriteDebug();
                    if (DebugMode) $"Generating char=> i: {i}".FL_ConsoleWriteDebug();
                    pg = Convert.ToInt64(Z_Asc % 91);
                    if (pg == 0)
                    {
                        pg = 122;
                    }
                    else
                    {
                        pg += 31;
                    }
                    f_str = $"{char.ConvertFromUtf32(Convert.ToInt32(pg))}{f_str}";
                    var k = Z_Asc / 91;
                    i = Convert.ToInt64(Math.Truncate(double.Parse(k.ToString())));
                    if (pg == 122)
                    {
                        i = Convert.ToInt64(Math.Truncate(double.Parse((Z_Asc / 91).ToString())));
                        pg = Convert.ToInt64(Math.Truncate(double.Parse((Z_Asc / 91).ToString())))-1;
                    }
                    else
                    {
                        i = Convert.ToInt64(Math.Truncate(double.Parse((Z_Asc / 91).ToString())));
                    }
                }
                return f_str;
            }
            catch
            {
                return "";
            }

        }
    }
}
