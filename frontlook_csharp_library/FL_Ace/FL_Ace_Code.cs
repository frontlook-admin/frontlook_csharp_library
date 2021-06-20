﻿using System;
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
        private static int paddChar = 4;

        public enum DataBaseType
        {
            DBF = 1, MSSQL = 2
        }

        public static string FL_SetNewCode(this string TableName, string FieldName, string DbfFolderPath,  bool UseDirectoryPath = true, bool DebugMode = false)
        {
            if (DebugMode)
            {
                var LastCode = TableName.FL_GetLastCode(FieldName, DbfFolderPath, UseDirectoryPath);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"\nLast Code: {LastCode}");
                var Z_Asc = FL_Z_Asc(LastCode)+1;
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val : {Z_Asc}");
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val (Long): {(long)Z_Asc}");
                var Z_Chr = FL_Z_Chr((long)Z_Asc);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Code In Char : {Z_Chr}\n");
                return Z_Chr;
            }
            else
            {
                return FL_Z_Chr((long)FL_Z_Asc(TableName.FL_GetLastCode(FieldName, DbfFolderPath, UseDirectoryPath))+1);
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
            string query =  $"select top 1 {FieldName} as valFld from {TableName} order by {FieldName} desc";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = dt.Rows[0]["valFld"].ToString().PadRight(paddChar,' ');
            }
            return code;
        }

        public static double FL_Z_Asc(this string LastCode, bool DebugMode = false)
        {
            try
            {
                double pg = 0;
                var i = LastCode.Length;
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

        public static string FL_Z_Chr(this long Z_Asc, bool DebugMode = false)
        {
            string f_str = "";
            long i = 1L;

            try
            {
                while (i != 0L)
                {
                    if (DebugMode) $"Generating char: {f_str}".FL_ConsoleWriteDebug();
                    if (DebugMode) $"Generating char=> i: {i}".FL_ConsoleWriteDebug();
                    long pg = Convert.ToInt64(Z_Asc % 91L);
                    if (pg == 0L)
                    {
                        pg = 122L;
                    }
                    else
                    {
                        pg += 31L;
                    }
                    f_str = char.ConvertFromUtf32(Convert.ToInt32(pg)) + f_str;
                    i = Convert.ToInt64(Math.Truncate(Z_Asc / 91d));


                    if (pg == 122L)
                    {
                        i = Convert.ToInt64(Math.Truncate(i / 91d));
                        Z_Asc = Convert.ToInt64(Math.Truncate(Z_Asc / 91d)) - 1L;
                    }
                    else
                    {
                        Z_Asc = Convert.ToInt64(Math.Truncate(Z_Asc / 91d));
                    }
                }
                return f_str;
            }
            catch
            {
                return "";
            }

        }







        public static string FL_GetLastCodeForSql(this string TableName, string FieldName, string DbfFolderPath, DataBaseType dataBaseType = DataBaseType.DBF, bool UseDirectoryPath = true)
        {
            string query = $"select top 1 {FieldName} as valFld from {TableName} order by cast({FieldName} as varbinary) desc";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = dt.Rows[0]["valFld"].ToString().PadRight(paddChar, ' ');
            }
            return code;
        }
    }
}
