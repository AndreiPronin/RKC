using AutoMapper;
using BE.Court;
using BL.Counters;
using BL.Helper;
using BL.MapperProfile;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model.Court;
using DB.Model.Court.DictiomaryModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public interface IExcelCourt
    {
        Task<DataTable> ExcelsLoadCourt(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsEditGpCourt(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsEditPersDataCourt(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsEditSpAndIpCourt(XLWorkbook Excels, string User);
    }
    public class ExcelCourt : IExcelCourt
    {
        private readonly ICourt _court;
        private readonly List<ReportCourtLoadExcel> reportCourtLoadExcels;
        private readonly Mapper _mapper = new CourtProfile().GetMapperBe();
        private readonly IDictionary _dictionary;
        public ExcelCourt(ICourt court, IDictionary dictionary) 
        {
            _court = court;
            _dictionary = dictionary;
            reportCourtLoadExcels = new List<ReportCourtLoadExcel>();
        }
        public async Task<DataTable> ExcelsLoadCourt(XLWorkbook Excels, string User)
        {
            var dictionaryCourt = await _dictionary.GetCourtDictionaries();
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            var Count = nonEmptyDataRows.Count();

            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        StringBuilder exceptions = new StringBuilder();
                        var CourtGeneral = new DB.Model.Court.CourtGeneralInformation();
                        CourtGeneral.CourtExecutionFSSP = new DB.Model.Court.CourtExecutionFSSP();
                        CourtGeneral.CourtExecutionInPF = new DB.Model.Court.CourtExecutionInPF();
                        CourtGeneral.CourtInstallmentPlan = new DB.Model.Court.CourtInstallmentPlan();
                        CourtGeneral.CourtBankruptcy = new DB.Model.Court.CourtBankruptcy();
                        CourtGeneral.CourtLitigationWork = new DB.Model.Court.CourtLitigationWork();
                        CourtGeneral.CourtStateDuty = new DB.Model.Court.CourtStateDuty();
                        CourtGeneral.CourtWriteOff = new DB.Model.Court.CourtWriteOff();
                        CourtGeneral.CourtWork = new DB.Model.Court.CourtWork();
                        CourtGeneral.Lic = dataRow.Cell(2).Value.TryGetLic();
                        CourtGeneral.Street = dataRow.Cell(3).Value.ToString();
                        CourtGeneral.Home = dataRow.Cell(4).Value.ToString();
                        CourtGeneral.Flat = dataRow.Cell(5).Value.ToString();
                        CourtGeneral.FirstName = $"{dataRow.Cell(7).Value.ToString()}".Trim();
                        CourtGeneral.LastName = $"{dataRow.Cell(6).Value.ToString()}".Trim();
                        CourtGeneral.Surname = $"{dataRow.Cell(8).Value.ToString()}".Trim();
                        CourtGeneral.CourtWork.FioSendCourt = dataRow.Cell(9).Value.ToString();
                        CourtGeneral.CourtWork.DateTask = Convert.ToDateTime(dataRow.Cell(10).Value.ToString());
                        CourtGeneral.CourtWork.PeriodDebtBegin = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());
                        CourtGeneral.CourtWork.PeriodDebtEnd = Convert.ToDateTime(dataRow.Cell(12).Value.ToString());
                        CourtGeneral.CourtWork.SumOdSendCourt = Convert.ToDouble(dataRow.Cell(13).Value.ToString());
                        CourtGeneral.CourtWork.SumPenySendCourt = Convert.ToDouble(dataRow.Cell(14).Value.ToString());

                        if (dictionaryCourt.FirstOrDefault(x => x.Id == 20).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.CourtWork.FioSendCourt) == null)
                            exceptions.Append("ФИО сотрудника не найдена в справочнике");
                        var ex = exceptions.ToString();
                        if (ex != "")
                        {
                            throw new Exception(ex);
                        }

                        var result = await _court.CreateCourtExcel(CourtGeneral, User);

                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Id = $"П-{result.Id}", Description = "Успешно создано дело" });

                    }
                    catch (Exception ex)
                    {
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            }
            return CreateResultCourtLoader(reportCourtLoadExcels);
        }
        public async Task<DataTable> ExcelsEditGpCourt(XLWorkbook Excels, string User)
        {
            var dictionaryCourt = await _dictionary.GetCourtDictionaries();
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            var Count = nonEmptyDataRows.Count();
            var mapper = new CourtProfile().GetMapperBe();
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        StringBuilder exceptions = new StringBuilder();
                        var CourtGeneral = await _court.DetailInfroms(dataRow.Cell(1).Value.TryGetCardNumber());
                        var CourtWorkRequisites = new BE.Court.CourtWorkRequisites
                            {
                                CourtGeneralInformId = CourtGeneral.Id,
                                Suma = dataRow.Cell(2).Value.ToString(),
                                Number = dataRow.Cell(3).Value.ToString(),
                                Date = Convert.ToDateTime(dataRow.Cell(4).Value)
                            };
                        if(dataRow.Cell(5).Value != "" && CourtGeneral.CourtStateDuty.DateSendOnReturnFNS != Convert.ToDateTime(dataRow.Cell(5).Value))
                            CourtGeneral.CourtStateDuty.DateSendOnReturnFNS = Convert.ToDateTime(dataRow.Cell(5).Value);
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtStateDuty.DateReturnFNS != Convert.ToDateTime(dataRow.Cell(6).Value))
                            CourtGeneral.CourtStateDuty.DateReturnFNS = Convert.ToDateTime(dataRow.Cell(6).Value);
                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtStateDuty.ReasonReturn != dataRow.Cell(7).Value.ToString())
                            CourtGeneral.CourtStateDuty.ReasonReturn = dataRow.Cell(7).Value.ToString();
                        if (dataRow.Cell(7).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 16).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.CourtStateDuty.ReasonReturn) == null)
                            exceptions.Append("Причина возврата заявления ФНС не найдена в справочнике");
                        var ex = exceptions.ToString();
                        if (ex != "")
                        {
                            throw new Exception(ex);
                        }
                      
                        var result = await _court.SaveCourt(mapper.Map<DB.Model.Court.CourtGeneralInformation, BE.Court.CourtGeneralInformation>(CourtGeneral), User);
                        await _court.AddCourtWorkRequisites(CourtWorkRequisites);
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Id = $"П-{result}", Description = "Успешно обновлено дело" });

                    }
                    catch (Exception ex)
                    {
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            }
            return CreateResultCourtLoader(reportCourtLoadExcels);
        }
        public async Task<DataTable> ExcelsEditPersDataCourt(XLWorkbook Excels, string User)
        {
            var dictionaryCourt = await _dictionary.GetCourtDictionaries();
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            var Count = nonEmptyDataRows.Count();
            var mapper = new CourtProfile().GetMapperBe();
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        StringBuilder exceptions = new StringBuilder();
                        var CourtGeneral = await _court.DetailInfroms(dataRow.Cell(1).Value.TryGetCardNumber());
                        if (dataRow.Cell(2).Value != "" && CourtGeneral.Floor != dataRow.Cell(2).Value.ToString())
                            CourtGeneral.Floor = dataRow.Cell(2).Value.ToString();
                        if (dataRow.Cell(2).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 13).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.Floor) == null)
                            exceptions.Append("Пол не найдена в справочнике" + Environment.NewLine);

                        if (dataRow.Cell(3).Value != "" && CourtGeneral.DateBirthday != dataRow.Cell(3).Value.ToString())
                            CourtGeneral.DateBirthday = dataRow.Cell(3).Value.ToString();
                        if (dataRow.Cell(4).Value != "" && CourtGeneral.PlaceBirth != dataRow.Cell(4).Value.ToString())
                            CourtGeneral.PlaceBirth = dataRow.Cell(4).Value.ToString();
                        if (dataRow.Cell(5).Value != "" && CourtGeneral.PasportSeria != dataRow.Cell(5).Value.ToString())
                            CourtGeneral.PasportSeria = dataRow.Cell(5).Value.ToString();
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.PasportDate != dataRow.Cell(6).Value.ToString())
                            CourtGeneral.PasportDate = dataRow.Cell(6).Value.ToString();
                        if (dataRow.Cell(7).Value != "" && CourtGeneral.PasportIssue != dataRow.Cell(7).Value.ToString())
                            CourtGeneral.PasportIssue = dataRow.Cell(7).Value.ToString();
                        if (dataRow.Cell(8).Value != "" && CourtGeneral.Inn != dataRow.Cell(8).Value.ToString())
                            CourtGeneral.PasportNumber = dataRow.Cell(8).Value.ToString();
                        if (dataRow.Cell(9).Value != "" && CourtGeneral.Inn != dataRow.Cell(9).Value.ToString())
                            CourtGeneral.PasportNumber = dataRow.Cell(9).Value.ToString();
                        if (dataRow.Cell(10).Value != "" && CourtGeneral.Snils != dataRow.Cell(10).Value.ToString())
                            CourtGeneral.Snils = dataRow.Cell(10).Value.ToString();
                        if (dataRow.Cell(11).Value != "" && CourtGeneral.ShareOfOwnership != dataRow.Cell(11).Value.ToString())
                            CourtGeneral.ShareOfOwnership = dataRow.Cell(11).Value.ToString();
                        if (dataRow.Cell(11).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 21).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.ShareOfOwnership) == null)
                            exceptions.Append("Вид собственности не найдена в справочнике" + Environment.NewLine);
                        if (dataRow.Cell(12).Value != "" && CourtGeneral.ShareInRight != dataRow.Cell(12).Value.ToString())
                            CourtGeneral.ShareInRight = dataRow.Cell(12).Value.ToString();
                        if (dataRow.Cell(12).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 22).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.ShareInRight) == null)
                            exceptions.Append("Доля в праве не найдена в справочнике" + Environment.NewLine);

                        if (dataRow.Cell(13).Value != "" && CourtGeneral.InSolidarityWith != dataRow.Cell(13).Value.ToString())
                            CourtGeneral.InSolidarityWith = dataRow.Cell(13).Value.ToString();
                        
                        if (dataRow.Cell(14).Value != "" && CourtGeneral.AddressRegister != dataRow.Cell(14).Value.ToString())
                            CourtGeneral.AddressRegister = dataRow.Cell(14).Value.ToString();

                        var ex = exceptions.ToString();
                        if (ex != "")
                        {
                            throw new Exception(ex);
                        }

                        var result = await _court.SaveCourt(mapper.Map<DB.Model.Court.CourtGeneralInformation, BE.Court.CourtGeneralInformation>(CourtGeneral), User);
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Id = $"П-{result}", Description = "Успешно обновлено дело" });

                    }
                    catch (Exception ex)
                    {
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            }
            return CreateResultCourtLoader(reportCourtLoadExcels);
        }
        public async Task<DataTable> ExcelsEditSpAndIpCourt(XLWorkbook Excels, string User)
        {
            var dictionaryCourt = await _dictionary.GetCourtDictionaries();
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            var Count = nonEmptyDataRows.Count();
            var mapper = new CourtProfile().GetMapperBe();
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        StringBuilder exceptions = new StringBuilder();
                        var CourtGeneral = await _court.DetailInfroms(dataRow.Cell(1).Value.TryGetCardNumber());

                        if (dataRow.Cell(2).Value != "")
                        {
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 1).CourtValueDictionaries.FirstOrDefault(x => x.Name.Split('|')[0]?.Trim() == dataRow.Cell(2).Value.ToString());
                            if (dataRow.Cell(2).Value != "" && CourtName?.Name?.Split('|')[0] == CourtGeneral.CourtWork.NameCourt)
                                exceptions.Append("Наименование суда не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtWork.NameCourt != dataRow.Cell(2).Value.ToString())
                                {
                                    CourtGeneral.CourtWork.NameCourt = CourtName.Name.Split('|')[0];
                                    CourtGeneral.CourtWork.AddressCourt = CourtName.Name.Split('|')[1];
                                }
                            }
                        }
                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtWork.DateReceptionCourt != Convert.ToDateTime(dataRow.Cell(3).Value))
                            CourtGeneral.CourtWork.DateReceptionCourt = Convert.ToDateTime(dataRow.Cell(3).Value);
                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtWork.NumberSP != dataRow.Cell(4).Value.ToString())
                            CourtGeneral.CourtWork.NumberSP = dataRow.Cell(4).Value.ToString();
                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtWork.DateSP != Convert.ToDateTime(dataRow.Cell(5).Value))
                            CourtGeneral.CourtWork.DateSP = Convert.ToDateTime(dataRow.Cell(5).Value);
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtWork.FioSendCourt != dataRow.Cell(6).Value.ToString())
                            CourtGeneral.CourtWork.FioSendCourt = dataRow.Cell(6).Value.ToString();
                        if (dataRow.Cell(6).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 20).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.CourtWork.FioSendCourt) == null)
                            exceptions.Append("ФИО сотрудника не найдена в справочнике" + Environment.NewLine);
                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtWork.DateTask != Convert.ToDateTime(dataRow.Cell(7).Value))
                            CourtGeneral.CourtWork.DateTask = Convert.ToDateTime(dataRow.Cell(7).Value);
                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtExecutionFSSP.DateSendingApplicationFSSP != Convert.ToDateTime(dataRow.Cell(8).Value))
                            CourtGeneral.CourtExecutionFSSP.DateSendingApplicationFSSP = Convert.ToDateTime(dataRow.Cell(8).Value);
                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtExecutionFSSP.NumberIP != dataRow.Cell(9).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.NumberIP = dataRow.Cell(9).Value.ToString();
                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtExecutionFSSP.IPInitiationDate != Convert.ToDateTime(dataRow.Cell(10).Value))
                            CourtGeneral.CourtExecutionFSSP.IPInitiationDate = Convert.ToDateTime(dataRow.Cell(10).Value);
                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtExecutionFSSP.IPEndDate != Convert.ToDateTime(dataRow.Cell(11).Value))
                            CourtGeneral.CourtExecutionFSSP.IPEndDate = Convert.ToDateTime(dataRow.Cell(11).Value);

                        //if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtExecutionFSSP.GroundsEndingIP != dataRow.Cell(12).Value.ToString())
                        //    CourtGeneral.CourtExecutionFSSP.GroundsEndingIP = dataRow.Cell(12).Value.ToString();
                        if (dataRow.Cell(12).Value != "")
                        {
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 6).CourtValueDictionaries.FirstOrDefault(x => x.Name.Split('|')[1]?.Trim() == dataRow.Cell(12).Value.ToString());
                            if (dataRow.Cell(12).Value != "" && CourtName?.Name?.Split('|')[1] == CourtGeneral.CourtExecutionFSSP.SatyaEndingIP)
                                exceptions.Append("Статья окончания ИП1 не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtExecutionFSSP.SatyaEndingIP != dataRow.Cell(12).Value.ToString())
                                {
                                    CourtGeneral.CourtExecutionFSSP.GroundsEndingIP = CourtName.Name.Split('|')[0];
                                    CourtGeneral.CourtExecutionFSSP.SatyaEndingIP = CourtName.Name.Split('|')[1];
                                }
                            }
                        }
                        if (dataRow.Cell(13).Value != "" && CourtGeneral.CourtExecutionFSSP.NumberIP2 != dataRow.Cell(13).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.NumberIP2 = dataRow.Cell(13).Value.ToString();
                        if (dataRow.Cell(14).Value != "" && CourtGeneral.CourtExecutionFSSP.IPInitiationDate2 != Convert.ToDateTime(dataRow.Cell(14).Value))
                            CourtGeneral.CourtExecutionFSSP.IPInitiationDate2 = Convert.ToDateTime(dataRow.Cell(14).Value);
                        if (dataRow.Cell(15).Value != "" && CourtGeneral.CourtExecutionFSSP.IPEndDate2 != Convert.ToDateTime(dataRow.Cell(15).Value))
                            CourtGeneral.CourtExecutionFSSP.IPEndDate2 = Convert.ToDateTime(dataRow.Cell(15).Value);

                        //if (dataRow.Cell(16).Value != "" && CourtGeneral.CourtExecutionFSSP.GroundsEndingIP2 != dataRow.Cell(16).Value.ToString())
                        //    CourtGeneral.CourtExecutionFSSP.GroundsEndingIP2 = dataRow.Cell(16).Value.ToString();
                        if (dataRow.Cell(16).Value != "")
                        {
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 4).CourtValueDictionaries.FirstOrDefault(x => x.Name.Split('|')[1]?.Trim() == dataRow.Cell(16).Value.ToString());
                            if (dataRow.Cell(16).Value != "" && CourtName?.Name?.Split('|')[1] == CourtGeneral.CourtExecutionFSSP.SatyaEndingIP2)
                                exceptions.Append("Статья окончания ИП1 не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(16).Value != "" && CourtGeneral.CourtExecutionFSSP.SatyaEndingIP2 != dataRow.Cell(16).Value.ToString())
                                {
                                    CourtGeneral.CourtExecutionFSSP.GroundsEndingIP2 = CourtName.Name.Split('|')[0];
                                    CourtGeneral.CourtExecutionFSSP.SatyaEndingIP2 = CourtName.Name.Split('|')[1];
                                }
                            }
                        }


                        var ex = exceptions.ToString();
                        if (ex != "")
                        {
                            throw new Exception(ex);
                        }

                        var result = await _court.SaveCourt(mapper.Map<DB.Model.Court.CourtGeneralInformation, BE.Court.CourtGeneralInformation>(CourtGeneral), User);
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Id = $"П-{result}", Description = "Успешно обновлено дело" });

                    }
                    catch (Exception ex)
                    {
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            }
            return CreateResultCourtLoader(reportCourtLoadExcels);
        }
        private DataTable CreateResultCourtLoader(List<ReportCourtLoadExcel> reportCourtLoadExcels)
        {
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Уникальный переданный номер из загрузочного файла"), new DataColumn("Индификатор судебного дела"),new DataColumn("Строка"),
                                        new DataColumn("Описание")});
            foreach (var Item in reportCourtLoadExcels)
                dt.Rows.Add(Item.IdCourt,Item.Id, Item.Line, Item.Description);

            return dt;
        }
    }
}
