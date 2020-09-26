using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using frontlook_csharp_library.FL_General;

namespace frontlook_csharp_library.FL_Dbf_Helper
{
    [Guid("3A1A8463-73F7-47FE-BCAD-9DDCB9103B07")]
    public class FL_Dbf_Manager
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

        public static DataTable FL_dbf_datatable(string dbfFilepath)
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
        
        public static DataTable FL_dbf_datatable(string dbfFilepath,string sql)
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
            //string s_without_ext = "";
            //string s = "";


            //var s = dbfFilepath;
            /*excelFilename = "";
            s_without_ext = "";
            excelFilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xlsx";
            s_without_ext = Path.GetFileNameWithoutExtension(s);*/
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
       
        public static string FL_dbf_constring(string dbfFilepath)
        {
            string operatingSystem = FL_Os_Helper.FL_get_os();
            //string dbfConstring1;
            FileInfo fileInfo = new FileInfo(dbfFilepath);
            string dbfDirectoryFilepath = fileInfo.DirectoryName;
            if (operatingSystem != "")
            {
                operatingSystem = "Windows " + operatingSystem;
            }

            switch (operatingSystem)
            {
                case "Windows XP":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath + ";Extended Properties=dBase IV;User ID=;Password=";
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

        public static string FL_dbf_constring(string dbfFilepath, string UserId, string password)
        {
            string operatingSystem = FL_Os_Helper.FL_get_os();
            //string dbfConstring1;
            FileInfo fileInfo = new FileInfo(dbfFilepath);
            string dbfDirectoryFilepath = fileInfo.DirectoryName;
            if (operatingSystem != "")
            {
                operatingSystem = "Windows " + operatingSystem;
            }

            switch (operatingSystem)
            {
                case "Windows XP":
                    return "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + dbfDirectoryFilepath +
                           ";Extended Properties=dBase IV;User ID=" + UserId + ";Password=" + password;
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
