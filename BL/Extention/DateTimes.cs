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
    }
}
