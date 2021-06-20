using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontlook_csharp_library.FL_Ace
{
    public static class FL_Ace_DataParser
    {
        public const string DBF_Empty_Value = "''";
        public static object FL_AceDataTableDataParser(this object value, Type type)
        {
            var s = string.IsNullOrEmpty(value.ToString());
            if (s)
            {
                if (type == typeof(double) || type == typeof(int) || type == typeof(long) || type == typeof(decimal))
                {
                    return 0;
                }
                else if (type == typeof(DateTime))
                {
                    var val = $"##";
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
                    return $"'{value}'";
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
