using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using frontlook_csharp_library.FL_DataBase;
using frontlook_csharp_library.FL_General;
using JetBrains.Annotations;

namespace frontlook_csharp_library.FL_Dbf_Helper
{
    [Guid("3A1A8463-73F7-47FE-BCAD-9DDCB9103B07")]
    public static class FL_Dbf_Manager
    {
        public static DataSet get_all_datatable_in_dataset(string[] filepaths)
        {
            DataSet ds = new DataSet("data_collection");
            ds.Locale = Thread.CurrentThread.CurrentCulture;
            foreach (string s in filepaths)
            {
                DataTable dt;
                dt = FL_DbfData_To_Excel.FL_get_only_datatable_for_dbf(s);
                ds.Tables.Add(dt);
            }
            return ds;
        }

        public static DataTable FL_DBF_ExecuteQuery(string dbfFilepath)
        {
            //FileInfo fileInfo = new FileInfo(dbfFilepath);
            //string dbfDirectoryFilepath = fileInfo.DirectoryName;
            //string x = Path.GetDirectoryName(dbfFilepath);
            string dbfConstring1 = FL_dbf_constring(dbfFilepath);
            //Get version information about the os.
            //data_helper1.constring(dbf_filepath);

            //Variable to hold our return value

            //string excelFilename = "";
            //string xml = "";
            //string xml_schema = "";
            //string sWithoutExt;
            //string s = "";

            var s = dbfFilepath;
            //excelFilename = "";
            //sWithoutExt = "";
            //var excelFilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xlsx";
            var sWithoutExt = Path.GetFileNameWithoutExtension(s);
            DataTable dt = new DataTable("Database");
            try
            {
                OleDbConnection connection = new OleDbConnection(dbfConstring1);
                string sql = "SELECT * FROM `" + sWithoutExt + "`;";

                OleDbCommand cmd = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                //DA.Update(dt);
                connection.Close();
                //BackgroundWorker bgw = new BackgroundWorker();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return dt;
        }

        public static DataTable FL_DBFExecuteQuery(string DbfFolderPath, string sql, bool UseDirectoryPath = true)
        {
            //string dbfConstring1 = FL_dbf_constring(dbfFilepath);
            string dbfConstring1 = UseDirectoryPath ? FL_DBFConstring(DbfFolderPath) : FL_dbf_constring(DbfFolderPath);

            DataTable dt = new DataTable();
            try
            {
                OleDbConnection connection = new OleDbConnection(dbfConstring1);
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                //DA.Update(dt);
                connection.Close();
                //BackgroundWorker bgw = new BackgroundWorker();
                cmd.Dispose();
                connection.Dispose();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return dt;
        }


        public static DataTable FL_DBF_ExecuteQuery(this string sql, string DbfFolderPath, bool UseDirectoryPath = true, bool ShowExceptionMessage = true)
        {
            //string dbfConstring1 = FL_dbf_constring(dbfFilepath);
            string dbfConstring1 = UseDirectoryPath ? FL_DBFConstring(DbfFolderPath) : FL_dbf_constring(DbfFolderPath);

            DataTable dt = new DataTable();
            try
            {
                OleDbConnection connection = new OleDbConnection(dbfConstring1);
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                //DA.Update(dt);
                connection.Close();
                //BackgroundWorker bgw = new BackgroundWorker();
                cmd.Dispose();
                connection.Dispose();
            }
            catch (OleDbException e)
            {
                if (ShowExceptionMessage)
                {
                    MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return dt;
        }




        public static DataTable FL_DBF_ExecuteQuery(this string sql, string DataTableName, string DbfFolderPath, bool UseDirectoryPath = true)
        {
            //string dbfConstring1 = FL_dbf_constring(dbfFilepath);
            string dbfConstring1 = UseDirectoryPath ? FL_DBFConstring(DbfFolderPath) : FL_dbf_constring(DbfFolderPath);

            DataTable dt = new DataTable(DataTableName);
            try
            {
                OleDbConnection connection = new OleDbConnection(dbfConstring1);
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                //DA.Update(dt);
                connection.Close();
                //BackgroundWorker bgw = new BackgroundWorker();
                cmd.Dispose();
                connection.Dispose();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return dt;
        }

        public static int FL_DBF_ExecuteNonQuery(this string DbfFolderPath, string sql, bool UseDirectoryPath = true, bool UseTransaction = true)
        {
            //string dbfConstring1 = FL_dbf_constring(dbfFilepath);
            string dbfConstring1 = FL_GetDbfConnectionString(DbfFolderPath,UseDirectoryPath);
            var i = 0;
            //DataTable dt = new DataTable();
            OleDbConnection connection = new OleDbConnection(dbfConstring1);
            connection.Open();
            if (UseTransaction)
            {
                using var transaction = connection.BeginTransaction();
                OleDbCommand cmd = new OleDbCommand(sql, connection, transaction);
                try
                {

                    i = cmd.ExecuteNonQuery();
                    //BackgroundWorker bgw = new BackgroundWorker();
                    transaction.Commit();
                    cmd.Dispose();
                    transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
                catch (OleDbException e)
                {
                    cmd.Dispose();
                    transaction.Rollback();
                    transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                    MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return i;
            }
            else
            {
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                try
                {

                    i = cmd.ExecuteNonQuery();
                    //BackgroundWorker bgw = new BackgroundWorker();
                    //transaction.Commit();
                    cmd.Dispose();
                    //transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
                catch (OleDbException e)
                {
                    cmd.Dispose();
                    //transaction.Rollback();
                    //transaction.Dispose();
                    connection.Close();
                    connection.Dispose();
                    MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return i;
            }
        }

        public static void FL_DBF_BulkExecuteNonQuery(this DataTable dt, string DbfFolderPath, OleDbConnection connection = null, OleDbCommand cmd = null,[CanBeNull] string destinationTableName = null, bool UseDirectoryPath = true, bool UseTransaction = true)
        {
            string dbfConstring1 = DbfFolderPath.FL_GetDbfConnectionString(UseDirectoryPath);

            //DataTable dt = new DataTable();
            destinationTableName = destinationTableName ?? dt.TableName;
            if(connection == null || cmd == null)
            {
                 connection = new OleDbConnection(dbfConstring1);
                 cmd = new OleDbCommand("", connection);
            }
            var sql = new Sql(dt, true, destinationTableName).InsertSqls;
            var dsql = "";
            try
            {
                for (int i = 0; i < sql.Count; i++)
                {
                    string _sql = sql[i];
                    dsql = _sql;
                    cmd.CommandText = _sql;
                    cmd.ExecuteNonQuery();
                    //BackgroundWorker bgw = new BackgroundWorker();
                }
            }
            catch (OleDbException e)
            {

                throw new Exception($"{e.Message}\n Sql: {dsql}");
            }
        }

        public static int FL_DBF_ExecuteNonQuery(string DbfFolderPath, List<string> sql, bool UseDirectoryPath = true)
        {
            //string dbfConstring1 = FL_dbf_constring(dbfFilepath);
            string dbfConstring1 = FL_Dbf_Manager.FL_GetDbfConnectionString(DbfFolderPath, UseDirectoryPath);
            var i = 0;
            //DataTable dt = new DataTable();
            OleDbConnection connection = new OleDbConnection(dbfConstring1);
            connection.Open();
            using var transaction = connection.BeginTransaction();
            OleDbCommand cmd = new OleDbCommand("", connection, transaction);
            try
            {
                foreach (var _sql in sql)
                {
                    cmd.CommandText = _sql;
                    i = cmd.ExecuteNonQuery();
                    //BackgroundWorker bgw = new BackgroundWorker();
                }
            }
            catch (OleDbException e)
            {
                transaction.Rollback();
                connection.Close();
                connection.Dispose();
                MessageBox.Show($"Error : {e.Message}\n Details : {e.InnerException.Message}\n\n{e.StackTrace}", @"ERROR..!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            cmd.Dispose();
            transaction.Commit();
            connection.Close();
            connection.Dispose();
            return i;
        }

        public static string FL_GetDbfConnectionString(this string DbfFolderPath, bool UseDirectoryPath = true)
        {
            string dbfConstring1 = UseDirectoryPath ? FL_DBFConstring(DbfFolderPath) : FL_dbf_constring(DbfFolderPath);
            return dbfConstring1;
        }

        public static string FL_GetDbfConnectionString(this string DbfFolderPath, string UserId, string password, bool UseDirectoryPath = true)
        {
            string dbfConstring1 = UseDirectoryPath ? FL_DBFConstring(DbfFolderPath, UserId, password) : FL_dbf_constring(DbfFolderPath, UserId, password);
            return dbfConstring1;
        }

        public static string FL_dbf_constring(string dbfFilepath)
        {
            //string dbfConstring1;
            FileInfo fileInfo = new FileInfo(dbfFilepath);
            string dbfDirectoryFilepath = fileInfo.DirectoryName;
            return FL_DBFConstring(dbfDirectoryFilepath);
        }

        public static string FL_dbf_constring(string dbfFilepath, string UserId, string password)
        {
            FileInfo fileInfo = new FileInfo(dbfFilepath);
            string dbfDirectoryFilepath = fileInfo.DirectoryName;
            return FL_DBFConstring(dbfDirectoryFilepath, UserId, password);
        }

        public static string FL_DBFConstring(string dbfDirectoryFilepath)
        {
            string operatingSystem = FL_Os_Helper.FL_get_os();
            //string dbfConstring1;
            if (operatingSystem != "")
            {
                operatingSystem = "Windows " + operatingSystem;
            }

            switch (operatingSystem)
            {
                /*case "Windows XP":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";*/
                case "Windows 7":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";
                case "Windows Vista":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";
                case "Windows 8":
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";
                case "Windows 8.1":
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";
                case "Windows 10":
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";
                default:
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";
            }
        }

        public static string FL_DBFConstring(string dbfDirectoryFilepath, string UserId, string password)
        {
            string operatingSystem = FL_Os_Helper.FL_get_os();
            //string dbfConstring1;
            if (operatingSystem != "")
            {
                operatingSystem = "Windows " + operatingSystem;
            }

            switch (operatingSystem)
            {
                /*case "Windows XP":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;*/
                case "Windows 7":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;
                case "Windows Vista":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;
                case "Windows 8":
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;
                case "Windows 8.1":
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;
                case "Windows 10":
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;
                default:
                    return "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;
            }
        }
    }
}