using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Windows;

namespace frontlook_csharp_library.Data_Manager.FL_database_helper
{
    public static class FL_Oledb_Helper
    {
        public static DataTable FL_get_oledb_datatable(string constring, string query)
        {
            DataTable dt = new DataTable();
            try
            {
                OleDbConnection connection = new OleDbConnection(constring);
                OleDbCommand cmd = new OleDbCommand(query, connection);
                connection.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
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
        public static DataSet FL_get_oledb_dataset(string constring, string query)
        {
            DataSet ds = new DataSet("data_set");
            try
            {
                OleDbConnection connection = new OleDbConnection(constring);
                OleDbCommand cmd = new OleDbCommand(query, connection);
                connection.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ds.Locale = Thread.CurrentThread.CurrentCulture;
                ds.Tables.Add(dt);
                connection.Close();
                cmd.Dispose();
                connection.Dispose();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return ds;
        }

        public static int FL_oledb_execute_command(string constring, string sqlCommand)
        {
            int r = 0;
            try
            {
                OleDbConnection connection = new OleDbConnection(constring);
                OleDbCommand cmd = new OleDbCommand(sqlCommand, connection);
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

        public static DataSet FL_get_only_oledbdataset(string constring, string query)
        {
            DataSet ds = FL_get_oledb_dataset(constring, query);
            return ds;
        }
    }
}