using BE.ApiT_;
using BE.DPU;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public interface IExcelDpu
    {
        Task<DataTable> LoadDPUSummaryHouses(XLWorkbook Excels);
        Task<DataTable> LoadDPUHelpCalculationInstallation(XLWorkbook Excels);
    }
    public class ExcelDpu : IExcelDpu
    {
        public async Task<DataTable> LoadDPUHelpCalculationInstallation(XLWorkbook excels)
        {
            using (var context = new ApplicationDbContext())
            {
                var nonEmptyDataRows = excels.Worksheet(1).RowsUsed();
                int i = 1;
                List<DpuNotAddDb> notAddDbs = new List<DpuNotAddDb>();
                foreach (var dataRow in nonEmptyDataRows)
                {
                    if (dataRow.RowNumber() > 1)
                    {
                        string Message = "";
                        i++;
                        try
                        {
                            if (dataRow.Cell(4).Value == "")
                                Message = "Ошибка не указан лицевой счет \r\n";
                            if (dataRow.Cell(1).Value == "")
                                Message = "Ошибка не указан период \r\n";
                            var Period = Convert.ToDateTime(dataRow.Cell(1).Value);
                            var Lic = dataRow.Cell(6).Value != "" ? Convert.ToString(dataRow.Cell(4).Value) : "";
                            var lic = await context.dPUHelpCalculationInstallations.Where(x => x.Lic == Lic
                            && x.Period.Value.Year == Period.Year && x.Period.Value.Month == Period.Month).ToListAsync();
                            if(lic.Count() != 0)
                                Message = "Ошибка такой месяц уже существует \r\n";
                            if (!string.IsNullOrEmpty(Message))
                                throw new Exception(Message);
                            var Models = new DPUHelpCalculationInstallation();
                            Models.Period = Period;
                            Models.Street =  dataRow.Cell(2).Value != "" ? Convert.ToString(dataRow.Cell(2).Value) : "";
                            Models.Home = dataRow.Cell(3).Value != "" ? Convert.ToString(dataRow.Cell(3).Value) : "";
                            Models.Cadr = dataRow.Cell(4).Value != "" ? Convert.ToString(dataRow.Cell(4).Value) : "";
                            Models.Flat = dataRow.Cell(5).Value != "" ? Convert.ToString(dataRow.Cell(5).Value) : "";
                            Models.Lic = dataRow.Cell(6).Value != "" ? Convert.ToString(dataRow.Cell(6).Value) : "";
                            Models.Kl = dataRow.Cell(7).Value != "" ? Convert.ToInt32(dataRow.Cell(7).Value) : 0;
                            Models.Sobs = dataRow.Cell(8).Value != "" ? Convert.ToDouble(dataRow.Cell(8).Value) : 0;
                            Models.FullName = dataRow.Cell(9).Value != "" ? Convert.ToString(dataRow.Cell(9).Value) : "";
                            Models.FillLic = dataRow.Cell(10).Value != "" ? Convert.ToString(dataRow.Cell(10).Value) : "";
                            Models.NewFullLic = dataRow.Cell(11).Value != "" ? Convert.ToString(dataRow.Cell(11).Value) : "";
                            Models.CostDpuResidentialPremises = dataRow.Cell(12).Value != "" ? Convert.ToDouble(dataRow.Cell(12).Value) : 0;
                            Models.TotalAreaOfResidentialPremises = dataRow.Cell(13).Value != "" ? Convert.ToDouble(dataRow.Cell(13).Value) : 0;
                            Models.ShareInCommonOwnership = dataRow.Cell(14).Value != "" ? Convert.ToDouble(dataRow.Cell(14).Value) : 0;
                            Models.OneTimePayment = dataRow.Cell(15).Value != "" ? Convert.ToDouble(dataRow.Cell(15).Value) : 0;
                            Models.Note = dataRow.Cell(16).Value != "" ? Convert.ToString(dataRow.Cell(16).Value) : "";
                            Models.TotalCostOdpu = dataRow.Cell(17).Value != "" ? Convert.ToDouble(dataRow.Cell(17).Value) : 0;
                            Models.TotalCostOdpuResidentialPremises = dataRow.Cell(18).Value != "" ? Convert.ToDouble(dataRow.Cell(18).Value) : 0;
                            Models.TotalCostOdpuNonResidentialPremises = dataRow.Cell(19).Value != "" ? Convert.ToDouble(dataRow.Cell(19).Value) : 0;
                            Models.TotalAreaMKD = dataRow.Cell(20).Value != "" ? Convert.ToDouble(dataRow.Cell(20).Value) : 0;
                            Models.TotalAreaMKDResidentialPremises = dataRow.Cell(21).Value != "" ? Convert.ToDouble(dataRow.Cell(21).Value) : 0;
                            Models.TotalAreaMKDNonResidentialPremises = dataRow.Cell(22).Value != "" ? Convert.ToDouble(dataRow.Cell(22).Value) : 0;
                            Models.SaldoBeginningPeriod = dataRow.Cell(23).Value != "" ? Convert.ToDouble(dataRow.Cell(23).Value) : 0;
                            Models.TotalAccrued = dataRow.Cell(24).Value != "" ? Convert.ToDouble(dataRow.Cell(24).Value) : 0;
                            Models.AccruedMainPayment = dataRow.Cell(25).Value != "" ? Convert.ToDouble(dataRow.Cell(25).Value) : 0;
                            Models.PercentageRate = dataRow.Cell(26).Value != "" ? Convert.ToDouble(dataRow.Cell(26).Value) : 0;
                            Models.PercentageRateOneMonth = dataRow.Cell(27).Value != "" ? Convert.ToDouble(dataRow.Cell(27).Value) : 0;
                            Models.AccruedPercentage = dataRow.Cell(28).Value != "" ? Convert.ToDouble(dataRow.Cell(28).Value) : 0;
                            Models.Paid = dataRow.Cell(29).Value != "" ? Convert.ToDouble(dataRow.Cell(29).Value) : 0;
                            Models.DatePayment = dataRow.Cell(30).Value != "" ? Convert.ToDateTime(dataRow.Cell(30).Value) : new DateTime(1970,1,1);
                            Models.PercentagePayment = dataRow.Cell(31).Value != "" ? Convert.ToDouble(dataRow.Cell(31).Value) : 0; 
                            Models.PaymentMainDebt = dataRow.Cell(32).Value != "" ? Convert.ToDouble(dataRow.Cell(32).Value) : 0; 
                            Models.ToPay = dataRow.Cell(33).Value != "" ? Convert.ToDouble(dataRow.Cell(33).Value) : 0;
                            Models.SaldoEndPeriodDebt = dataRow.Cell(34).Value != "" ? Convert.ToDouble(dataRow.Cell(34).Value) : 0;
                            Models.SaldoEndPeriodPercentage = dataRow.Cell(35).Value != "" ? Convert.ToDouble(dataRow.Cell(35).Value) : 0;
                            context.dPUHelpCalculationInstallations.Add(Models);
                        }
                        catch(Exception e)
                        {
                            notAddDbs.Add(new DpuNotAddDb { Row = i, Message = Message == "" ? e.Message : Message });
                        }
                    }
                }
                DataTable dt = new DataTable("загрузка дпу");
                dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Строка"),
                                        new DataColumn("Описание ошибки")});
                await context.SaveChangesAsync();
                foreach(var Item in notAddDbs)
                {
                    dt.Rows.Add(Item.Row,Item.Message);
                }
                return dt;
            }
        }

        public async Task<DataTable> LoadDPUSummaryHouses(XLWorkbook excels)
        {
            using (var context = new ApplicationDbContext())
            {
                var nonEmptyDataRows = excels.Worksheet(1).RowsUsed();
                int i = 1;
                List<DpuNotAddDb> notAddDbs = new List<DpuNotAddDb>();
                foreach (var dataRow in nonEmptyDataRows)
                {
                    if (dataRow.RowNumber() > 1)
                    {
                        string Message = "";
                        i++;
                        try
                        {
                            DateTime? DateTransferRBR = null;
                            if (dataRow.Cell(1).Value == "")
                                Message = "Ошибка не указан Cadr \r\n";
                            if (dataRow.Cell(11).Value == "")
                                Message = "Ошибка не указан период \r\n";
                            var Period = Convert.ToDateTime(dataRow.Cell(11).Value);
                            if (dataRow.Cell(10).Value != "")
                                DateTransferRBR = Convert.ToDateTime(dataRow.Cell(10).Value);
                            var Cadr = dataRow.Cell(6).Value != "" ? Convert.ToString(dataRow.Cell(4).Value) : "";
                            var lic = await context.dPUSummaryHouses.Where(x => x.Cadr == Cadr
                            && x.PeriodExhibit.Value.Year == Period.Year && x.PeriodExhibit.Value.Month == Period.Month).ToListAsync();
                            if (lic.Count() != 0)
                                Message = "Ошибка такой месяц уже существует \r\n";
                            if (!string.IsNullOrEmpty(Message))
                                throw new Exception(Message);
                            var Models = new DPUSummaryHouses();
                            Models.Cadr = dataRow.Cell(1).Value != "" ? Convert.ToString(dataRow.Cell(1).Value) : "";
                            Models.Street = dataRow.Cell(2).Value != "" ? Convert.ToString(dataRow.Cell(2).Value) : "";
                            Models.Home = dataRow.Cell(3).Value != "" ? Convert.ToString(dataRow.Cell(3).Value) : "";
                            Models.TotalCostODPU = dataRow.Cell(4).Value != "" ? Convert.ToDouble(dataRow.Cell(4).Value) : 0;
                            Models.TotalCostOdpuResidentialPremises = dataRow.Cell(5).Value != "" ? Convert.ToDouble(dataRow.Cell(4).Value) : 0;
                            Models.TotalCostOdpuNonResidentialPremises = dataRow.Cell(6).Value != "" ? Convert.ToDouble(dataRow.Cell(4).Value) : 0;
                            Models.TotalAreaMKD = dataRow.Cell(7).Value != "" ? Convert.ToDouble(dataRow.Cell(4).Value) : 0;
                            Models.TotalAreaMKDResidentialPremises = dataRow.Cell(8).Value != "" ? Convert.ToDouble(dataRow.Cell(4).Value) : 0;
                            Models.TotalAreaMKDNonResidentialPremises = dataRow.Cell(9).Value != "" ? Convert.ToDouble(dataRow.Cell(4).Value) : 0;
                            Models.DateTransferRBR = DateTransferRBR;
                            Models.PeriodExhibit = Period;
                            context.dPUSummaryHouses.Add(Models);
                        }
                        catch (Exception e)
                        {
                            notAddDbs.Add(new DpuNotAddDb { Row = i, Message = Message == "" ? e.Message : Message });
                        }
                    }
                }
                DataTable dt = new DataTable("загрузка дпу");
                dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Строка"),
                                        new DataColumn("Описание ошибки")});
                await context.SaveChangesAsync();
                foreach (var Item in notAddDbs)
                {
                    dt.Rows.Add(Item.Row, Item.Message);
                }
                return dt;
            }
        }
    }
}
