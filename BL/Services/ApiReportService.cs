using BE.Counter;
using BE.PersData;
using BE.Recalculation;
using BL.Helper;
using BL.http;
using BL.Security;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IApiReportService
    {
        Task<byte[]> GetSberbankInvoicesOldFormat(DateTime period);
        Task<byte[]> GetSberbankInvoices(DateTime period);
        Task<byte[]> GetSberbankCounters(DateTime period);
        Task<byte[]> GetRecalculation();
        Task<byte[]> GetNss(DateTime period, Stream stream, string fileName);
        Task<byte[]> GetSubagent(DateTime period);
        Task<byte[]> GetShortSaldo(DateTime period);
        Task<byte[]> GetFullSaldo(DateTime period);
        Task<byte[]> GetInvoices(DateTime period);
        Task<byte[]> GetConsumerData(DateTime period);
        Task<byte[]> GetNssErrors(DateTime period, Stream stream, string fileName);
        Task<byte[]> GetNssWithRecalculations(DateTime period, Stream stream, string fileName);
        Task<byte[]> GenerateCheckoutReport(DateTime period);
        Task<byte[]> CheckPreliminariesReports(Stream stream, string fileName);
        Task<byte[]> CheckMainReports(Stream stream, string fileName);
        Task<byte[]> GetSummaryInvoices(DateTime period);
        Task<byte[]> GetReadingsQuantity(DateTime period);
        Task<byte[]> GetPenyByLicWithSaldo(string fileName);
        Task<byte[]> GetPenyByLicFile(string FullLic);
        Task<List<PenyModel>> GetPenyByLicModel(string FullLic);
        Task<byte[]> UpdateDataWithGIS(Stream stream, string fileName);
    }
    public class ApiReportService : IApiReportService
    {
        private readonly ITokenCreator _tokenCreator;
        private readonly string Url;
        private readonly string UrlDbfReport;
        private readonly string RecalculationUrl;
        public ApiReportService(ITokenCreator tokenCreator) 
        { 
            _tokenCreator = tokenCreator;
            Url = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.ReportServiceUrl).GetString();
            UrlDbfReport = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.ReportServiceUrlDbf).GetString();
            RecalculationUrl = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.RecalculationServiceUrl).GetString();
        }
        public async Task<byte[]> GetSberbankInvoicesOldFormat(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/TextReports/GetSberbankInvoicesOldFormat?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }
        public async Task<byte[]> GetSberbankInvoices(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/TextReports/GetSberbankInvoices?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }
        public async Task<byte[]> GetSberbankCounters(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/TextReports/GetSberbankCounters?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }
        public async Task<byte[]> GetRecalculation()
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetRecalculation", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }

        public async Task<byte[]> GetNss(DateTime period, Stream stream, string fileName)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.UploadFileAndGetFile($"{Url}/api/v1/ExcelReports/GetNss?period={period.ToString("yyyy-MM-dd")}", token, stream, fileName);
            return reult;
        }

        public async Task<byte[]> GetSubagent(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetSubagent?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> GetShortSaldo(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetShortSaldo?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> GetFullSaldo(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetFullSaldo?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> GetInvoices(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetPenyInvoice?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> GetConsumerData(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetSummaryConsumedVolume?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> GetNssErrors(DateTime period, Stream stream, string fileName)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.UploadFileAndGetFile($"{Url}/api/v1/ExcelReports/GetNssErrors", token, stream, fileName);
            return reult;
        }
        public async Task<byte[]> GetNssWithRecalculations(DateTime period, Stream stream, string fileName)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.UploadFileAndGetFile($"{Url}/api/v1/ExcelReports/GetNssWithRecalculations?period={period.ToString("yyyy-MM-dd")}", token, stream, fileName);
            return reult;
        }
        public async Task<byte[]> GenerateCheckoutReport(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{UrlDbfReport}/api/v1/Misc/GenerateCheckoutReport?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> CheckPreliminariesReports(Stream stream, string fileName)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.UploadFileAndGetFile($"{Url}/api/v1/ReportsCheck/CheckPreliminariesReports", token, stream, fileName);
            return reult;
        }

        public async Task<byte[]> CheckMainReports(Stream stream, string fileName)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.UploadFileAndGetFile($"{Url}/api/v1/ReportsCheck/CheckMainReports", token, stream, fileName);
            return reult;
        }

        public async Task<byte[]> GetSummaryInvoices(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetSummaryInvoices?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> GetReadingsQuantity(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetReadingsQuantity?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }
        public async Task<byte[]> UpdateDataWithGIS(Stream stream, string fileName)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.UploadFileAndGetFile($"{Url}/api/v1/InnerTemplates/UpdateFlatFromExcel", token, stream, fileName, "flatGisTemplate");
            return reult;
        }
        public Task<byte[]> GetPenyByLicWithSaldo(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetPenyByLicFile(string FullLic)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetPenyByLic?lic={FullLic}", token);
            return reult;
        }
        public async Task<List<PenyModel>> GetPenyByLicModel(string FullLic)
        {
            try
            {
                var convert = new ConvertJson<List<PenyModel>>();
                _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
                var token = _tokenCreator.CreateTokenReportService();
                var Reuqests = new Reuqest<object>();

                var reult = await Reuqests.GetRequestWithTocken($"{Url}/api/v1/Peny/all?fullLic={FullLic}", token);
                return convert.ConverJsonToModel(reult);
            }catch (WebException ex)
            {
                return new List<PenyModel>();
            }catch (Exception ex)
            {
                throw;
            }
        }
    }
}
