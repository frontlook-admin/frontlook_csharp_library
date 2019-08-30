using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace frontlook_csharp_library.Data_Manager.FL_database_helper
{
    public class FL_Odbc_Helper
    {
        public static DataTable FL_get_odbc_datatable(string constring, string query)
        {
            DataTable dt = new DataTable();
            try
            {
                OdbcConnection connection = new OdbcConnection(constring);
                OdbcCommand cmd = new OdbcCommand(query, connection);
                connection.Open();
                OdbcDataAdapter da = new OdbcDataAdapter(cmd);
                da.Fill(dt);
                //DA.Update(dt);
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return dt;
        }

        [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        public static int FL_odbc_execute_command(string constring, string sqlCommand)
        {
            int r = 0;
            try
            {
                OdbcConnection connection = new OdbcConnection(constring);
                OdbcCommand cmd = new OdbcCommand(sqlCommand, connection);
                connection.Open();

                r = cmd.ExecuteNonQuery();
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
            return r;
        }
    }
}