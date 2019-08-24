using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace frontlook_csharp_library.general
{
    //Returns DateTime
    public class FL_DateTime
    {

        ///<summary>
        /// Returns DateTime from string like 30-11-2019
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static System.DateTime Parse_dd_mm_yyyy(string date_string)
        {
            var ci = new CultureInfo("en-IN");
            if (date_string.Length.Equals(8))
            {
                return System.DateTime.ParseExact(date_string, "dd-MM-yy", ci, DateTimeStyles.AssumeLocal);
            }
            else if (date_string.Length.Equals(6))
            {
                return System.DateTime.ParseExact(date_string, "dd-MM-", ci, DateTimeStyles.AssumeLocal);
            }
            else
            {
                return System.DateTime.ParseExact(date_string, "dd-MM-yyyy", ci, DateTimeStyles.AssumeLocal);
            }
            
        }

        ///<summary>
        /// Returns DateTime from string like 30112019
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static System.DateTime Parse_ddmmyyyy(string date_string)
        {
            var ci = new CultureInfo("en-IN");
            return System.DateTime.ParseExact(date_string, "ddMMyyyy", ci, DateTimeStyles.AssumeLocal);
        }

        ///<summary>
        /// Returns DateTime from string like 2019-11-30
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static System.DateTime Parse_yyyy_mm_dd(string date_string)
        {
            var ci = new CultureInfo("en-IN");
            return System.DateTime.ParseExact(date_string, "yyyy_MM_dd", ci, DateTimeStyles.AssumeLocal);
        }

        public static System.DateTime Parse_DateTime(string DateTime)
        {
            var ci = new CultureInfo("en-IN");
            if (database_helper.database_helper.fl_get_os().Equals("7"))
            {
                return System.DateTime.ParseExact(DateTime, "M/d/yyyy h:mm:ss tt", ci, DateTimeStyles.AssumeLocal);
            }
            else
            {
                return System.DateTime.ParseExact(DateTime, "dd/MMM/yy h:mm:ss tt", ci, DateTimeStyles.AssumeLocal);
            }
        }
    }
}
