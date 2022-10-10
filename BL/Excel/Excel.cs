using AppCache;
using BE.Counter;
using BL.Counters;
using BL.Helper;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model;
using DB.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public class Excel
    {
        public DataTable CreateExcelCounters()
        {

            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Тип ПУ"),
                                        new DataColumn("Номер"),
                                        new DataColumn("Дата поверки"),  new DataColumn("Дата следующей поверки") });

            var DB = new DbTPlus();
            List<IPU_COUNTERS> cOUNTERs = DB.IPU_COUNTERS.Where(x => x.CLOSE_ != true).ToList();
            foreach (var Items in cOUNTERs)
            {
                dt.Rows.Add(Items.FULL_LIC,Items.TYPE_PU,Items.FACTORY_NUMBER_PU,Items.DATE_CHECK,Items.DATE_CHECK_NEXT);
            }
            return dt;
        }
        public DataTable CreateExcelLic(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User, "0");
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[32] { new DataColumn("Улица         "),
                                        new DataColumn("Дом "),
                                        new DataColumn("КАДР   "),
                                        new DataColumn("Квартира         "),
                                        new DataColumn("Кор. лицевой счет"),new DataColumn("Лицевой счет"),new DataColumn("ZAK"),new DataColumn("         ФИО         ")
                                        , new DataColumn("         ТИП расчет ГВС1         "), new DataColumn(" Начальные показания ГВС1"), new DataColumn(" Конечные показания ГВС1")
                                        , new DataColumn("         ТИП расчет ГВС2         "), new DataColumn(" Начальные показания ГВС2"), new DataColumn(" Конечные показания ГВС2")
                                        , new DataColumn("         ТИП расчет ГВС3         "), new DataColumn(" Начальные показания ГВС3"), new DataColumn(" Конечные показания ГВС3")
                                        , new DataColumn("         ТИП расчет ГВС4         "), new DataColumn(" Начальные показания ГВС4"), new DataColumn(" Конечные показания ГВС4")
                                        , new DataColumn("         ТИП расчет ОТП1         "), new DataColumn(" Начальные показания ОТП1"), new DataColumn(" Конечные показания ОТП1")
                                        , new DataColumn("         ТИП расчет ОТП2         "), new DataColumn(" Начальные показания ОТП2"), new DataColumn(" Конечные показания ОТП2")
                                        , new DataColumn("         ТИП расчет ОТП3         "), new DataColumn(" Начальные показания ОТП3"), new DataColumn(" Конечные показания ОТП3")
                                        , new DataColumn("         ТИП расчет ОТП4         "), new DataColumn(" Начальные показания ОТП4"), new DataColumn(" Конечные показания ОТП4")}) ;
            cacheApp.UpdateProgress(User, "Получаю данные из бд");
            var DbLIC = new DbLIC();
            List<ALL_LICS> cOUNTERs = DbLIC.ALL_LICS.ToList();
            cacheApp.UpdateProgress(User, $@"Получил {cOUNTERs.Count()} записей");
            foreach (var Items in cOUNTERs)
            {
                dt.Rows.Add(Items.UL, Items.DOM, Items.CADR, Items.KW, Items.LIC, Items.F4ENUMELS, Items.ZAK, Items.FIO,
                    Items.FKUBSXVS,Items.FKUB1XVS, Items.FKUB2XVS, Items.FKUBSXV_2,Items.FKUB1XV_2, Items.FKUB2XV_2,
                    Items.FKUBSXV_3, Items.FKUB1XV_3, Items.FKUB2XV_3, Items.FKUBSXV_4, Items.FKUB1XV_4, Items.FKUB2XV_4,
                    Items.FKUBSOT_1, Items.FKUB1OT_1, Items.FKUB2OT_1, Items.FKUBSOT_2, Items.FKUB1OT_2, Items.FKUB2OT_2,
                    Items.FKUBSOT_3, Items.FKUB1OT_3, Items.FKUB2OT_3, Items.FKUBSOT_4, Items.FKUB1OT_4, Items.FKUB2OT_4);
            }
            cacheApp.UpdateProgress(User, "Ожидайте... Идет скачивание файла.");
            return dt;
        }
        public DataTable CreateExcelGeneral()
        {
            var DbLIC = new DbLIC();
            var DbTPlus = new DbTPlus();
            var Result = DbLIC.ALL_LICS.Join(DbTPlus.IPU_COUNTERS,
                a => a.F4ENUMELS,
                p => p.FULL_LIC,
                (a, p) => new { a.UL,a.DOM,a.CADR,a.KW,a.LIC,p.FULL_LIC,p.TYPE_PU,p.FACTORY_NUMBER_PU,p.BRAND_PU,p.MODEL_PU,p.GIS_ID_PU }
                ).ToList();
            DataTable dt = new DataTable("Counter");
            //dt.Columns.AddRange(new DataColumn[32] { new DataColumn("Улица         "),
            //                            new DataColumn("Дом "),
            //                            new DataColumn("КАДР   "),
            //                            new DataColumn("Квартира         "),
            //                            new DataColumn("Кор. лицевой счет"),new DataColumn("Лицевой счет"),new DataColumn("ZAK"),new DataColumn("         ФИО         ")
            //                            , new DataColumn("         ТИП расчет ГВС1         "), new DataColumn(" Начальные показания ГВС1"), new DataColumn(" Конечные показания ГВС1")
            //                            , new DataColumn("         ТИП расчет ГВС2         "), new DataColumn(" Начальные показания ГВС2"), new DataColumn(" Конечные показания ГВС2")
            //                            , new DataColumn("         ТИП расчет ГВС3         "), new DataColumn(" Начальные показания ГВС3"), new DataColumn(" Конечные показания ГВС3")
            //                            , new DataColumn("         ТИП расчет ГВС4         "), new DataColumn(" Начальные показания ГВС4"), new DataColumn(" Конечные показания ГВС4")
            //                            , new DataColumn("         ТИП расчет ОТП1         "), new DataColumn(" Начальные показания ОТП1"), new DataColumn(" Конечные показания ОТП1")
            //                            , new DataColumn("         ТИП расчет ОТП2         "), new DataColumn(" Начальные показания ОТП2"), new DataColumn(" Конечные показания ОТП2")
            //                            , new DataColumn("         ТИП расчет ОТП3         "), new DataColumn(" Начальные показания ОТП3"), new DataColumn(" Конечные показания ОТП3")
            //                            , new DataColumn("         ТИП расчет ОТП4         "), new DataColumn(" Начальные показания ОТП4"), new DataColumn(" Конечные показания ОТП4")});
            //foreach (var Items in Result)
            //{
            //    //var FKUBSXVS = Items.FKUBSXVS == 0 ? "нет ИПУ" : Items.FKUBSXVS == 1 ? "расчет по ИПУ" : Items.FKUBSXVS == 2 ? "расчет по среднему" : Items.FKUBSXVS == 3 ? "расчет по нормативу" : "";
            //    //var FKUBSXV_2 = Items.FKUBSXV_2 == 0 ? "нет ИПУ" : Items.FKUBSXV_2 == 1 ? "расчет по ИПУ" : "";
            //    //var FKUBSXV_3 = Items.FKUBSXV_3 == 0 ? "нет ИПУ" : Items.FKUBSXV_3 == 1 ? "расчет по ИПУ" : Items.FKUBSXV_3 == 2 ? "расчет по средему" : Items.FKUBSXV_3 == 3 ? "расчет по нормативу" : "";
            //    //var FKUBSXV_4 = Items.FKUBSXV_4 == 0 ? "нет ИПУ" : Items.FKUBSXV_4 == 1 ? "расчет по ИПУ" : "";
            //    //var FKUBSOT_1 = Items.FKUBSOT_1 == 0 ? "нет ИПУ" : Items.FKUBSOT_1 == 1 ? "расчет по ИПУ" : Items.FKUBSOT_1 == 2 ? "расчет по средему" : Items.FKUBSOT_1 == 3 ? "расчет по нормативу" : "";
            //    //var FKUBSOT_2 = Items.FKUBSOT_2 == 0 ? "нет ИПУ" : Items.FKUBSOT_2 == 1 ? "расчет по ИПУ" : "";
            //    //var FKUBSOT_3 = Items.FKUBSOT_3 == 0 ? "нет ИПУ" : Items.FKUBSOT_3 == 1 ? "расчет по ИПУ" : "";
            //    //var FKUBSOT_4 = Items.FKUBSOT_4 == 0 ? "нет ИПУ" : Items.FKUBSOT_4 == 1 ? "расчет по ИПУ" : "";
            //    //dt.Rows.Add(Items.UL, Items.DOM, Items.CADR, Items.KW, Items.LIC, Items.F4ENUMELS, Items.ZAK, Items.FIO,
            //    //    Items.FKUBSXVS, Items.FKUB1XVS, Items.FKUB2XVS, Items.FKUBSXV_2, Items.FKUB1XV_2, Items.FKUB2XV_2,
            //    //    Items.FKUBSXV_3, Items.FKUB1XV_3, Items.FKUB2XV_3, Items.FKUBSXV_4, Items.FKUB1XV_4, Items.FKUB2XV_4,
            //    //    Items.FKUBSOT_1, Items.FKUB1OT_1, Items.FKUB2OT_1, Items.FKUBSOT_2, Items.FKUB1OT_2, Items.FKUB2OT_2,
            //    //    Items.FKUBSOT_3, Items.FKUB1OT_3, Items.FKUB2OT_3, Items.FKUBSOT_4, Items.FKUB1OT_4, Items.FKUB2OT_4);
            //}
            return dt;
        }
        public DataTable LoadExcelPU(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User, "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            Counter counter = new Counter(new Logger(),new GeneratorDescriptons());
            List<SaveModelIPU> COUNTERsNotAdded = new List<SaveModelIPU>();
            var DbTPlus = new DbTPlus();
            var DbLIC = new DbLIC();
            var dbApp = new ApplicationDbContext();
            int i = 0;
            bool Error = false;
            var Count = nonEmptyDataRows.Count();
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    i++;
                    try
                    {
                         
                        Error = false;
                        var Procent = Math.Round((float)i / Count * 100, 0);
                        cacheApp.UpdateProgress(User, Procent.ToString());
                        SaveModelIPU saveModel = new SaveModelIPU();
                        var integrationReadings = new IntegrationReadings();
                        saveModel.FULL_LIC = dataRow.Cell(2).Value == "" ? "" : Convert.ToString(dataRow.Cell(2).Value).Replace(" ", "");
                        saveModel.TypePU = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Replace(" ", "");
                        saveModel.NumberPU = dataRow.Cell(5).Value == "" ? "" : Convert.ToString(dataRow.Cell(5).Value).Replace(" ", "");
                        saveModel.SEALNUMBER = dataRow.Cell(9).Value == "" ? "" : Convert.ToString(dataRow.Cell(9).Value).Replace(" ", "");
                        var Readings = DbLIC.ALL_LICS.FirstOrDefault(x => x.F4ENUMELS == saveModel.FULL_LIC);
                        var IPU_COUNTERS = DbTPlus.IPU_COUNTERS.Where(x => x.FULL_LIC == saveModel.FULL_LIC && x.TYPE_PU.Contains(saveModel.TypePU) && (x.CLOSE_ == null || x.CLOSE_ == false)).ToList();
                        if (dataRow.Cell(6).Value != "") { saveModel.DATE_CHECK = Convert.ToDateTime(dataRow.Cell(6).Value); }
                        if (dataRow.Cell(7).Value != "") { saveModel.DATE_CHECK_NEXT = Convert.ToDateTime(dataRow.Cell(7).Value); }
                        saveModel.MODEL_PU = dataRow.Cell(10).Value == "" ? "" : Convert.ToString(dataRow.Cell(10).Value).Replace(" ", "");
                        if (saveModel.TypePU == "ГВС1" && dataRow.Cell(8).Value != "") saveModel.FKUB2XVS = Convert.ToDecimal(dataRow.Cell(8).Value);
                        if (saveModel.TypePU == "ГВС2" && dataRow.Cell(8).Value != "") saveModel.FKUB2XV_2 = Convert.ToDecimal(dataRow.Cell(8).Value);
                        if (saveModel.TypePU == "ГВС3" && dataRow.Cell(8).Value != "") saveModel.FKUB2XV_3 = Convert.ToDecimal(dataRow.Cell(8).Value);
                        if (saveModel.TypePU == "ГВС4" && dataRow.Cell(8).Value != "") saveModel.FKUB2XV_4 = Convert.ToDecimal(dataRow.Cell(8).Value);
                        if (saveModel.TypePU == "ОТП1" && dataRow.Cell(8).Value != "") saveModel.FKUB2OT_1 = Convert.ToDecimal(dataRow.Cell(8).Value);
                        if (saveModel.TypePU == "ОТП2" && dataRow.Cell(8).Value != "") saveModel.FKUB2OT_2 = Convert.ToDecimal(dataRow.Cell(8).Value);
                        if (saveModel.TypePU == "ОТП3" && dataRow.Cell(8).Value != "") saveModel.FKUB2OT_3 = Convert.ToDecimal(dataRow.Cell(8).Value);
                        if (saveModel.TypePU == "ОТП4" && dataRow.Cell(8).Value != "") saveModel.FKUB2OT_4 = Convert.ToDecimal(dataRow.Cell(8).Value);
                        var Month = DateTime.Now.Date.Month;
                        var Year = DateTime.Now.Date.Year;
                        var Integr = dbApp.IntegrationReadings.Where(x => x.Lic == saveModel.FULL_LIC && x.TypePu.Contains(saveModel.TypePU)
                        && x.DateTime.Value.Month == Month && x.DateTime.Value.Year == Year).ToList();
                        if (Integr.Count() == 0)
                        {
                            integrationReadings.Lic = saveModel.FULL_LIC;
                            integrationReadings.TypePu = saveModel.TypePU;
                            integrationReadings.DateTime = Convert.ToDateTime(dataRow.Cell(11).Value);
                            if (saveModel.TypePU == TypePU.GVS1.GetDescription())
                            {
                                if (Readings.FKUB2XVS - Convert.ToDecimal(saveModel.FKUB2XVS) > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (Convert.ToDecimal(saveModel.FKUB2XVS) - Readings.FKUB2XVS < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2XVS = saveModel.FKUB2XVS;
                                }

                            }
                            if (saveModel.TypePU == TypePU.GVS2.GetDescription())
                            {
                                if (Readings.FKUB2XV_2 - saveModel.FKUB2XV_2 > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (saveModel.FKUB2XV_2 - Readings.FKUB2XV_2 < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2XV_2 = saveModel.FKUB2XV_2;
                                }
                            }
                            if (saveModel.TypePU == TypePU.GVS3.GetDescription())
                            {
                                if (Readings.FKUB2XV_3 - saveModel.FKUB2XV_3 > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (saveModel.FKUB2XV_3 - Readings.FKUB2XV_3 < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2XV_3 = saveModel.FKUB2XV_3;
                                }
                            }
                            if (saveModel.TypePU == TypePU.GVS4.GetDescription())
                            {
                                if (Readings.FKUB2XV_4 - saveModel.FKUB2XV_4 > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (saveModel.FKUB2XV_4 - Readings.FKUB2XV_4 < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2XV_4 = saveModel.FKUB2XV_4;
                                }
                            }
                            if (saveModel.TypePU == TypePU.ITP1.GetDescription())
                            {
                                if (Readings.FKUB2OT_1 - saveModel.FKUB2OT_1 > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (saveModel.FKUB2OT_1 - Readings.FKUB2OT_1 < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2OT_1 = saveModel.FKUB2OT_1;
                                }
                            }
                            if (saveModel.TypePU == TypePU.ITP2.GetDescription())
                            {
                                if (Readings.FKUB2OT_2 - saveModel.FKUB2OT_2 > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (saveModel.FKUB2OT_2 - Readings.FKUB2OT_2 < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2OT_2 = saveModel.FKUB2OT_2;
                                }
                            }
                            if (saveModel.TypePU == TypePU.ITP3.GetDescription())
                            {
                                if (Readings.FKUB2OT_3 - saveModel.FKUB2OT_3 > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (saveModel.FKUB2OT_3 - Readings.FKUB2OT_3 < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2OT_3 = saveModel.FKUB2OT_3;
                                }
                            }
                            if (saveModel.TypePU == TypePU.ITP4.GetDescription())
                            {
                                if (Readings.FKUB2OT_4 - saveModel.FKUB2OT_4 > 30)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.High.GetDescription()} ";
                                }
                                else if (saveModel.FKUB2OT_4 - Readings.FKUB2OT_4 < 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.Low.GetDescription()} ";
                                }
                                else if (IPU_COUNTERS.Count() == 0)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.NoPU.GetDescription()} {saveModel.TypePU} ";
                                }
                                else if (IPU_COUNTERS.Count() > 1)
                                {
                                    Error = true;
                                    integrationReadings.Description += $@"{ErrorIntegration.ManyPU.GetDescription()} ";
                                }
                                else
                                {
                                    saveModel.FKUB2OT_4 = saveModel.FKUB2OT_4;
                                }
                            }
                            if (Error == false)
                            {
                                counter.UpdatePUIntegrations(saveModel, User, IPU_COUNTERS.FirstOrDefault().ID_PU);
                                dbApp.IntegrationReadings.Add(integrationReadings);
                                dbApp.SaveChanges();
                            }
                            else
                            {
                                saveModel.DESCRIPTION = integrationReadings.Description;
                                COUNTERsNotAdded.Add(saveModel);
                                integrationReadings.IsError = true;
                                dbApp.IntegrationReadings.Add(integrationReadings);
                            }
                        }
                        else
                        {
                            saveModel.DESCRIPTION = $"Уже были внесены показания по этому ПУ {Integr.FirstOrDefault().DateTime}";
                            COUNTERsNotAdded.Add(saveModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        COUNTERsNotAdded.Add(new SaveModelIPU { FULL_LIC = $"Ошибка на {i} строке" });
                    }
                }
            }
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Тип ПУ"),
                                        new DataColumn("Номер"),
                                        new DataColumn("Примечание")});
            foreach (var Items in COUNTERsNotAdded)
            {
                dt.Rows.Add(Items.FULL_LIC, Items.TypePU, Items.NumberPU,Items.DESCRIPTION);
            }
            return dt;
        }
        public DataTable LoadExcelPUProperty(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User, "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            Counter counter = new Counter(new Logger(), new GeneratorDescriptons());
            List<SaveModelIPU> COUNTERsNotAdded = new List<SaveModelIPU>();
            var dbApp = new ApplicationDbContext();
            var Count = nonEmptyDataRows.Count();
            int i = 0;
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    i++;
                    try
                    {
                        var Procent = Math.Round((float)i / Count * 100, 0);
                        cacheApp.UpdateProgress(User, Procent.ToString());
                        SaveModelIPU saveModel = new SaveModelIPU();
                        var integrationReadings = new IntegrationReadings();
                        saveModel.FULL_LIC = dataRow.Cell(2).Value == "" ? "" : Convert.ToString(dataRow.Cell(2).Value).Replace(" ", "");
                        saveModel.TypePU = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Replace(" ", "");
                        saveModel.NumberPU = dataRow.Cell(5).Value == "" ? "" : Convert.ToString(dataRow.Cell(5).Value).Replace(" ", "");
                        saveModel.SEALNUMBER = dataRow.Cell(9).Value == "" ? "" : Convert.ToString(dataRow.Cell(9).Value).Replace(" ", "");
                        if (dataRow.Cell(6).Value != "") { saveModel.DATE_CHECK = Convert.ToDateTime(dataRow.Cell(6).Value); }
                        if (dataRow.Cell(7).Value != "") { saveModel.DATE_CHECK_NEXT = Convert.ToDateTime(dataRow.Cell(7).Value); }
                        saveModel.MODEL_PU = dataRow.Cell(10).Value == "" ? "" : Convert.ToString(dataRow.Cell(10).Value).Replace(" ", "");
                        if (!counter.UpdatePU(saveModel, User))
                        {
                            saveModel.DESCRIPTION = $"Нет такого ПУ {saveModel.TypePU}";
                            COUNTERsNotAdded.Add(saveModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        COUNTERsNotAdded.Add(new SaveModelIPU { FULL_LIC = $"Ошибка на {i} строке" });
                    }
                }
            }
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Тип ПУ"),
                                        new DataColumn("Номер"),
                                        new DataColumn("Примечание")});
            foreach (var Items in COUNTERsNotAdded)
            {
                dt.Rows.Add(Items.FULL_LIC, Items.TypePU, Items.NumberPU, Items.DESCRIPTION);
            }
            return dt;
        }
        public DataTable ErroIntegratin()
        {

            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[7] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Тип ПУ"),
                                        new DataColumn("  Дата  "),
                                        new DataColumn("     Описание ошибки    "),  new DataColumn("Начальные показания") 
            , new DataColumn("Конечные показания") ,new DataColumn("Текущие показания") });

            var DB = new ApplicationDbContext();
            var IntegrationReadings = DB.IntegrationReadings.ToList();
            foreach (var Items in IntegrationReadings)
            {
                dt.Rows.Add(Items.Lic, Items.TypePu, Items.DateTime, Items.Description, 
                    Items.InitialReadings, Items.EndReadings, Items.NowReadings);
            }
            return dt;
        }
        public DataTable ReestrIPU(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User, "Получаю данные из бд");
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[17] { new DataColumn("КОД ДОМА"),
                                        new DataColumn("   УЛИЦА   "),
                                        new DataColumn("  ДОМ  "),
                                        new DataColumn("     КВАРТИРА    "),  new DataColumn("   ЛИЦЕВОЙ СЧЕТ   ")
            ,new DataColumn("   ФИО   ") ,new DataColumn("   ПРИБОР УЧЕТА   "),new DataColumn("   ЗАВОДСКОЙ НОМЕР ИПУ   "),new DataColumn("   ДАТА ПОВЕРКИ ИПУ   ")
            ,new DataColumn("   ДАТА СЛЕДУЮЩЕЙ ПОВЕРКИ ИПУ   "),new DataColumn("   ПЛОМБА   "),new DataColumn("   ТИП ПЛОМБА   ")
            ,new DataColumn("   ПЛОМБА 2   "),new DataColumn("   ТИП ПЛОМБА 2   "),new DataColumn("   ПРИЗНАК ИПУ 1   ")
            ,new DataColumn("   КОНЕЧНЫЕ ПОКАЗАНИЯ ИПУ 1   "),new DataColumn("   ТЕКУЩИЕ ПОКАЗАНИЯ ИПУ 1   ")});
            var DB = new ApplicationDbContext();
            var Counters = DB.Database.SqlQuery<vw_CounterTPlus>("select * from dbo.vw_CounterTPlus").ToList();
            cacheApp.Update(User, "Формирую Excel");
            foreach (var Items in Counters)
            {
                dt.Rows.Add(Items.CodeHouse, Items.Streer, Items.Home, Items.Flat,
                    Items.Lic, Items.Fio, Items.Pu, Items.PuNumber, Items.DataCheck, Items.DataNextCheck, 
                    Items.Seal, Items.TypeSeal, Items.Seal2, Items.TypeSeal2, Items.SignPu, Items.EndReadings,Items.NowReadings);
            }
            cacheApp.Update(User, "Скачиваю Excel");
            
            return dt;
        }
    }
}
