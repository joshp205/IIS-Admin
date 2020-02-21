using System;
using System.Text;

namespace IISAdministration.Helpers {

    /// <summary>
    /// Small helper class for parsing date information into different formats. This is primarly meant for the metrics database.
    /// </summary>
    public static class DateHelper {

        /// <summary>
        /// Parses a DateTime object into an integer that represents the date only.
        /// </summary>
        /// <param name="date">The date to parse.</param>
        /// <returns>Integer representation of the date.</returns>
        public static int DateToInt(DateTime date) {
            StringBuilder sb = new StringBuilder();
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;
            
            sb.Append(year);

            if (month < 10) { sb.Append(0); }
            sb.Append(month);

            if (day < 10) { sb.Append(0); }
            sb.Append(day);

            return int.Parse(sb.ToString());
        }

        /// <summary>
        /// Parses a DateTime object into an integer that represents the time only.
        /// </summary>
        /// <param name="date">The date to parse.</param>
        /// <returns>Integer representation of the time.</returns>
        public static int TimeToInt(DateTime date) {
            StringBuilder sb = new StringBuilder();
            int hour = date.Hour;
            int minute = date.Minute;
            int second = date.Second;

            if (hour < 10) { sb.Append(0); }
            sb.Append(hour);

            if (minute < 10) { sb.Append(0); }
            sb.Append(minute);

            if (second < 10) { sb.Append(0); }
            sb.Append(second);

            return int.Parse(sb.ToString());
        }
    }
}
