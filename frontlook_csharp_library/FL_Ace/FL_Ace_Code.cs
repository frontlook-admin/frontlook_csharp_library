using frontlook_csharp_library.FL_Dbf_Helper;
using frontlook_csharp_library.FL_General;
using System.Data;
using System.Linq;

namespace frontlook_csharp_library.FL_Ace
{
    public static class FL_Ace_Code
    {

        private static int paddChar = 4;
        public enum DataBaseType
        {
            DBF = 1, MSSQL = 2
        }

        public static string FL_SetAceNewCode(this string TableName, string FieldName, string DbfFolderPath,  bool UseDirectoryPath = true, bool DebugMode = false)
        {
            if (DebugMode)
            {
                var LastCode = TableName.FL_GetAceLastCode(FieldName, DbfFolderPath, UseDirectoryPath);
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
                return FL_AsciiSerial.FL_Z_Chr((long)FL_AsciiSerial.FL_Z_Asc(TableName.FL_GetAceLastCode(FieldName, DbfFolderPath, UseDirectoryPath))+1L);
            }
        }

        public static string FL_GetAceLastCode(this string TableName, string FieldName, string DbfFolderPath, bool UseDirectoryPath = true, bool EnablePadding = true, int PaddingChar = 4)
        {
            string query =  $"select top 1 {FieldName} as valFld from {TableName} order by {FieldName} desc";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = EnablePadding ? dt.Rows[0]["valFld"].ToString().PadRight(PaddingChar, ' '): dt.Rows[0]["valFld"].ToString();
            }
            return code;
        }

        public static string FL_GetAceLastCodeAlt(this string TableName, string FieldName, string DbfFolderPath, bool UseDirectoryPath = true, bool EnablePadding = true, int PaddingChar = 4)
        {
            string query = $"select * from {TableName}";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var rs = dt.AsEnumerable().OrderByDescending(e => (long)FL_AsciiSerial.FL_Z_Asc(e.Field<string>(FieldName))).ToList().First()[FieldName];
            var cdc = rs.ToString();
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = EnablePadding ? cdc.PadRight(PaddingChar, ' ') : cdc.ToString();
            }
            return code;
        }

        /*public static string FL_GetAceLastCodeAlt(this string TableName, string FieldName, string DbfFolderPath, bool UseDirectoryPath = true)
        {
            string query =  $"select top 1 {FieldName} as valFld from {TableName} order by {FieldName} desc";
            var dt = query.FL_DBF_ExecuteQuery(DbfFolderPath, UseDirectoryPath);
            var code = " ";
            if (dt.Rows.Count > 0)
            {
                code = dt.Rows[0]["valFld"].ToString();
            }
            return code;
        }*/
    }
}
