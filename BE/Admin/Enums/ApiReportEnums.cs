using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Admin.Enums
{
    public enum ApiReportEnums
    {
        [Description("6315376946_40702810748000001123_7_001_.zip")]
        GetSberbankInvoicesOldFormat = 0,
        [Description("6315376946_40702810748000001123_7_001_.zip")]
        GetSberbankInvoices = 1,
        [Description("COUNTERS_6315376946_40702810748000001123_7_001_.zip")]
        GetSberbankCounters = 2,
        [Description("Отчёт по перерасчётам.xlsx")]
        GetRecalculation = 3,
        [Description("Отчёт НСС РКС Пенза.zip")]
        GetNss = 4 ,
        [Description("Отчёт субагента.xlsx")]
        GetSubagent = 5,
        [Description("Свёрнутая сальдовая ведомость.xlsx")]
        GetShortSaldo = 6,
        [Description("Оборотно-сальдовая ведомость.xlsx")]
        GetFullSaldo = 7,
        [Description("Отчёт по начислениям.xlsx")]
        GetInvoices = 8,
        [Description("Данные потребителей.xlsx")]
        GetConsumerData = 9,
        [Description("Отчёт об ошибках НСС.xlsx")]
        GetNssErrors = 10,
    }
}
