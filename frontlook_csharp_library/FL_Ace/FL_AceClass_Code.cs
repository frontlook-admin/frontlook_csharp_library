using frontlook_csharp_library.FL_Dbf_Helper;
using frontlook_csharp_library.FL_General;

namespace frontlook_csharp_library.FL_Ace
{
    public static class FL_AceClass_Code
    {

        private static int paddChar = 2;
        public enum DataBaseType
        {
            DBF = 1, MSSQL = 2
        }

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
    }
}
