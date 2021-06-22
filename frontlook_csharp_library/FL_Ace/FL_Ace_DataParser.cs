using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontlook_csharp_library.FL_Ace
{
    public static class FL_Ace_DataParser
    {
        //public const string DBF_Empty_Value = "null";
        public static object FL_AceDataTableDataParser(this object value, Type type)
        {
            string DBF_Empty_Value = "null";
            var s = string.IsNullOrEmpty(value.ToString()) || value.ToString() == "";
            if (s)
            {
                if (type == typeof(double) || type == typeof(int) || type == typeof(long) || type == typeof(decimal))
                {
                    return "null";
                }
                else if (type == typeof(Boolean))
                {
                    var val = $"null";
                    return val;
                }
                else if (type == typeof(DateTime))
                {
                    var val = $"null";
                    return val;
                }
                else
                {
                    return DBF_Empty_Value;
                }
            }
            else
            {
                if (type == typeof(string))
                {
                    return $"'{value.ToString().Replace("'", "''")}'";
                }
                else if (type == typeof(Boolean))
                {
                    var val = $"{(bool)value}";
                    return val;
                }
                else if (type == typeof(double) || type == typeof(int) || type == typeof(long) || type == typeof(decimal))
                {
                    return (double)value;
                }
                else if (type == typeof(DateTime))
                {
                    var val = $"#{(DateTime)value:MM-dd-yyyy}#";
                    return val;
                }
                else
                {
                    return DBF_Empty_Value;
                }
            }
        }
    }
}
