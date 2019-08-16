﻿using System;
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

        public static void dbf_to_xls_series(string dbf_filepath/*, forms.DataGridView dataGridView1*/)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";
            string dbf_constring1 = "";
            //string dbf_filename = "";
            string[] filePaths;
            string x = Path.GetDirectoryName(dbf_filepath);
            filePaths = Directory.GetFiles(x, "*.dbf");
            //string[] filePaths = Directory.GetFiles(Path.GetDirectoryName(dbf_filepath), "*.dbf");


            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "7";
                        else if (vs.Minor == 2)
                            operatingSystem = "8";
                        else
                            operatingSystem = "8.1";
                        break;
                    case 10:
                        operatingSystem = "10";
                        break;
                    default:
                        break;
                }
            }
            //Make sure we actually got something in our OS check
            //We don't want to just return " Service Pack 2" or " 32-bit"
            //That information is useless without the OS version.
            if (operatingSystem != "")
            {
                //Got something.  Let's prepend "Windows" and get more info.
                operatingSystem = "Windows " + operatingSystem;
                //See if there's a service pack installed.

                /*if (os.ServicePack != "")
                {
                    //Append it to the OS name.  i.e. "Windows XP Service Pack 3"
                    operatingSystem += " " + os.ServicePack;
                }*/

                //Append the OS architecture.  i.e. "Windows XP Service Pack 3 32-bit"
                //operatingSystem += " " + getOSArchitecture().ToString() + "-bit";
            }
            //Return the information we've gathered.

            switch (operatingSystem)
            {
                case "Windows XP":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 7":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows Vista":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 8":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 8.1":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 10":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                default:
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
            }
            string excelFilename = "";
            //string xml = "";
            //string xml_schema = "";
            string s_without_ext = "";
            foreach (string s in filePaths)
            {
                DataTable dt = new DataTable();
                //MessageBox.Show(s);
                excelFilename = "";
                s_without_ext = "";
                excelFilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xlsx";
                //xml = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xml";
                //xml_schema = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + "_schema.xml";
                s_without_ext = Path.GetFileNameWithoutExtension(s);

                //Path.GetFileNameWithoutExtension(dbf_filepath);
                try
                {
                    
                    OleDbConnection connection = new OleDbConnection(dbf_constring1);
                    string sql = "SELECT * FROM " + s_without_ext;
                    //string sql = "SELECT COUNT(),* FROM " + dbf_filename;
                    MessageBox.Show(sql + "      \n" + dbf_constring1);
                    OleDbCommand cmd = new OleDbCommand(sql, connection);
                    connection.Open();
                    OleDbDataAdapter DA = new OleDbDataAdapter(cmd);
                    DA.Fill(dt);
                    //DA.Update(dt);
                   /* dataGridView1.DataSource = null;
                    dataGridView1.Refresh();
                    forms.BindingSource bsource = new forms.BindingSource();
                    bsource.DataSource = dt;
                    dataGridView1.DataSource = bsource;
                    dataGridView1.Refresh();*/
                    connection.Close();
                    DataTableToExcel(dt, Path.GetDirectoryName(s) + @"\" + Path.GetFileNameWithoutExtension(s));
                    //excel_data_interop.DataTableToExcel(dt, excelFilename);
                }
                catch (OleDbException e)
                {
                    MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            MessageBox.Show("DataTables are Successfully saved in Excle files..","Done",MessageBoxButton.OK,MessageBoxImage.Information);
        }


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

        public static void dbf_to_xls_single(string dbf_filepath/*,forms.DataGridView dataGridView1*/)
        {
            FileInfo fileInfo = new FileInfo(dbf_filepath);
            string dbf_directory_filepath = fileInfo.DirectoryName;
            string x = Path.GetDirectoryName(dbf_filepath);
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";
            string dbf_constring1 = "";
            //string dbf_filename = "";

            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "7";
                        else if (vs.Minor == 2)
                            operatingSystem = "8";
                        else
                            operatingSystem = "8.1";
                        break;
                    case 10:
                        operatingSystem = "10";
                        break;
                    default:
                        break;
                }
            }

            if (operatingSystem != "")
            {
                operatingSystem = "Windows " + operatingSystem;
            }

            switch (operatingSystem)
            {
                case "Windows XP":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 7":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows Vista":
                    dbf_constring1 = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 8":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 8.1":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                case "Windows 10":
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
                default:
                    dbf_constring1 = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + @dbf_directory_filepath + ";Extended Properties=dBase IV;User ID=;Password=";
                    break;
            }
            string excelFilename = "";
            //string xml = "";
            //string xml_schema = "";
            string s_without_ext = "";
            string s = "";

            DataTable dt = new DataTable();
            s = dbf_filepath;
            excelFilename = "";
            s_without_ext = "";
            excelFilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + s + ".xlsx";
            s_without_ext = Path.GetFileNameWithoutExtension(s);

            try
            {

                OleDbConnection connection = new OleDbConnection(dbf_constring1);
                string sql = "SELECT * FROM " + s_without_ext;

                OleDbCommand cmd = new OleDbCommand(sql, connection);
                connection.Open();
                OleDbDataAdapter DA = new OleDbDataAdapter(cmd);
                DA.Fill(dt);

                /*dataGridView1.DataSource = null;
                dataGridView1.Refresh();
                forms.BindingSource bsource = new forms.BindingSource();
                bsource.DataSource = dt;
                dataGridView1.DataSource = bsource;
                dataGridView1.Refresh();*/

                //DA.Update(dt);
                connection.Close();
                //BackgroundWorker bgw = new BackgroundWorker();

                DataTableToExcel(dt, Path.GetDirectoryName(s) + @"\" + Path.GetFileNameWithoutExtension(s));
            }
            catch (OleDbException e)
            {
                MessageBox.Show("Error : " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            MessageBox.Show("DataTable is Successfully saved in Excle file..", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
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
