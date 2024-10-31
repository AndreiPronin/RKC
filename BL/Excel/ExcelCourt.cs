using AutoMapper;
using BE.Court;
using BL.Counters;
using BL.Helper;
using BL.MapperProfile;
using BL.Notification;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model.Court;
using DB.Model.Court.DictiomaryModel;
using DocumentFormat.OpenXml.VariantTypes;
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
        Task<DataTable> ExcelsEditOwnerCourt(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadNote(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadInstallmentPlan(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadBankruptcy(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadWriteOff(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadOpenLitigationWork(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadLitigationWork(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadEnteringDecision(XLWorkbook Excels, string User);
        Task<DataTable> ExcelsDownloadPdFromIp(XLWorkbook Excels, string User);
    }
    public class ExcelCourt : IExcelCourt
    {
        private readonly ICourt _court;
        private readonly List<ReportCourtLoadExcel> reportCourtLoadExcels;
        private readonly Mapper _mapper = new CourtProfile().GetMapperBe();
        private readonly IDictionary _dictionary;
        private readonly INotificationMail _notificationMail;
        public ExcelCourt(ICourt court, IDictionary dictionary, INotificationMail notificationMail) 
        {
            _court = court;
            _dictionary = dictionary;
            reportCourtLoadExcels = new List<ReportCourtLoadExcel>();
            _notificationMail = notificationMail;
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
                        CourtGeneral.CourtOwnerInformation = new DB.Model.Court.CourtOwnerInformation();
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
                        if (CourtGeneral.CourtWork.PeriodDebtBegin.HasValue && CourtGeneral.CourtWork.PeriodDebtBegin > DateTime.Now)
                        {
                            exceptions.Append("Дата период задолженности начальный не может быть больше текущей даты");
                        }
                        CourtGeneral.CourtWork.PeriodDebtEnd = Convert.ToDateTime(dataRow.Cell(12).Value.ToString());
                        if (CourtGeneral.CourtWork.PeriodDebtEnd.HasValue && CourtGeneral.CourtWork.PeriodDebtEnd > DateTime.Now)
                        {
                            exceptions.Append("Дата период задолженности конечный не может быть больше текущей даты");
                        }
                        CourtGeneral.CourtWork.SumOdSendCourt = Convert.ToDouble(dataRow.Cell(13).Value.ToString());
                        CourtGeneral.CourtWork.SumPenySendCourt = Convert.ToDouble(dataRow.Cell(14).Value.ToString());
                        if(CourtGeneral.CourtWork.DateTask.HasValue && CourtGeneral.CourtWork.DateTask > DateTime.Now)
                        {
                            exceptions.Append("Дата задания не может быть больше текущей даты");
                        }

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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат загрузки.xlsx");
            return results;
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
                        if(CourtWorkRequisites.Suma != "")
                        {
                            CourtWorkRequisites.Suma = Math.Round(Convert.ToDouble(CourtWorkRequisites.Suma),2).ToString();
                        }
                        if(CourtWorkRequisites.Date > DateTime.Now)
                        {
                            exceptions.Append("Дата в реквизитах ГП больше текущей");
                        }
                        if(dataRow.Cell(5).Value != "" && CourtGeneral.CourtStateDuty.DateSendOnReturnFNS != Convert.ToDateTime(dataRow.Cell(5).Value))
                            CourtGeneral.CourtStateDuty.DateSendOnReturnFNS = Convert.ToDateTime(dataRow.Cell(5).Value);
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtStateDuty.DateReturnFNS != Convert.ToDateTime(dataRow.Cell(6).Value))
                            CourtGeneral.CourtStateDuty.DateReturnFNS = Convert.ToDateTime(dataRow.Cell(6).Value);
                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtStateDuty.ReasonReturn != dataRow.Cell(7).Value.ToString())
                            CourtGeneral.CourtStateDuty.ReasonReturn = dataRow.Cell(7).Value.ToString();
                        if (dataRow.Cell(7).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 16).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.CourtStateDuty.ReasonReturn) == null)
                            exceptions.Append("Причина возврата заявления ФНС не найдена в справочнике");
                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtWork.SumGP != Convert.ToDouble(dataRow.Cell(8).Value.ToString()))
                            CourtGeneral.CourtWork.SumGP = Convert.ToDouble(dataRow.Cell(8).Value.ToString());
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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления ГП.xlsx");
            return results;
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
                        {
                            CourtGeneral.DateBirthday = dataRow.Cell(3).Value.ToString();
                            if (Convert.ToDateTime(CourtGeneral.DateBirthday) > DateTime.Now)
                            {
                                exceptions.Append("Дата рождения не может быть больше текущей даты");
                            }
                        }
                        if (dataRow.Cell(4).Value != "" && CourtGeneral.PlaceBirth != dataRow.Cell(4).Value.ToString())
                            CourtGeneral.PlaceBirth = dataRow.Cell(4).Value.ToString();
                        if (dataRow.Cell(5).Value != "" && CourtGeneral.PasportSeria != dataRow.Cell(5).Value.ToString())
                            CourtGeneral.PasportSeria = dataRow.Cell(5).Value.ToString();
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.PasportNumber != dataRow.Cell(6).Value.ToString())
                            CourtGeneral.PasportNumber = dataRow.Cell(6).Value.ToString();
                        if (dataRow.Cell(7).Value != "" && CourtGeneral.PasportDate != dataRow.Cell(7).Value.ToString())
                        {
                            CourtGeneral.PasportDate = dataRow.Cell(7).Value.ToString();
                            if (Convert.ToDateTime(CourtGeneral.PasportDate) > DateTime.Now)
                            {
                                exceptions.Append("Дата рождения не может быть больше текущей даты");
                            }
                        }
                        if (dataRow.Cell(8).Value != "" && CourtGeneral.PasportIssue != dataRow.Cell(8).Value.ToString())
                            CourtGeneral.PasportIssue = dataRow.Cell(8).Value.ToString();
                        if (dataRow.Cell(9).Value != "" && CourtGeneral.Inn != dataRow.Cell(9).Value.ToString())
                            CourtGeneral.Inn = dataRow.Cell(9).Value.ToString();
                        if (dataRow.Cell(10).Value != "" && CourtGeneral.Snils != dataRow.Cell(10).Value.ToString())
                            CourtGeneral.Snils = dataRow.Cell(10).Value.ToString();
                        if (dataRow.Cell(11).Value != "" && CourtGeneral.ShareOfOwnership != dataRow.Cell(11).Value.ToString())
                            CourtGeneral.ShareOfOwnership = dataRow.Cell(11).Value.ToString();
                        if (dataRow.Cell(11).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 21).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.ShareOfOwnership) == null)
                            exceptions.Append("Вид собственности не найдена в справочнике" + Environment.NewLine);
                        if (dataRow.Cell(12).Value != "" && CourtGeneral.ShareInRight != dataRow.Cell(12).Value.ToString())
                            CourtGeneral.ShareInRight = dataRow.Cell(12).Value.ToString().Replace("/",".").Replace(",", ".");
                        //if (dataRow.Cell(12).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 22).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.ShareInRight) == null)
                        //    exceptions.Append("Доля в праве не найдена в справочнике" + Environment.NewLine);

                        if (dataRow.Cell(13).Value != "" && CourtGeneral.InSolidarityWith != dataRow.Cell(13).Value.ToString())
                            CourtGeneral.InSolidarityWith = dataRow.Cell(13).Value.ToString();
                        
                        if (dataRow.Cell(14).Value != "" && CourtGeneral.AddressRegister != dataRow.Cell(14).Value.ToString())
                            CourtGeneral.AddressRegister = dataRow.Cell(14).Value.ToString();

                        if (dataRow.Cell(15).Value != "" && CourtGeneral.DateDeath != Convert.ToDateTime(dataRow.Cell(15).Value.ToString()))
                        {
                            CourtGeneral.DateDeath = Convert.ToDateTime(dataRow.Cell(15).Value.ToString());
                            if (Convert.ToDateTime(CourtGeneral.DateDeath) > DateTime.Now)
                            {
                                exceptions.Append("Дата смерти не может быть больше текущей даты");
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
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            }
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления перс данных.xlsx");
            return results;
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
                            if (dataRow.Cell(2).Value != "" && CourtName is null)
                                exceptions.Append("Наименование суда не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtWork.NameCourt != CourtName.Name.Split('|')[0])
                                {
                                    CourtGeneral.CourtWork.NameCourt = CourtName.Name.Split('|')[0];
                                    CourtGeneral.CourtWork.AddressCourt = CourtName.Name.Split('|')[1];
                                }
                            }
                        }
                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtWork.SumGP != Convert.ToDouble(dataRow.Cell(3).Value))
                            CourtGeneral.CourtWork.SumGP = Convert.ToDouble(dataRow.Cell(3).Value);
                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtWork.DateReceptionCourt != Convert.ToDateTime(dataRow.Cell(4).Value))
                            CourtGeneral.CourtWork.DateReceptionCourt = Convert.ToDateTime(dataRow.Cell(4).Value);
                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtWork.NumberSP != dataRow.Cell(5).Value.ToString())
                            CourtGeneral.CourtWork.NumberSP = dataRow.Cell(5).Value.ToString();
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtWork.DateSP != Convert.ToDateTime(dataRow.Cell(6).Value))
                            CourtGeneral.CourtWork.DateSP = Convert.ToDateTime(dataRow.Cell(6).Value);
                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtExecutionFSSP.FioSendSpIo != dataRow.Cell(7).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.FioSendSpIo = dataRow.Cell(7).Value.ToString();
                        if (dataRow.Cell(7).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 20).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.CourtExecutionFSSP.FioSendSpIo) == null)
                            exceptions.Append("ФИО сотрудника не найдена в справочнике" + Environment.NewLine);
                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtExecutionFSSP.DateTask != Convert.ToDateTime(dataRow.Cell(8).Value))
                            CourtGeneral.CourtExecutionFSSP.DateTask = Convert.ToDateTime(dataRow.Cell(8).Value);
                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtExecutionFSSP.DateSendingApplicationFSSP != Convert.ToDateTime(dataRow.Cell(9).Value))
                            CourtGeneral.CourtExecutionFSSP.DateSendingApplicationFSSP = Convert.ToDateTime(dataRow.Cell(9).Value);
                        //if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtExecutionFSSP.ExecutiveBody != dataRow.Cell(10).Value.ToString())
                        //    CourtGeneral.CourtExecutionFSSP.ExecutiveBody = dataRow.Cell(10).Value.ToString();
                        if (dataRow.Cell(10).Value != "")
                        {
                            var xxx = dataRow.Cell(10).Value.ToString();
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 3).CourtValueDictionaries.FirstOrDefault(x => x.Name.Split('|')[0]?.Trim() == dataRow.Cell(10).Value.ToString());
                            if (dataRow.Cell(10).Value != "" && CourtName is null)
                                exceptions.Append("Исполнительный орган (ФССП) не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtExecutionFSSP.ExecutiveBody != CourtName.Name.Split('|')[0])
                                {
                                    CourtGeneral.CourtExecutionFSSP.ExecutiveBody = CourtName.Name.Split('|')[0];
                                    CourtGeneral.CourtExecutionFSSP.AddressIO = CourtName.Name.Split('|')[1];
                                }
                            }
                        }
                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtExecutionFSSP.NumberIP != dataRow.Cell(11).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.NumberIP = dataRow.Cell(11).Value.ToString();
                        if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtExecutionFSSP.IPInitiationDate != Convert.ToDateTime(dataRow.Cell(12).Value))
                            CourtGeneral.CourtExecutionFSSP.IPInitiationDate = Convert.ToDateTime(dataRow.Cell(12).Value);
                        if (dataRow.Cell(13).Value != "" && CourtGeneral.CourtExecutionFSSP.IPEndDate != Convert.ToDateTime(dataRow.Cell(13).Value))
                            CourtGeneral.CourtExecutionFSSP.IPEndDate = Convert.ToDateTime(dataRow.Cell(13).Value);

                        //if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtExecutionFSSP.GroundsEndingIP != dataRow.Cell(12).Value.ToString())
                        //    CourtGeneral.CourtExecutionFSSP.GroundsEndingIP = dataRow.Cell(12).Value.ToString();
                        if (dataRow.Cell(14).Value != "")
                        {
                            var xxx = dataRow.Cell(14).Value.ToString();
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 4).CourtValueDictionaries.FirstOrDefault(x => x.Name.Split('|')[1]?.Trim() == dataRow.Cell(14).Value.ToString());
                            if (dataRow.Cell(14).Value != "" && CourtName is null)
                                exceptions.Append("Статья окончания ИП1 не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(14).Value != "" && CourtGeneral.CourtExecutionFSSP.SatyaEndingIP != CourtName.Name.Split('|')[1])
                                {
                                    CourtGeneral.CourtExecutionFSSP.GroundsEndingIP = CourtName.Name.Split('|')[0];
                                    CourtGeneral.CourtExecutionFSSP.SatyaEndingIP = CourtName.Name.Split('|')[1];
                                }
                            }
                        }
                        if (dataRow.Cell(15).Value != "" && CourtGeneral.CourtExecutionFSSP.NumberIP2 != dataRow.Cell(15).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.NumberIP2 = dataRow.Cell(15).Value.ToString();
                        if (dataRow.Cell(16).Value != "" && CourtGeneral.CourtExecutionFSSP.IPInitiationDate2 != Convert.ToDateTime(dataRow.Cell(16).Value))
                            CourtGeneral.CourtExecutionFSSP.IPInitiationDate2 = Convert.ToDateTime(dataRow.Cell(16).Value);
                        if (dataRow.Cell(17).Value != "" && CourtGeneral.CourtExecutionFSSP.IPEndDate2 != Convert.ToDateTime(dataRow.Cell(17).Value))
                            CourtGeneral.CourtExecutionFSSP.IPEndDate2 = Convert.ToDateTime(dataRow.Cell(17).Value);

                        //if (dataRow.Cell(16).Value != "" && CourtGeneral.CourtExecutionFSSP.GroundsEndingIP2 != dataRow.Cell(16).Value.ToString())
                        //    CourtGeneral.CourtExecutionFSSP.GroundsEndingIP2 = dataRow.Cell(16).Value.ToString();
                        if (dataRow.Cell(18).Value != "")
                        {
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 4).CourtValueDictionaries.FirstOrDefault(x => x.Name.Split('|')[1]?.Trim() == dataRow.Cell(18).Value.ToString()?.Trim());
                            if (dataRow.Cell(18).Value != "" && CourtName?.Name?.Split('|')[1] == CourtGeneral.CourtExecutionFSSP.SatyaEndingIP2)
                                exceptions.Append("Статья окончания ИП2 не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(18).Value != "" && CourtGeneral.CourtExecutionFSSP.SatyaEndingIP2 != dataRow.Cell(18).Value.ToString())
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
            var results = CreateResultCourtLoaderDataColumn3(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Изменение СП и ИП.xlsx");
            return results;
        }

        public async Task<DataTable> ExcelsEditOwnerCourt(XLWorkbook Excels, string User)
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
                
                        if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerLastName != dataRow.Cell(2).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerLastName =dataRow.Cell(2).Value.ToString();
                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerFirstName != dataRow.Cell(3).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerFirstName = dataRow.Cell(3).Value.ToString();

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerSurname != dataRow.Cell(4).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerSurname = dataRow.Cell(4).Value.ToString();
                         if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerFloor != dataRow.Cell(5).Value.ToString())
                            CourtGeneral.Floor = dataRow.Cell(5).Value.ToString();
                        if (dataRow.Cell(5).Value != "" && dictionaryCourt.FirstOrDefault(x => x.Id == 13).CourtValueDictionaries.FirstOrDefault(x => x.Name == CourtGeneral.CourtOwnerInformation.OwnerFloor) == null)
                            exceptions.Append("Пол не найдена в справочнике" + Environment.NewLine);
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerDateBirthday != dataRow.Cell(6).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerDateBirthday = dataRow.Cell(6).Value.ToString();
                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerPlaceBirth != dataRow.Cell(7).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerPlaceBirth = dataRow.Cell(7).Value.ToString();
                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerTypeDocuments != dataRow.Cell(8).Value.ToString())
                        {
                            var OwnerTypeDocuments = dataRow.Cell(8).Value.ToString();
                            var dictionaryTypeDocuments = new List<string> { "паспорт гр-на РФ", "св-во о рождении", "загранпаспорт", "В/у" };
                            if (dictionaryTypeDocuments.Where(x => x == OwnerTypeDocuments).Count() > 0)
                                CourtGeneral.CourtOwnerInformation.OwnerTypeDocuments = dataRow.Cell(8).Value.ToString();
                            else
                                exceptions.Append("Вид документа, удостоверяющего личность не найден в справочнике" + Environment.NewLine);
                        }

                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerPasportSeria != dataRow.Cell(9).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerPasportSeria = dataRow.Cell(9).Value.ToString();
                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerPasportNumber != dataRow.Cell(10).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerPasportNumber = dataRow.Cell(10).Value.ToString();
                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerPasportDate != dataRow.Cell(11).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerPasportDate = dataRow.Cell(11).Value.ToString();
                        if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerPasportIssue != dataRow.Cell(12).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerPasportIssue = dataRow.Cell(12).Value.ToString();
                        if (dataRow.Cell(13).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerInn != dataRow.Cell(13).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerInn = dataRow.Cell(13).Value.ToString();
                        if (dataRow.Cell(14).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerSnils != dataRow.Cell(14).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerSnils = dataRow.Cell(14).Value.ToString();
                        if (dataRow.Cell(15).Value != "" && CourtGeneral.CourtOwnerInformation.OwnerAddressRegister != dataRow.Cell(15).Value.ToString())
                            CourtGeneral.CourtOwnerInformation.OwnerAddressRegister = dataRow.Cell(15).Value.ToString();


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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Изменение Собственника.xlsx");
            return results;
        }

        public async Task<DataTable> ExcelsDownloadNote(XLWorkbook Excels, string User)
        {
            var dictionaryCourt = await _dictionary.GetCourtDictionaries();
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        StringBuilder exceptions = new StringBuilder();
                        var CourtGeneral = await _court.DetailInfroms(dataRow.Cell(1).Value.TryGetCardNumber());

                        _court.SaveNoteWithTemplate(dataRow.Cell(2).Value.ToString(), CourtGeneral.Id, CourtGeneral.Lic);

                        var ex = exceptions.ToString();
                        if (ex != "")
                        {
                            throw new Exception(ex);
                        }
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Id = $"П-{CourtGeneral.Id}", Description = "Успешно обновлено дело" });

                    }
                    catch (Exception ex)
                    {
                        reportCourtLoadExcels.Add(new ReportCourtLoadExcel { IdCourt = dataRow.Cell(1).Value.ToString(), Line = dataRow.RowNumber().ToString(), Description = ex.Message });
                    }
                }
            }
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления примечания.xlsx");
            return results;
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
        private DataTable CreateResultCourtLoaderDataColumn3(List<ReportCourtLoadExcel> reportCourtLoadExcels)
        {
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Индификатор судебного дела"),new DataColumn("Строка"),
                                        new DataColumn("Описание")});
            foreach (var Item in reportCourtLoadExcels)
                dt.Rows.Add(Item.Id, Item.Line, Item.Description);

            return dt;
        }

        public async Task<DataTable> ExcelsDownloadInstallmentPlan(XLWorkbook Excels, string User)
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

                        if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtInstallmentPlan.DateAcceptanceApplicationRestructuring != Convert.ToDateTime(dataRow.Cell(2).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.DateAcceptanceApplicationRestructuring = Convert.ToDateTime(dataRow.Cell(2).Value.ToString());

                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtInstallmentPlan.AmountRestructuringOd !=  Convert.ToDouble(dataRow.Cell(3).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.AmountRestructuringOd = Convert.ToDouble(dataRow.Cell(3).Value.ToString());

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtInstallmentPlan.AmountRestructuringPeny != Convert.ToDouble(dataRow.Cell(4).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.AmountRestructuringPeny = Convert.ToDouble(dataRow.Cell(4).Value.ToString());

                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtInstallmentPlan.StartingMonthRestructuring != Convert.ToDateTime(dataRow.Cell(5).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.StartingMonthRestructuring = Convert.ToDateTime(dataRow.Cell(5).Value.ToString());

                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtInstallmentPlan.FinalMonthRestructuring != Convert.ToDateTime(dataRow.Cell(6).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.FinalMonthRestructuring = Convert.ToDateTime(dataRow.Cell(6).Value.ToString());

                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtInstallmentPlan.DateStartPayment != Convert.ToDateTime(dataRow.Cell(7).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.DateStartPayment = Convert.ToDateTime(dataRow.Cell(7).Value.ToString());

                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtInstallmentPlan.DateEndPayment != Convert.ToDateTime(dataRow.Cell(8).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.DateEndPayment = Convert.ToDateTime(dataRow.Cell(8).Value.ToString());

                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtInstallmentPlan.AmountMonthlyRestructuringPayment != Convert.ToDouble(dataRow.Cell(9).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.AmountMonthlyRestructuringPayment = Convert.ToDouble(dataRow.Cell(9).Value.ToString());

                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtInstallmentPlan.Comment != dataRow.Cell(10).Value.ToString())
                            CourtGeneral.CourtInstallmentPlan.Comment = dataRow.Cell(10).Value.ToString();

                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtInstallmentPlan.AmountPaymentRestructuring != Convert.ToDouble(dataRow.Cell(11).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.AmountPaymentRestructuring = Convert.ToDouble(dataRow.Cell(11).Value.ToString());

                        if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtInstallmentPlan.RemainderAmountPaymentRestructuring != Convert.ToDouble(dataRow.Cell(12).Value.ToString()))
                            CourtGeneral.CourtInstallmentPlan.RemainderAmountPaymentRestructuring = Convert.ToDouble(dataRow.Cell(12).Value.ToString());




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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Изменение Исковая работа.xlsx");
            return results;
        }

        public async Task<DataTable> ExcelsDownloadBankruptcy(XLWorkbook Excels, string User)
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

                        if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtBankruptcy.BankruptcyCaseNumber != dataRow.Cell(2).Value.ToString())
                            CourtGeneral.CourtBankruptcy.BankruptcyCaseNumber = dataRow.Cell(2).Value.ToString();

                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtBankruptcy.DateDeterminationAcceptance != Convert.ToDateTime(dataRow.Cell(3).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.DateDeterminationAcceptance = Convert.ToDateTime(dataRow.Cell(3).Value.ToString());

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtBankruptcy.DateDecisioDeclareCitizenBankrupt != Convert.ToDateTime(dataRow.Cell(4).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.DateDecisioDeclareCitizenBankrupt = Convert.ToDateTime(dataRow.Cell(4).Value.ToString());

                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtBankruptcy.DateDeterminationCompletion != Convert.ToDateTime(dataRow.Cell(5).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.DateDeterminationCompletion = Convert.ToDateTime(dataRow.Cell(5).Value.ToString());

                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtBankruptcy.DateDeterminationApplication != Convert.ToDateTime(dataRow.Cell(6).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.DateDeterminationApplication = Convert.ToDateTime(dataRow.Cell(6).Value.ToString());

                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtBankruptcy.SumOd != Convert.ToDouble(dataRow.Cell(7).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.SumOd = Convert.ToDouble(dataRow.Cell(7).Value.ToString());

                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtBankruptcy.SumPeny != Convert.ToDouble(dataRow.Cell(8).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.SumPeny = Convert.ToDouble(dataRow.Cell(8).Value.ToString());

                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtBankruptcy.SumGp != Convert.ToDouble(dataRow.Cell(9).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.SumGp = Convert.ToDouble(dataRow.Cell(9).Value.ToString());

                        if(dataRow.Cell(10).Value != "" && CourtGeneral.CourtBankruptcy.DateWriteOffBegin != Convert.ToDateTime(dataRow.Cell(10).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.DateWriteOffBegin = Convert.ToDateTime(dataRow.Cell(10).Value.ToString());

                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtBankruptcy.DateWriteOffEnd != Convert.ToDateTime(dataRow.Cell(11).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.DateWriteOffEnd = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());

                        if (dataRow.Cell(12).Value != "")
                        {
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 11).CourtValueDictionaries.FirstOrDefault(x => x.Name?.Trim() == dataRow.Cell(12).Value.ToString());
                            if (dataRow.Cell(12).Value != "" && CourtName is null)
                                exceptions.Append("Статус списания не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtBankruptcy.WriteOffStatus != CourtName.Name)
                                {
                                    CourtGeneral.CourtBankruptcy.WriteOffStatus = CourtName.Name;
                                }
                            }
                        }

                        if (dataRow.Cell(13).Value != "" && CourtGeneral.CourtBankruptcy.DateWrite != Convert.ToDateTime(dataRow.Cell(13).Value.ToString()))
                            CourtGeneral.CourtBankruptcy.DateWrite = Convert.ToDateTime(dataRow.Cell(13).Value.ToString());

                        if (dataRow.Cell(14).Value != "" && CourtGeneral.CourtBankruptcy.Comment != dataRow.Cell(14).Value.ToString())
                            CourtGeneral.CourtBankruptcy.Comment = dataRow.Cell(14).Value.ToString();

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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Изменение Банкротство.xlsx");
            return results;
        }

        public async Task<DataTable> ExcelsDownloadWriteOff(XLWorkbook Excels, string User)
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
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 12).CourtValueDictionaries.FirstOrDefault(x => x.Name?.Trim() == dataRow.Cell(2).Value.ToString());
                            if (dataRow.Cell(2).Value != "" && CourtName is null)
                                exceptions.Append("Документы подготовлены к списанию не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtWriteOff.WriteOffStatus != CourtName.Name)
                                {
                                    CourtGeneral.CourtWriteOff.WriteOffStatus = CourtName.Name;
                                }
                            }
                        }

                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtWriteOff.SumGp != Convert.ToDouble(dataRow.Cell(3).Value.ToString()))
                            CourtGeneral.CourtWriteOff.SumGp = Convert.ToDouble(dataRow.Cell(3).Value.ToString());

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtWriteOff.SumPeny != Convert.ToDouble(dataRow.Cell(4).Value.ToString()))
                            CourtGeneral.CourtWriteOff.SumPeny = Convert.ToDouble(dataRow.Cell(4).Value.ToString());

                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtWriteOff.SumGp != Convert.ToDouble(dataRow.Cell(5).Value.ToString()))
                            CourtGeneral.CourtWriteOff.SumGp = Convert.ToDouble(dataRow.Cell(5).Value.ToString());

                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtWriteOff.DateWriteOffBegin != Convert.ToDateTime(dataRow.Cell(6).Value.ToString()))
                            CourtGeneral.CourtWriteOff.DateWriteOffBegin = Convert.ToDateTime(dataRow.Cell(6).Value.ToString());

                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtWriteOff.DateWriteOffEnd != Convert.ToDateTime(dataRow.Cell(7).Value.ToString()))
                            CourtGeneral.CourtWriteOff.DateWriteOffEnd = Convert.ToDateTime(dataRow.Cell(7).Value.ToString());

                        if (dataRow.Cell(8).Value != "")
                        {
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 11).CourtValueDictionaries.FirstOrDefault(x => x.Name?.Trim() == dataRow.Cell(8).Value.ToString());
                            if (dataRow.Cell(8).Value != "" && CourtName is null)
                                exceptions.Append("Статус списания не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtWriteOff.WriteOffStatus != CourtName.Name)
                                {
                                    CourtGeneral.CourtWriteOff.WriteOffStatus = CourtName.Name;
                                }
                            }
                        }

                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtWriteOff.DateWriteOff != Convert.ToDateTime(dataRow.Cell(9).Value.ToString()))
                            CourtGeneral.CourtWriteOff.DateWriteOff = Convert.ToDateTime(dataRow.Cell(9).Value.ToString());

                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtWriteOff.Comment != dataRow.Cell(10).Value.ToString())
                            CourtGeneral.CourtWriteOff.Comment = dataRow.Cell(10).Value.ToString();

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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Изменение Банкротство.xlsx");
            return results;
        }
        public async Task<DataTable> ExcelsDownloadOpenLitigationWork(XLWorkbook Excels, string User)
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

                        if (dataRow.Cell(2).Value != "" && CourtGeneral.Lic != dataRow.Cell(2).Value.ToString())
                        {
                            if (!dataRow.Cell(2).Value.ToString().StartsWith("7"))
                            {
                                CourtGeneral.Lic = dataRow.Cell(2).Value.ToString();
                            }
                            else
                            {
                                exceptions.Append("Нельзя изменить поле лицевой счет так как оно начинается с 7");
                            }
                        }


                        if (dataRow.Cell(3).Value != "" && CourtGeneral.Street != dataRow.Cell(3).Value.ToString())
                            CourtGeneral.Street = dataRow.Cell(3).Value.ToString();

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.Home != dataRow.Cell(4).Value.ToString())
                            CourtGeneral.Home = dataRow.Cell(4).Value.ToString();

                        if (dataRow.Cell(5).Value != "" && CourtGeneral.Flat != dataRow.Cell(5).Value.ToString())
                            CourtGeneral.Flat = dataRow.Cell(5).Value.ToString();

                        if (dataRow.Cell(6).Value != "" && CourtGeneral.LastName != dataRow.Cell(6).Value.ToString())
                            CourtGeneral.LastName = dataRow.Cell(6).Value.ToString();

                        if (dataRow.Cell(7).Value != "" && CourtGeneral.FirstName != dataRow.Cell(7).Value.ToString())
                            CourtGeneral.FirstName = dataRow.Cell(7).Value.ToString();

                        if (dataRow.Cell(8).Value != "" && CourtGeneral.Surname != dataRow.Cell(8).Value.ToString())
                            CourtGeneral.Surname = dataRow.Cell(8).Value.ToString();

                        if (dataRow.Cell(9).Value != "")
                        {
                            var values1 = dictionaryCourt.FirstOrDefault(x => x.Id == 20).CourtValueDictionaries.FirstOrDefault(x => x.Name?.Trim() == dataRow.Cell(9).Value.ToString());
                            if (dataRow.Cell(9).Value != "" && values1 is null)
                                exceptions.Append("ФИО сотрудника (направившего дело в суд) не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtLitigationWork.FioSendCourt != values1.Name)
                                {
                                    CourtGeneral.CourtLitigationWork.FioSendCourt = values1.Name;
                                }
                            }
                        }

                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtLitigationWork.DateTask != Convert.ToDateTime(dataRow.Cell(10).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateTask = Convert.ToDateTime(dataRow.Cell(10).Value.ToString());

                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtLitigationWork.PeriodDebtBegin != Convert.ToDateTime(dataRow.Cell(11).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.PeriodDebtBegin = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());

                        if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtLitigationWork.PeriodDebtEnd != Convert.ToDateTime(dataRow.Cell(12).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.PeriodDebtEnd = Convert.ToDateTime(dataRow.Cell(12).Value.ToString());

                        if (dataRow.Cell(13).Value != "" && CourtGeneral.CourtLitigationWork.SumOdSendCourt != Convert.ToDouble(dataRow.Cell(13).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.SumOdSendCourt = Convert.ToDouble(dataRow.Cell(13).Value.ToString());

                        if (dataRow.Cell(14).Value != "" && CourtGeneral.CourtLitigationWork.SumPenySendCourt != Convert.ToDouble(dataRow.Cell(14).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.SumPenySendCourt = Convert.ToDouble(dataRow.Cell(14).Value.ToString());



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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Исковая работа.xlsx");
            return results;
        }
        public async Task<DataTable> ExcelsDownloadLitigationWork(XLWorkbook Excels, string User)
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

                        if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtLitigationWork.DateDecisionCansel != Convert.ToDateTime(dataRow.Cell(2).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateDecisionCansel = Convert.ToDateTime(dataRow.Cell(2).Value.ToString());


                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtLitigationWork.DateReceipt != Convert.ToDateTime(dataRow.Cell(3).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateReceipt = Convert.ToDateTime(dataRow.Cell(3).Value.ToString());

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtLitigationWork.DateSendPirRCO != Convert.ToDateTime(dataRow.Cell(4).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateSendPirRCO = Convert.ToDateTime(dataRow.Cell(4).Value.ToString());

                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtLitigationWork.DateSubmission != Convert.ToDateTime(dataRow.Cell(5).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateSubmission = Convert.ToDateTime(dataRow.Cell(5).Value.ToString());

                        if (dataRow.Cell(6).Value != "")
                        {
                            var CourtName = dictionaryCourt.FirstOrDefault(x => x.Id == 1).CourtValueDictionaries.FirstOrDefault(x => x.Name.Split('|')[0]?.Trim() == dataRow.Cell(6).Value.ToString());
                            if (dataRow.Cell(6).Value != "" && CourtName is null)
                                exceptions.Append("Наименование суда не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtLitigationWork.NameCourt != CourtName.Name.Split('|')[0])
                                {
                                    CourtGeneral.CourtLitigationWork.NameCourt = CourtName.Name;
                                }
                            }
                        }

                        if (dataRow.Cell(7).Value != "")
                        {
                            var values1 = dictionaryCourt.FirstOrDefault(x => x.Id == 17).CourtValueDictionaries.FirstOrDefault(x => x.Name?.Trim() == dataRow.Cell(7).Value.ToString());
                            if (dataRow.Cell(7).Value != "" && values1 is null)
                                exceptions.Append("Статус списания не найдена в справочнике" + Environment.NewLine);
                            else
                            {
                                if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtLitigationWork.HowSubmitApplicationCourt != values1.Name)
                                {
                                    CourtGeneral.CourtLitigationWork.HowSubmitApplicationCourt = values1.Name;
                                }
                            }
                        }

                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtLitigationWork.SumOtherCourt != Convert.ToDouble(dataRow.Cell(8).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.SumOtherCourt = Convert.ToDouble(dataRow.Cell(8).Value.ToString());

                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtLitigationWork.SumStateDuty != Convert.ToDouble(dataRow.Cell(9).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.SumStateDuty = Convert.ToDouble(dataRow.Cell(9).Value.ToString());

                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtLitigationWork.SumPayGP != Convert.ToDouble(dataRow.Cell(10).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.SumPayGP = Convert.ToDouble(dataRow.Cell(10).Value.ToString());

                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtLitigationWork.CaseNumber != dataRow.Cell(11).Value.ToString())
                            CourtGeneral.CourtLitigationWork.CaseNumber = dataRow.Cell(11).Value.ToString();

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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Исковая работа.xlsx");
            return results;
        }
        public async Task<DataTable> ExcelsDownloadEnteringDecision(XLWorkbook Excels, string User)
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

                        if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtLitigationWork.DateDecision != Convert.ToDateTime(dataRow.Cell(2).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateDecision = Convert.ToDateTime(dataRow.Cell(2).Value.ToString());

                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtLitigationWork.DateEntryDecision != Convert.ToDateTime(dataRow.Cell(3).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateEntryDecision = Convert.ToDateTime(dataRow.Cell(3).Value.ToString());

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtLitigationWork.AmountWithdrawnOd != Convert.ToDouble(dataRow.Cell(4).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.AmountWithdrawnOd = Convert.ToDouble(dataRow.Cell(4).Value.ToString());

                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtLitigationWork.AmountWithdrawnPeny != Convert.ToDouble(dataRow.Cell(5).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.AmountWithdrawnPeny = Convert.ToDouble(dataRow.Cell(5).Value.ToString());
                        
                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtLitigationWork.AmountRecoveredExpenses != Convert.ToDouble(dataRow.Cell(6).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.AmountRecoveredExpenses = Convert.ToDouble(dataRow.Cell(6).Value.ToString());

                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtLitigationWork.AmountWithdrawnGp != Convert.ToDouble(dataRow.Cell(7).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.AmountWithdrawnGp = Convert.ToDouble(dataRow.Cell(7).Value.ToString());

                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtLitigationWork.PeriodDebtInitialCollected != Convert.ToDateTime(dataRow.Cell(8).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.PeriodDebtInitialCollected = Convert.ToDateTime(dataRow.Cell(8).Value.ToString());

                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtLitigationWork.PeriodDebtEnd != Convert.ToDateTime(dataRow.Cell(9).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.PeriodDebtEnd = Convert.ToDateTime(dataRow.Cell(9).Value.ToString());

                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtLitigationWork.RequestDateIl != Convert.ToDateTime(dataRow.Cell(10).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.RequestDateIl = Convert.ToDateTime(dataRow.Cell(10).Value.ToString());

                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtLitigationWork.DateIssueIL != Convert.ToDateTime(dataRow.Cell(11).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateIssueIL = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());

                        if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtLitigationWork.DateFactGetIL != Convert.ToDateTime(dataRow.Cell(12).Value.ToString()))
                            CourtGeneral.CourtLitigationWork.DateFactGetIL = Convert.ToDateTime(dataRow.Cell(12).Value.ToString());

                        if (dataRow.Cell(13).Value != "" && CourtGeneral.CourtLitigationWork.NumberIl != dataRow.Cell(13).Value.ToString())
                            CourtGeneral.CourtLitigationWork.NumberIl = dataRow.Cell(13).Value.ToString();

                        if (dataRow.Cell(14).Value != "" && CourtGeneral.CourtLitigationWork.Comment != dataRow.Cell(14).Value.ToString())
                            CourtGeneral.CourtLitigationWork.Comment = dataRow.Cell(14).Value.ToString();

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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления Внесение решения по иску.xlsx");
            return results;
        }
        public async Task<DataTable> ExcelsDownloadPdFromIp(XLWorkbook Excels, string User)
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

                        if (dataRow.Cell(2).Value != "" && CourtGeneral.CourtExecutionFSSP.FullNameDebtorIP != dataRow.Cell(2).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.FullNameDebtorIP = dataRow.Cell(2).Value.ToString();

                        if (dataRow.Cell(3).Value != "" && CourtGeneral.CourtExecutionFSSP.IPDateBirth != Convert.ToDateTime(dataRow.Cell(3).Value.ToString()))
                            CourtGeneral.CourtExecutionFSSP.IPDateBirth = Convert.ToDateTime(dataRow.Cell(3).Value.ToString());

                        if (dataRow.Cell(4).Value != "" && CourtGeneral.CourtExecutionFSSP.SnilsIp != dataRow.Cell(4).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.SnilsIp = dataRow.Cell(4).Value.ToString();

                        if (dataRow.Cell(5).Value != "" && CourtGeneral.CourtExecutionFSSP.InnIp != dataRow.Cell(5).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.InnIp = dataRow.Cell(5).Value.ToString();

                        if (dataRow.Cell(6).Value != "" && CourtGeneral.CourtExecutionFSSP.PasportIp != dataRow.Cell(6).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.PasportIp = dataRow.Cell(6).Value.ToString();

                        if (dataRow.Cell(7).Value != "" && CourtGeneral.CourtExecutionFSSP.AddressIp != dataRow.Cell(7).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.AddressIp = dataRow.Cell(7).Value.ToString();

                        if (dataRow.Cell(8).Value != "" && CourtGeneral.CourtExecutionFSSP.DeathRegistryOfficeData != dataRow.Cell(8).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.DeathRegistryOfficeData = dataRow.Cell(8).Value.ToString();

                        if (dataRow.Cell(9).Value != "" && CourtGeneral.CourtExecutionFSSP.NumberInheritanceCase != dataRow.Cell(9).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.NumberInheritanceCase = dataRow.Cell(9).Value.ToString();

                        if (dataRow.Cell(10).Value != "" && CourtGeneral.CourtExecutionFSSP.FullNameNotary != dataRow.Cell(10).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.FullNameNotary = dataRow.Cell(10).Value.ToString();

                        if (dataRow.Cell(11).Value != "" && CourtGeneral.CourtExecutionFSSP.MonthCheckInheritance != Convert.ToDateTime(dataRow.Cell(11).Value.ToString()))
                            CourtGeneral.CourtExecutionFSSP.MonthCheckInheritance = Convert.ToDateTime(dataRow.Cell(11).Value.ToString());

                        if (dataRow.Cell(12).Value != "" && CourtGeneral.CourtExecutionFSSP.FullNameHeir != dataRow.Cell(12).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.FullNameHeir = dataRow.Cell(12).Value.ToString();

                        if (dataRow.Cell(13).Value != "" && CourtGeneral.CourtExecutionFSSP.AdditionalInformation != dataRow.Cell(13).Value.ToString())
                            CourtGeneral.CourtExecutionFSSP.AdditionalInformation = dataRow.Cell(13).Value.ToString();

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
            var results = CreateResultCourtLoader(reportCourtLoadExcels);
            _notificationMail.SendEmailResultLoadCourt(results, "Результат обновления ПД из ИП.xlsx");
            return results;
        }
    }
}
