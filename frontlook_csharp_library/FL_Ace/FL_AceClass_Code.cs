using frontlook_csharp_library.FL_Dbf_Helper;
using frontlook_csharp_library.FL_General;
using System.Data;
using System.Linq;

namespace frontlook_csharp_library.FL_Ace
{
    public static class FL_AceClass_Code
    {

        private static int paddChar = 2;
        public enum DataBaseType
        {
            DBF = 1, MSSQL = 2
        }
/*
        public static string FL_SetAceClassNewCode(string FieldName, string DbfFolderPath,  bool UseDirectoryPath = true, bool DebugMode = false)
        {
            if (DebugMode)
            {
                var LastCode = DbfFolderPath.FL_GetAceClassLastCode(UseDirectoryPath);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"\nLast Code: {LastCode}");
                var Z_Asc = LastCode.FL_Z_Asc();
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val : {Z_Asc}");
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val (Long): {(long)Z_Asc}");
                var Z_Chr = FL_AsciiSerial.FL_Z_Chr((long)Z_Asc+1L);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Code In Char : {Z_Chr}\n");
                return Z_Chr;
            }
            else
            {
                return FL_AsciiSerial.FL_Z_Chr((long)FL_AsciiSerial.FL_Z_Asc(DbfFolderPath.FL_GetAceClassLastCode(UseDirectoryPath))+1L);
            }
        }
        */
        public static string FL_SetAceClassNewCode(this string LastCode, bool DebugMode = false)
        {
            if (DebugMode)
            {
                FL_ConsoleManager.FL_ConsoleWriteDebug($"\nLast Code: {LastCode}");
                var Z_Asc = LastCode.FL_Z_Asc();
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val : {Z_Asc}");
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Ascii val (Long): {(long)Z_Asc}");
                var Z_Chr = FL_AsciiSerial.FL_Z_Chr((long)Z_Asc+1L);
                FL_ConsoleManager.FL_ConsoleWriteDebug($"New Code In Char : {Z_Chr}\n");
                var x = Z_Chr;
                return x;
                /*if (x.Length > 2)
                {
                    x = x.Substring(1);
                    return x.Substring(1);
                }
                else
                {
                    return x;
                }*/
            }
            else
            {
                return FL_AsciiSerial.FL_Z_Chr((long)FL_AsciiSerial.FL_Z_Asc(LastCode)+1L);
            }
        }
        public static string FL_SetAceClassNewCode(this string LastCode, string DbfFolderPath, bool UseDirectoryPath = true, bool DebugMode = false)
        {
            string x = LastCode.FL_SetAceClassNewCode(DebugMode); 
            
            string query = $"select * from `CLASS`";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            while(dt.AsEnumerable().Any(e => e.Field<string>("CLASS") == x))
            {
                x = x.FL_SetAceClassNewCode(DebugMode);
            }
            return x;
        }

        public static string FL_GetAceClassLastCode(this string DbfFolderPath, bool UseDirectoryPath = true)
        {
            string query =  $"select top 1 `CLASS` as valFld from `CLASS` order by `CLASS` desc";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = dt.Rows[0]["valFld"].ToString();
                    //.PadRight(paddChar,' ');
            }
            return code;
        }

        public static string FL_GetAceClassLastCodeAlt(this string DbfFolderPath, bool UseDirectoryPath = true)
        {
            string query = $"select * from `CLASS`";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            //var rs1 = dt.AsEnumerable().OrderByDescending(e => (long)FL_AsciiSerial.FL_Z_Asc(e.Field<string>("CLASS"))).ToList();
            var rs1 = dt.AsEnumerable().ToList();
            var rs = rs1.Last()["CLASS"];
            var cdc = rs.ToString();
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = cdc.ToString();
            }
            return code;
        }


        public static string FL_GetAceClassCodesAlt(this string DbfFolderPath, bool UseDirectoryPath = true)
        {
            string query = $"select * from `CLASS`";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var rs1 = dt.AsEnumerable().OrderByDescending(e => e.Field<string>("CLASS")).ToList();
            var rs = rs1.Select(e=>e["CLASS"].ToString()).ToList();
            var cdc = rs.ToString();
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = cdc.ToString();
            }
            return code;
        }
    }
}
