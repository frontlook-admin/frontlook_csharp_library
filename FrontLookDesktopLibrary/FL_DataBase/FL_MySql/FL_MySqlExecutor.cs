using MySql.Data.MySqlClient;
using System.Data;

namespace frontlook_csharp_library.FL_DataBase.FL_MySql
{
	/// <summary>
	/// Defines the <see cref="FL_MySqlExecutor" />
	/// </summary>
	public static class FL_MySqlExecutor
	{
		/// <summary>
		/// The MySqlConswitch_On
		/// </summary>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		public static void MySqlConswitch_On(this MySqlConnection Con)
		{
			if (Con.State == ConnectionState.Closed)
			{
				Con.Open();
			}
			else if (Con.State == ConnectionState.Broken)
			{
				var Con1 = new MySqlConnection
				{
					ConnectionString = Con.ConnectionString
				};
				Con.Dispose();
				Con = Con1;
				Con.Open();
			}
			else
			{
				Con.Close();
				Con.Open();
			}
		}

		/// <summary>
		/// The MySqlConswitch_Off
		/// </summary>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		public static void MySqlConswitch_Off(this MySqlConnection Con)
		{
			if (Con.State == ConnectionState.Open)
			{
				Con.Close();
			}
		}

		/// <summary>
		/// The MySqlConswitch
		/// </summary>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		public static void MySqlConswitch(this MySqlConnection Con)
		{
			if (Con.State == ConnectionState.Open)
			{
				MySqlConswitch_Off(Con);
			}
			else if (Con.State == ConnectionState.Closed)
			{
				MySqlConswitch_On(Con);
			}
		}

		/// <summary>
		/// The ExecuteStoredProcedure
		/// </summary>
		public static void ExecuteStoredProcedure()
		{
		}

		/// <summary>
		/// The ExecuteMySqlCommand
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <param name="Query">The Query<see cref="string"/></param>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		/// <returns>The <see cref="int"/></returns>
		public static int ExecuteMySqlCommand(this MySqlCommand Cmd, string Query, MySqlConnection Con)
		{
			Cmd.Connection = Con;
			Cmd.CommandText = Query;
			MySqlConswitch(Con);
			var r = Cmd.ExecuteNonQuery();
			MySqlConswitch(Con);
			return r;
		}

		/// <summary>
		/// The ExecuteMySqlCommand
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <returns>The <see cref="int"/></returns>
		public static int ExecuteMySqlCommand(this MySqlCommand Cmd)
		{
			MySqlConswitch(Cmd.Connection);
			var r = Cmd.ExecuteNonQuery();
			MySqlConswitch(Cmd.Connection);
			return r;
		}

		/// <summary>
		/// The FL_MySql_DataTable
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <param name="Query">The Query<see cref="string"/></param>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		/// <returns>The <see cref="DataTable"/></returns>
		public static DataTable FLMySqlDataTable(this MySqlCommand Cmd, string Query, MySqlConnection Con)
		{
			var dt = new DataTable();
			Cmd.Connection = Con;
			Cmd.CommandText = Query;
			MySqlConswitch(Con);
			var adp = new MySqlDataAdapter(Cmd);
			adp.Fill(dt);
			MySqlConswitch(Con);
			return dt;
		}

		/// <summary>
		/// The FL_MySql_DataTable
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <returns>The <see cref="DataTable"/></returns>
		public static DataTable FLMySqlDataTable(this MySqlCommand Cmd)
		{
			var dt = new DataTable();
			MySqlConswitch(Cmd.Connection);
			var adp = new MySqlDataAdapter(Cmd);
			adp.Fill(dt);
			MySqlConswitch(Cmd.Connection);
			return dt;
		}

		public static DataTable FLMySqlDataTable(this string connection, string query)
		{
			return new MySqlCommand(query, new MySqlConnection(connection)).FLMySqlDataTable();
		}

		/// <summary>
		/// The FL_MySql_DataSet
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <param name="Query">The Query<see cref="string"/></param>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		/// <returns>The <see cref="DataSet"/></returns>
		public static DataSet FL_MySql_DataSet(this MySqlCommand Cmd, string Query, MySqlConnection Con)
		{
			var ds = new DataSet();
			Cmd.Connection = Con;
			Cmd.CommandText = Query;
			MySqlConswitch(Con);
			var adp = new MySqlDataAdapter(Cmd);
			adp.Fill(ds);
			MySqlConswitch(Con);
			return ds;
		}

		/// <summary>
		/// The FL_MySql_DataSet
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <returns>The <see cref="DataSet"/></returns>
		public static DataSet FL_MySql_DataSet(this MySqlCommand Cmd)
		{
			var ds = new DataSet();
			MySqlConswitch(Cmd.Connection);
			var adp = new MySqlDataAdapter(Cmd);
			adp.Fill(ds);
			MySqlConswitch(Cmd.Connection);
			return ds;
		}

		/// <summary>
		/// The FL_MySql_Check_Column_Exists
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		/// <param name="Database_Name">The Database_Name<see cref="string"/></param>
		/// <param name="TableName">The TableName<see cref="string"/></param>
		/// <param name="Columnname">The Columnname<see cref="string"/></param>
		/// <returns>The <see cref="bool"/></returns>
		public static bool FL_MySql_Check_Column_Exists(MySqlCommand Cmd, MySqlConnection Con, string Database_Name, string TableName, string Columnname)
		{
			Cmd.Connection = Con;
			Cmd.CommandText = "SELECT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA='" + Database_Name + "' AND TABLE_NAME='" +
							  TableName + "' and COLUMN_NAME = '" + Columnname + "') as exist;";
			MySqlConswitch(Con);
			var reader = Cmd.ExecuteReader();
			var v = "";
			while (reader.Read())
			{
				v = reader["exist"].ToString();
			}
			reader.Dispose();
			reader.Close();
			MySqlConswitch(Con);
			return !v.Equals("0") && !v.Equals("");
		}

		/*Copied*/

		/// <summary>
		/// The FL_mysql_execute_command
		/// </summary>
		/// <param name="Constring">The Constring<see cref="string"/></param>
		/// <param name="SqlCommand">The sqlCommand<see cref="string"/></param>
		/// <returns>The <see cref="int"/></returns>
		public static int FL_mysql_execute_command(string Constring, string SqlCommand)
		{
			var Connection = new MySqlConnection(Constring);
			var Cmd = new MySqlCommand(SqlCommand, Connection);
			Connection.Open();
			var r = Cmd.ExecuteNonQuery();
			Connection.Close();
			Cmd.Dispose();
			Connection.Dispose();
			return r;
		}

		/// <summary>
		/// The FL_mysql_dataadapter
		/// </summary>
		/// <param name="Constring">The Constring<see cref="string"/></param>
		/// <param name="SqlCommand">The sqlCommand<see cref="string"/></param>
		/// <returns>The <see cref="MySqlDataAdapter"/></returns>
		public static MySqlDataAdapter FL_mysql_dataadapter(string Constring, string SqlCommand)
		{
			//DataSet ds = new DataSet();

			var Connection = new MySqlConnection(Constring);
			var Cmd = new MySqlCommand(SqlCommand, Connection);
			Connection.Open();
			var da = new MySqlDataAdapter(Cmd);
			//DataTable dt = new DataTable();
			//DA.Fill(dt);
			//ds.Locale = System.Threading.Thread.CurrentThread.CurrentCulture;
			//ds.Tables.Add(dt);
			Connection.Close();
			//Cmd.Dispose();
			//Connection.Dispose();
			return da;
		}
	}
}