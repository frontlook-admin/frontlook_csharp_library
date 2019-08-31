using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using frontlook_csharp_library.FL_Excel_Data_Interop;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
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

        public static void FL_data_to_xls(string query,string constring1)
        {
            FL_data_to_xls(query, constring1, null);
        }

        public static void FL_data_to_xls(string query,string constring1 = null,string dbfFilepathWithNameAndExtension = null)
        {
            if(string.IsNullOrEmpty(dbfFilepathWithNameAndExtension))
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


        public static void FL_data_to_xls_multiple_datatable_in_single_excel_file(string dbfFilepathWithNameAndExtension)
        {
            string dirName = Path.GetDirectoryName(dbfFilepathWithNameAndExtension);
            string[] filePaths;
            //string[] filepath_null;
            filePaths = Directory.GetFiles(dirName, "*.dbf");
            DataToExcel_single_excel_file(filePaths);
            //FL_Excel_Data_Interop.FL_DataTableToExcel_Helper.FL_DataTableToExcel(dt, Path.GetDirectoryName(dbf_filepath_with_name_and_extension) + @"\" + Path.GetFileNameWithoutExtension(dbf_filepath_with_name_and_extension));
        }


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

        public static void DataToExcel_single_excel_file(string[] filepaths)
        {
            string dirName = Path.GetDirectoryName(filepaths[0]);
            var excelApp = new Application();
            Workbook excelWorkBook = null;
            Range headerRange = null;
            excelWorkBook = excelApp.Workbooks.Add();
            int noWorksheet = 0;
            foreach (string dbfFilepathWithNameAndExtension in filepaths)
            {
                
                DataTable dataTable = FL_get_only_datatable_for_dbf(dbfFilepathWithNameAndExtension);
                try
                {
                    headerRange = null;
                    int columnsCount = 0;

                    if (dataTable == null || (columnsCount = dataTable.Columns.Count) == 0)
                        MessageBox.Show("FL_Excel_Data_Interop.FL_DataTableToExcel_Helper.FL_DataTableToExcel: Null or empty input table!", "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);

                    
                    excelApp.Visible = true;
                   
                    Worksheet excelWorkSheet;
                    if (noWorksheet == 0)
                    {
                        excelWorkSheet = excelWorkBook.ActiveSheet;
                        noWorksheet = noWorksheet + 1;
                    }
                    else
                    {
                        excelWorkSheet = excelWorkBook.Worksheets.Add();
                    }
                    

                    object[] header = new object[columnsCount];
                                  
                    for (int i = 0; i < columnsCount; i++)
                        header[i] = dataTable.Columns[i].ColumnName;

                    headerRange = excelWorkSheet.get_Range((Range)(excelWorkSheet.Cells[1, 1]), (Range)(excelWorkSheet.Cells[1, columnsCount]));
                    headerRange.Value = header;
                    headerRange.Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                    
                    headerRange.Font.Bold = true;

                    // DataCells
                    int rowsCount = dataTable.Rows.Count;
                    object[,] cells = new object[rowsCount, columnsCount];

                    for (int j = 0; j < rowsCount; j++)
                    {
                        for (int i = 0; i < columnsCount; i++)
                        {
                            cells[j, i] = dataTable.Rows[j][i];

                        }
                        
                    }
                    excelWorkSheet.get_Range((Range)(excelWorkSheet.Cells[2, 1]), (Range)(excelWorkSheet.Cells[rowsCount + 1, columnsCount])).Value2 = cells;
                    

                    excelWorkSheet.Name = Path.GetFileNameWithoutExtension(dbfFilepathWithNameAndExtension);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            FileInfo fileinfo = new FileInfo(filepaths[0]);


            //var ExcelFilePath1 = dir_name + @"\" + Path.GetFileNameWithoutExtension(filepaths[0]);
            var excelFilePath = dirName + @"\" + Path.GetFileNameWithoutExtension(dirName);
            //MessageBox.Show(ExcelFilePath1);
            if (string.IsNullOrEmpty(excelFilePath) || File.Exists(excelFilePath))
            {
                excelApp.Visible = true;
            }
            else
            {
                try
                {
                    excelWorkBook.SaveAs(excelFilePath);
                    excelWorkBook.Close();
                    excelApp.Quit();
                    //MessageBox.Show("Excel file saved as "+ExcelFilePath,"DataTable Saved In Excel File",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                        + ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //Marshal.FinalReleaseComObject(ExcelWorkSheet);
            Marshal.FinalReleaseComObject(headerRange);
            Marshal.FinalReleaseComObject(excelApp);
        }

        public static void DataSetToExcel_single_excel_file(DataSet ds)
        {
            //string dir_name = Path.GetDirectoryName(filepaths[0]);
            var excelApp = new Application();
            Workbook excelWorkBook = null;
            Range headerRange = null;
            excelWorkBook = excelApp.Workbooks.Add();
            int noWorksheet = 0;
            var name = ds.DataSetName;
            for (int count = 0; count < ds.Tables.Count; count++)
            {

                DataTable dataTable = ds.Tables[count];
                var xlssheetname = ds.Tables[count].TableName;

                try
                {
                    headerRange = null;
                    int columnsCount = 0;

                    if (dataTable == null || (columnsCount = dataTable.Columns.Count) == 0)
                        MessageBox.Show("FL_Excel_Data_Interop.FL_DataTableToExcel_Helper.FL_DataTableToExcel: Null or empty input table!", "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);


                    excelApp.Visible = true;

                    Worksheet excelWorkSheet;
                    if (noWorksheet == 0)
                    {
                        excelWorkSheet = excelWorkBook.ActiveSheet;
                        noWorksheet = noWorksheet + 1;
                    }
                    else
                    {
                        excelWorkSheet = excelWorkBook.Worksheets.Add();
                    }


                    object[] header = new object[columnsCount];

                    for (int i = 0; i < columnsCount; i++)
                        header[i] = dataTable.Columns[i].ColumnName;

                    headerRange = excelWorkSheet.get_Range((Range)(excelWorkSheet.Cells[1, 1]), (Range)(excelWorkSheet.Cells[1, columnsCount]));
                    headerRange.Value = header;
                    headerRange.Interior.Color = ColorTranslator.ToOle(Color.LightGray);

                    headerRange.Font.Bold = true;

                    // DataCells
                    int rowsCount = dataTable.Rows.Count;
                    object[,] cells = new object[rowsCount, columnsCount];

                    for (int j = 0; j < rowsCount; j++)
                    {
                        for (int i = 0; i < columnsCount; i++)
                        {
                            cells[j, i] = dataTable.Rows[j][i];

                        }

                    }
                    excelWorkSheet.get_Range((Range)(excelWorkSheet.Cells[2, 1]), (Range)(excelWorkSheet.Cells[rowsCount + 1, columnsCount])).Value2 = cells;


                    excelWorkSheet.Name = xlssheetname;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //FileInfo fileinfo = new FileInfo(filepaths[0]);


            //var ExcelFilePath1 = dir_name + @"\" + Path.GetFileNameWithoutExtension(filepaths[0]);
            var excelFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + name;
            //MessageBox.Show(ExcelFilePath1);
            if (string.IsNullOrEmpty(excelFilePath) || File.Exists(excelFilePath))
            {
                excelApp.Visible = true;
            }
            else
            {
                try
                {
                    excelWorkBook.SaveAs(excelFilePath);
                    excelWorkBook.Close();
                    excelApp.Quit();
                    //MessageBox.Show("Excel file saved as "+ExcelFilePath,"DataTable Saved In Excel File",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                                        + ex.Message, "Error..!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            //Marshal.FinalReleaseComObject(ExcelWorkSheet);
            Marshal.FinalReleaseComObject(headerRange);
            Marshal.FinalReleaseComObject(excelApp);
        }
    }
}
