/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Data.OleDb;
using System.Data;

namespace frontlook_csharp_library.dbf_helperx
{
    public static class function
    {
        public static void dbf_to_xls_series(string dbf_filepath)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";
            string dbf_constring1 = "";
            //string dbf_filename = "";
            string[] filePaths;
            string x = Path.GetDirectoryName(dbf_filepath);
            filePaths = Directory.GetFiles(x, "*.dbf");
            //string[] filePaths = Directory.GetFiles(Path.GetDirectoryName(dbf_filepath), "*.dbf");


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
            //Make sure we actually got something in our OS check
            //We don't want to just return " Service Pack 2" or " 32-bit"
            //That information is useless without the OS version.
            if (operatingSystem != "")
            {
                //Got something.  Let's prepend "Windows" and get more info.
                operatingSystem = "Windows " + operatingSystem;
                //See if there's a service pack installed.

                /*if (os.ServicePack != "")
                {
                    //Append it to the OS name.  i.e. "Windows XP Service Pack 3"
                    operatingSystem += " " + os.ServicePack;
                }*/

                //Append the OS architecture.  i.e. "Windows XP Service Pack 3 32-bit"
                //operatingSystem += " " + getOSArchitecture().ToString() + "-bit";
            }
            //Return the information we've gathered.

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
            string excelFilename = "";
            //string xml = "";
            //string xml_schema = "";
            string s_without_ext = "";
            foreach (string s in filePaths)
            {
                DataTable dt = new DataTable();
                //MessageBox.Show(s);
                excelFilename = "";
                s_without_ext = "";
                excelFilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xlsx";
                //xml = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xml";
                //xml_schema = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + "_schema.xml";
                s_without_ext = Path.GetFileNameWithoutExtension(s);

                //Path.GetFileNameWithoutExtension(dbf_filepath);
                try
                {

                    OleDbConnection connection = new OleDbConnection(dbf_constring1);
                    string sql = "SELECT * FROM " + s_without_ext;
                    //string sql = "SELECT COUNT(),* FROM " + dbf_filename;
                    MessageBox.Show(sql + "      \n" + dbf_constring1);
                    OleDbCommand cmd = new OleDbCommand(sql, connection);
                    connection.Open();
                    OleDbDataAdapter DA = new OleDbDataAdapter(cmd);
                    DA.Fill(dt);
                    //DA.Update(dt);
                    /* dataGridView1.DataSource = null;
                     dataGridView1.Refresh();
                     forms.BindingSource bsource = new forms.BindingSource();
                     bsource.DataSource = dt;
                     dataGridView1.DataSource = bsource;
                     dataGridView1.Refresh();*/
                    connection.Close();
                    DataTableToExcel(dt, Path.GetDirectoryName(s) + @"\" + Path.GetFileNameWithoutExtension(s));
                    //excel_data_interop.DataTableToExcel(dt, excelFilename);
                }
                catch (OleDbException e)
                {
                    MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            MessageBox.Show("DataTables are Successfully saved in Excle files..", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void get_os(this string operatingSystem)
        {
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
        }

        public static void dbf_con_string(this string dbf_directory_filepath, string dbf_constring1="")
        {
            string operatingSystem = "";

            //string dbf_filename = "";

            get_os(operatingSystem);
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
        }

        public static void getDataTable(this DataTable dt, string dbf_filepath)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            string dbf_constring1 = "";
            //Get version information about the os.
            dbf_con_string(dbf_directory_filepath, dbf_constring1);

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
                MessageBox.Show("Error : " + e.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }

}*/
