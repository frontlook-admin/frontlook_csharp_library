using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using MySql.Data;
using Microsoft.SqlServer.Server;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using MySql.Data.MySqlClient;

namespace frontlook_csharp_library.FL_database_helper
{
    public class FL_mysql_helper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public static int FL_mysql_execute_command(string constring, string sql_command)
        {
            int r = 0;
            try
            {
                MySqlConnection connection = new MySqlConnection(constring);
                MySqlCommand cmd = new MySqlCommand(sql_command, connection);
                connection.Open();
                r = cmd.ExecuteNonQuery();
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }

        public static MySqlDataAdapter FL_mysql_dataadapter(string constring, string sql_command)
        {
            MySqlDataAdapter DA = new MySqlDataAdapter();
            DataSet ds = new DataSet();
            try
            {
                MySqlConnection connection = new MySqlConnection(constring);
                MySqlCommand cmd = new MySqlCommand(sql_command, connection);
                connection.Open();
                DA = new MySqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //DA.Fill(dt);
                //ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
                //ds.Tables.Add(dt);
                connection.Close();
                //cmd.Dispose();
                //connection.Dispose();
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return DA;
        }

        

        public static MySqlDataReader FL_mysql_myreader(MySqlConnection connection, MySqlCommand cmd)
        {
            MySqlDataReader r=null;
            try
            {
                connection.Open();
                r = cmd.ExecuteReader();
            }
            catch (MySqlException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return r;
        }
    }
}
