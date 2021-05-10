using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ExpressionIsAlwaysNull

namespace frontlook_csharp_library.FL_General
{
    public static class FL_DateHelper
    {
        public enum FL_DateTimeFormats
        {
            FL_DateParseFormats, FL_DateParseFormatsT1, FL_DateParseFormatsT2, FL_DateParseFormatsT3, FL_DateParseFormatsT4, FL_DateParseFormatsT5, NoFormat
        }

        public static string[] FL_GetDateTimeFormats(FL_DateTimeFormats DateTimeEnumFormat = FL_DateTimeFormats.FL_DateParseFormats)
        {
            return DateTimeEnumFormat switch
            {
                FL_DateTimeFormats.FL_DateParseFormats => FL_DateParseFormats,
                FL_DateTimeFormats.FL_DateParseFormatsT1 => FL_DateParseFormatsT1,
                FL_DateTimeFormats.FL_DateParseFormatsT2 => FL_DateParseFormatsT2,
                FL_DateTimeFormats.FL_DateParseFormatsT3 => FL_DateParseFormatsT3,
                FL_DateTimeFormats.FL_DateParseFormatsT4 => FL_DateParseFormatsT4,
                FL_DateTimeFormats.FL_DateParseFormatsT5 => FL_DateParseFormatsT5,
                FL_DateTimeFormats.NoFormat => null,
                _ => FL_DateParseFormats
            };
        }

        public static string[] FL_DateParseFormats => new string[]
        { "MM/dd/yyyy", "M/d/yyyy", "MM-dd-yyyy", "M-d-yyyy",
            "dd/M/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy" ,
            "MMM/dd/yyyy", "MMM/d/yyyy", "MMM-dd-yyyy", "MMM-d-yyyy",
            "dd/MMM/yyyy","d/MMM/yyyy","dd-MMM-yyyy","d-MMM-yyyy",
            "ddMMyyyy","MMddyyyy","yyyyMMdd"
        };

        public static string[] FL_TimeParseFormats => new string[]
        { "hh:mm:ss", "HH:mm:ss tt"
        };

        private static string[] FL_DateParseFormatsT1 => new string[] { "MM/dd/yyyy", "M/d/yyyy", "MM-dd-yyyy", "M-d-yyyy" };
        private static string[] FL_DateParseFormatsT2 => new string[] { "dd/M/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy" };
        private static string[] FL_DateParseFormatsT3 => new string[] { "MMM/dd/yyyy", "MMM/d/yyyy", "MMM-dd-yyyy", "MMM-d-yyyy" };
        private static string[] FL_DateParseFormatsT4 => new string[] { "dd/MMM/yyyy", "d/MMM/yyyy", "dd-MMM-yyyy", "d-MMM-yyyy" };
        private static string[] FL_DateParseFormatsT5 => new string[] { "ddMMyyyy", "MMddyyyy", "yyyyMMdd" };

