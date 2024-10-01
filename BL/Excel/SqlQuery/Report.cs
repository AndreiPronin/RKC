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
        public const string IPUpload = @"SELECT * FROM [WEB_APP].[dbo].IPUpload()";
        public const string UploadExecutionSP = @"SELECT * FROM [WEB_APP].[dbo].UploadExecutionSP()";
        public const string TaskForGPH = @"SELECT * FROM [WEB_APP].[dbo].TaskForGPH()";
        public const string CurrentDebt = @"SELECT * FROM [WEB_APP].[dbo].CurrentDebt()";
    }
}
