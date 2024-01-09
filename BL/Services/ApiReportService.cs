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
        Task<byte[]> GetSberbankSevens(DateTime period);
    }
    public class ApiReportService : IApiReportService
    {
        private readonly ITokenCreator _tokenCreator;
        public ApiReportService(ITokenCreator tokenCreator) 
        { 
            _tokenCreator = tokenCreator;
        }
        public async Task<byte[]> GetSberbankSevens(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();
            var url = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.ReportServiceUrl).GetString();
            var reult = await Reuqests.GetRequestWithTockenAsync($"{url}/api/v1/TextReports/GetSberbankSevens?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
            //return File(reult, "application/octet-stream", $"{TypeFile.EbdMkd.GetDescription()}.txt");
        }
    }
}
