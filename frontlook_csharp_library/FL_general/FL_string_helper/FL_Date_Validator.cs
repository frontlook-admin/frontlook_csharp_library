using System;
using System.Globalization;

namespace frontlook_csharp_library.FL_general.FL_string_helper
{
    public class FL_Date_Validator
    {
        public static Boolean val_date_dd_mm_yyyy(string str)
        {
            var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy", "dd-MM-", "dd-MM-yy"};
            DateTime scheduleDate;
            /*bool validDate = DateTime.TryParseExact(
                str,
                dateFormats,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None,
                out scheduleDate);*/
            return DateTime.TryParseExact(
                str,
                dateFormats,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None,
                out scheduleDate);
        }
    }
}
