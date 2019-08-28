using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace frontlook_csharp_library.FL_general
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

        ///<summary>
        /// Returns DateTime from string like 8/30/2019 1:15:36 PM In windows 7
        /// Returns DateTime from string like 30/AUG/2019 1:15:36 PM/30/08/2019 1:15:36 PM In windows 8 onwards.
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static System.DateTime Parse_DateTime(string DateTime)
        {

            var ci = new CultureInfo("en-IN");
            //var cu = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern;
            //var cd = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns();
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            var cf = currentCulture.DateTimeFormat.ShortDatePattern+" "+ currentCulture.DateTimeFormat.LongTimePattern;
            return System.DateTime.ParseExact(DateTime, cf, ci, DateTimeStyles.AssumeLocal);
        }
    }
}
