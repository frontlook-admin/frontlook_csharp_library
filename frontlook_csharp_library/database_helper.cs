using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace frontlook_csharp_library.database_helper
{
    public class database_helper
    {
        public static string fl_get_os()
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


        public static DataTable fl_get_oledb_datatable(string constring, string query)
        {
            DataTable dt = new DataTable();
            try
            {
                OleDbConnection connection = new OleDbConnection(constring);                
                OleDbCommand cmd = new OleDbCommand(query, connection);
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
            return dt;
        }

        public static int fl_oledb_execute_command(string constring, string sql_command)
        {
            int r = 0;
            try
            {
                OleDbConnection connection = new OleDbConnection(constring);
                OleDbCommand cmd = new OleDbCommand(sql_command, connection);
                connection.Open();
                
                r = cmd.ExecuteNonQuery();
                //DA.Update(dt);
                connection.Close();
                //BackgroundWorker bgw = new BackgroundWorker();
                

            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }

        public static DataTable fl_get_odbc_datatable(string constring, string query)
        {
            DataTable dt = new DataTable();
            try
            {
                OdbcConnection connection = new OdbcConnection(constring);
                OdbcCommand cmd = new OdbcCommand(query, connection);
                connection.Open();
                OdbcDataAdapter DA = new OdbcDataAdapter(cmd);
                DA.Fill(dt);
                //DA.Update(dt);
                connection.Close();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return dt;
        }

        public static int fl_odbc_execute_command(string constring, string sql_command)
        {
            int r = 0;
            try
            {
                OdbcConnection connection = new OdbcConnection(constring);
                OdbcCommand cmd = new OdbcCommand(sql_command, connection);
                connection.Open();

                r = cmd.ExecuteNonQuery();
                //DA.Update(dt);
                connection.Close();
                //BackgroundWorker bgw = new BackgroundWorker();


            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }

    }
}
