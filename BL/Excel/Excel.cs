using AppCache;
using AutoMapper;
using BE.Counter;
using BE.PersData;
using BL.Counters;
using BL.Extention;
using BL.Helper;
using BL.Rules;
using BL.Services;
using ClosedXML.Excel;
using DB.DataBase;
using DB.Model;
using DB.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Excel
{
    public interface IExcel
    {
        DataTable CreateExcelCounters();
        Task<DataTable> CreateExcelLic(string User, ICacheApp cacheApp);
        Task<DataTable> CreateExcelLogPers();
        Task<DataTable> CreateExcelLogCounter();
        DataTable LoadExcelPUProperty(XLWorkbook Excels, string User, ICacheApp cacheApp);
        DataTable LoadExcelNewPUProperty(XLWorkbook Excels, string User, ICacheApp cacheApp);
        DataTable OpenNewPuWithIndications(XLWorkbook Excels, string User, ICacheApp cacheApp);
        DataTable LoadExcelNewPersonalData(XLWorkbook Excels, string User, ICacheApp cacheApp);
        DataTable LoadExcelUpdatePersonalDataMain(XLWorkbook Excels, string User, ICacheApp cacheApp);
        Task<DataTable> LoadExcelArrayCloseLicAsync(XLWorkbook Excels, string User, ICacheApp cacheApp);
        Task<DataTable> MassClosePU(XLWorkbook Excels, string User, ICacheApp cacheApp);
        DataTable LoadExcelSquarePersProperty(XLWorkbook Excels, string User, ICacheApp cacheApp);
        DataTable ErroIntegratin();
        DataTable TIpuGvs(string User, ICacheApp cacheApp);
        DataTable TIpuOtp(string User, ICacheApp cacheApp);
        DataTable LoadExcelUpdatePersonalDataMainFio(XLWorkbook Excels, string User, ICacheApp cacheApp);
        XLWorkbook SummaryReportGVS(XLWorkbook Excels, string User, ICacheApp cacheApp);
        XLWorkbook SummaryReportOTP(XLWorkbook Excels, string User, ICacheApp cacheApp);
        Task<XLWorkbook> GetPeriodPaymentSD(XLWorkbook Excels, string User, ICacheApp cacheApp);
        XLWorkbook ExcelReportFunction(XLWorkbook Excels, string Report, int column,string LasExcelColimn);
    }
    public class Excel:IExcel
    {
        private readonly ICacheApp _cacheApp;
        private readonly IGeneratorDescriptons _generatorDescriptons;
        private readonly Ilogger _logger;
        private readonly Services.IDictionary _dictionary;
        private readonly IReport _report;
        private readonly IMapper _mapper;
        private readonly ICounter _counter;
        private readonly IPersonalData _personalData;
        private readonly IMkdInformationService _mkdInformationService;
        private readonly ICourt _court;
        public Excel(ICacheApp cacheApp, IGeneratorDescriptons generatorDescriptons, Ilogger logger, Services.IDictionary dictionary,IReport report, 
            IMapper mapper, ICounter counter, IPersonalData personalData, IMkdInformationService mkdInformationService, ICourt court)
        {
            _cacheApp = cacheApp;
            _generatorDescriptons = generatorDescriptons;
            _logger = logger;
            _dictionary = dictionary;
            _report = report;
            _mapper = mapper;
            _counter = counter;
            _personalData = personalData;
            _mkdInformationService = mkdInformationService;
            _court = court;
        }
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
        public async Task<DataTable> CreateExcelLic(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
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
            List<ALL_LICS> AllLic = await DbLIC.ALL_LICS.ToListAsync();
            cacheApp.Update(User + "_", $@"Получил {AllLic.Count()} записей");
            foreach (var Items in AllLic)
            {
                dt.Rows.Add(Items.UL, Items.DOM, Items.CADR, Items.KW, Items.LIC, Items.F4ENUMELS, Items.ZAK, Items.FIO,
                    Items.FKUBSXVS,Items.FKUB1XVS, Items.FKUB2XVS, Items.FKUBSXV_2,Items.FKUB1XV_2, Items.FKUB2XV_2,
                    Items.FKUBSXV_3, Items.FKUB1XV_3, Items.FKUB2XV_3, Items.FKUBSXV_4, Items.FKUB1XV_4, Items.FKUB2XV_4,
                    Items.FKUBSOT_1, Items.FKUB1OT_1, Items.FKUB2OT_1, Items.FKUBSOT_2, Items.FKUB1OT_2, Items.FKUB2OT_2,
                    Items.FKUBSOT_3, Items.FKUB1OT_3, Items.FKUB2OT_3, Items.FKUBSOT_4, Items.FKUB1OT_4, Items.FKUB2OT_4);
            }
            cacheApp.Update(User + "_", "Ожидайте... Идет скачивание файла.");
            return dt;
        }
        public DataTable LoadExcelPUProperty(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            Counter counter = new Counter(new Logger(), new GeneratorDescriptons(), _mapper, _mkdInformationService);
            List<SaveModelIPU> COUNTERsNotAdded = new List<SaveModelIPU>();
            var dbApp = new ApplicationDbContext();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
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
                        saveModel.DIMENSION = new BE.Counter.DIMENSION();
                        var integrationReadings = new IntegrationReadings();
                        saveModel.FULL_LIC = dataRow.Cell(1).Value == "" ? "" : Convert.ToString(dataRow.Cell(1).Value).Trim();
                        saveModel.TypePU = dataRow.Cell(2).Value == "" ? "" : Convert.ToString(dataRow.Cell(2).Value).Trim();
                        if (dataRow.Cell(3).Value != "") { 
                            saveModel.INSTALLATIONDATE = Convert.ToDateTime(dataRow.Cell(3).Value);
                            if (saveModel?.INSTALLATIONDATE > DateTime.Now)
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                        }
                        saveModel.NumberPU = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Trim();
                        saveModel.BRAND_PU = dataRow.Cell(5).Value == "" ? "" : Convert.ToString(dataRow.Cell(5).Value).Trim();
                        saveModel.MODEL_PU = dataRow.Cell(6).Value == "" ? "" : Convert.ToString(dataRow.Cell(6).Value).Trim();
                        var Counter = counter.GetInfoPU(saveModel.FULL_LIC, saveModel.TypePU);
                        if (dataRow.Cell(7).Value != "") {
                            var DateCheck = Convert.ToDateTime(dataRow.Cell(7).Value);
                            if (DateCheck > DateTime.Now)
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                            if (DateCheck < Counter?.DATE_CHECK)
                                throw new Exception("Дата поверки меньше даты в базе");

                            saveModel.DATE_CHECK = Convert.ToDateTime(dataRow.Cell(7).Value);
                        }
                        if (dataRow.Cell(8).Value != "") {
                            var DateCheckNext = Convert.ToDateTime(dataRow.Cell(8).Value);
                            if (DateCheckNext > DateTime.Now.AddYears(6))
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                            if (DateCheckNext < Counter?.DATE_CHECK_NEXT)
                                throw new Exception("Дата слудющей поверки меньше даты в базе");

                            saveModel.DATE_CHECK_NEXT = Convert.ToDateTime(dataRow.Cell(8).Value);
                        }
                        saveModel.TYPEOFSEAL = dataRow.Cell(9).Value == "" ? "" : Convert.ToString(dataRow.Cell(9).Value).Trim();
                        saveModel.SEALNUMBER = dataRow.Cell(10).Value == "" ? "" : Convert.ToString(dataRow.Cell(10).Value).Trim();
                        saveModel.TYPEOFSEAL2 = dataRow.Cell(11).Value == "" ? "" : Convert.ToString(dataRow.Cell(11).Value).Trim();
                        saveModel.SEALNUMBER2 = dataRow.Cell(12).Value == "" ? "" : Convert.ToString(dataRow.Cell(12).Value).Trim();
                        saveModel.GIS_ID_PU = dataRow.Cell(13).Value == "" ? "" : Convert.ToString(dataRow.Cell(13).Value).Trim();
                        //saveModel.DIMENSION.Id = dataRow.Cell(14).Value == "" ? 0 : Convert.ToInt32(dataRow.Cell(14).Value);
                        if (dataRow.Cell(14).Value != "")
                        {
                            var str = dataRow.Cell(14).Value.ToString().Trim();
                            saveModel.CHECKPOINT_DATE = Convert.ToDateTime(Convert.ToString(dataRow.Cell(14).Value).Trim());
                        }
                        if (dataRow.Cell(15).Value != "")
                        {
                            saveModel.CHECKPOINT_READINGS = Convert.ToDouble(Convert.ToString(dataRow.Cell(15).Value).Trim());
                        }
                        saveModel.DIMENSION.Id = dataRow.Cell(16).Value == "" ? 0 : Convert.ToInt32(dataRow.Cell(16).Value);
                        if (dataRow.Cell(17).Value != "")
                        {
                            saveModel.InterVerificationInterval = Convert.ToInt32(Convert.ToString(dataRow.Cell(17).Value));
                        }
                        if (!counter.UpdatePU(saveModel, User))
                        {
                            saveModel.DESCRIPTION = $"Нет такого ПУ {saveModel.TypePU}";
                            COUNTERsNotAdded.Add(saveModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        COUNTERsNotAdded.Add(new SaveModelIPU { FULL_LIC = $"Ошибка на {i} строке", DESCRIPTION = ex.Message });
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
        public DataTable OpenNewPuWithIndications(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            Counter counter = new Counter(new Logger(), new GeneratorDescriptons(), _mapper, _mkdInformationService);
            List<ModelAddPU> COUNTERsNotAdded = new List<ModelAddPU>();
            var dbApp = new ApplicationDbContext();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
            var dictionaryBrand = new List<BRAND>();
            using (var db = new DbTPlus())
            {
                dictionaryBrand = db.BRAND.Include(x=>x.MODEL).ToList();
            }
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    i++;
                    try
                    {
                        var Procent = Math.Round((float)i / Count * 100, 0);
                        cacheApp.UpdateProgress(User, Procent.ToString());
                        ModelAddPU modelAddPU = new ModelAddPU();
                        modelAddPU.DIMENSION = new BE.Counter.DIMENSION();
                        var integrationReadings = new IntegrationReadings();
                        modelAddPU.FULL_LIC = dataRow.Cell(1).Value == "" ? "" : Convert.ToString(dataRow.Cell(1).Value).Trim();
                        var typePu = dataRow.Cell(2).Value == "" ? null : Convert.ToString(dataRow.Cell(2).Value).Trim().GetTypePu();
                        if(typePu.HasValue)
                            modelAddPU.TYPE_PU = typePu.Value;

                        if (dataRow.Cell(3).Value != "")
                        {
                            modelAddPU.INSTALLATIONDATE = Convert.ToDateTime(dataRow.Cell(3).Value);
                            if (modelAddPU?.INSTALLATIONDATE > DateTime.Now)
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                        }
                        modelAddPU.FACTORY_NUMBER_PU = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Trim();
                        modelAddPU.BRAND_PU = dataRow.Cell(5).Value == "" ? "" : Convert.ToString(dataRow.Cell(5).Value).Trim();
                        modelAddPU.MODEL_PU = dataRow.Cell(6).Value == "" ? "" : Convert.ToString(dataRow.Cell(6).Value).Trim();
                        var Counter = counter.GetInfoPU(modelAddPU.FULL_LIC, modelAddPU.TYPE_PU.GetDescription());
                        if (dataRow.Cell(7).Value != "")
                        {
                            var DateCheck = Convert.ToDateTime(dataRow.Cell(7).Value);
                            if (DateCheck > DateTime.Now)
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                            if (DateCheck < Counter?.DATE_CHECK)
                                throw new Exception("Дата поверки меньше даты в базе");

                            modelAddPU.DATE_CHECK = Convert.ToDateTime(dataRow.Cell(7).Value);
                        }
                        if (dataRow.Cell(8).Value != "")
                        {
                            var DateCheckNext = Convert.ToDateTime(dataRow.Cell(8).Value);
                            if (DateCheckNext > DateTime.Now.AddYears(6))
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                            if (DateCheckNext < Counter?.DATE_CHECK_NEXT)
                                throw new Exception("Дата следующей поверки меньше даты в базе");

                            modelAddPU.DATE_CHECK_NEXT = Convert.ToDateTime(dataRow.Cell(8).Value);
                        }
                        modelAddPU.TYPEOFSEAL = dataRow.Cell(9).Value == "" ? "" : Convert.ToString(dataRow.Cell(9).Value).Trim();
                        modelAddPU.SEALNUMBER = dataRow.Cell(10).Value == "" ? "" : Convert.ToString(dataRow.Cell(10).Value).Trim();
                        modelAddPU.TYPEOFSEAL2 = dataRow.Cell(11).Value == "" ? "" : Convert.ToString(dataRow.Cell(11).Value).Trim();
                        modelAddPU.SEALNUMBER2 = dataRow.Cell(12).Value == "" ? "" : Convert.ToString(dataRow.Cell(12).Value).Trim();
                        modelAddPU.DIMENSION.Id = dataRow.Cell(13).Value == "" ? 0 : Convert.ToInt32(dataRow.Cell(13).Value);
                        if (dataRow.Cell(14).Value != "")
                        {
                            modelAddPU.InterVerificationInterval = Convert.ToInt32(Convert.ToString(dataRow.Cell(14).Value));
                        }
                        modelAddPU.InitialReadings = dataRow.Cell(15).Value == "" ? 0 : Convert.ToDecimal(dataRow.Cell(15).Value);
                        modelAddPU.EndReadings = dataRow.Cell(16).Value == "" ? 0 : Convert.ToDecimal(dataRow.Cell(16).Value);
                        modelAddPU.DESCRIPTION = dataRow.Cell(17).Value == "" ? "" : Convert.ToString(dataRow.Cell(17).Value).Trim();
                        var typeNotUsePu = _counter.GetTypeNowUsePU(modelAddPU.FULL_LIC);
                        SaveModelIPURules.ValidationExcelAddPu(modelAddPU, dictionaryBrand, typeNotUsePu);
                        counter.AddPU(modelAddPU, User);
                        COUNTERsNotAdded.Add(new ModelAddPU { FULL_LIC =modelAddPU.FULL_LIC, TYPE_PU = modelAddPU.TYPE_PU, DESCRIPTION ="Успешно добавлен" });
                    }
                    catch (Exception ex)
                    {
                        COUNTERsNotAdded.Add(new ModelAddPU { FULL_LIC = $"Ошибка на {i} строке", DESCRIPTION = ex.Message });
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
                dt.Rows.Add(Items.FULL_LIC, Items.TYPE_PU?.GetDescription(), Items.FACTORY_NUMBER_PU, Items.DESCRIPTION);
            }
            return dt;
        }
        public DataTable LoadExcelNewPUProperty(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            Counter counter = new Counter(new Logger(), new GeneratorDescriptons(), _mapper, _mkdInformationService);
            List<SaveModelIPU> COUNTERsNotAdded = new List<SaveModelIPU>();
            var dbApp = new ApplicationDbContext();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
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
                        saveModel.DIMENSION = new BE.Counter.DIMENSION();
                        var integrationReadings = new IntegrationReadings();
                        saveModel.FULL_LIC = dataRow.Cell(1).Value == "" ? "" : Convert.ToString(dataRow.Cell(1).Value).Trim();
                        saveModel.TypePU = dataRow.Cell(2).Value == "" ? "" : Convert.ToString(dataRow.Cell(2).Value).Trim();
                        if (dataRow.Cell(3).Value != "") { 
                            saveModel.INSTALLATIONDATE = Convert.ToDateTime(dataRow.Cell(3).Value); 
                            if(saveModel?.INSTALLATIONDATE > DateTime.Now) {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                        }
                        
                        saveModel.NumberPU = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Trim();
                        saveModel.BRAND_PU = dataRow.Cell(5).Value == "" ? "" : Convert.ToString(dataRow.Cell(5).Value).Trim();
                        saveModel.MODEL_PU = dataRow.Cell(6).Value == "" ? "" : Convert.ToString(dataRow.Cell(6).Value).Trim();
                        if (dataRow.Cell(7).Value != "") { 
                            var DateCheck = Convert.ToDateTime(dataRow.Cell(7).Value); 
                            if (saveModel?.DATE_CHECK > DateTime.Now)
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                            if(DateCheck < saveModel?.DATE_CHECK)
                                throw new Exception("Дата поверки меньше даты в базе");

                            saveModel.DATE_CHECK = Convert.ToDateTime(dataRow.Cell(7).Value);
                        }
                        if (dataRow.Cell(8).Value != "") { 
                            var DateCheckNext = Convert.ToDateTime(dataRow.Cell(8).Value); 
                            if (saveModel?.DATE_CHECK_NEXT > DateTime.Now.AddYears(6))
                            {
                                throw new Exception("Дата акта ввода в эксплуатацию должна быть строго меньше текущей даты");
                            }
                            if (DateCheckNext < saveModel?.DATE_CHECK)
                                throw new Exception("Дата слудющей поверки меньше даты в базе");

                            saveModel.DATE_CHECK_NEXT = Convert.ToDateTime(dataRow.Cell(8).Value);
                        }
                        saveModel.TYPEOFSEAL = dataRow.Cell(9).Value == "" ? "" : Convert.ToString(dataRow.Cell(9).Value).Trim();
                        saveModel.SEALNUMBER = dataRow.Cell(10).Value == "" ? "" : Convert.ToString(dataRow.Cell(10).Value).Trim();
                        saveModel.TYPEOFSEAL2 = dataRow.Cell(11).Value == "" ? "" : Convert.ToString(dataRow.Cell(11).Value).Trim();
                        saveModel.SEALNUMBER2 = dataRow.Cell(12).Value == "" ? "" : Convert.ToString(dataRow.Cell(12).Value).Trim();
                        saveModel.DIMENSION.Id = dataRow.Cell(13).Value == "" ? 0 : Convert.ToInt32(dataRow.Cell(13).Value);
                        if (dataRow.Cell(14).Value != "")
                        {
                            saveModel.InterVerificationInterval = Convert.ToInt32(Convert.ToString(dataRow.Cell(14).Value));
                        }
                        if (!counter.UpdateNewPU(saveModel, User))
                        {
                            saveModel.DESCRIPTION = $"Нет такого ПУ {saveModel.TypePU}";
                            COUNTERsNotAdded.Add(saveModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        COUNTERsNotAdded.Add(new SaveModelIPU { FULL_LIC = $"Ошибка на {i} строке", DESCRIPTION = ex.Message });
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
        public DataTable LoadExcelUpdatePersonalDataMain(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            PersonalData personalData = new PersonalData(new Logger(), new GeneratorDescriptons(), _dictionary, _mapper);
            List<PersDataModel> PersNotAdded = new List<PersDataModel>();
            var flatTypes = _dictionary.GetFlatType();
            var dbApp = new ApplicationDbContext();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    i++;
                    try
                    {
                        var Procent = Math.Round((float)i / Count * 100, 0);
                        cacheApp.UpdateProgress(User, Procent.ToString());
                        PersDataModel saveModel = new PersDataModel();

                        saveModel.Lic = dataRow.Cell(1).Value == "" ? "" : Convert.ToString(dataRow.Cell(1).Value).Trim();

                        if (dataRow.Cell(5).Value != "") {
                            DateTime.TryParse(Convert.ToString(dataRow.Cell(2).Value).Replace(".", ","), out DateTime date);
                            if(date == DateTime.MinValue) 
                                saveModel.DateOfBirth = null;
                            else
                                saveModel.DateOfBirth = date;
                        }
                        saveModel.PlaceOfBirth = dataRow.Cell(3).Value == "" ? null : Convert.ToString(dataRow.Cell(3).Value).Trim();
                        saveModel.PassportSerial = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Trim();
                        saveModel.PassportNumber = dataRow.Cell(5).Value == "" ? "" : Convert.ToString(dataRow.Cell(5).Value).Trim();
                        saveModel.PassportIssued = dataRow.Cell(6).Value == "" ? "" : Convert.ToString(dataRow.Cell(6).Value).Trim();

                        if (dataRow.Cell(7).Value != "")
                        {
                            DateTime.TryParse(Convert.ToString(dataRow.Cell(7).Value).Replace(".", ","), out DateTime date);
                            if (date == DateTime.MinValue)
                                saveModel.PassportDate = null;
                            else
                                saveModel.PassportDate = date;

                        }
                           
                        if (dataRow.Cell(8).Value != "")
                        {
                            var tel = Convert.ToString(dataRow.Cell(8).Value).Trim();
                            saveModel.Tel1 = $"+7{tel}";
                        }
                        saveModel.Comment1 = dataRow.Cell(9).Value == "" ? "" : Convert.ToString(dataRow.Cell(9).Value).Trim();
                        if (dataRow.Cell(10).Value != "")
                        {
                            var tel = Convert.ToString(dataRow.Cell(10).Value).Trim();
                            saveModel.Tel2 = $"+7{tel}";
                        }
                        saveModel.Email = dataRow.Cell(11).Value == "" ? "" : Convert.ToString(dataRow.Cell(11).Value).Trim();
                        saveModel.Comment = dataRow.Cell(12).Value == "" ? "" : Convert.ToString(dataRow.Cell(12).Value).Trim();
                        if (dataRow.Cell(14).Value != "")
                            saveModel.DateAdd = Convert.ToDateTime(Convert.ToString(dataRow.Cell(14).Value).Replace(".", ","));
                        saveModel.RoomType = dataRow.Cell(15).Value == "" ? "" : Convert.ToString(dataRow.Cell(15).Value).Trim();
                        //if (dataRow.Cell(16).Value != "")
                        //    saveModel.Main = Convert.ToBoolean(Convert.ToInt16(Convert.ToString(dataRow.Cell(16).Value).Trim()));
                        if (dataRow.Cell(17).Value != "")
                            saveModel.IsDelete = Convert.ToBoolean(Convert.ToInt16(Convert.ToString(dataRow.Cell(17).Value).Trim()));
                        saveModel.SnilsNumber = dataRow.Cell(18).Value == "" ? "" : Convert.ToString(dataRow.Cell(18).Value).Trim();
                        saveModel.Inn = dataRow.Cell(19).Value == "" ? "" : Convert.ToString(dataRow.Cell(19).Value).Trim();
                        if (dataRow.Cell(20).Value != "")
                            saveModel.NumberOfPersons = Convert.ToInt32(Convert.ToString(dataRow.Cell(20).Value).Trim());
                        if (dataRow.Cell(21).Value != "")
                            saveModel.Square = Convert.ToDouble(Convert.ToString(dataRow.Cell(21).Value).Trim());
                        saveModel.SendingElectronicReceipt = dataRow.Cell(22).Value == "" ? "" : Convert.ToString(dataRow.Cell(22).Value).Trim();
                        saveModel.FlatTypeId = dataRow.Cell(23).Value == "" ? "" : Convert.ToString(dataRow.Cell(23).Value).Trim();
                        if (!string.IsNullOrEmpty(saveModel.FlatTypeId))
                            if (flatTypes.FirstOrDefault(x => x.FlatTypeId == saveModel.FlatTypeId) == null)
                                throw new Exception("Указаный Id помещения не найден в справочнике");

                        personalData.SavePersonalDataMain(saveModel, User);
                    }
                    catch (Exception ex)
                    {
                        if(ex.InnerException != null)
                            PersNotAdded.Add(new PersDataModel { Lic = $"Ошибка на {i} строке", Comment = ex.InnerException?.Message });
                        else
                            PersNotAdded.Add(new PersDataModel { Lic = $"Ошибка на {i} строке", Comment = ex.Message });
                    }
                }
            }
            DataTable dt = new DataTable("PersData");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Примечание")});
            foreach (var Items in PersNotAdded)
            {
                dt.Rows.Add(Items.Lic, Items.Comment);
            }
            return dt;
        }
        public DataTable LoadExcelUpdatePersonalDataMainFio(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            PersonalData personalData = new PersonalData(new Logger(), new GeneratorDescriptons(), _dictionary, _mapper);
            List<PersDataModel> PersNotAdded = new List<PersDataModel>();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    i++;
                    try
                    {
                        var Procent = Math.Round((float)i / Count * 100, 0);
                        cacheApp.UpdateProgress(User, Procent.ToString());
                        PersDataModel saveModel = new PersDataModel();

                        saveModel.Lic = dataRow.Cell(1).Value == "" ? null : Convert.ToString(dataRow.Cell(1).Value).Trim();
                        saveModel.LastName = dataRow.Cell(2).Value == "" ? "" : Convert.ToString(dataRow.Cell(2).Value).Trim();
                        saveModel.FirstName = dataRow.Cell(3).Value == "" ? "" : Convert.ToString(dataRow.Cell(3).Value).Trim();
                        saveModel.MiddleName = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Trim();
                        personalData.SavePersonalDataMain(saveModel, User);
                        personalData.SavePersonalDataFioLic(saveModel);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                            PersNotAdded.Add(new PersDataModel { Lic = $"Ошибка на {i} строке", Comment = ex.InnerException?.Message });
                        else
                            PersNotAdded.Add(new PersDataModel { Lic = $"Ошибка на {i} строке", Comment = ex.Message });
                    }
                }
            }
            DataTable dt = new DataTable("PersData");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Примечание")});
            foreach (var Items in PersNotAdded)
            {
                dt.Rows.Add(Items.Lic, Items.Comment);
            }
            return dt;
        }
        public async Task<DataTable> LoadExcelArrayCloseLicAsync(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
          
            List<MassCloseLicReport> massCloseLicReports = new List<MassCloseLicReport>();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    i++;
                    try
                    {
                        var Procent = Math.Round((float)i / Count * 100, 0);
                        cacheApp.UpdateProgress(User, Procent.ToString());

                        var Lic = dataRow.Cell(1).Value == "" ? null : Convert.ToString(dataRow.Cell(1).Value).Trim();
                        await _personalData.CloseLicAsync(Lic, "", _counter, User);
                        massCloseLicReports.Add(new MassCloseLicReport { Id = $"{Lic}", Description = "Закрыт успешно" });
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                            massCloseLicReports.Add(new MassCloseLicReport { Id = $"Ошибка на {i} строке", Description = ex.InnerException?.Message });
                        else
                            massCloseLicReports.Add(new MassCloseLicReport { Id = $"Ошибка на {i} строке", Description = ex.Message });
                    }
                }
            }
            DataTable dt = new DataTable("ClosLic");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Примечание")});
            foreach (var Items in massCloseLicReports)
            {
                dt.Rows.Add(Items.Id, Items.Description);
            }
            return dt;
        }
        public DataTable LoadExcelNewPersonalData(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            PersonalData personalData = new PersonalData(new Logger(), new GeneratorDescriptons(), _dictionary, _mapper);
            List<PersDataModel> PersNotAdded = new List<PersDataModel>();
            var dbApp = new ApplicationDbContext();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
            foreach (var dataRow in nonEmptyDataRows)
            {
                if (dataRow.RowNumber() > 1)
                {
                    i++;
                    try
                    {
                        var Procent = Math.Round((float)i / Count * 100, 0);
                        cacheApp.UpdateProgress(User, Procent.ToString());
                        PersDataModel saveModel = new PersDataModel();

                        saveModel.Lic = dataRow.Cell(1).Value == "" ? "" : Convert.ToString(dataRow.Cell(1).Value).Trim();
                        saveModel.LastName = dataRow.Cell(2).Value == "" ? null : Convert.ToString(dataRow.Cell(2).Value).Trim();
                        saveModel.FirstName = dataRow.Cell(3).Value == "" ? null : Convert.ToString(dataRow.Cell(3).Value).Trim();
                        saveModel.MiddleName = dataRow.Cell(4).Value == "" ? null : Convert.ToString(dataRow.Cell(4).Value).Trim();

                        if (dataRow.Cell(5).Value != "")
                            saveModel.DateOfBirth = Convert.ToDateTime(Convert.ToString(dataRow.Cell(5).Value).Replace(".", ","));
                        saveModel.PlaceOfBirth = dataRow.Cell(6).Value == "" ? null : Convert.ToString(dataRow.Cell(6).Value).Trim();
                        saveModel.PassportSerial = dataRow.Cell(7).Value == "" ? "" : Convert.ToString(dataRow.Cell(7).Value).Trim();
                        saveModel.PassportNumber = dataRow.Cell(8).Value == "" ? "" : Convert.ToString(dataRow.Cell(8).Value).Trim();
                        saveModel.PassportIssued = dataRow.Cell(9).Value == "" ? "" : Convert.ToString(dataRow.Cell(9).Value).Trim();
                        
                        if (dataRow.Cell(10).Value != "")
                            saveModel.PassportDate = Convert.ToDateTime(Convert.ToString(dataRow.Cell(10).Value).Replace(".", ","));
                        if (dataRow.Cell(11).Value != "")
                        {
                            var tel = Convert.ToString(dataRow.Cell(11).Value).Trim();
                            saveModel.Tel1 = $"+7{tel}";
                        }
                        saveModel.Comment1 = dataRow.Cell(12).Value == "" ? "" : Convert.ToString(dataRow.Cell(12).Value).Trim();
                        if (dataRow.Cell(13).Value != "")
                        {
                            var tel = Convert.ToString(dataRow.Cell(13).Value).Trim();
                            saveModel.Tel2 = $"+7{tel}";
                        }
                        saveModel.Email = dataRow.Cell(14).Value == "" ? "" : Convert.ToString(dataRow.Cell(14).Value).Trim();
                        saveModel.Comment = dataRow.Cell(15).Value == "" ? "" : Convert.ToString(dataRow.Cell(15).Value).Trim();
                        if (dataRow.Cell(17).Value != "")
                            saveModel.DateAdd = Convert.ToDateTime(Convert.ToString(dataRow.Cell(17).Value).Replace(".", ","));
                        saveModel.RoomType = dataRow.Cell(18).Value == "" ? "" : Convert.ToString(dataRow.Cell(18).Value).Trim();
                        if (dataRow.Cell(19).Value != "")
                            saveModel.Main = Convert.ToBoolean(Convert.ToInt16(Convert.ToString(dataRow.Cell(19).Value).Trim()));
                        if (dataRow.Cell(20).Value != "")
                            saveModel.IsDelete = Convert.ToBoolean(Convert.ToInt16(Convert.ToString(dataRow.Cell(20).Value).Trim()));
                        saveModel.SnilsNumber = dataRow.Cell(21).Value == "" ? "" : Convert.ToString(dataRow.Cell(21).Value).Trim();
                        saveModel.Inn = dataRow.Cell(22).Value == "" ? "" : Convert.ToString(dataRow.Cell(22).Value).Trim();
                        if (dataRow.Cell(23).Value != "")
                            saveModel.NumberOfPersons = Convert.ToInt32(Convert.ToString(dataRow.Cell(23).Value).Trim());
                        else if (saveModel.Main == true && dataRow.Cell(23).Value != "")
                            throw new Exception("Не указано колличество человек");
                        if (dataRow.Cell(24).Value != "")
                            saveModel.Square = Convert.ToDouble(Convert.ToString(dataRow.Cell(24).Value).Trim());
                        else if(saveModel.Main == true && dataRow.Cell(24).Value != "")
                            throw new Exception("Не указана площадь");
                        saveModel.SendingElectronicReceipt = dataRow.Cell(26).Value == "" ? "" : Convert.ToString(dataRow.Cell(26).Value).Trim();
                        personalData.AddPersData(saveModel, User);
                    }
                    catch (Exception ex)
                    {
                        PersNotAdded.Add(new PersDataModel { Lic = $"Ошибка на {i} строке", Comment = ex.Message + ex.StackTrace });
                    }
                }
            }
            DataTable dt = new DataTable("PersData");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Примечание")});
            foreach (var Items in PersNotAdded)
            {
                dt.Rows.Add(Items.Lic, Items.Comment);
            }
            return dt;
        }
        public async Task<DataTable> MassClosePU(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            Counter _counter = new Counter(new Logger(), new GeneratorDescriptons(), _mapper, _mkdInformationService);
            List<IPU_COUNTERS> CounterNotClose = new List<IPU_COUNTERS>();
            var reasonClode = await _dictionary.GetIpuArchiveReason();
            var DbTPlus = new DbTPlus();
            var Count = nonEmptyDataRows.Count();
            int i = 1;
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
                        var TypePu = Convert.ToString(dataRow.Cell(2).Value).Trim();
                        var result = _counter.DetailInfroms(Convert.ToString(dataRow.Cell(1).Value).Trim()).FirstOrDefault(x=>x.TYPE_PU.Contains(TypePu));
                        if (result == null) throw new Exception("Не найден ПУ");
                        saveModel.DESCRIPTION = dataRow.Cell(3).Value == "" ? "" : Convert.ToString(dataRow.Cell(3).Value).Trim();
                        saveModel.IdPU = result.ID_PU;
                        if (dataRow.Cell(4).Value != "")
                        {
                            saveModel.OPERATOR_CLOSE_DATE = Convert.ToDateTime(dataRow.Cell(4).Value);
                        }
                        if (dataRow.Cell(5).Value != "")
                        {
                            saveModel.OPERATOR_CLOSE_READINGS = Convert.ToDouble(Convert.ToString(dataRow.Cell(5).Value).Replace(".",","));
                        }
                        if (dataRow.Cell(6).Value == "")
                        {
                            throw new Exception("Укажите причину закрытия");
                        }
                        if (dataRow.Cell(6).Value != "")
                        {
                            var reasonText = dataRow.Cell(6).Value.ToString();
                            var reason = reasonClode.FirstOrDefault(x => x.Name == reasonText);
                            if (reason != null)
                            {
                                saveModel.ARCHIVEREASON = new ARCHIVEREASON
                                {
                                    Id = reason.Id,
                                    Name = reason.Name,
                                };

                                //if (saveModel.ARCHIVEREASON.Id == 13)
                                //{
                                //    if (!_counter.CheckNoReadings(result.ID_PU))
                                //    {
                                //        throw new InvalidOperationException("По прибору есть показания. Конечные не равны начальным или дублированы оператором");
                                //    }
                                //}
                            }
                            else
                            {
                                throw new Exception("Не найдена причина закрытия в справочнике");
                            }
                        }
                        var T1 = _logger.ActionUsersAsync(result.ID_PU, _generatorDescriptons.Generate(saveModel), User);
                        var T2 = _counter.UpdateReadings(saveModel);
                        await Task.WhenAll(T1, T2);
                        if (saveModel.ARCHIVEREASON.Id != 13)
                        {
                            _counter.DeleteIPU(result.ID_PU);
                        }
                    }
                    catch (Exception ex)
                    {
                        CounterNotClose.Add(new IPU_COUNTERS { FULL_LIC = $"Ошибка на {i} строке", DESCRIPTION = ex.Message });
                    }
                }
            }
            DataTable dt = new DataTable("CounterNotDelete");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Примечание")});
            foreach (var Items in CounterNotClose)
            {
                dt.Rows.Add(Items.FULL_LIC, Items.DESCRIPTION);
            }
            return dt;
        }
        public DataTable LoadExcelSquarePersProperty(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "0");
            var nonEmptyDataRows = Excels.Worksheet(1).RowsUsed();
            PersonalData personalData = new PersonalData(new Logger(), new GeneratorDescriptons(), _dictionary, _mapper);
            List<PersDataModel> PersDataModelNotAdded = new List<PersDataModel>();
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
                        PersDataModel saveModel = new PersDataModel();
                        var integrationReadings = new IntegrationReadings();
                        saveModel.Square = dataRow.Cell(5).Value == "" ? 0 : Convert.ToDouble(dataRow.Cell(5).Value.ToString().Replace(".", ","));
                        saveModel.Lic = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(4).Value).Replace("RBR","").Trim();
                        var CadastrNumber = dataRow.Cell(4).Value == "" ? "" : Convert.ToString(dataRow.Cell(3).Value).Trim();
                        try
                        {
                            personalData.UpdatePersDataSquareExcel(saveModel, User);
                            personalData.UpdateSquareCadastrFlat(saveModel.Square, CadastrNumber, saveModel.Lic);
                        }
                        catch(Exception ex)
                        {
                            saveModel.Comment = $"Ошибка при обновление лицевого счета {saveModel.Lic} {ex}";
                            PersDataModelNotAdded.Add(saveModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        PersDataModelNotAdded.Add(new PersDataModel { Comment = $"Ошибка на {i} строке" });
                    }
                }
            }
            DataTable dt = new DataTable("PersData");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"), new DataColumn("Информация об ошибке") });
            foreach (var Items in PersDataModelNotAdded)
            {
                dt.Rows.Add(Items.Lic,Items.Comment);
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
        public DataTable TIpuGvs(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "Получаю данные из бд");
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[22] { new DataColumn("КОД ДОМА"),
                                        new DataColumn("   УЛИЦА   "),
                                        new DataColumn("  ДОМ  "),
                                        new DataColumn("     КВАРТИРА    "),  new DataColumn("   ЛИЦЕВОЙ СЧЕТ   ")
            ,new DataColumn("   ФИО   ") ,new DataColumn("   ПРИБОР УЧЕТА   "),new DataColumn("   ДАТА АКТА ВВОДА В ЭКСПЛУАТАЦИЮ   ")
            ,new DataColumn("   ЗАВОДСКОЙ НОМЕР ИПУ   ")
            ,new DataColumn("   БРЕНД ПУ   ")
            ,new DataColumn("   МОДЕЛЬ ПУ   ")
            ,new DataColumn("   ДАТА ПОВЕРКИ ИПУ   ")
            ,new DataColumn("   ДАТА СЛЕДУЮЩЕЙ ПОВЕРКИ ИПУ   ") ,new DataColumn("   МПИ   "),new DataColumn("   № пломбы 1   "),new DataColumn("   Тип пломбы1   ")
            ,new DataColumn("   № пломбы 2   "),new DataColumn("   Тип пломбы 2   "),new DataColumn("   ПРИЗНАК ИПУ 1   ")
            ,new DataColumn("   РАЗМЕРНОСТЬ   ")
            ,new DataColumn("   КОНЕЧНЫЕ ПОКАЗАНИЯ ИПУ 1   "),new DataColumn("   ТЕКУЩИЕ ПОКАЗАНИЯ ИПУ 1   ")});
            var DB = new DbTPlus();
            var Counters = DB.Database.SqlQuery<view_TplusIPU_GVS>("SELECT * FROM [dbo].[view_TplusIPU_GVS]").ToList();
            cacheApp.Update(User + "_", "Формирую Excel");
            foreach (var Items in Counters)
            {
                dt.Rows.Add(Items.CODE_HOUSE, Items.STREET, Items.HOME, Items.FLAT,
                    Items.FULL_LIC, Items.FIO, Items.TYPE_PU, Items.INSTALLATIONDATE, Items.FACTORY_NUMBER_PU,
                    Items.BRAND_PU,
                    Items.MODEL_PU, 
                    Items.DATE_CHECK, Items.DATE_CHECK_NEXT, Items.InterVerificationInterval,
                    Items.SEALNUMBER, Items.TYPEOFSEAL, Items.SEALNUMBER2, Items.TYPEOFSEAL2, Items.SIGN_PU, Items.DIMENSION_NAME, Items.END_READINGS, Items.NOW_READINGS);
            }
            cacheApp.Update(User + "_", "Скачиваю Excel");
            return dt;
        }
        public DataTable TIpuOtp(string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress(User + "_", "Получаю данные из бд");
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[22] { new DataColumn("КОД ДОМА"),
                                        new DataColumn("   УЛИЦА   "),
                                        new DataColumn("  ДОМ  "),
                                        new DataColumn("     КВАРТИРА    "),  new DataColumn("   ЛИЦЕВОЙ СЧЕТ   ")
            ,new DataColumn("   ФИО   ") ,new DataColumn("   ПРИБОР УЧЕТА   "),new DataColumn("   ДАТА АКТА ВВОДА В ЭКСПЛУАТАЦИЮ   ")
            ,new DataColumn("   ЗАВОДСКОЙ НОМЕР ИПУ   ")
            ,new DataColumn("   БРЕНД ПУ   ")
            ,new DataColumn("   МОДЕЛЬ ПУ   ")
            ,new DataColumn("   ДАТА ПОВЕРКИ ИПУ   ")
            ,new DataColumn("   ДАТА СЛЕДУЮЩЕЙ ПОВЕРКИ ИПУ   "),new DataColumn("   МПИ   "),new DataColumn("   № пломбы 1   "),new DataColumn("   Тип пломбы1   ")
            ,new DataColumn("   № пломбы 2   "),new DataColumn("   Тип пломбы 2   "),new DataColumn("   ПРИЗНАК ИПУ 1   ")
            ,new DataColumn("   РАЗМЕРНОСТЬ   ")
            ,new DataColumn("   КОНЕЧНЫЕ ПОКАЗАНИЯ ИПУ 1   "),new DataColumn("   ТЕКУЩИЕ ПОКАЗАНИЯ ИПУ 1   ")});
            var DB = new DbTPlus();
            var Counters = DB.Database.SqlQuery<view_TplusIPU_OTP>("select * from [dbo].[view_TplusIPU_OTP]").ToList();
            cacheApp.Update(User + "_", "Формирую Excel");
            foreach (var Items in Counters)
            {
                dt.Rows.Add(Items.CODE_HOUSE, Items.STREET, Items.HOME, Items.FLAT,
                    Items.FULL_LIC, Items.FIO, Items.TYPE_PU, Items.INSTALLATIONDATE, Items.FACTORY_NUMBER_PU,
                    Items.BRAND_PU,
                    Items.MODEL_PU,
                    Items.DATE_CHECK, Items.DATE_CHECK_NEXT, Items.InterVerificationInterval,
                    Items.SEALNUMBER, Items.TYPEOFSEAL, Items.SEALNUMBER2, Items.TYPEOFSEAL2, Items.SIGN_PU, Items.DIMENSION_NAME, Items.END_READINGS, Items.NOW_READINGS);
            }
            cacheApp.Update(User + "_", "Скачиваю Excel");

            return dt;
        }
        public async Task<DataTable> CreateExcelLogPers()
        {
            DataTable dt = new DataTable("Pers");
            dt.Columns.AddRange(new DataColumn[5] {new DataColumn("Лицевой счет"),new DataColumn("Основной/не основной"), new DataColumn("Дата"), new DataColumn("Пользователь"),
            new DataColumn("Описание действий")});
            using (var AppDb = new ApplicationDbContext())
            {
                var Period = DateTime.Now.AddMonths(-2);
                var logPers = await AppDb.LogsPersData.Where(x => x.DateTime >= Period).ToListAsync();
                var PersData = await AppDb.PersData.ToListAsync();
                Object Lock = new Object();
                Parallel.ForEach(logPers, new ParallelOptions { MaxDegreeOfParallelism = 4 }, Item =>
                {
                    try
                    {
                        var Pers = PersData.FirstOrDefault(x => x.idPersData == Item.idPersData);
                        lock (Lock)
                        {
                            if (Pers != null)
                                if (Pers.Main.HasValue && Pers.Main.Value == true)
                                    Pers.Comment = "Основной";
                                else
                                    Pers.Comment = "Не основной";

                            dt.Rows.Add(Pers?.Lic, Pers.Comment, Item?.DateTime, Item.UserName, Item.Description);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                });
                
            }
            return dt;
        }
        public async Task<DataTable> CreateExcelLogCounter()
        {
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[5] {new DataColumn("Лицевой счет"),new DataColumn("Тип ПУ"), new DataColumn("Дата"), new DataColumn("Пользователь"),
            new DataColumn("Описание действий")});
            using (var AppDb = new ApplicationDbContext())
            {
                var Period = DateTime.Now.AddMonths(-1);
                var logCounter = await AppDb.Log.Where(x => x.DateTime >= Period).ToListAsync();
                using (var DbTplus = new DbTPlus())
                {
                    Object Lock = new Object();
                    var IPU_COUNTERS = await DbTplus.IPU_COUNTERS.ToListAsync();
                    Parallel.ForEach(logCounter, new ParallelOptions { MaxDegreeOfParallelism = 4 }, Item =>
                    {
                        lock (Lock)
                        {
                            var Pu = IPU_COUNTERS.FirstOrDefault(x => x.ID_PU == Item.IdPU);
                            dt.Rows.Add(Pu?.FULL_LIC, Pu?.TYPE_PU, Item.DateTime, Item.UserName, Item.Description);
                        }
                    });
                    
                }
            }
            return dt;
        }

        public XLWorkbook SummaryReportGVS(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            var worksheet = Excels.Worksheets.Add("Лист1");
            //worksheet.MergeAndValue(1, 2, 1, 4, $"Адрес");
            //worksheet.MergeAndValue(1, 14, 1, 20, $"ИПУ1");
            //worksheet.MergeAndValue(1, 21, 1, 27, $"ИПУ2");
            //worksheet.MergeAndValue(1, 28, 1, 29, $"Величина начисления");
            worksheet.Row(2).Height = 42.5;
            worksheet.Cell(2, 32).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 32).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            List<List<object>> lists = new List<List<object>>();
            var ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(BL.Excel.SqlQuery.Report.GVSReport, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    bool trigger = true;
                    var listObg = new List<object>();
                    while (reader.Read())
                    {
                        if (trigger)
                        {
                            for (int i = 0; i <= reader.FieldCount - 1; i++)
                            {
                                listObg.Add(reader.GetName(i));
                            }
                            lists.Add(listObg);
                        }
                        listObg = new List<object>();
                        for (int i = 0; i <= reader.FieldCount - 1; i++)
                        {
                            listObg.Add(reader[i]);
                        }
                        lists.Add(listObg);
                        trigger = false;
                    }
                    reader.Close();
                    var rowUse =  ExcelReport.Generate(lists,worksheet);
                    var rngTable = worksheet.Range("A1:AU" + rowUse);
                    rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            
            worksheet.Columns().AdjustToContents();
            return Excels;
        }

        public XLWorkbook SummaryReportOTP(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            var worksheet = Excels.Worksheets.Add("Лист1");
            //worksheet.MergeAndValue(1, 2, 1, 4, $"Адрес");
            //worksheet.MergeAndValue(1, 14, 1, 20, $"ИПУ1");
            //worksheet.MergeAndValue(1, 21, 1, 27, $"ИПУ2");
            //worksheet.MergeAndValue(1, 28, 1, 29, $"Величина начисления");
            worksheet.Row(2).Height = 42.5;
            worksheet.Cell(2, 32).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, 32).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            List<List<object>> lists = new List<List<object>>();
            var ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(BL.Excel.SqlQuery.Report.OTPReport, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    bool trigger = true;
                    var listObg = new List<object>();
                    while (reader.Read())
                    {
                        if (trigger)
                        {
                            for (int i = 0; i <= reader.FieldCount - 1; i++)
                            {
                                listObg.Add(reader.GetName(i));
                            }
                            lists.Add(listObg);
                        }
                        listObg = new List<object>();
                        for (int i = 0; i <= reader.FieldCount - 1; i++)
                        {
                            listObg.Add(reader[i]);
                        }
                        lists.Add(listObg);
                        trigger = false;
                    }
                    reader.Close();
                    var rowUse = ExcelReport.Generate(lists, worksheet);
                    var rngTable = worksheet.Range("A1:AU" + rowUse);
                    rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            worksheet.Columns().AdjustToContents();
            return Excels;
        }
        public async Task<XLWorkbook> GetPeriodPaymentSD(XLWorkbook Excels, string User, ICacheApp cacheApp)
        {
            cacheApp.AddProgress("_", "10");
            var workSheet = Excels.Worksheet(1);
            workSheet.Column(5).Style.NumberFormat.Format = "0;-0;; @";
            var nonEmptyDataRows = workSheet.RowsUsed();

            var Count = nonEmptyDataRows.Count();
            int i = 0;
            foreach (var dataRow in nonEmptyDataRows)
            {
                i++;
                if (dataRow.RowNumber() > 1)
                {
                    try
                    {
                        if (i % 2 == 0)
                        {
                            var Procent = Math.Round((float)i / Count * 100, 0);
                            cacheApp.UpdateProgress("_", Procent.ToString());
                        }
                    }
                    catch { }
                    try
                    {
                        var lic = dataRow.Cell(2).Value.TryGetLic();
                        var numberCourt = dataRow.Cell(7).Value.ToString();
                        var court = (await _court.GetCourtWithFilter((x) => x.Lic == lic && x.CourtWork.NumberSP == numberCourt))?.FirstOrDefault();
                        if (court == null)
                            continue;
                        if (decimal.TryParse(dataRow.Cell(6).Value.ToString(), out decimal summOperation))
                        {
                            if(summOperation > 0 && court.CourtWork.PeriodDebtEnd.HasValue)
                            {
                                
                                var date = court.CourtWork.PeriodDebtEnd.Value.ToString("MMyy");
                                dataRow.Cell(5).Value = $"'{date}" ;
                            }
                        }
                        
                    }
                    catch
                    {
                        dataRow.Cell(8).Value = "Ошибка обновления записи";
                    }
                   
                }
                  
            }
            return Excels;
        }
        public XLWorkbook ExcelReportFunction(XLWorkbook Excels,string Report, int column, string LasExcelColimn)
        {
            var worksheet = Excels.Worksheets.Add("Лист1");
            worksheet.Row(2).Height = 42.5;
            worksheet.Cell(2, column).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(1, column).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            List<List<object>> lists = new List<List<object>>();
            var ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(Report,connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    bool trigger = true;
                    var listObg = new List<object>();
                    while (reader.Read())
                    {
                        if (trigger)
                        {
                            for (int i = 0; i <= reader.FieldCount - 1; i++)
                            {
                                listObg.Add(reader.GetName(i));
                            }
                            lists.Add(listObg);
                        }
                        listObg = new List<object>();
                        for (int i = 0; i <= reader.FieldCount - 1; i++)
                        {
                            listObg.Add(reader[i]);
                        }
                        lists.Add(listObg);
                        trigger = false;
                    }
                    reader.Close();
                    var rowUse = ExcelReport.Generate(lists, worksheet);
                    var rngTable = worksheet.Range($"A1:{LasExcelColimn}" + rowUse);
                    rngTable.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    rngTable.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }

            worksheet.Columns().AdjustToContents();
            return Excels;
        }

    }
}
