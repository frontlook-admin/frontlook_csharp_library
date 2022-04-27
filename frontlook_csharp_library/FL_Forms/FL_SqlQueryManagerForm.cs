﻿using frontlook_csharp_library.FL_Dbf_Helper;
using frontlook_csharp_library.FL_Excel_Data_Interop;
using frontlook_csharp_library.FL_General;
using frontlook_csharp_library.FL_General.FL_string_helper;
using JetBrains.Annotations;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace frontlook_csharp_library.FL_Forms
{
	public sealed partial class FL_SqlQueryManagerForm : Form
	{
		private string dbf_filepath, dbf_filename, dbf_filename_withext;
		private DataSet ds = new DataSet("new_dataset1");
		private DataSet ds1 = new DataSet("new_dataset2");
		private DataSet ds2 = new DataSet("new_dataset3");
		private DataSet ds3 = new DataSet("new_dataset4");
		private DataTable dt = new DataTable();
		private DataTable dt1 = new DataTable();
		private DataTable dt2 = new DataTable();
		private DataTable dt3 = new DataTable();

		private string dbf_constring, dbf_constring1, dbf_constring2, s_without_ext;
		private string[] filePaths;
		private static string tab = "	";
		public FL_SqlQueryManagerForm([CanBeNull] string path = null)
		{
			this.Dock = DockStyle.Fill;
			InitializeComponent();
			if (path != null)
			{
				dbf_filepath = path;
				dbf_filename = string.Empty;
				dbf_filename_withext = string.Empty;
				filePaths = Directory.GetFiles(Path.GetDirectoryName(path), "*.dbf");
				filePaths.ToList().ForEach(e => e.FL_ConsoleWriteDebug());
				if (filePaths.Any(e => e.ToLower().Contains("comp.dbf")))
				{
					UpdateWindowName();
				}
			}
			else
			{
				dbf_filepath = string.Empty;
				dbf_filename = string.Empty;
				dbf_filename_withext = string.Empty;
			}
		}

		public FL_SqlQueryManagerForm()
		{
		}

		private void Button1_Click(object sender, EventArgs e)
		{
			//dbf_selection();
			//backgroundWorker1.RunWorkerAsync();
			dbf_folder_selection();
		}

		private void View_db_Click(object sender, EventArgs e)
		{
			//dbf_folder_selection();
			if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
			{
				dbf_folder_selection();
				dataGridView1.DataSource = "";
				dataGridView1.Refresh();
				view_db_in_grid();
			}
			else
			{
				dataGridView1.DataSource = "";
				dataGridView1.Refresh();
				view_db_in_grid();
			}
		}

		private void Db_viewer_DoWork(object sender, DoWorkEventArgs e)
		{
			try_1();
		}

		private void Db_viewer_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
		}

		private void Db_viewer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
		}

		private void Dbf_to_excel_series_Click(object sender, EventArgs e)
		{
			//dbf_folder_selection();
			if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
			{
				dbf_folder_selection();
				if (!dbf_to_excel_series_worker.IsBusy)
				{
					dbf_to_excel_series_worker.WorkerReportsProgress = true;
					dbf_to_excel_series_worker.RunWorkerAsync();
				}
			}
			else
			{
				if (!dbf_to_excel_series_worker.IsBusy)
				{
					dbf_to_excel_series_worker.WorkerReportsProgress = true;
					dbf_to_excel_series_worker.RunWorkerAsync();
				}
			}
		}

		private void Dbf_to_class_series_Click(object sender, EventArgs e)
		{
			//dbf_folder_selection();
			CreateClasses();
			/*if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
            {
                dbf_folder_selection();
                if (!Dbf_to_class_series_worker.IsBusy)
                {
                    Dbf_to_class_series_worker.WorkerReportsProgress = true;
                    Dbf_to_class_series_worker.RunWorkerAsync();
                }
            }
            else
            {
                if (!dbf_to_excel_series_worker.IsBusy)
                {
                    Dbf_to_class_series_worker.WorkerReportsProgress = true;
                    Dbf_to_class_series_worker.RunWorkerAsync();
                }
            }*/
		}

		public string SetDirectory()
		{
			FolderBrowserDialog sf = new FolderBrowserDialog
			{
				Description = "Select Backup Location",
				ShowNewFolderButton = true,
				RootFolder = Environment.SpecialFolder.MyComputer
			};
			//sf.ShowDialog();

			if (sf.ShowDialog() != DialogResult.OK) return string.Empty;
			var bacDir = sf.SelectedPath;

			return bacDir;
		}
		private void Dbf_to_class_series_worker_DoWork(object sender, DoWorkEventArgs e)
		{
			CreateClasses();
			//excel_data_interop.dbf_to_xls_series(dbf_filepath);
		}

		public void CreateClasses()
		{
			//Dbf_to_class_series_worker.ReportProgress((1));
			var i = 0;
			var j = filePaths.Length.FL_ConsoleWriteDebug();

			var fileName = SetDirectory();
			if (!string.IsNullOrEmpty(fileName))
			{

				var classData1 = new StringBuilder();
				var classData2 = new StringBuilder();
				classData1.AppendLine($"using System;\n");
				classData1.AppendLine("namespace AceLib");
				classData1.AppendLine("{");
				classData1.AppendLine("public static class EnumData");
				classData1.AppendLine("{");

				classData2.AppendLine($"using System;");
				classData2.AppendLine($"using System.Collections.Generic;");
				classData2.AppendLine($"using System.Data;");
				classData2.AppendLine($"using frontlook_csharp_library.FL_Dbf_Helper;");
				classData2.AppendLine($"using AceLib.Repository;");
				classData2.AppendLine($"using System.Linq;\n");
				classData2.AppendLine("namespace AceLib");
				classData2.AppendLine("{");
				classData2.AppendLine("public static class DbfToList");
				classData2.AppendLine("{");

				foreach (var dbf_filepath_series in filePaths)
				{
					i = i + 1;
					// label2.Invoke((MethodInvoker)delegate
					// {
					//      label2.Text = dbf_filepath_series;
					//  });
					//var dt = FL_DbfData_To_Excel.FL_data_to_xls_with_datatable(dbf_filepath_series);
					try
					{
						string sWithoutExt = Path.GetFileNameWithoutExtension(dbf_filepath_series);
						string query1 = $"SELECT * FROM `{sWithoutExt}`";
						var v = query1.FL_DBF_ExecuteQuery(dbf_filepath_series, false);
						v.TableName = Path.GetFileNameWithoutExtension(dbf_filepath_series);
						var dbtc = "";
						if (!Directory.Exists(Path.Combine(fileName, "GenClass"))) Directory.CreateDirectory(Path.Combine(fileName, "GenClass"));
						classData1.Append("\n" + GenClass(v, Path.Combine(fileName, "GenClass"), out dbtc) + "\n");
						classData2.Append($"\n{dbtc}");
						//dt = v;
						//dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.DataSource = dt; });
						//dataGridView1.DataSource = dbf_helper.FL_DBF_ExecuteQuery(dbf_filepath_series);
						//label2.Text = dbf_filepath_series;
						//Dbf_to_class_series_worker.ReportProgress((i * 100 / j));
					}
					catch (Exception ex)
					{
						MessageBox.Show($"{ex.Message}", "Error..!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
						//throw;
					}
				}

				classData1.AppendLine("}");
				classData1.AppendLine("}");
				classData2.AppendLine("}");
				classData2.AppendLine("}");

				File.WriteAllText(Path.Combine(fileName, "EnumData.cs"), classData1.ToString());
				File.WriteAllText(Path.Combine(fileName, "DbfToList.cs"), classData2.ToString());
			}
			else
			{
				MessageBox.Show("Data folder not selected..!!", "Info..!!", MessageBoxButtons.OK);
			}
		}

		public string GetType(DataColumn column)
		{
			if (column.DataType == typeof(double))
			{
				return "double?";
			}
			else if (column.DataType == typeof(string))
			{
				return "string";
			}
			else if (column.DataType == typeof(bool))
			{
				return "bool?";
			}
			else if (column.DataType == typeof(int))
			{
				return "int?";
			}
			else if (column.DataType == typeof(DateTime))
			{
				return "DateTime?";
			}
			else if (column.DataType == typeof(TimeSpan))
			{
				return "TimeSpan?";
			}
			else return "";
		}

		public string GetTypeAlt(DataColumn column)
		{
			if (column.DataType == typeof(double))
			{
				return "double";
			}
			else if (column.DataType == typeof(string))
			{
				return "string";
			}
			else if (column.DataType == typeof(bool))
			{
				return "bool";
			}
			else if (column.DataType == typeof(int))
			{
				return "int";
			}
			else if (column.DataType == typeof(DateTime))
			{
				return "DateTime";
			}
			else if (column.DataType == typeof(TimeSpan))
			{
				return "TimeSpan";
			}
			else return "";
		}

		public string GenClass(DataTable dt, string FolderPath, out string dbtc)
		{
			var sbraceOp = "{";
			var sbraceCl = "}";
			var fbraceOp = "(";
			var fbraceCl = ")";
			var cOP = "@";
			var cCL = "#";
			var sep = "^";

			var classData = new StringBuilder();
			classData.AppendLine($"using System;");
			classData.AppendLine($"using System.Collections.Generic;");
			classData.AppendLine($"using System.Data;");
			classData.AppendLine($"using ED = AceLib.EnumData; ");
			classData.AppendLine("namespace AceLib.Repository");
			classData.AppendLine("{");
			classData.AppendLine($"public class {dt.TableName.Trim().FL_ToTitleCase()}");
			classData.AppendLine($"{sbraceOp}");
			classData.AppendLine($"public {dt.TableName.Trim().FL_ToTitleCase()}()");
			classData.AppendLine($"{sbraceOp}");
			classData.AppendLine($"{sbraceCl}");

			classData.AppendLine($"public {dt.TableName.Trim().FL_ToTitleCase()}(DataTableReader data)");
			classData.AppendLine($"{sbraceOp}");
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				var st = /*GetTypeAlt(dt.Columns[i]) == "string" || GetTypeAlt(dt.Columns[i]) == "DateTime" ? */$"data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()] == DBNull.Value ? null : ({GetType(dt.Columns[i])})data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()];" /*: $"({GetType(dt.Columns[i])})data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()];"*/;
				//classData.AppendLine($"this.{dt.Columns[i].ColumnName} = ({GetType(dt.Columns[i])})data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()];");
				classData.AppendLine($"this.{dt.Columns[i].ColumnName} = {st}");
			}
			classData.AppendLine($"{sbraceCl}\n");

			classData.AppendLine($"public {dt.TableName.Trim().FL_ToTitleCase()}(DataRow data)");
			classData.AppendLine($"{sbraceOp}");
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				var st = /*GetTypeAlt(dt.Columns[i]) == "string" || GetTypeAlt(dt.Columns[i]) == "DateTime" ? */$"data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()] == DBNull.Value ? null : ({GetType(dt.Columns[i])})data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()];" /*: $"({GetType(dt.Columns[i])})data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()];"*/;
				//classData.AppendLine($"this.{dt.Columns[i].ColumnName} = ({GetType(dt.Columns[i])})data[ED.{dt.TableName.Trim().FL_ToTitleCase()}.{dt.Columns[i].ColumnName}.ToS()];");
				classData.AppendLine($"this.{dt.Columns[i].ColumnName} = {st}");
			}
			classData.AppendLine($"{sbraceCl}\n");


			classData.AppendLine($"public {dt.TableName.Trim().FL_ToTitleCase()}(");
			var items = new StringBuilder();
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				items.Append($"{cOP}{GetType(dt.Columns[i])} {sep}{dt.Columns[i].ColumnName}{cCL}");
			}
			var items1 = new StringBuilder();
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				items1.Append($"\nthis.{dt.Columns[i].ColumnName} = _{dt.Columns[i].ColumnName};");
			}
			var _items = items.ToString();
			var l = _items.Remove(items.Length - 1).Replace(cOP, "").Replace(sep, "_").Replace(cCL, ",");
			classData.AppendLine(l + ")");
			classData.AppendLine("\n{" + items1 + "\n}");
			classData.Append(items.Replace(cOP, "\npublic ").Replace(sep, "").Replace(cCL, "{ get; set; }") + "}\n}");

			var s = classData.ToString();
			File.WriteAllText(Path.Combine(FolderPath, $"{dt.TableName.Trim().FL_ToTitleCase()}.cs"), s);

			var _1items = new StringBuilder();
			_1items.Append($"\npublic static string ToS(this {dt.TableName.Trim().FL_ToTitleCase()} _{dt.TableName.Trim().ToLower()}) => _{dt.TableName.Trim().ToLower()}.ToString();");
			_1items.AppendLine($"\npublic enum {dt.TableName.Trim().FL_ToTitleCase()}" + "\n{");
			for (var i = 0; i < dt.Columns.Count; i++)
			{
				if (i == 0)
				{
					_1items.Append($"{dt.Columns[i].ColumnName}");
				}
				else
				{
					_1items.Append($"{sep}{dt.Columns[i].ColumnName}");
				}
			}
			var _2items = new StringBuilder();
			_2items.AppendLine($"\npublic static List<{dt.TableName.Trim().FL_ToTitleCase()}> Get{dt.TableName.Trim().FL_ToTitleCase()}List (this string CompanyDir)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var dt = \"SELECT * FROM {dt.TableName.Trim()};\".FL_DBF_ExecuteQuery(CompanyDir);\n");
			_2items.AppendLine($"var rowList = new List<{dt.TableName.Trim().FL_ToTitleCase()}>();");
			_2items.AppendLine($"for(var i = 0; i < dt.Rows.Count; i++)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var row = new {dt.TableName.Trim().FL_ToTitleCase()}(dt.Rows[i]);");
			_2items.AppendLine($"rowList.Add(row);");
			_2items.AppendLine("}");
			_2items.AppendLine("return rowList;");
			_2items.AppendLine("}");

			_2items.AppendLine($"\npublic static List<{dt.TableName.Trim().FL_ToTitleCase()}> Get{dt.TableName.Trim().FL_ToTitleCase()}List (this string SqlQuery,string CompanyDir)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var dt = SqlQuery.FL_DBF_ExecuteQuery(CompanyDir);\n");
			_2items.AppendLine($"var rowList = new List<{dt.TableName.Trim().FL_ToTitleCase()}>();");
			_2items.AppendLine($"for(var i = 0; i < dt.Rows.Count; i++)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var row = new {dt.TableName.Trim().FL_ToTitleCase()}(dt.Rows[i]);");
			_2items.AppendLine($"rowList.Add(row);");
			_2items.AppendLine("}");
			_2items.AppendLine("return rowList;");
			_2items.AppendLine("}");

			_2items.AppendLine($"\npublic static List<{dt.TableName.Trim().FL_ToTitleCase()}> Get{dt.TableName.Trim().FL_ToTitleCase()}List (this DataTable dt)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var rowList = new List<{dt.TableName.Trim().FL_ToTitleCase()}>();");
			_2items.AppendLine($"for(var i = 0; i < dt.Rows.Count; i++)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var row = new {dt.TableName.Trim().FL_ToTitleCase()}(dt.Rows[i]);");
			_2items.AppendLine($"rowList.Add(row);");
			_2items.AppendLine("}");
			_2items.AppendLine("return rowList;");
			_2items.AppendLine("}");

			_2items.AppendLine($"\npublic static List<{dt.TableName.Trim().FL_ToTitleCase()}> Get{dt.TableName.Trim().FL_ToTitleCase()}List (this List<DataRow> dtrow)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var rowList = new List<{dt.TableName.Trim().FL_ToTitleCase()}>();");
			_2items.AppendLine($"for(var i = 0; i < dtrow.Count; i++)");
			_2items.AppendLine("{");
			_2items.AppendLine($"var row = new {dt.TableName.Trim().FL_ToTitleCase()}(dtrow[i]);");
			_2items.AppendLine($"rowList.Add(row);");
			_2items.AppendLine("}");
			_2items.AppendLine("return rowList;");
			_2items.AppendLine("}");

			dbtc = _2items.ToString();

			return _1items.ToString().Replace(sep, ", ") + "\n}";
			//return FolderPath;
		}


		private void Dbf_to_class_series_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progress.FL_Progress(e.ProgressPercentage);
			progress.FL_Progress(e.ProgressPercentage + "%", "Progress...", true);
		}

		private void Dbf_to_class_series_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Done..!!", "Work Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
			progress.Dismiss();
		}


		private void Dbf_to_excel_series_worker_DoWork(object sender, DoWorkEventArgs e)
		{
			dbf_to_excel_series_worker.ReportProgress((1));
			var i = 0;
			var j = filePaths.Length.FL_ConsoleWriteDebug();
			foreach (var dbf_filepath_series in filePaths)
			{
				i = i + 1;
				// label2.Invoke((MethodInvoker)delegate
				// {
				//      label2.Text = dbf_filepath_series;
				//  });
				//var dt = FL_DbfData_To_Excel.FL_data_to_xls_with_datatable(dbf_filepath_series);
				try
				{
					string sWithoutExt = Path.GetFileNameWithoutExtension(dbf_filepath_series);
					string query1 = $"SELECT * FROM `{sWithoutExt}`";
					var v = query1.FL_DBF_ExecuteQuery(dbf_filepath_series, false);
					v.TableName = "Table1";
					var fileName = Path.Combine(Path.GetDirectoryName(dbf_filepath_series),
						Path.GetFileNameWithoutExtension(dbf_filepath_series) + ".xlsx");
					v.FL_WriteExcelAsync(null, fileName, ApplyStyles: false, ApplyHeadStyles: true);

					//dt = v;
					//dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.DataSource = dt; });
					//dataGridView1.DataSource = dbf_helper.FL_DBF_ExecuteQuery(dbf_filepath_series);
					//label2.Text = dbf_filepath_series;
					dbf_to_excel_series_worker.ReportProgress((i * 100 / j));
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}", "Error..!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
					//throw;
				}
			}
			//excel_data_interop.dbf_to_xls_series(dbf_filepath);
		}

		private void Dbf_to_excel_series_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			//label3.Text = e.ProgressPercentage + "%";
			//progress.Value = e.ProgressPercentage;

			progress.FL_Progress(e.ProgressPercentage);
			progress.FL_Progress(e.ProgressPercentage + "%", "Progress...", true);
		}

		private void Dbf_to_excel_series_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MessageBox.Show("Done..!!", "Work Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
			progress.Dismiss();
			//dbf_to_excel_series_worker.ReportProgress(0);
			//progress.Value = 0;
		}

		private void Dbf_to_excel_single_Click(object sender, EventArgs e)
		{
			if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
			{
				dbf_folder_selection();
				if (!db_to_excel_single_worker.IsBusy)
				{
					db_to_excel_single_worker.RunWorkerAsync();
				}
			}
			else
			{
				if (!db_to_excel_single_worker.IsBusy)
				{
					db_to_excel_single_worker.RunWorkerAsync();
				}
			}
		}

		private void Db_to_excel_single_worker_DoWork(object sender, DoWorkEventArgs e)
		{
			FL_DbfData_To_Excel.FL_data_to_xls(dbf_filepath);
		}

		private void Db_to_excel_single_worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
		}

		private void Db_to_excel_single_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
		}

		private void Clear_Click(object sender, EventArgs e)
		{
			dbf_filepath = string.Empty;
			dbf_filename = string.Empty;
			dbf_filename_withext = string.Empty;
			dataGridView1.DataSource = null;
			dataGridView1.Refresh();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//MessageBox.Show(DateTime.Now.ToString());
			//MessageBox.Show("OS Version: "+frontlook_csharp_library.FL_General.FL_Os_Helper.FL_get_os());

			dataGridView1.DataSource = "";

			if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
			{
				dbf_folder_selection();

				var v = query.Text.ToString().Trim().FL_DBF_ExecuteQuery(dbf_filepath, false);
				v.TableName = "Table1";
				v.FL_WriteExcelAsync(null, Path.GetDirectoryName(dbf_filepath));
				//FL_DataTableToExcel_Helper.FL_DataTableToExcel(v, Path.GetDirectoryName(dbf_filename));
				//dataGridView1.DataSource = v;
				/*
                 SELECT smast.SDES as SDES,bill.DT,billmed.VNO,billmed.BATCH,billmed.EXPDT, bill.MPT,aconf.ADD1,aconf.ADD2,aconf.ADD3, (SELECT smast.SDES FROM [smast],[bill] WHERE bill.LCOD=smast.SCOD AND bill.VNO='00534' AND bill.MPT='M') AS TRANSPORT_SDES FROM [billmed],[bill],[smast],[aconf] WHERE billmed.VNO = bill.VNO AND bill.SCOD=smast.SCOD AND bill.MPT='M' AND bill.SCOD=aconf.GCOD AND bill.VNO='00534'
                 */
				var ds = new DataSet("client_info");

				ds.Tables.Add(v);
				//ReportViewer rv = new ReportViewer();

				/*rv.ShowPrintButton = true;
                rv.ShowProgress = true;*/

				//query.Text = query1;

				MessageBox.Show(ds.DataSetName);

				dataGridView1.DataSource = ds.Tables["table1"];
				fast_report();
				//db_viewer.RunWorkerAsync();
				/*using (var report = new Report())
                {
                    report.Load("report1.frx");
                    report.RegisterData(ds, "client_info");
                    report.Prepare(true);
                    PDFSimpleExport export = new PDFSimpleExport();

                    report.Export(export, "result.pdf");
                    //report.SavePrepared("a.pdf");
                    //report.Save("a.pdf");
                }*/
			}
			else
			{
				/*
                 SELECT smast.SDES as SDES,bill.DT,billmed.VNO,billmed.BATCH,billmed.EXPDT, bill.MPT,aconf.ADD1,aconf.ADD2,aconf.ADD3, (SELECT smast.SDES FROM [smast],[bill] WHERE bill.LCOD=smast.SCOD AND bill.VNO='00534' AND bill.MPT='M') AS TRANSPORT_SDES FROM [billmed],[bill],[smast],[aconf] WHERE billmed.VNO = bill.VNO AND bill.SCOD=smast.SCOD AND bill.MPT='M' AND bill.SCOD=aconf.GCOD AND bill.VNO='00534'
                 */
				var v = query.Text.ToString().Trim().FL_DBF_ExecuteQuery(dbf_filepath, false);
				v.TableName = "Table1";
				FL_DataTableToExcel_Helper.FL_DataTableToExcel(v, Path.GetDirectoryName(dbf_filename));
				dataGridView1.DataSource = v;
				var ds = new DataSet("client_info");
				ds.Tables.Add(v);
				//ReportViewer rv = new ReportViewer();

				/*rv.ShowPrintButton = true;
                rv.ShowProgress = true;*/

				//query.Text = query1;
				File.WriteAllText("C:\\Users\\deban\\Desktop\\xml.txt", v.DataSet.GetXml());
				File.WriteAllText("C:\\Users\\deban\\Desktop\\xml1.txt", v.DataSet.GetXmlSchema());

				MessageBox.Show(ds.DataSetName);
				/*using (var report = new Report())
                {
                    report.Load("C:\\Users\\deban\\Desktop\\dbtest.frx");
                    report.RegisterData(ds, "client_info");
                    report.Prepare(true);
                    PDFSimpleExport export = new PDFSimpleExport();

                    report.Export(export, "C:\\Users\\deban\\Desktop\\result.pdf");
                    //report.SavePrepared("a.pdf");
                    //report.Save("a.pdf");
                }*/
			}

			/*using (var report = new Report())
            {
                report.Load("report1.frx");
                report.RegisterData(t, "NorthWind");
                report.Prepare(true);
                PDFSimpleExport export = new PDFSimpleExport();

                report.Export(export, "result.pdf");
                //report.SavePrepared("a.pdf");
                //report.Save("a.pdf");
            }*/
			/* var ci = new CultureInfo("en-IN");
             var cu = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern;
             var cd = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns();
             MessageBox.Show(Thread.CurrentThread.CurrentCulture.ToString());
             MessageBox.Show(CultureInfo.CurrentCulture.Name + "1");
             CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
             var cf_d = currentCulture.DateTimeFormat.ShortDatePattern;
             var cf_t = currentCulture.DateTimeFormat.LongTimePattern;
             MessageBox.Show(cf_d + " 1 "+cf_t+" 2 "+ currentCulture.DateTimeFormat.FullDateTimePattern+" 3 "+
                 currentCulture.DateTimeFormat.SortableDateTimePattern+" 4 "+ currentCulture.DateTimeFormat.LongDatePattern);*/
			//MessageBox.Show(DateTime.ParseExact(DateTime.Now.ToString(),cf_d+" "+cf_t,ci,DateTimeStyles.AssumeLocal).ToString());
		}

		private void nonquery_Click(object sender, EventArgs e)
		{
			var sql = query.Text.ToString().Trim();
			var ct = sql.Count() > 10 ? 10 : sql.Count() - 1;
			var sub = sql.Substring(0, ct).ToLower().Contains("select");
			if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
			{
				dbf_folder_selection();
				ExecuteNonQuery(sql, sub);
			}
			else
			{
				ExecuteNonQuery(sql, sub);
			}
		}

		public void ExecuteNonQuery(string sql, bool sub)
		{
			if (!sub)
			{
				var v = FL_Dbf_Manager.FL_DBF_ExecuteNonQuery(dbf_filepath, sql, false);
				if (v == 1)
				{
					if (sql.ToLower().Contains("insert") && sql.ToLower().Contains("update"))
					{
						MessageBox.Show(@"Query Executed Successfully..!!", @"Success..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Information);
					}
					if (sql.ToLower().Contains("update"))
					{
						MessageBox.Show(@"Update Query Executed Successfully..!!", @"Success..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Information);
					}
					if (sql.ToLower().Contains("insert"))
					{
						MessageBox.Show(@"Insert Query Executed Successfully..!!", @"Success..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Information);
					}
				}
				else
				{
					if (sql.ToLower().Contains("insert") && sql.ToLower().Contains("update"))
					{
						MessageBox.Show(@"Query Execution Failed..!!", @"Error..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Error);
					}
					if (sql.ToLower().Contains("update"))
					{
						MessageBox.Show(@"Update Query Execution Failed..!!", @"Error..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Error);
					}
					if (sql.ToLower().Contains("insert"))
					{
						MessageBox.Show(@"Insert Query Execution Failed..!!", @"Error..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Error);
					}
				}
			}
			else
			{
				MessageBox.Show($"{sql}\n\n THIS QUERY CAN NOT BE EXECUTED FROM EXECUTE NON QUERY..!!", @"ERROR..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Error);
			}
		}

		private void stop_Click(object sender, EventArgs e)
		{
			dbf_to_excel_series_worker.CancelAsync();
			db_to_excel_single_worker.CancelAsync();
			db_viewer.CancelAsync();
		}

		protected void dbf_folder_selection()
		{
			var dbfselect = new OpenFileDialog
			{
				InitialDirectory = string.IsNullOrEmpty(dbf_filepath) ? @"C:\" : Path.GetDirectoryName(dbf_filepath),
				RestoreDirectory = true,
				Filter = "DBF files (*.dbf)|*.dbf|All files (*.*)|*.*",
				FilterIndex = 1,
				Title = "Select DBF File",
				CheckFileExists = true,
				CheckPathExists = true,
				//ReadOnlyChecked = true,
				//ShowReadOnly = true,
				DefaultExt = "dbf",
			};
			//dbfselect.ShowDialog();

			if (dbfselect.ShowDialog() == DialogResult.OK)
			{
				//dataGridView1.Rows.Clear();
				dataGridView1.DataSource = null;
				dataGridView1.Refresh();
				dbf_filepath = dbfselect.FileName;
				dbf_filename_withext = Path.GetFileNameWithoutExtension(dbf_filepath) + Path.GetExtension(dbf_filepath);
				dbf_filename = Path.GetFileNameWithoutExtension(dbf_filepath);
				MessageBox.Show(dbf_filename);
				//label2.Text = dbf_filepath + "    " + dbf_filename;

				var fileInfo = new FileInfo(dbf_filepath);
				var directoryFullPath = fileInfo.DirectoryName;
				var x = Path.GetDirectoryName(dbf_filepath);
				string[] filePaths1;
				//string[] filepath_null;
				filePaths1 = Directory.GetFiles(x, "*.dbf");
				filePaths = filePaths1;
				UpdateWindowName();
				//excel_data_interop.dbf_to_xls_series(dbf_filepath);
			}
			else if (dbfselect.ShowDialog() == DialogResult.Cancel || dbfselect.ShowDialog() == DialogResult.None)
			{
				dbf_filepath = string.Empty;
				dbf_filename_withext = string.Empty;
				dbf_filename = string.Empty;
				//filePaths = string[] filepath_null;
			}
			else
			{
				dbf_filepath = string.Empty;
				dbf_filename_withext = string.Empty;
				dbf_filename = string.Empty;
			}
		}

		private void Test_Click(object sender, EventArgs e)
		{
			//var ci = new CultureInfo("en-IN");
			//MessageBox.Show(frontlook_csharp_library.database_helper.database_helper.FL_get_os());
			var sqlQuery = query.Text.ToString().Trim();

			var ct = sqlQuery.Count() > 10 ? 10 : sqlQuery.Count() - 1;
			if (sqlQuery.Substring(0, ct).ToLower().Contains("select"))
			{
				if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
				{
					dbf_folder_selection();
					//try_1();
					var v = sqlQuery.FL_DBF_ExecuteQuery(dbf_filepath, false);
					dataGridView1.DataSource = v;
					//fast_report();
					//db_viewer.RunWorkerAsync();
				}
				else
				{
					dataGridView1.DataSource = "";
					//try_1();
					var v = sqlQuery.FL_DBF_ExecuteQuery(dbf_filepath, false);
					dataGridView1.DataSource = v;
				}
			}
			else
			{
				MessageBox.Show($"{sqlQuery}\n\n THIS QUERY CAN NOT BE EXECUTED FROM EXECUTE QUERY..!!", @"ERROR..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Error);
			}
		}

		private void CompanyDetails(object sender, EventArgs e)
		{
			//var ci = new CultureInfo("en-IN");
			//MessageBox.Show(frontlook_csharp_library.database_helper.database_helper.FL_get_os());
			var sqlQuery = "SELECT CCOD, CNAME,DT1,DT2,DATAPATH,CADD1,CADD2,CADD3 FROM `COMP`";

			var ct = sqlQuery.Count() > 10 ? 10 : sqlQuery.Count() - 1;
			if (sqlQuery.Substring(0, ct).ToLower().Contains("select"))
			{
				if (dbf_filepath.Equals("") || dbf_filepath.Equals(string.Empty))
				{
					dbf_folder_selection();
					//try_1();
					var v = sqlQuery.FL_DBF_ExecuteQuery(dbf_filepath, false);
					dataGridView1.DataSource = v;
					//fast_report();
					//db_viewer.RunWorkerAsync();
				}
				else
				{
					dataGridView1.DataSource = "";
					//try_1();
					var v = sqlQuery.FL_DBF_ExecuteQuery(dbf_filepath, false);
					dataGridView1.DataSource = v;
				}
			}
			else
			{
				MessageBox.Show($"{sqlQuery}\n\n THIS QUERY CAN NOT BE EXECUTED FROM EXECUTE QUERY..!!", @"ERROR..!!", MessageBoxButtons.OK,
							MessageBoxIcon.Error);
			}
		}

		public void UpdateWindowName()
		{
			var sqlQuery = "SELECT CCOD, CNAME,DT1,DT2 FROM `COMP`";
			var v = sqlQuery.FL_DBF_ExecuteQuery(dbf_filepath, false);
			var dta = $"DBF MANAGER ({v.Rows[0]["CNAME"]} - {v.Rows[0]["CCOD"]} - {(DateTime)v.Rows[0]["DT1"]:dd-MM-yyyy} - {(DateTime)v.Rows[0]["DT2"]:dd-MM-yyyy})";
			this.Text = dta;

		}

		public void fast_report()
		{
			var query1 = "SELECT " +
						 "smast.SDES as SDES,bill.DT,billmed.VNO,billmed.BATCH,billmed.EXPDT, bill.MPT,aconf.ADD1,aconf.ADD2,aconf.ADD3," +
						 "(SELECT smast.SDES FROM [smast],[bill] WHERE bill.LCOD=smast.SCOD AND bill.VNO='00534' AND bill.MPT='M') AS TRANSPORT_SDES " +
						 "FROM " +
						 "[billmed],[bill],[smast],[aconf] WHERE billmed.VNO = bill.VNO AND bill.SCOD=smast.SCOD AND bill.MPT='M' AND bill.SCOD=aconf.GCOD " +
						 "AND " +
						 "bill.VNO='00534'";
			/*
             SELECT smast.SDES as SDES,bill.DT,billmed.VNO,billmed.BATCH,billmed.EXPDT, bill.MPT,aconf.ADD1,aconf.ADD2,aconf.ADD3, (SELECT smast.SDES FROM [smast],[bill] WHERE bill.LCOD=smast.SCOD AND bill.VNO='00534' AND bill.MPT='M') AS TRANSPORT_SDES FROM [billmed],[bill],[smast],[aconf] WHERE billmed.VNO = bill.VNO AND bill.SCOD=smast.SCOD AND bill.MPT='M' AND bill.SCOD=aconf.GCOD AND bill.VNO='00534'
             */
			var ds = new DataSet("client_info");
			ds.Tables.Add(query1.FL_DBF_ExecuteQuery(dbf_filepath, false));
			//SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
			//ReportViewer rv = new ReportViewer();
			/*rv.ShowPrintButton = true;
            rv.ShowProgress = true;*/
			//Report r = new LocalReport();
			//r.DisplayName = "BILL";
			//rv.LocalReport.DataSources = ds.Tables[0];
		}

		protected void view_db_in_grid()
		{
			s_without_ext = "";
			s_without_ext = Path.GetFileNameWithoutExtension(dbf_filename);
			/*var datastring = frontlook_csharp_library.data_helper.data_helper1.constring(dbf_filepath);
            DataTable dtx = new DataTable();
            try
            {
                OleDbConnection connection = new OleDbConnection(datastring);
                string sql = "SELECT * FROM " + s_without_ext;
                MessageBox.Show(sql + "     \n" + datastring+"    "+s_without_ext+"    "+ dbf_filepath + "    ");
                OleDbCommand cmd1 = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter DA = new OleDbDataAdapter(cmd1);
                DA.Fill(dtx);

                //DA.Update(dt);
                connection.Close();
                dataGridView1.DataSource = dtx;
                //BackgroundWorker bgw = new BackgroundWorker();
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message);
            }*/
			dataGridView1.DataSource = FL_Dbf_Manager.FL_DBF_ExecuteQuery(dbf_filepath);
		}

		/*protected void dbf_selection()
        {
            OpenFileDialog dbfselect = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                RestoreDirectory = true,
                Filter = "DBF files (*.dbf)|*.dbf|All files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Select DBF File",
                CheckFileExists = true,
                CheckPathExists= true,
                DefaultExt = "dbf",
            };
            //dbfselect.ShowDialog();

            if(dbfselect.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                dbf_filepath = dbfselect.FileName;
                dbf_filename_withext = Path.GetFileNameWithoutExtension(dbf_filepath)+Path.GetExtension(dbf_filepath);
                dbf_filename = Path.GetFileNameWithoutExtension(dbf_filepath);
                label2.Text = dbf_filepath + "    " +dbf_filename;
                panel1.Visible = true;
                if (!dbf_to_excel_series_worker.IsBusy)
                {
                    dbf_to_excel_series_worker.RunWorkerAsync();
                }
                //data_to_excel.dbf_to_xls_series(dbf_filepath);
                //dbf_file_open();
            }
        }*/

		protected void try_1()
		{
			//query.Text = query1;
			var v = query.Text.ToString().Trim().FL_DBF_ExecuteQuery(dbf_filepath, false);
			dataGridView1.DataSource = v;
			//dataGridView1.DataSource =

			/*DataSet all_ds = dbf_helper.get_all_datatable_in_dataset(filePaths);
            var a = "SELECT * FROM '" + all_ds + "'.BILLMED";
            all_ds.Tables[0].Select()
            */

			//data_to_excel.FL_data_to_xls(dbf_filepath);
			//data_to_excel.FL_data_to_xls_multiple_datatable_in_single_excel_file(dbf_filepath);
			//   MessageBox.Show(frontlook_csharp_library.database_helper.database_helper.FL_odbc_execute_command());
			/*FileInfo fileInfo = new FileInfo(dbf_filepath);
            string directoryFullPath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            string[] filePaths;
            filePaths = Directory.GetFiles(x, "*.dbf");
            foreach(string s in filePaths)
            {
                MessageBox.Show("Ok"+"   "+FL_dbf_helper.get_os()+"   "+FL_dbf_helper.constring(dbf_filepath));
                dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                dataGridView1.DataSource = FL_dbf_helper.data(s);
            }*/
			//view_db_in_grid();

			/*Demo*/
			/*
            using (var report = new Report())
            {
                report.Load("report1.frx");
                report.RegisterData(v.DataSet, "NorthWind");
                report.Prepare(true);
                PDFSimpleExport export = new PDFSimpleExport();

                report.Export(export, "result.pdf");
                //report.SavePrepared("a.pdf");
                //report.Save("a.pdf");
            }
            */
		}
	}
}