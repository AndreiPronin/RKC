using BE.Counter;
using BL.Helper;
using BL.http;
using BL.Security;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Task<byte[]> GetNss(DateTime period);
        Task<byte[]> GetSubagent(DateTime period);
        Task<byte[]> GetShortSaldo(DateTime period);
        Task<byte[]> GetFullSaldo(DateTime period);
        Task<byte[]> GetInvoices(DateTime period);
        Task<byte[]> GetConsumerData(DateTime period);
        Task<byte[]> GetNssErrors();
    }
    public class ApiReportService : IApiReportService
    {
        private readonly ITokenCreator _tokenCreator;
        private string Url =>  new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.ReportServiceUrl).GetString();
        public ApiReportService(ITokenCreator tokenCreator) 
        { 
            _tokenCreator = tokenCreator;
        }
        public async Task<byte[]> GetSberbankInvoicesOldFormat(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/TextReports/GetSberbankInvoicesOldFormat?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }
        public async Task<byte[]> GetSberbankInvoices(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/TextReports/GetSberbankInvoices?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }
        public async Task<byte[]> GetSberbankCounters(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/TextReports/GetSberbankCounters?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }
        public async Task<byte[]> GetRecalculation()
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetRecalculation", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }

        public async Task<byte[]> GetNss(DateTime period)
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> GetSubagent(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetSubagent?period={period}", token);
            return reult;
        }

        public async Task<byte[]> GetShortSaldo(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetShortSaldo?period={period}", token);
            return reult;
        }

        public async Task<byte[]> GetFullSaldo(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetFullSaldo?period={period}", token);
            return reult;
        }

        public async Task<byte[]> GetInvoices(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetInvoices?period={period}", token);
            return reult;
        }

        public async Task<byte[]> GetConsumerData(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetConsumerData?period={period}", token);
            return reult;
        }

        public async Task<byte[]> GetNssErrors()
        {
            throw new NotImplementedException();
        }
    }
}
