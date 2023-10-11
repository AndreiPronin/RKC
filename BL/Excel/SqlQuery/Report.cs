using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel.SqlQuery
{
    public class Report
    {
        public const string OTPReport = @"SELECT * FROM [T+].[dbo].[OTP_report]";
        public const string GVSReport = @"SELECT * FROM [T+].[dbo].[GVS_report]";
    }
}
