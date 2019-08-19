using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows;

namespace frontlook_csharp_library.dbf_helper
{
    [System.Runtime.InteropServices.Guid("3A1A8463-73F7-47FE-BCAD-9DDCB9103B07")]
    public class dbf_helper
    {

        public static DataSet get_all_datatable_in_dataset(string[] filepaths)
        {
            DataSet ds = new DataSet("data_collection");
            ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
            foreach (string s in filepaths)
            {
                DataTable dt = new DataTable();
                dt = excel_data_interop.data_to_excel.fl_get_only_datatable_for_dbf(s);
                ds.Tables.Add(dt);
            }
            return ds;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public static DataTable fl_dbf_datatable(string dbf_filepath)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            string dbf_constring1 = fl_dbf_constring(dbf_filepath); ;
            //Get version information about the os.
            //data_helper1.constring(dbf_filepath);

            //Variable to hold our return value

            string excelFilename = "";
            //string xml = ""; 
            //string xml_schema = ""; 
            string s_without_ext = "";
            string s = "";


            s = dbf_filepath;
            excelFilename = "";
            s_without_ext = "";
            excelFilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xlsx";
            s_without_ext = Path.GetFileNameWithoutExtension(s);
            DataTable dt = new DataTable();
            try
            {

                OleDbConnection connection = new OleDbConnection(dbf_constring1);
                string sql = "SELECT * FROM " + s_without_ext;

                OleDbCommand cmd = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter DA = new OleDbDataAdapter(cmd);
                DA.Fill(dt);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public static DataTable fl_dbf_datatable(string dbf_filepath,string sql)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            string dbf_constring1 = fl_dbf_constring(dbf_filepath); ;
            //Get version information about the os.
            //data_helper1.constring(dbf_filepath);

            //Variable to hold our return value

            string excelFilename = "";
            //string xml = ""; 
            //string xml_schema = ""; 
            string s_without_ext = "";
            string s = "";


            s = dbf_filepath;
            excelFilename = "";
            s_without_ext = "";
            excelFilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xlsx";
            s_without_ext = Path.GetFileNameWithoutExtension(s);
            DataTable dt = new DataTable();
            try
            {

                OleDbConnection connection = new OleDbConnection(dbf_constring1);
                OleDbCommand cmd = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter DA = new OleDbDataAdapter(cmd);
                DA.Fill(dt);
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

        public static string fl_dbf_get_os()
        {
            string operatingSystem = "";
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;
            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "7";
                        else if (vs.Minor == 2)
                            operatingSystem = "8";
                        else
                            operatingSystem = "8.1";
                        break;
                    case 10:
                        operatingSystem = "10";
                        break;
                    default:
                        break;
                }
            }
            return operatingSystem;
        }

        public static string fl_dbf_constring(string dbf_filepath)
        {
            string operatingSystem = fl_dbf_get_os(); ;
            string dbf_constring1 = "";
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            //string dbf_filename = "";

            //data_helper.get_os(operatingSystem);
            if (operatingSystem != "")
            {
                operatingSystem = "Windows " + operatingSystem;
            }

            switch (operatingSystem)
            {
                case "Windows XP":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 7":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows Vista":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 8":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 8.1":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 10":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                default:
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
            }
            return dbf_constring1;
        }
    }
}
