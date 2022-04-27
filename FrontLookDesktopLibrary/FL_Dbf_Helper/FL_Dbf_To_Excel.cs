using frontlook_csharp_library.FL_Excel_Data_Interop;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using DataTable = System.Data.DataTable;

namespace frontlook_csharp_library.FL_Dbf_Helper
{
	public static class FL_DbfData_To_Excel
	{
		public static void FL_data_to_xls(string dbfFilepathWithNameAndExtension)
		{
			string constring = FL_Dbf_Manager.FL_dbf_constring(dbfFilepathWithNameAndExtension);
			string sWithoutExt = Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension);
			string query = "SELECT * FROM " + sWithoutExt;
			DataTable dt = FL_Oledb_Helper.FL_Oledb_Manager.FL_get_oledb_datatable(constring, query);
			FL_DataTableToExcel_Helper.FL_DataTableToExcel(dt, Path.GetDirectoryName(dbfFilepathWithNameAndExtension) + @"\" + Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension));
		}

		public static void FL_data_to_xls(string query, string constring1)
		{
			FL_data_to_xls(query, constring1, null);
		}

		public static void FL_data_to_xls(string query, string constring1 = null, string dbfFilepathWithNameAndExtension = null)
		{
			if (string.IsNullOrEmpty(dbfFilepathWithNameAndExtension))
			{
				DataTable dt = FL_Oledb_Helper.FL_Oledb_Manager.FL_get_oledb_datatable(constring1, query);
				var filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + dt.TableName;
				FL_Excel_Data_Interop.FL_DataTableToExcel_Helper.FL_DataTableToExcel(dt, filename);
			}
			else
			{

				string constring = FL_Dbf_Manager.FL_dbf_constring(dbfFilepathWithNameAndExtension);
				string sWithoutExt = Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension);
				DataTable dt = FL_Oledb_Helper.FL_Oledb_Manager.FL_get_oledb_datatable(constring, query);
				FL_Excel_Data_Interop.FL_DataTableToExcel_Helper.FL_DataTableToExcel(dt, Path.GetDirectoryName(dbfFilepathWithNameAndExtension) + @"\" + Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension));

			}
		}


		/*public static void FL_data_to_xls(string query,string constring)
        {
            DataTable dt = FL_database_helper.FL_oledb_helper.FL_get_oledb_datatable(constring, query);
            var filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + dt.TableName;
            FL_Excel_Data_Interop.FL_DataTableToExcel_Helper.FL_DataTableToExcel(dt, filename);
        }*/


		public static DataTable FL_data_to_xls_with_datatable(string dbfFilepathWithNameAndExtension)
		{
			FileInfo fileInfo = new FileInfo(dbfFilepathWithNameAndExtension);
			string constring = FL_Dbf_Manager.FL_dbf_constring(dbfFilepathWithNameAndExtension);
			string sWithoutExt = Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension);
			string query = "SELECT * FROM " + sWithoutExt;
			DataTable dt = FL_Oledb_Helper.FL_Oledb_Manager.FL_get_oledb_datatable(constring, query);
			FL_Excel_Data_Interop.FL_DataTableToExcel_Helper.FL_DataTableToExcel(dt, Path.GetDirectoryName(dbfFilepathWithNameAndExtension) + @"\" + Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension));
			return dt;
		}

		public static DataTable FL_get_only_datatable_for_dbf(string dbfFilepathWithNameAndExtension)
		{
			FileInfo fileInfo = new FileInfo(dbfFilepathWithNameAndExtension);
			string constring = FL_Dbf_Manager.FL_dbf_constring(dbfFilepathWithNameAndExtension);
			string sWithoutExt = Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension);
			string query = "SELECT * FROM " + sWithoutExt;
			DataTable dt = FL_Oledb_Helper.FL_Oledb_Manager.FL_get_oledb_datatable(constring, query);
			return dt;
		}


		public static DataTable FL_get_OnlyDatatableForDbf_variableQuery(string dbfFilepathWithNameAndExtension, String query)
		{
			FileInfo fileInfo = new FileInfo(dbfFilepathWithNameAndExtension);
			string constring = FL_Dbf_Manager.FL_dbf_constring(dbfFilepathWithNameAndExtension);
			string sWithoutExt = Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension);
			DataTable dt = FL_Oledb_Helper.FL_Oledb_Manager.FL_get_oledb_datatable(constring, query);
			return dt;
		}


		public static DataSet FL_get_only_dataset_for_dbf(string dbfFilepathWithNameAndExtension)
		{
			FileInfo fileInfo = new FileInfo(dbfFilepathWithNameAndExtension);
			string constring = FL_Dbf_Manager.FL_dbf_constring(dbfFilepathWithNameAndExtension);
			string sWithoutExt = Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension);
			string query = "SELECT * FROM " + sWithoutExt;
			DataSet ds = FL_Oledb_Helper.FL_Oledb_Manager.FL_get_oledb_dataset(constring, query);
			return ds;
		}
	}
}
