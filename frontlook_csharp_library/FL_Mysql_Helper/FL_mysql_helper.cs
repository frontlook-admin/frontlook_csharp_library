//using System.Data;

using MySql.Data.MySqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace frontlook_csharp_library.FL_Mysql_Helper
{
	public class FL_Mysql_Manager
	{
		[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
		public static int FL_mysql_execute_command(string constring, string sqlCommand)
		{
			int r = 0;
			try
			{
				MySqlConnection connection = new MySqlConnection(constring);
				MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
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

		public static MySqlDataAdapter FL_mysql_dataadapter(string constring, string sqlCommand)
		{
			MySqlDataAdapter da = new MySqlDataAdapter();
			//DataSet ds = new DataSet();
			try
			{
				MySqlConnection connection = new MySqlConnection(constring);
				MySqlCommand cmd = new MySqlCommand(sqlCommand, connection);
				connection.Open();
				da = new MySqlDataAdapter(cmd);
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
			return da;
		}



		public static MySqlDataReader FL_mysql_myreader(MySqlConnection connection, MySqlCommand cmd)
		{
			MySqlDataReader r = null;
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
