using System;
using System.Globalization;
using System.Threading;

namespace frontlook_csharp_library.FL_General.FL_string_helper
{
    //Returns DateTime
    public class FL_DateTime
    {

        ///<summary>
        /// Returns DateTime from string like 30-11-2019
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static DateTime Parse_dd_mm_yyyy(string dateString)
        {
            var ci = new CultureInfo("en-IN");
            if (dateString.Length.Equals(8))
            {
                return DateTime.ParseExact(dateString, "dd-MM-yy", ci, DateTimeStyles.AssumeLocal);
            }
            if (dateString.Length.Equals(6))
            {
                return DateTime.ParseExact(dateString, "dd-MM-", ci, DateTimeStyles.AssumeLocal);
            }
            return DateTime.ParseExact(dateString, "dd-MM-yyyy", ci, DateTimeStyles.AssumeLocal);
        }

        ///<summary>
        /// Returns DateTime from string like 30112019
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static DateTime Parse_ddmmyyyy(string dateString)
        {
            var ci = new CultureInfo("en-IN");
            return DateTime.ParseExact(dateString, "ddMMyyyy", ci, DateTimeStyles.AssumeLocal);
        }

        ///<summary>
        /// Returns DateTime from string like 2019-11-30
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static DateTime Parse_yyyy_mm_dd(string dateString)
        {
            var ci = new CultureInfo("en-IN");
            return DateTime.ParseExact(dateString, "yyyy_MM_dd", ci, DateTimeStyles.AssumeLocal);
        }

        ///<summary>
        /// Returns DateTime from string like 8/30/2019 1:15:36 PM In windows 7
        /// Returns DateTime from string like 30/AUG/2019 1:15:36 PM/30/08/2019 1:15:36 PM In windows 8 onwards.
        /// </summary>
        /// <remarks>Returns Datetime</remarks>
        /// <returns>DateTime</returns>
        public static DateTime Parse_DateTime(string dateTime)
        {

            var ci = new CultureInfo("en-IN");
            //var cu = CultureInfo.CurrentUICulture.DateTimeFormat.FullDateTimePattern;
            //var cd = CultureInfo.CurrentCulture.DateTimeFormat.GetAllDateTimePatterns();
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var cf = currentCulture.DateTimeFormat.ShortDatePattern+" "+ currentCulture.DateTimeFormat.LongTimePattern;
            return DateTime.ParseExact(dateTime, cf, ci, DateTimeStyles.AssumeLocal);
        }
    }
}