        public static DateTime? FL_ParseDateTime(this string date)
        {
            if (string.IsNullOrEmpty(date)) return null;
            DateTime? dt;
            try
            {
                dt = DateTime.ParseExact(date, FL_DateParseFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        public static TimeSpan? FL_ParseTime(this string date)
        {
            if (string.IsNullOrEmpty(date)) return null;
            TimeSpan? dt;
            try
            {
                dt = DateTime.ParseExact(date, FL_TimeParseFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal).TimeOfDay;
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

        public static IEnumerable<int> GetDays(int NoDays)
        {
            var days = new int[NoDays];
            for (var i = 0; i < NoDays; i++)
            {
                days[i] = i;
            }
            return days;
        }

        public static IEnumerable<int> GetHour()
        {
            var hour = new int[24];
            for (var i = 0; i < 24; i++)
            {
                hour[i] = i;
            }
            return hour;
        }

        public static IEnumerable<int> GetMinute()
        {
            var minute = new int[60];
            for (var i = 0; i < 60; i++)
            {
                minute[i] = i;
            }
            return minute;
        }

        public static IEnumerable<int> GetSecond()
        {
            return GetMinute();
        }

        public static IEnumerable<int> GetMiliSecond()
        {
            var miliSecond = new int[1000];
            for (var i = 0; i < 1000; i++)
            {
                miliSecond[i] = i;
            }
            return miliSecond;
        }

        public static IEnumerable<int> GetDays(this DateTime date, bool? Inverse = null)
        {
            var countDays = date.LastDateOfMonth().Day;
            var days = new int[countDays];
            for (var i = 1; i <= countDays; i++)
            {
                days[i - 1] = i;
            }

            return Inverse.GetValueOrDefault() ? days.Reverse() : days;
        }

        public static IEnumerable<string> GetMonths(this DateTime date, bool? Inverse = null)
        {
            var month = new string[12];
            for (var i = 1; i <= 12; i++)
            {
                month[i - 1] = DateTime.ParseExact(i + "-", "M-", new CultureInfo("EN")).ToString("MMMM");
            }

            return Inverse.GetValueOrDefault() ? month.Reverse() : month;
        }

        public static IEnumerable<int> GetPreviousYears(this DateTime date, int count, bool? Inverse = null)
        {
            var year = new int[count + 1];
            var curyear = int.Parse(date.ToString("yyyy"));
            year[0] = curyear;
            for (var i = count; i >= 0; i--)
            {
                year[i] = curyear - i;
            }
            return Inverse.GetValueOrDefault() ? year.Reverse() : year;
        }

        public static IEnumerable<int> GetNextYears(this DateTime date, int count, bool? Inverse = null)
        {
            var year = new int[count + 1];
            var curyear = int.Parse(date.ToString("yyyy"));
            year[0] = curyear;
            for (var i = count; i >= 0; i--)
            {
                year[i] = curyear + i;
            }

            return Inverse.GetValueOrDefault() ? year.Reverse() : year;
        }

        public static IEnumerable<int> GetNextPrevYears(this DateTime date, int count, bool? Inverse = null)
        {
            var year = new int[count + 1 + count];
            var curyear = int.Parse(date.ToString("yyyy"));
            var j = count + 1 + count;
            year[0] = curyear;
            for (var i = -count; i <= count; i++)
            {
                year[i + count] = curyear + i;
            }

            return Inverse.GetValueOrDefault() ? year.Reverse() : year;
        }

        /// <summary>
        /// Returns Last Date of a month at absolute time
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Returns Last Date of a month at 0 time</returns>
        public static DateTime LastDateOfMonth(this DateTime date)
        {
            var DayNo = DateTime.DaysInMonth(date.Year, date.Month);
            return DateTime.Parse(date.Year + "-" + date.Month + "-" + DayNo).Add(date.TimeOfDay);
        }

        /// <summary>
        /// Returns First Date of a month at absolute time
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Returns First Date of a month at 0 time</returns>
        public static DateTime FirstDateOfMonth(this DateTime date)
        {
            return DateTime.Parse(date.Year + "-" + date.Month + "-" + "01").Add(date.TimeOfDay);
        }

        /// <summary>
        /// Returns Last Date of a month at 0 time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="OHour">If OHour true Returns O Hour</param>
        /// <returns>Returns Last Date of a month at 0/Last time</returns>
        public static DateTime LastDateOfMonthMod(this DateTime date, bool OHour = false)
        {
            var DayNo = DateTime.DaysInMonth(date.Year, date.Month);
            return OHour ? DateTime.Parse(date.Year + "-" + date.Month + "-" + DayNo).MinTimeOfDay() :
                DateTime.Parse(date.Year + "-" + date.Month + "-" + DayNo).MaxTimeOfDay();
        }

        /// <summary>
        /// Returns First Date of a month at 0 time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="OHour">If OHour false Returns Max Hour</param>
        /// <returns>Returns First Date of a month at 0 time</returns>
        public static DateTime FirstDateOfMonthMod(this DateTime date, bool OHour = true)
        {
            return OHour ? DateTime.Parse(date.Year + "-" + date.Month + "-" + "01").MinTimeOfDay() :
                DateTime.Parse(date.Year + "-" + date.Month + "-" + "01").MaxTimeOfDay();
        }

        public static int GetNumberOfDays(this DateTime date1, DateTime date2)
        {
            return date2.DayOfYear - date1.DayOfYear;
        }

        public static int GetNumberOfDaysOfSpecificName(this DateTime date1, DateTime date2, DayOfWeek DayName)
        {
            var ts = date2 - date1;                       // Total duration
            var count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            var remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            var sinceLastDay = date2.DayOfWeek - DayName;   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }

        public static int GetNumberOfDaysOfSpecificName(this DateTime date1, DateTime date2, string DayName)
        {
            var day = GetDayOfWeek(DayName);
            return GetNumberOfDaysOfSpecificName(date1, date2, day);
        }

        public static DayOfWeek GetDayOfWeek(this string DayName)
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), DayName);
        }

        public static string GetNormalisedDayOfWeek(this string DayName)
        {
            if (string.IsNullOrEmpty(DayName) || string.IsNullOrWhiteSpace(DayName) || DayName == "NA" || DayName == "na")
            {
                DayName = "NA";
            }
            string v;
            switch (DayName.ToLower())
            {
                case "sunday":
                    v = "Sunday";
                    break;

                case "monday":
                    v = "Monday";
                    break;

                case "tuesday":
                    v = "Tuesday";
                    break;

                case "wednesday":
                    v = "Wednesday";
                    break;

                case "thursday":
                    v = "Thursday";
                    break;

                case "friday":
                    v = "Friday";
                    break;

                case "saturday":
                    v = "Saturday";
                    break;

                default:
                    v = null;
                    break;
            }
            return string.IsNullOrEmpty(v) || string.IsNullOrWhiteSpace(v) ? "NA" : GetDayOfWeek(v).ToString();
        }

        /// <summary>
        /// Returns Total Number Of Week Ends i.e. Saturdays & Sundays in a given period of time.
        /// </summary>
        /// <param name="startDate">Denotes the starting date</param>
        /// <param name="endDate">Denotes the ending date</param>
        /// <returns></returns>
        public static int CountWeekEnds(DateTime startDate, DateTime endDate)
        {
            var weekEndCount = 0;
            if (startDate > endDate)
            {
                var temp = startDate;
                startDate = endDate;
                endDate = temp;
            }
            var diff = endDate - startDate;
            var days = diff.Days;
            for (var i = 0; i <= days; i++)
            {
                var testDate = startDate.AddDays(i);
                if (testDate.DayOfWeek == DayOfWeek.Saturday || testDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    weekEndCount += 1;
                }
            }
            return weekEndCount;
        }

        public static DateTime SubstractMonth(this DateTime date, int NumberOfMonths)
        {
            /*var monthSpanVal = (date.Month - NumberOfMonths);
            int yearSpan;
            int monthSpan;
            if (monthSpanVal < 0 || monthSpanVal == 0)
            {
                if ((monthSpanVal + 12) < 0)
                {
                    monthSpan = (-(monthSpanVal + 12));
                    //Console.WriteLine("a1:" + monthSpan + " " + monthSpanVal);
                }
                else
                {
                    monthSpan = (monthSpanVal + 12);
                    //Console.WriteLine("a2:" + monthSpan + " " + monthSpanVal);
                }
                yearSpan = (monthSpan / 12) + 1;
            }
            else
            {
                monthSpan = (date.Month - NumberOfMonths);
                //Console.WriteLine("a3:" + monthSpan);
                yearSpan = date.Year;
            }

            var Year = date.Year - yearSpan;
            int Month;
            if (monthSpan <= 12)
            {
                Month = monthSpan;
            }
            else
            {
                Month = monthSpan % 12;
            }
            //Console.WriteLine(Month);
            var DayNo = DateTime.DaysInMonth(Year, Month);

            var Day = date.Day > DayNo ? DayNo : date.Day;

            var Time = date.TimeOfDay;
            var ResultDate = DateTime.Parse(Year + "-" + Month + "-" + Day).Add(Time);*/
            //Console.WriteLine(ResultDate);
            return date.AddMonths(-NumberOfMonths);
        }

        public static DateTime AddMonth(this DateTime date, int NumberOfMonths)
        {
            /*var monthSpan = (date.Month + NumberOfMonths);
            var yearSpan = monthSpan / 12;
            var Year = date.Year + yearSpan;
            int Month;
            if (monthSpan <= 12)
            {
                Month = monthSpan;
            }
            else
            {
                Month = monthSpan % 12;
            }
            var DayNo = DateTime.DaysInMonth(Year, Month);

            var Day = date.Day > DayNo ? DayNo : date.Day;

            var Time = date.TimeOfDay;
            var ResultDate = DateTime.Parse(Year + 1 + "-" + Month + "-" + Day).Add(Time);
            //Console.WriteLine(ResultDate);
            return ResultDate;*/
            return date.AddMonths(NumberOfMonths);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static DateTime MinTimeOfDay(this DateTime date)
        {
            return date.Add(-date.TimeOfDay);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static DateTime MinTimeOfDay(this DateTime? date)
        {
            return date.GetValueOrDefault(DateTime.Now).MinTimeOfDay();
        }

        public static DateTime MaxTimeOfDay(this DateTime date)
        {
            return date.MinTimeOfDay().Add(date.MinTimeOfDay().AddMilliseconds(-1).TimeOfDay);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static DateTime MaxTimeOfDay(this DateTime? date)
        {
            return date.GetValueOrDefault(DateTime.Now).MaxTimeOfDay();
        }

        public static DateTime FromDateDefaultLastMonth(this DateTime? date)
        {
            return date.GetValueOrDefault(DateTime.Now.SubstractMonth(1).FirstDateOfMonth()).MinTimeOfDay();
        }

        public static DateTime ToDateDefaultLastMonth(this DateTime? date)
        {
            return date.GetValueOrDefault(DateTime.Now.SubstractMonth(1).LastDateOfMonth()).MaxTimeOfDay();
        }

        public static string DateTimeForRawSql(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string DateTimeForRawSql(this DateTime? date)
        {
            return date.GetValueOrDefault(DateTime.Now).DateTimeForRawSql();
        }
    }

    public class DateSelector
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        [RegularExpression(@"[[0-9]+]*")]
        [Display(Name = "Day")]
        public int? FromDay { get; set; }

        [RegularExpression(@"^(2[0-4]|[0-1]?[0-9])$")]
        [Display(Name = "Hour")]
        public int? FromHour { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Minute")]
        public int? FromMinute { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Second")]
        public int? FromSecond { get; set; }

        [RegularExpression(@"^([0-9]?[0-9]?[0-9])$")]
        [Display(Name = "Mili Second")]
        public int? FromMiliSecond { get; set; }

        [Display(Name = "AM/PM")]
        [CanBeNull]
        public string FromAmPm { get; set; }

        [RegularExpression(@"[[0-9]+]*")]
        [Display(Name = "Day")]
        public int? ToDay { get; set; }

        [RegularExpression(@"^(2[0-4]|[0-1]?[0-9])$")]
        [Display(Name = "Hour")]
        public int? ToHour { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Minute")]
        public int? ToMinute { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Second")]
        public int? ToSecond { get; set; }

        [Display(Name = "Mili Second")]
        [RegularExpression(@"^([0-9]?[0-9]?[0-9])$")]
        public int? ToMiliSecond { get; set; }

        [Display(Name = "AM/PM")]
        [CanBeNull]
        public string ToAmPm { get; set; }

        [RegularExpression(@"[[0-9]+]*")]
        [Display(Name = "Day")]
        public int? ChangeDay { get; set; }

        [RegularExpression(@"^(2[0-4]|[0-1]?[0-9])$")]
        [Display(Name = "Hour")]
        public int? ChangeHour { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Minute")]
        public int? ChangeMinute { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Second")]
        public int? ChangeSecond { get; set; }

        [RegularExpression(@"^([0-9]?[0-9]?[0-9])$")]
        [Display(Name = "Mili Second")]
        public int? ChangeMiliSecond { get; set; }

        [RegularExpression(@"[[0-9]+]*")]
        [Display(Name = "Day")]
        public int? IDay { get; set; }

        [RegularExpression(@"^(2[0-4]|[0-1]?[0-9])$")]
        [Display(Name = "Hour")]
        public int? IHour { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Minute")]
        public int? IMinute { get; set; }

        [RegularExpression(@"^[1-5]?[0-9]$")]
        [Display(Name = "Second")]
        public int? ISecond { get; set; }

        [RegularExpression(@"^([0-9]?[0-9]?[0-9])$")]
        [Display(Name = "Mili Second")]
        public int? IMiliSecond { get; set; }

        [Display(Name = "AM/PM")]
        [CanBeNull]
        public string IAmPm { get; set; }

        [Display(Name = "Month")]
        [CanBeNull] public string IMonth { get; set; }

        [Display(Name = "Year")]
        public int? IYear { get; set; }

        [Display(Name = "From Month")]
        [CanBeNull] public string FromMonth { get; set; }

        [Display(Name = "From Year")]
        public int? FromYear { get; set; }

        [Display(Name = "To Month")]
        [CanBeNull] public string ToMonth { get; set; }

        [Display(Name = "To Year")]
        public int? ToYear { get; set; }

        public DateTime IDate => (!string.IsNullOrEmpty(IMonth)) ?
            DateTime.ParseExact(IMonth + ", " + IYear, "MMMM, yyyy", new CultureInfo("EN"))
                .Add((new TimeSpan(IDay.GetValueOrDefault(0),
                        IAmPm == "PM" && !(IHour.GetValueOrDefault(0) > 11) ?
                            (IHour.GetValueOrDefault(0) + 12) : (IHour.GetValueOrDefault(0)), IMinute.GetValueOrDefault(0),
                        ISecond.GetValueOrDefault(0), IMiliSecond.GetValueOrDefault(0))))
            : DateTime.Now.MaxTimeOfDay();

        public TimeSpan IFromTime => new TimeSpan(FromDay.GetValueOrDefault(0),
                (FromAmPm == "PM" && !(FromHour.GetValueOrDefault(0) > 11) ? (FromHour.GetValueOrDefault(0) + 12) : FromHour.GetValueOrDefault(0)),
                    FromMinute.GetValueOrDefault(0),
                    FromSecond.GetValueOrDefault(0),
                    FromMiliSecond.GetValueOrDefault(0));

        public TimeSpan IToTime => new TimeSpan(ToDay.GetValueOrDefault(0),
            (ToAmPm == "PM" && !(ToHour.GetValueOrDefault(0) > 11) ? (ToHour.GetValueOrDefault(0) + 12) : ToHour.GetValueOrDefault(0)),
            ToMinute.GetValueOrDefault(0),
            ToSecond.GetValueOrDefault(0),
            ToMiliSecond.GetValueOrDefault(0)) < IFromTime ?
            new TimeSpan((ToDay.GetValueOrDefault(0) + 1),
                (ToAmPm == "PM" && !(ToHour.GetValueOrDefault(0) > 11) ? (ToHour.GetValueOrDefault(0) + 12) : ToHour.GetValueOrDefault(0)),
                ToMinute.GetValueOrDefault(0),
                ToSecond.GetValueOrDefault(0),
                ToMiliSecond.GetValueOrDefault(0)) :
            new TimeSpan(ToDay.GetValueOrDefault(0),
                (ToAmPm == "PM" && !(ToHour.GetValueOrDefault(0) > 11) ? (ToHour.GetValueOrDefault(0) + 12) : ToHour.GetValueOrDefault(0)),
                ToMinute.GetValueOrDefault(0),
                ToSecond.GetValueOrDefault(0),
                ToMiliSecond.GetValueOrDefault(0));

        public TimeSpan IChangeTime => IToTime - IFromTime;

        public DateTime IFromDate => IDate.FirstDateOfMonth().MinTimeOfDay();
        public DateTime IToDate => IDate.LastDateOfMonth().MaxTimeOfDay();

        public DateTime FromDate => (!string.IsNullOrEmpty(FromMonth) && FromYear != null) ?
            (DateTime.ParseExact(FromMonth + ", " + FromYear, "MMMM, yyyy", new CultureInfo("EN"))) : DateTime.Now.MinTimeOfDay();

        public DateTime ToDate => (!string.IsNullOrEmpty(ToMonth) && ToYear != null) ? DateTime.ParseExact(ToMonth + ", " + ToYear, "MMMM, yyyy", new CultureInfo("EN")) : DateTime.Now.MaxTimeOfDay();
    }

    public static class FL_DateSelectorDefinations
    {
        public static DateSelector FL_DateSelectorInit()
        {
            return new DateSelector
            {
                IMonth = DateTime.Now.SubstractMonth(1).ToString("MMMM"),
                IYear = DateTime.Now.SubstractMonth(1).Year
            };
        }

        public static DateSelector FL_DateSelectorCurrentInit()
        {
            return new DateSelector
            {
                IMonth = DateTime.Now.ToString("MMMM"),
                IYear = DateTime.Now.Year
            };
        }
    }
}