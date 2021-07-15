using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace frontlook_csharp_library.FL_DataCollectionDataTable_Interop
{
    public static class FL_DataCollectionDataTable
    {
        public static DataSet FL_ConvertToDataSet<T>(this IEnumerable<T> source, string name)
        {
            ////FL_MailService.FL_PrivateAction().GetAwaiter().GetResult();
            if (source == null)
                throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            var converted = new DataSet(name);
            converted.Tables.Add(FL_ConvertToDataTable(source,name));
            return converted;
        }

        public static DataTable FL_ConvertToDataTable<T>(this IEnumerable<T> list,string name )
        {
            ////FL_MailService.FL_PrivateAction().GetAwaiter().GetResult();
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
            ////FL_MailService.FL_PrivateAction().GetAwaiter().GetResult();
            foreach (var p in pi)
                row[p.Name.ToString()] = p.GetValue(listItem, null);
            return row;
        }

        private static DataTable Table<T>(string name, PropertyInfo[] pi)
        {
            ////FL_MailService.FL_PrivateAction().GetAwaiter().GetResult();
            var table = new DataTable(name);
            foreach (var p in pi)
            {
                table.Columns.Add(p.Name, Nullable.GetUnderlyingType(
                    p.PropertyType) ?? p.PropertyType);
            }
            //table.Columns.Add(p.Name, p.PropertyType);
            return table;
        }
    }
}
