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
    public static class data_to_excel
    {
        public static void fl_data_to_xls(string dbf_filepath_with_name_and_extension)
        {
            string constring = dbf_helper.dbf_helper.fl_dbf_constring(dbf_filepath_with_name_and_extension);
            string s_without_ext = Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension);
            string query = "SELECT * FROM " + s_without_ext;
            DataTable dt = database_helper.database_helper.fl_get_oledb_datatable(constring, query);
            DataTableToExcel(dt, Path.GetDirectoryName(dbf_filepath_with_name_and_extension) + @"\" + Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension));
        }


        public static void fl_data_to_xls_multiple_datatable_in_single_excel_file(string dbf_filepath_with_name_and_extension)
        {
            string dir_name = Path.GetDirectoryName(dbf_filepath_with_name_and_extension);
            string[] filePaths;
            //string[] filepath_null;
            filePaths = Directory.GetFiles(dir_name, "*.dbf");
            DatsetToExcel_single_excel_file(filePaths);
            //DataTableToExcel(dt, Path.GetDirectoryName(dbf_filepath_with_name_and_extension) + @"\" + Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension));
        }


        public static DataTable fl_data_to_xls_with_datatable(string dbf_filepath_with_name_and_extension)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath_with_name_and_extension);
            string constring = dbf_helper.dbf_helper.fl_dbf_constring(dbf_filepath_with_name_and_extension);
            string s_without_ext = Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension);
            string query = "SELECT * FROM " + s_without_ext;
            DataTable dt = database_helper.database_helper.fl_get_oledb_datatable(constring, query);
            DataTableToExcel(dt, Path.GetDirectoryName(dbf_filepath_with_name_and_extension) + @"\" + Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension));
            return dt;
        }








        public static DataTable fl_get_only_datatable(string dbf_filepath_with_name_and_extension)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath_with_name_and_extension);
            string constring = dbf_helper.dbf_helper.fl_dbf_constring(dbf_filepath_with_name_and_extension);
            string s_without_ext = Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension);
            string query = "SELECT * FROM " + s_without_ext;
            DataTable dt = database_helper.database_helper.fl_get_oledb_datatable(constring, query);
            return dt;
        }


        public static void DatsetToExcel_single_excel_file(string[] filepaths)
        {
            string dir_name = Path.GetDirectoryName(filepaths[0]);
            var ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook = null;
            Microsoft.Office.Interop.Excel.Range HeaderRange = null;
            ExcelWorkBook = ExcelApp.Workbooks.Add();
            int no_worksheet = 0;
            foreach (string dbf_filepath_with_name_and_extension in filepaths)
            {
                
                DataTable DataTable = fl_get_only_datatable(dbf_filepath_with_name_and_extension);
                try
                {
                    HeaderRange = null;
                    int ColumnsCount = 0;

                    if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                        MessageBox.Show("DataTableToExcel: Null or empty input table!", "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);

                    
                    ExcelApp.Visible = true;
                   
                    Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
                    if (no_worksheet == 0)
                    {
                        ExcelWorkSheet = ExcelWorkBook.ActiveSheet;
                        no_worksheet = no_worksheet + 1;
                    }
                    else
                    {
                        ExcelWorkSheet = ExcelWorkBook.Worksheets.Add();
                    }
                    

                    object[] Header = new object[ColumnsCount];
                                  
                    for (int i = 0; i < ColumnsCount; i++)
                        Header[i] = DataTable.Columns[i].ColumnName;

                    HeaderRange = ExcelWorkSheet.get_Range((Microsoft.Office.Interop.Excel.Range)(ExcelWorkSheet.Cells[1, 1]), (Microsoft.Office.Interop.Excel.Range)(ExcelWorkSheet.Cells[1, ColumnsCount]));
                    HeaderRange.Value = Header;
                    HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    
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
                        
                    }
                    ExcelWorkSheet.get_Range((Microsoft.Office.Interop.Excel.Range)(ExcelWorkSheet.Cells[2, 1]), (Microsoft.Office.Interop.Excel.Range)(ExcelWorkSheet.Cells[RowsCount + 1, ColumnsCount])).Value2 = Cells;
                    

                    ExcelWorkSheet.Name = Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            FileInfo fileinfo = new FileInfo(filepaths[0]);


            //var ExcelFilePath1 = dir_name + @"\" + Path.GetFileNameWithoutExtension(filepaths[0]);
            var ExcelFilePath = dir_name + @"\" + Path.GetFileNameWithoutExtension(dir_name);
            //MessageBox.Show(ExcelFilePath1);
            if (string.IsNullOrEmpty(ExcelFilePath) || File.Exists(ExcelFilePath))
            {
                ExcelApp.Visible = true;
            }
            else
            {
                try
                {
                    ExcelWorkBook.SaveAs(ExcelFilePath);
                    ExcelWorkBook.Close();
                    ExcelApp.Quit();
                    //MessageBox.Show("Excel file saved as "+ExcelFilePath,"DataTable Saved In Excel File",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                        + ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //Marshal.FinalReleaseComObject(ExcelWorkSheet);
            Marshal.FinalReleaseComObject(HeaderRange);
            Marshal.FinalReleaseComObject(ExcelApp);
        }

        public static void DataTableToExcel(DataTable DataTable, string ExcelFilePath)
        {
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
                HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
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


        public static void DataTableToExcel_Excel_Visible(DataTable DataTable, string ExcelFilePath)
        {
            try
            {
                var total_cells = (DataTable.Columns.Count + 1) * (DataTable.Rows.Count + 1);

                int ColumnsCount = 0;

                if (DataTable == null || (ColumnsCount = DataTable.Columns.Count) == 0)
                    MessageBox.Show("DataTableToExcel: Null or empty input table!", "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);

                // load excel, and create a new workbook
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
                Excel.Workbooks.Add();
                Excel.Visible = true;
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
                HeaderRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
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
