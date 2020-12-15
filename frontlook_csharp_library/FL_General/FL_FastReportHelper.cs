using FastReport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection;
using FastReport.Web;
using frontlook_csharp_library.FL_DataBase;
using JetBrains.Annotations;

namespace frontlook_csharp_library.FL_General
{
    public static class FL_FastReportHelper
    {

        public static Report FL_GenerateReportFromDataTable(this DataTable dt, string ReportFilePath, string DatabaseName, [CanBeNull] Exception GetExceptions = null)
        {
            try
            {
                var ds = dt.FL_DataTableToDataSet("Database");
                var report = new Report();
                report.Report.RegisterData(ds, DatabaseName);
                report.Report.Load(ReportFilePath);
                report.Report.Prepare();
                return report;
            }
            catch (Exception e)
            {
                GetExceptions = e;
                return null;
            }
        }

        public static WebReport FL_GenerateWebReportFromDataTable(this DataTable dt, string ReportFilePath, string DatabaseName, [CanBeNull] Exception GetExceptions = null)
        {
            try
            {
                var ds = dt.FL_DataTableToDataSet("Table1");
                var report = new WebReport();
                report.Report.RegisterData(ds, DatabaseName);
                report.Report.Load(ReportFilePath);
                report.EmbedPictures = true;
                report.Report.Prepare();
                return report;
            }
            catch (Exception e)
            {
                GetExceptions = e;
                return null;
            }
        }

        public static Report FL_GenerateReportFromDataSet(this DataSet ds, string ReportFilePath, string DatabaseName, [CanBeNull] Exception GetExceptions = null)
        {
            try
            {
                var report = new Report();
                report.Report.RegisterData(ds, DatabaseName);
                report.Report.Load(ReportFilePath);
                report.Report.Prepare();
                return report;
            }
            catch (Exception e)
            {
                GetExceptions = e;
                return null;
            }
        }


        public static void FL_PrintReport(this Report report, PrinterSettings settings = null, [CanBeNull] Exception GetExceptions = null)
        {
            if (settings != null)
            {
                report.Print(settings);
            }
            else
            {
                report.PrintWithDialog();
            }
        }

        public static void FL_PrintReportFromDataSet(this DataSet ds, string ReportFilePath, string DatabaseName, PrinterSettings settings = null, [CanBeNull] Exception GetExceptions = null)
        {
            var report = FL_GenerateReportFromDataSet(ds, ReportFilePath, DatabaseName, GetExceptions);
            report.FL_PrintReport(settings);
        }

        public static Report FL_GenerateReportFromDataSetDebug(this DataSet ds, string ReportFilePath, string DatabaseName)
        {
            var report = new FastReport.Report();
            report.Report.RegisterData(ds, DatabaseName);
            report.Report.Load(ReportFilePath);
            report.Report.Prepare();
            return report;
        }

        public static void FL_PrintReportFromDataSetDebug(this DataSet ds, string ReportFilePath, string DatabaseName, PrinterSettings settings = null)
        {
            var report = FL_GenerateReportFromDataSetDebug(ds, ReportFilePath, DatabaseName);
            report.FL_PrintReport(settings);
        }


        public static WebReport FL_GenerateWebReportFromDataSet(this DataSet ds, string ReportFilePath, string DatabaseName, [CanBeNull] Exception GetExceptions)
        {
            try
            {
                var report = new WebReport();
                report.Report.RegisterData(ds, DatabaseName);
                report.Report.Load(ReportFilePath);
                report.EmbedPictures = true;
                report.Report.Prepare();
                return report;
            }
            catch (Exception e)
            {
                GetExceptions = e;
                return null;
            }
        }

        public static WebReport FL_GenerateWebReportFromDataSetDebug(this DataSet ds, string ReportFilePath, string DatabaseName)
        {
            var report = new WebReport();
            report.Report.RegisterData(ds, DatabaseName);
            report.Report.Load(ReportFilePath);
            report.EmbedPictures = true;
            report.Report.Prepare();
            return report;
        }






        public static DataSet FL_ConvertToDataSet<T>(this IEnumerable<T> source, string name)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            var converted = new DataSet(name);
            converted.Tables.Add(NewTable(name, source));
            return converted;
        }

        public static DataSet FL_ConvertToDataSet<T>(this IEnumerable<T> source, string TableName, string DatasetName)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(TableName))
                throw new ArgumentNullException("TableName");
            if (string.IsNullOrEmpty(DatasetName))
                throw new ArgumentNullException("DatasetName");
            var converted = new DataSet(DatasetName);
            converted.Tables.Add(NewTable(TableName, source));
            return converted;
        }

        public static DataTable FL_ConvertToDataTable<T>(IEnumerable<T> list, string name)
        {
            var propInfo = typeof(T).GetProperties();
            var enumerable = list.ToList();
            var table = Table<T>(name, propInfo);
            using IEnumerator<T> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
                table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, propInfo));
            return table;
        }

        private static DataTable NewTable<T>(string name, IEnumerable<T> list)
        {
            var propInfo = typeof(T).GetProperties();
            var enumerable = list.ToList();
            var table = Table<T>(name, propInfo);
            using IEnumerator<T> enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
                table.Rows.Add(CreateRow<T>(table.NewRow(), enumerator.Current, propInfo));
            return table;
        }

        private static DataRow CreateRow<T>(DataRow row, T listItem, PropertyInfo[] pi)
        {
            foreach (var p in pi)
                row[p.Name.ToString()] = p.GetValue(listItem, null);
            return row;
        }

        private static DataTable Table<T>(string name, PropertyInfo[] pi)
        {
            var table = new DataTable(name);
            foreach (var p in pi)
            {
                table.Columns.Add(p.Name, Nullable.GetUnderlyingType(
                    p.PropertyType) ?? p.PropertyType);
            }
            //table.Columns.Add(p.Name, p.PropertyType);
            return table;
        }

        public static List<EmployeesDemo> GetEmployees()
        {
            var EmployeesList = new List<EmployeesDemo>();
            var myEmp = new EmployeesDemo() { Address = "Oradea", FirstName = "Adrian", LastName = "Rossini", BirthDate = new DateTime(1975, 10, 18), EmployeeID = 1 };
            EmployeesList.Add(myEmp);
            myEmp = new EmployeesDemo() { Address = "Salonta", FirstName = "Arpad", LastName = "Dolgos", BirthDate = new DateTime(1960, 05, 30), EmployeeID = 2 };
            EmployeesList.Add(myEmp);
            myEmp = new EmployeesDemo() { Address = "Beius", FirstName = "Ioane", LastName = "George", BirthDate = new DateTime(1980, 06, 11), EmployeeID = 3 };
            EmployeesList.Add(myEmp);
            myEmp = new EmployeesDemo() { Address = "Debrecen", FirstName = "Janos", LastName = "Pista", BirthDate = new DateTime(1995, 02, 27), EmployeeID = 4 };
            EmployeesList.Add(myEmp);
            return EmployeesList;
        }
    }

    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class EmployeesDemo
    {
        public int EmployeeID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }

    }
}
