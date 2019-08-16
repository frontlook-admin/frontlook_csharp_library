using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.OleDb;
using forms=System.Windows.Forms;

namespace frontlook_csharp_library.excel_data_interop
{
    public static class excel_data_interop
    {
        public static void fl_data_to_xls(string dbf_filepath)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            string constring = dbf_helper.dbf_helper.fl_dbf_constring(dbf_filepath);
            string s_without_ext = Path.GetFileNameWithoutExtension(dbf_filepath);
            string query = "SELECT * FROM " + s_without_ext;
            DataTable dt = database_helper.database_helper.fl_get_oledb_datatable(constring, query);
            DataTableToExcel(dt, Path.GetDirectoryName(dbf_filepath) + @"\" + Path.GetFileNameWithoutExtension(dbf_filepath));
        }


        public static DataTable fl_data_to_xls_with_datatable(string dbf_filepath)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            string constring = dbf_helper.dbf_helper.fl_dbf_constring(dbf_filepath);
            string s_without_ext = Path.GetFileNameWithoutExtension(dbf_filepath);
            string query = "SELECT * FROM " + s_without_ext;
            DataTable dt = database_helper.database_helper.fl_get_oledb_datatable(constring, query);
            DataTableToExcel(dt, Path.GetDirectoryName(dbf_filepath) + @"\" + Path.GetFileNameWithoutExtension(dbf_filepath));
            return dt;
        }


        public static void DataTableToExcel(DataTable DataTable, string ExcelFilePath)
        {
            //AttachConsole(ATTACH_PARENT_PROCESS);
            try
            {
                var total_cells = (DataTable.Columns.Count + 1) * (DataTable.Rows.Count + 1);
                
                int ColumnsCount = 0;

                if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                    MessageBox.Show("DataTableToExcel: Null or empty input table!", "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);

                // load excel, and create a new workbook
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();
                //Excel.Visible = true;
                // single worksheet
                Microsoft.Office.Interop.Excel._Worksheet Worksheet = Excel.ActiveSheet;

                object[] Header = new object[ColumnsCount];
                //var stopwatch = new Stopwatch();
                //stopwatch.Reset();
                //stopwatch.Start();
                // column headings               
                for (int i = 0; i < ColumnsCount; i++)
                    Header[i] = DataTable.Columns[i].ColumnName;

                Microsoft.Office.Interop.Excel.Range HeaderRange = Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[1, ColumnsCount]));
                HeaderRange.Value = Header;
                //HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                //HeaderRange.Interior.Color = SystemColors.GrayTextBrush;
                HeaderRange.Font.Bold = true;

                // DataCells
                int RowsCount = DataTable.Rows.Count;
                object[,] Cells = new object[RowsCount, ColumnsCount];

                for (int j = 0; j < RowsCount; j++)
                {
                    for (int i = 0; i < ColumnsCount; i++)
                    {
                        Cells[j, i] = DataTable.Rows[j][i];

                    }
                    //rel_cells = 0;
                    //rel_cells = ColumnsCount * j;
                    //var time = stopwatch.ElapsedMilliseconds;
                    //var speed = (rel_cells / time);
                    //Console.WriteLine("Speed:" + speed + "cells/sec");
                }
                Worksheet.get_Range((Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(Worksheet.Cells[RowsCount + 1, ColumnsCount])).Value2 = Cells;
                //stopwatch.Stop();
                //var final_speed = (total_cells / stopwatch.ElapsedMilliseconds);
                //Console.WriteLine("Completed At Speed:" + final_speed + "cells/sec");
                if (string.IsNullOrEmpty(ExcelFilePath) || File.Exists(ExcelFilePath))
                {

                    Excel.Visible = true;
                }
                else
                { // no file path is given
                    try
                    {
                        Worksheet.SaveAs(ExcelFilePath);
                        Excel.Quit();
                        //MessageBox.Show("Excel file saved as "+ExcelFilePath,"DataTable Saved In Excel File",MessageBoxButton.OK,MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                            + ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                Marshal.FinalReleaseComObject(Worksheet);
                Marshal.FinalReleaseComObject(HeaderRange);
                Marshal.FinalReleaseComObject(Excel);

                //System.Windows.MessageBox.Show("Excel file saved!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();


    }
}
