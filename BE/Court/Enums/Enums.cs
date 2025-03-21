﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public enum CourtTypeLoadFiles
    {
        OpenNewCourt = 1,
        EditGp = 2,
        EditPersData = 3,
        EditSpAndIp = 4,
        EditOwner = 5,
        UpdateNote = 6,
        InstallmentPlan = 7,
        Bankruptcy = 8,
        WriteOff = 9,
        OpenLitigationWork = 10,
        LitigationWork = 11,
        EnteringDecision = 12,
        PdFromIp = 13,
    }
    public enum CourtTypeReport
    {
        [Description("Реестр ГП в бухгалтерию")]
        ReestyGPAccountingDepartment = 1,
    }
    public enum CourtTypeReport2
    {
        [Description("Выгрузка ИП")]
        LoadIP = 1,
        [Description("Выгрузка направления на исполнения СП")]
        ExecutorSP = 2,
        [Description("Задание для ГПХ")]
        TaskGPH = 3,
        [Description("Текущая ДЗ")]
        NowDZ = 4,
        [Description("Отчет по списанию")]
        WriteOff = 5,
    }
}
