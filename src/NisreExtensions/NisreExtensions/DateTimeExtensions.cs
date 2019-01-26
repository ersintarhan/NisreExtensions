using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace System
{
    public static class DateTimeExtensions
    {
	    /// <summary>
	    ///     Sets the dates to their defaults.
	    /// </summary>
	    /// <param name="defaultFromDate">Defaults to DateTime.MinValue if null</param>
	    /// <param name="defaultToDate">Defaults to DateTime.MaxValue if null</param>
	    public static void DefaultDatesIfNull(ref DateTime? fromDate, ref DateTime? toDate,
            DateTime? defaultFromDate = null, DateTime? defaultToDate = null)
        {
            try
            {
                if (!fromDate.HasValue)
                    fromDate = defaultFromDate.HasValue ? defaultFromDate : DateTime.MinValue;

                if (!toDate.HasValue)
                    toDate = defaultToDate.HasValue ? defaultToDate : DateTime.MaxValue;
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null) ex = ex.InnerException;
            }
        }


	    /// <summary>
	    ///     First day of given year
	    /// </summary>
	    /// <example>
	    ///     2017-02-21 13:12:32PM => 2017-01-01 13:12:32PM
	    /// </example>
	    public static DateTime FirstDayOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

	    /// <summary>
	    ///     Last day of given year
	    /// </summary>
	    /// <example>
	    ///     2017-02-21 13:12:32PM => 2017-12-31 13:12:32PM
	    /// </example>
	    public static DateTime LastDayOfYear(this DateTime date)
        {
            return new DateTime(date.Year + 1, 1, 1).AddDays(-1);
        }

	    /// <summary>
	    ///     First day of given month
	    /// </summary>
	    /// <example>
	    ///     2017-02-21 13:12:32PM => 2017-02-01 13:12:32PM
	    /// </example>
	    public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

	    /// <summary>
	    ///     Last day of given month.
	    ///     NOTE: Includes Logic for leap years
	    /// </summary>
	    /// <example>
	    ///     2017-02-21 13:12:32PM => 2017-02-28 13:12:32PM
	    /// </example>
	    public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

	    /// <summary>
	    ///     Start of the day
	    /// </summary>
	    /// <example>
	    ///     2017-02-21 13:12:32PM => 2017-02-21 12:00:00AM
	    /// </example>
	    public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

	    /// <summary>
	    ///     End of the day
	    /// </summary>
	    /// <example>
	    ///     2017-02-21 13:12:32PM => 2017-02-21 11:59:59PM
	    /// </example>
	    public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

	    /// <summary>
	    ///     Checks if the date matches the day of the month
	    /// </summary>
	    /// <param name="date">Date</param>
	    /// <param name="dayOfMonth">Day of the month as an <see cref="int" /></param>
	    public static bool IsDayOfMonth(this DateTime date, int dayOfMonth)
        {
            return date.Day == dayOfMonth;
        }

	    /// <summary>
	    ///     Checks if the date matches the day of the month
	    /// </summary>
	    /// <param name="date">Date</param>
	    /// <param name="dayOfMonth">Day of the month as an <see cref="int" /></param>
	    public static bool IsDayOfMonth(this DateTime date, params int[] dayOfMonth)
        {
            return ((IList) dayOfMonth).Contains(date.Day);
        }

	    /// <summary>
	    ///     Gets the amount of days between this date and the end Date
	    /// </summary>
	    /// <param name="date">Starting date</param>
	    /// <param name="endDate">End date</param>
	    public static int DaysBetween(this DateTime date, DateTime endDate)
        {
            var smallest = date < endDate ? date : endDate;
            var largest = date > endDate ? date : endDate;
            return (largest - smallest).Days;
        }

	    /// <summary>
	    ///     Gets the amount of Months between this date and the end Date
	    /// </summary>
	    /// <param name="date">Starting date</param>
	    /// <param name="endDate">End date</param>
	    public static int MonthsBetween(this DateTime date, DateTime endDate)
        {
            var smallest = date < endDate ? date : endDate;
            var largest = date > endDate ? date : endDate;
            return (largest.Year - smallest.Year) * 12 + largest.Month - smallest.Month;
        }

	    /// <summary>
	    ///     Gets the amount of years between this date and the end Date
	    /// </summary>
	    /// <param name="date">Starting date</param>
	    /// <param name="endDate">End date</param>
	    public static int YearsBetween(this DateTime date, DateTime endDate)
        {
            var smallest = date < endDate ? date : endDate;
            var largest = date > endDate ? date : endDate;
            return largest.Year - smallest.Year;
        }


	    /// <summary>
	    ///     Converts DateTime to UnixTimeStamp : Total seconds since 1st Jan 1970 12:00 AM
	    /// </summary>
	    /// <param name="dateTime"></param>
	    /// <returns>Total Seconds passed since 1st Jan 1970 12:00 AM</returns>
	    public static long ToUnixTimeStamp(this DateTime dateTime)
        {
            return (long) (dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

	    /// <summary>
	    ///     Adds Unix Time Stamp to dateTime
	    /// </summary>
	    /// <param name="dateTime"></param>
	    /// <param name="unixTimeStamp"></param>
	    /// <returns>DateTime</returns>
	    public static DateTime AddUnixTimeStamp(this DateTime dateTime, long unixTimeStamp)
        {
            return dateTime.AddSeconds(unixTimeStamp);
        }

	    /// <summary>
	    ///     Checks datetime's Year for Leap Year
	    /// </summary>
	    /// <param name="datetime"></param>
	    /// <returns>bool</returns>
	    public static bool IsLeapYear(this DateTime datetime)
        {
            if (datetime.Year % 4 != 0)
                return false;
            if (datetime.Year % 100 != 0)
                return true;
            if (datetime.Year % 400 != 0)
                return false;
            return true;
        }

	    /// <summary>
	    ///     Get all the Days of the week supplied in days parameter. If days empty then return all the days.
	    ///     Search is inclusive of fromDate and toDate
	    /// </summary>
	    /// <param name="fromDate">Start date</param>
	    /// <param name="toDate">To date</param>
	    /// <param name="days">DayOfWeek</param>
	    /// <returns>List of DateTime</returns>
	    public static List<DateTime> GetDaysOfWeek(this DateTime fromDate, DateTime toDate, params DayOfWeek[] days)
        {
            if (fromDate >= toDate) return new List<DateTime>();
            var result = new List<DateTime>();

            while (fromDate <= toDate)
            {
                if (days != null && days.Any())
                {
                    if (days.Contains(fromDate.DayOfWeek))
                        result.Add(fromDate);
                }
                else
                {
                    result.Add(fromDate);
                }

                fromDate = fromDate.AddDays(1);
            }

            return result;
        }

	    /// <summary>
	    ///     Returns only Monday to Friday from fromDate till toDate
	    /// </summary>
	    /// <param name="fromDate"></param>
	    /// <param name="toDate"></param>
	    /// <returns>List of DateTime</returns>
	    public static List<DateTime> GetWeekdays(this DateTime fromDate, DateTime toDate)
        {
            return fromDate.GetDaysOfWeek(toDate, DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
                DayOfWeek.Thursday, DayOfWeek.Friday);
        }

	    /// <summary>
	    ///     Returns only Saturday and Sunday from fromDate till toDate
	    /// </summary>
	    /// <param name="fromDate"></param>
	    /// <param name="toDate"></param>
	    /// <returns>List of DateTime</returns>
	    public static List<DateTime> GetWeekends(this DateTime fromDate, DateTime toDate)
        {
            return fromDate.GetDaysOfWeek(toDate, DayOfWeek.Saturday, DayOfWeek.Sunday);
        }

        public static int MinutesFrom(this DateTime dateTime, DateTime from)
        {
            var timeSpan = dateTime - from;
            return Convert.ToInt32(timeSpan.TotalMinutes);
        }

        public static int SecondsFrom(this DateTime dateTime, DateTime from)
        {
            var timeSpan = dateTime - from;
            return Convert.ToInt32(timeSpan.TotalSeconds);
        }

        public static DateTime GetUnixEpoch()
        {
            return new DateTime(1970, 1, 1);
        }

        public static int MinutesFromUnixEpoch(this DateTime dateTime)
        {
            return dateTime.MinutesFrom(GetUnixEpoch());
        }

        public static int SecondsFromUnixEpoch(this DateTime dateTime)
        {
            return dateTime.SecondsFrom(GetUnixEpoch());
        }

        public static double MillisecondOfDay(this DateTime dateTime)
        {
            return dateTime.TimeOfDay.TotalMilliseconds;
        }

        public static DateTime Midnight(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime Noon(this DateTime dateTime)
        {
            return dateTime.SetTime(12);
        }

        public static DateTime SetTime(this DateTime dateTime, int hour)
        {
            return SetTime(dateTime, hour, 0, 0, 0);
        }

        public static DateTime SetTime(this DateTime dateTime, int hour, int minute)
        {
            return SetTime(dateTime, hour, minute, 0, 0);
        }

        public static DateTime SetTime(this DateTime dateTime, int hour, int minute, int second)
        {
            return SetTime(dateTime, hour, minute, second, 0);
        }

        public static DateTime SetTime(this DateTime dateTime, int hour, int minute, int second, int millisecond)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second, millisecond);
        }


        /// <summary>
        ///     Returns a string representing of a <see cref="DateTime" /> relative value.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /></param>
        /// <returns>The string value of relative time.</returns>
        public static string ToRelativeTime(this DateTime dateTime)
        {
            var diff = DateTime.Now - dateTime;
            var suffix = string.Empty;
            var numeral = 0;

            if (diff.TotalDays >= 365)
            {
                numeral = (int) Math.Floor(diff.TotalDays / 365);
                suffix = numeral == 1 ? Resource.YearAgo : Resource.YearsAgo;
            }
            else if (diff.TotalDays >= 31)
            {
                numeral = (int) Math.Floor(diff.TotalDays / 31);
                suffix = numeral == 1 ? Resource.MonthAgo : Resource.MonthsAgo;
            }
            else if (diff.TotalDays >= 7)
            {
                numeral = (int) Math.Floor(diff.TotalDays / 7);
                suffix = numeral == 1 ? Resource.WeekAgo : Resource.WeeksAgo;
            }
            else if (diff.TotalDays >= 1)
            {
                numeral = (int) Math.Floor(diff.TotalDays);
                suffix = numeral == 1 ? Resource.DayAgo : Resource.DaysAgo;
            }
            else if (diff.TotalHours >= 1)
            {
                numeral = (int) Math.Floor(diff.TotalHours);
                suffix = numeral == 1 ? Resource.HourAgo : Resource.HoursAgo;
            }
            else if (diff.TotalMinutes >= 1)
            {
                numeral = (int) Math.Floor(diff.TotalMinutes);
                suffix = numeral == 1 ? Resource.MinuteAgo : Resource.MinutesAgo;
            }
            else if (diff.TotalSeconds >= 1)
            {
                numeral = (int) Math.Floor(diff.TotalSeconds);
                suffix = numeral == 1 ? Resource.SecondAgo : Resource.SecondsAgo;
            }
            else
            {
                suffix = Resource.JustNow;
            }

            var output = numeral == 0
                ? suffix
                : string.Format(CultureInfo.InvariantCulture, "{0} {1}", numeral, suffix);
            return output;
        }

        /// <summary>
        ///     Returns the <see cref="DateTime" /> with max time value of the specified date.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /></param>
        /// <returns>The datetime with max time of the <paramref name="dateTime" /></returns>
        public static DateTime ToDateWithMaxTime(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        ///     Returns the <see cref="DateTime" /> with min time value of the specified date.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /></param>
        /// <returns>The datetime with min time of the <paramref name="dateTime" /></returns>
        public static DateTime ToDateWithMinTime(this DateTime dateTime)
        {
            return dateTime.Date;
        }


        /// <summary>
        ///     Returns the ages of the specified date time until now.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /></param>
        /// <returns>The age.</returns>
        public static int Age(this DateTime dateTime)
        {
            if (dateTime > DateTime.Now) return 0;
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now < dateTime.AddYears(age)) age--;
            return age;
        }

        /// <summary>
        ///     Indicates whether the specified date time is a working day.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /></param>
        /// <returns>true if the value is Monday,Tuesday,Wednesday,Thursday or Friday;otherwise, false.</returns>
        public static bool IsWorkingDay(this DateTime dateTime)
        {
            return dateTime.DayOfWeek != DayOfWeek.Saturday && dateTime.DayOfWeek != DayOfWeek.Sunday;
        }

        /// <summary>
        ///     Indicates whether the specified date time is weekend.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime" /></param>
        /// <returns>true if the value is Saturday or Sunday;otherwise, false.</returns>
        public static bool IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}