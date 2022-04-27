using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace frontlook_csharp_library.FL_DataBase
{
	/// <summary>
	/// Defines the <see cref="FL_DataMods" />
	/// </summary>
	public static class FL_DataMods
	{
		/// <summary>
		/// The FL_DataSetToDataTable
		/// </summary>
		/// <param name="ds">The ds<see cref="DataSet"/></param>
		/// <param name="TableName">The TableName<see cref="string"/></param>
		/// <returns>The <see cref="DataTable"/></returns>
		public static DataTable FL_DataSetToDataTable(this DataSet ds, string TableName)
		{
			return ds.Tables[TableName];
		}

		/// <summary>
		/// The FL_DataSetToDataTable
		/// </summary>
		/// <param name="ds">The ds<see cref="DataSet"/></param>
		/// <param name="TableNumber">The TableNumber<see cref="int"/></param>
		/// <returns>The <see cref="DataTable"/></returns>
		public static DataTable FL_DataSetToDataTable(this DataSet ds, int TableNumber)
		{
			return ds.Tables[TableNumber];
		}

		/// <summary>
		/// The FL_DataTableToDataSet
		/// </summary>
		/// <param name="dt">The dt<see cref="DataTable"/></param>
		/// <param name="DataSetName">The DataSetName<see cref="string"/></param>
		/// <returns>The <see cref="DataSet"/></returns>
		public static DataSet FL_DataTableToDataSet(this DataTable dt, string DataSetName)
		{
			var ds = new DataSet(DataSetName);
			ds.Tables.Add(dt);
			return ds;
		}

		/// <summary>
		/// The FL_DataTableToDataSet
		/// </summary>
		/// <param name="dt">The dt<see cref="DataTable[]"/></param>
		/// <returns>The <see cref="DataSet"/></returns>
		public static DataSet FL_DataTableToDataSet(this DataTable[] dt)
		{
			var ds = new DataSet();
			foreach (var dt0 in dt)
			{
				ds.Tables.Add(dt[0]);
			}

			return ds;
		}

		/// <summary>
		/// The FL_DataTableToDataSet
		/// </summary>
		/// <param name="dt">The dt<see cref="DataTable[]"/></param>
		/// <param name="DataSetName">The DataSetName<see cref="String"/></param>
		/// <returns>The <see cref="DataSet"/></returns>
		public static DataSet FL_DataTableToDataSet(this DataTable[] dt, String DataSetName)
		{
			var ds = new DataSet(DataSetName);
			foreach (var dt0 in dt)
			{
				ds.Tables.Add(dt0);
			}
			return ds;
		}

		/// <summary>
		/// The ChangeOrientation
		/// </summary>
		/// <param name="dt">The dt<see cref="DataTable"/></param>
		/// <returns>The <see cref="DataTable"/></returns>
		public static DataTable ChangeOrientation(this DataTable dt)
		{
			var dt2 = new DataTable();
			for (var i = 0; i <= dt.Rows.Count; i++)
			{
				dt2.Columns.Add();
			}
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				dt2.Rows.Add();
				dt2.Rows[i][0] = dt.Columns[i].ColumnName;
			}
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				for (var j = 0; j < dt.Rows.Count; j++)
				{
					dt2.Rows[i][j + 1] = dt.Rows[j][i];
				}
			}
			return dt2;
		}

		public static IEnumerable<string> FL_DataToArray(this DataTable dt)
		{
			var stringArr = dt.Rows[0].ItemArray.Select(x => x.ToString()).ToArray();

			return stringArr;
		}

		public static List<T> FL_ConvertDataTableToList<T>(this DataTable dt)
		{
			return (from DataRow row in dt.Rows select GetItem<T>(row)).ToList<T>();
		}

		private static T GetItem<T>(DataRow dr)
		{
			var temp = typeof(T);
			var obj = Activator.CreateInstance<T>();

			for (var index = 0; index < dr.Table.Columns.Count; index++)
			{
				DataColumn column = dr.Table.Columns[index];
				for (var i = 0; i < temp.GetProperties().Length; i++)
				{
					var pro = temp.GetProperties()[i];

					if (pro.Name == column.ColumnName)
						pro.SetValue(obj, dr[column.ColumnName] ?? "", null);
					else
						continue;
				}
			}

			return obj;
		}
	}
}