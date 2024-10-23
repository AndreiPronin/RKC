using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    public enum Banks : int
    {
        VTB = 0,
        POCHTA = 1,
        ConnectionLost = 2,
        OutlierReading = 3
    }
    public enum TypePU 
    {
        [Description("ГВС1")]
        GVS1 = 1,
        [Description("ГВС2")]
        GVS2 = 2,
        [Description("ГВС3")]
        GVS3 = 3,
        [Description("ГВС4")]
        GVS4 = 4,
        [Description("ОТП1")]
        ITP1 = 5,
        [Description("ОТП2")]
        ITP2 = 6,
        [Description("ОТП3")]
        ITP3 = 7,
        [Description("ОТП4")]
        ITP4 = 8,

    }
    public enum TypeTemplateFile
    {
        [Description("Изменение информации ПУ")]
        LoadExcelPUProperty = 1,
        [Description("Загрузка персов")]
        LoadExcelPersData = 2,

    }
    public enum TypeFile
    {
        [Description("Counters")]
        Counters = 1,
        [Description("Lic")]
        Lic = 2,
        [Description("Log Pers")]
        LogPers = 3,
        [Description("Log Counter")]
        LogCounter = 4,
        [Description("EBD общий")]
        EbdAll = 5,
        [Description("T+IpuGvs")]
        TIpuGvs = 6,
        [Description("T+IpuOtp")]
        TIpuOtp = 7,
        [Description("EBD MKD")]
        EbdMkd = 8,
        [Description("EBD FLAT жп")]
        EbdFlatliving = 9,
        [Description("EBD FLAT нжп")]
        EbdFlatNotliving = 10,
        [Description("EBD FLAT")]
        DirectFlat = 11,
        [Description("EBD MKD")]
        DirectMkd = 12,
        [Description("Сводный отчёт ОТП")]
        SummaryReportOTP = 13,
        [Description("Сводный отчёт ГВС")]
        SummaryReportGVS = 14,
    }
    public enum ErrorIntegration
    {
        [Description("Большой объем. Объем больше ")]
        High = 1,
        [Description("Нет ИПУ на лицевом счёте")]
        NoPU = 2,
        [Description("На лицевом счёте больше 1 ПУ")]
        ManyPU = 3,
        [Description("Показания ИПУ меньше предыдущего")]
        Low = 4,
        [Description("Не найден лицевой счет")]
        NoLic = 5,
        [Description("Оплата есть а показаний нет")]
        NoReadings = 6,
        [Description("Прибор учета закрыт")]
        IpuClose = 7,
        [Description("Лицевой счет закрыт")]
        LicClose = 8,
        [Description("Закончился срок поверки")]
        DateCheckNext = 9,
        [Description("Нет показаний больше 3 месяцев")]
        NoIndications = 10,
    }
}
