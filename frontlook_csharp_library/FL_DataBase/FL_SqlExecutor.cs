using frontlook_csharp_library.FL_DataBase.FL_MySql;
using MySql.Data.MySqlClient;
using System.Data;

namespace frontlook_csharp_library.FL_DataBase
{
	/// <summary>
	/// Defines the <see cref="FL_SqlExecutor" />
	/// </summary>
	public static class FL_SqlExecutor
	{
		/// <summary>
		/// The Con_switch
		/// </summary>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		public static void Con_switch(this MySqlConnection Con)
		{
			Con.MySqlConswitch();
		}

		/// <summary>
		/// The Con_switch_off
		/// </summary>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		public static void Con_switch_off(this MySqlConnection Con)
		{
			Con.MySqlConswitch_Off();
		}

		/// <summary>
		/// The Con_switch_on
		/// </summary>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		public static void Con_switch_on(this MySqlConnection Con)
		{
			Con.MySqlConswitch_On();
		}

		/// <summary>
		/// The ExecuteCommand
		/// </summary>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <param name="Query">The Query<see cref="string"/></param>
		/// <returns>The <see cref="int"/></returns>
		public static int ExecuteCommand(MySqlConnection Con, MySqlCommand Cmd, string Query)
		{
			return Cmd.ExecuteMySqlCommand(Query, Con);
		}

		/// <summary>
		/// The ExecuteCommand
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <returns>The <see cref="int"/></returns>
		public static int ExecuteCommand(this MySqlCommand Cmd)
		{
			return Cmd.ExecuteMySqlCommand();
		}

		/// <summary>
		/// The FL_DataTable
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <param name="Query">The Query<see cref="string"/></param>
		/// <param name="Con">The Con<see cref="MySqlConnection"/></param>
		/// <returns>The <see cref="DataTable"/></returns>
		public static DataTable FLDataTable(this MySqlCommand Cmd, string Query, MySqlConnection Con)
		{
			return Cmd.FLMySqlDataTable(Query, Con);
		}

		public static DataTable FLDataTable(this string Con, string query)
		{
			return Con.FLMySqlDataTable(query);
		}

		/// <summary>
		/// The FL_DataTable
		/// </summary>
		/// <param name="Cmd">The Cmd<see cref="MySqlCommand"/></param>
		/// <returns>The <see cref="DataTable"/></returns>
		public static DataTable FLDataTable(this MySqlCommand Cmd)
		{
			return Cmd.FLMySqlDataTable();
		}


	}
}
