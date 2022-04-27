using frontlook_csharp_library.FL_Ace;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace frontlook_csharp_library.FL_DataBase
{
	public class DisposeClass : IDisposable
	{
		private bool disposed;
		/// <summary>
		/// Destructor
		/// </summary>
		~DisposeClass()
		{
			this.Dispose(false);
		}

		/// <summary>
		/// The dispose method that implements IDisposable.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// The virtual dispose method that allows
		/// classes inherithed from this one to dispose their resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// Dispose managed resources here.

				}
				GC.Collect();
				GC.WaitForPendingFinalizers();
				// Dispose unmanaged resources here.
			}

			disposed = true;
		}
	}
	public class Data : DisposeClass
	{

		public string tableName { get; set; }

	}

	public enum DataType
	{
		String, number, Date, Bool
	}
	public class DataTableParser : Data
	{
		public DataTableParser()
		{

		}

		public DataTableParser(DataTable dt, [CanBeNull] string TableName = null)
		{
			tableName = TableName ?? dt.TableName;
			var rows = dt.Rows.Count;
			var cols = dt.Columns.Count;
			var colNames = new List<string>();
			Cols = new List<ColName>();
			for (var i = 0; i < cols; i++)
			{
				DataColumn column = dt.Columns[i];
				Cols.Add(new ColName(column.ColumnName, tableName));
			}
			Rows = new List<Row>();
			for (var j = 0; j < rows; j++)
			{
				var dsrow = dt.Rows[j];
				Rows.Add(new Row(dsrow, Cols, tableName));
			}

			Sqls = new Sql(dt, false);

		}

		public List<ColName> Cols { get; set; }

		public List<Row> Rows { get; set; }

		public Sql Sqls { get; set; }
	}



	public class Row : Data
	{
		public Row()
		{

		}
		public Row(DataRow dtr, List<ColName> cols, string TableName)
		{
			//string InsertQ1_Init = $"INSERT INTO {tableName} ";
			tableName = TableName;
			colValues = new List<ColValue>();
			for (var i = 0; i < cols.Count; i++)
			{
				var d = dtr[cols[i].ColumnName];
				//var d = dtr[cols[i].ColumnName].FL_AceDataTableDataParser(dtr[cols[i].ColumnName].GetType());
				//var type = d.GetType();
				colValues.Add(new ColValue(cols[i], d));
			}

			/*var InsertQ2_Fields = "(";
            for (var i = 0; i < cols.Count; i++)
            {
                if (i == 0)
                {
                    InsertQ2_Fields += $"{cols[i].ColumnName}";
                }
                else if (i == cols.Count - 1)
                {
                    InsertQ2_Fields += $", {cols[i].ColumnName}) values ";
                }
                else
                {
                    InsertQ2_Fields += $", {cols[i].ColumnName}";
                }
            }
            string InsertQ3_Values = "(";
            for (var i = 0; i < cols.Count; i++)
            {
                var val = colValues.First(e => e.ColumnName == cols[i].ColumnName).colValue.FL_AceDataTableDataParser(colValues.First(e => e.ColumnName == cols[i].ColumnName).colValue.GetType());
                if (i == 0)
                {
                    InsertQ3_Values += $"{val}";
                }
                else if (i == cols.Count - 1)
                {
                    InsertQ3_Values += $", {val});";
                }
                else
                {
                    InsertQ3_Values += $", {val}";
                }
            }
            RowSql = $"{InsertQ1_Init}{InsertQ2_Fields}{InsertQ3_Values}";*/
		}

		public List<ColValue> colValues { get; set; }

		public bool ExecuteSql { get; set; }

		public string GetInsertSqlFields()
		{
			var InsertQ2_Fields = "";
			for (var i = 0; i < colValues.Count; i++)
			{
				if (i == 0)
				{
					InsertQ2_Fields += $"(`{colValues[i].ColumnName}`";
				}
				else if (i == colValues.Count - 1)
				{
					InsertQ2_Fields += $", `{colValues[i].ColumnName}`) values ";
				}
				else
				{
					InsertQ2_Fields += $", `{colValues[i].ColumnName}`";
				}
			}
			return InsertQ2_Fields;
		}

		public string GetInsertSqlValues(out string InsertQ2_Fields, bool EnableNull)
		{
			string InsertQ3_Values = "";
			string _InsertQ2_Values = "";

			var k = 0;
			for (var i = 0; i < colValues.Count; i++)
			{
				var val = colValues[i].colValue.FL_AceDataTableDataParser();
				if (EnableNull)
				{
					if (i == 0)
					{
						InsertQ3_Values += $"({val}";
						_InsertQ2_Values += $"(`{colValues[i].ColumnName}`";
					}
					else if (i == colValues.Count - (1))
					{
						InsertQ3_Values += $", {val})";
						_InsertQ2_Values += $", `{colValues[i].ColumnName}`) values ";
					}
					else
					{
						InsertQ3_Values += $", {val}";
						_InsertQ2_Values += $", `{colValues[i].ColumnName}`";
					}
				}
				else
				{

					if (val.ToString() != "null")
					{
						if (i - k == 0)
						{
							InsertQ3_Values += $"({val}";
							_InsertQ2_Values += $"(`{colValues[i].ColumnName}`";
						}
						else if (i - k == colValues.Count - (1 + k))
						{
							InsertQ3_Values += $", {val})";
							_InsertQ2_Values += $", `{colValues[i].ColumnName}`) values ";
						}
						else
						{
							InsertQ3_Values += $", {val}";
							_InsertQ2_Values += $", `{colValues[i].ColumnName}`";
						}
					}
					else
					{
						if (i - k == colValues.Count - (1 + k))
						{
							InsertQ3_Values += $")";
							_InsertQ2_Values += $") values ";
						}
						else
						{
							k++;
						}
					}
				}
			}

			InsertQ2_Fields = _InsertQ2_Values;
			return InsertQ3_Values;
		}

		public string GetRowSql()
		{
			string InsertQ1_Init = $"INSERT INTO {tableName} ";
			//var InsertQ2_Fields = GetInsertSqlFields();
			var InsertQ2_Fields = "";
			string InsertQ3_Values = GetInsertSqlValues(out InsertQ2_Fields, false);

			/*for (var i = 0; i < colValues.Count; i++)
            {
                var val = colValues[i].colValue.FL_AceDataTableDataParser();
                if (i == 0)
                {
                    InsertQ3_Values += $"({val}";
                }
                else if (i == colValues.Count - 1)
                {
                    InsertQ3_Values += $", {val})";
                }
                else
                {
                    InsertQ3_Values += $", {val}";
                }
            }*/
			var _RowSql = $"{InsertQ1_Init}{InsertQ2_Fields}{InsertQ3_Values};";
			return _RowSql;
		}
	}


	public class ColName : Data
	{
		public ColName()
		{

		}
		public ColName(string colName, string TableName)
		{
			tableName = TableName;
			ColumnName = colName;
		}
		public string ColumnName { get; set; }
	}

	public class ColValue : ColName
	{
		public ColValue()
		{

		}

		public ColValue(ColName colName, object Colvalue)
		{
			ColumnName = colName.ColumnName;
			colValue = Colvalue;
		}

		public object colValue { get; set; }
		public object colOldValue { get; set; }

		public object colTempValue { get; set; }
	}

	public class Sql : Data
	{
		public Sql(DataTable dt, bool EnableNull, [CanBeNull] string TableName = null)
		{
			InsertSqls = new List<string>();
			tableName = TableName ?? dt.TableName;
			var Cols = dt.Columns;

			for (var j = 0; j < dt.Rows.Count; j++)
			{
				string _InsertQ2_Values = "";
				string InsertQ3_Values = "";
				var k = 0;
				for (var i = 0; i < dt.Columns.Count; i++)
				{
					var val = dt.Rows[j][dt.Columns[i]].FL_AceDataTableDataParser();
					if (EnableNull)
					{
						if (i == 0)
						{
							InsertQ3_Values += $"({val}";
							_InsertQ2_Values += $"(`{dt.Columns[i].ColumnName}`";
						}
						else if (i == dt.Columns.Count - (1))
						{
							InsertQ3_Values += $", {val})";
							_InsertQ2_Values += $", `{dt.Columns[i].ColumnName}`) values ";
						}
						else
						{
							InsertQ3_Values += $", {val}";
							_InsertQ2_Values += $", `{dt.Columns[i].ColumnName}`";
						}
					}
					else
					{
						if (val.ToString() != "null")
						{
							if (i - k == 0)
							{
								InsertQ3_Values += $"({val}";
								_InsertQ2_Values += $"(`{dt.Columns[i].ColumnName}`";
							}
							else if (i - k == dt.Columns.Count - (1 + k))
							{
								InsertQ3_Values += $", {val})";
								_InsertQ2_Values += $", `{dt.Columns[i].ColumnName}`) values ";
							}
							else
							{
								InsertQ3_Values += $", {val}";
								_InsertQ2_Values += $", `{dt.Columns[i].ColumnName}`";
							}
						}
						else
						{
							if (i - k == dt.Columns.Count - (1 + k))
							{
								InsertQ3_Values += $")";
								_InsertQ2_Values += $") values ";
							}
							else
							{
								k++;
							}
						}
					}
				}

				InsertSqls.Add($"{InsertQ1_Init}{_InsertQ2_Values}{InsertQ3_Values};");
			}


		}

		public Sql(DataTable dt, string TableName, string a)
		{
			tableName = TableName;
			var Cols = dt.Columns;
			InsertQ2_Fields = "";
			string InsertQ3_Values = "";
			for (var i = 0; i < Cols.Count; i++)
			{
				if (i == 0)
				{
					InsertQ2_Fields += $"(`{Cols[i].ColumnName}`";
					InsertQ3_Values += $"(?";
				}
				else if (i == Cols.Count - 1)
				{
					InsertQ2_Fields += $", `{Cols[i].ColumnName}`) values ";
					InsertQ3_Values += $", ?)";
				}
				else
				{

					InsertQ2_Fields += $", `{Cols[i].ColumnName}`";
					InsertQ3_Values += $", ?";
				}
			}
			InsertSqls = new List<string>();
			InsertCmd = $"{InsertQ1_Init}{InsertQ2_Fields}{InsertQ3_Values}";
		}

		public Sql(string TableName, List<ColName> Cols, List<Row> Rows)
		{
			tableName = TableName;
			InsertQ2_Fields = "(";
			for (var i = 0; i < Cols.Count; i++)
			{
				if (i == 0)
				{
					InsertQ2_Fields += $"`{Cols[i].ColumnName}`";
				}
				else if (i == Cols.Count - 1)
				{
					InsertQ2_Fields += $", `{Cols[i].ColumnName}`) values ";
				}
				else
				{
					InsertQ2_Fields += $", `{Cols[i].ColumnName}`";
				}
			}
			InsertSqls = new List<string>();
			foreach (var row in Rows)
			{
				string InsertQ3_Values = "(";
				for (var i = 0; i < Cols.Count; i++)
				{
					var val = row.colValues.First(e => e.ColumnName == Cols[i].ColumnName).colValue.FL_AceDataTableDataParser();
					if (i == 0)
					{
						InsertQ3_Values += $"{val}";
					}
					else if (i == Cols.Count - 1)
					{
						InsertQ3_Values += $", {val});";
					}
					else
					{
						InsertQ3_Values += $", {val}";
					}
				}
				InsertSqls.Add($"{InsertQ1_Init}{InsertQ2_Fields}{InsertQ3_Values}");
			}
		}

		public Sql(string TableName, List<Row> Rows)
		{
			tableName = TableName;
			InsertSqls.AddRange(Rows.Select(e => e.GetRowSql()));
		}




		public string tableName { get; set; }
		private string InsertQ1_Init => $"INSERT INTO {tableName} ";
		private string InsertQ2_Fields { get; set; }
		public string InsertCmd { get; set; }

		public List<string> InsertSqls { get; set; }
	}
}
