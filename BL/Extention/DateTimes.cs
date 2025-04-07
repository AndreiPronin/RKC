using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class DateTimes
    {
        public static DateTime GetDateWhitMaxDate(this DateTime dateTime)
        {
            var endDate = dateTime.AddMonths(1).AddDays(-dateTime.Date.Day);
            return new DateTime(endDate.Year,endDate.Month,endDate.Day,23,59,59);
        }
        public static string GetYear(int value)
        {
            var str = "";
            if ((value % 10) == 1)
                str = "год";
            if (((value % 10) >= 2) && (value % 10) <= 4)
                str = "года";
            if (((value % 10) == 0) || ((value % 10) >= 5) && ((value % 10) <= 9))
                str = "лет";

            return $"{value} {str}";
        }

        public static DateTime GetLastDayInMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month), 23, 59, 59);
        }

        public static DateTime GetFirstDayInMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }
    }
}
