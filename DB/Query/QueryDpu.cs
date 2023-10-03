using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Query
{
    public class QueryDpu
    {
#if DEBUG
        public static string SqlDPUHelpCalcuLationInstallationView = @"SELECT * FROM [WEB_APP].[dbo].[DPUHelpCalcuLationInstallationView]";
        public static string SqlDPUHelpCalcuLationInstallationViewPeriodExhibid = @"  SELECT PeriodExhibid, NewFullLic, Period FROM [WEB_APP].[dbo].[DPUHelpCalcuLationInstallationView]";
        public static string SqlDPUSummaryHousesView = @"SELECT * FROM [WEB_APP].[dbo].[DPUSummaryHousesView]";
#else 
        public static string SqlDPUHelpCalcuLationInstallationView = @"SELECT * FROM [WEB_APP].[dbo].[DPUHelpCalcuLationInstallationView]";
        public static string SqlDPUHelpCalcuLationInstallationViewPeriodExhibid = @"  SELECT PeriodExhibid, NewFullLic, Period FROM [WEB_APP].[dbo].[DPUHelpCalcuLationInstallationView]";
        public static string SqlDPUSummaryHousesView = @"SELECT * FROM [WEB_APP].[dbo].[DPUSummaryHousesView]";
#endif
    }
}
