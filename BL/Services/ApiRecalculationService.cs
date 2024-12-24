using BE.Recalculation;
using BL.Extention;
using BL.Helper;
using BL.http;
using BL.Security;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IApiRecalculationService
    {
        Task<List<RecalculationReasons>> GetRecalculationInfosAsync();
        Task<Dictionary<string, string>> GetService();
        Task<RecalculationsDto> Calculation(Calculate calculate);
        Task ApplyCalculation(ApplyCalculation applyCalculation);
        Task<string> MassiveRecalculation(Stream stream, string fileName, MassRecalculationEnum recalculationReason, DateTime period);
    }

    public class ApiRecalculationService : IApiRecalculationService
    {
        private readonly ITokenCreator _tokenCreator;
        private readonly string _url;
        public ApiRecalculationService(ITokenCreator tokenCreator)
        {
            _tokenCreator = tokenCreator;
            _url = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.RecalculationServiceUrl).GetString();
        }
        public async Task<List<RecalculationReasons>> GetRecalculationInfosAsync()
        {
            var convert = new ConvertJson<List<RecalculationReasons>>();
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<RecalculationReasons>();

            var reult = await Reuqests.GetRequestWithTocken($"{_url}/recalculationInfos", token);
            return convert.ConverJsonToModel(reult);

        }
        public async Task<Dictionary<string, string>> GetService()
        {
            var convert = new ConvertJson<Dictionary<string, string>>();
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<RecalculationReasons>();

            var reult = await Reuqests.GetRequestWithTocken($"{_url}/services", token);
            return convert.ConverJsonToModel(reult);

        }
        public async Task<RecalculationsDto> Calculation(Calculate calculate)
        {
            var convert = new ConvertJson<RecalculationsDto>();
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<Calculate>();

            var result = await Reuqests.PostRequestWithTocken(calculate,$"{_url}/calculate", token);
            try
            {
                var response = convert.ConverJsonToModel(result);
                if(response == null || response.Recalculations == null)
                {
                    throw new Exception();
                }
                return response;
            }
            catch (Exception ex)
            {
                var convertError = new ConvertJson<ErrorCalculate>();
                var error = convertError.ConverJsonToModel(result);
                throw new Exception(error.Message);
            }
            
        }

        public async Task ApplyCalculation(ApplyCalculation applyCalculation)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<ApplyCalculation>();
            string result = string.Empty;
            try
            {
                result = await Reuqests.PostRequestWithTocken(applyCalculation, $"{_url}/apply", token);
            }
            catch (Exception ex)
            {
                var convertError = new ConvertJson<ErrorCalculate>();
                var error = convertError.ConverJsonToModel(result);
                throw new Exception(error.Message);
            }
        }
        public async Task<string> MassiveRecalculation(Stream stream,string fileName,MassRecalculationEnum recalculationReason, DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<ApplyCalculation>();
            string result = string.Empty;
            try
            {
                result = (await Reuqests.UploadFileAndGetFile(
                    $"{_url}/v1/MassiveRecalculation/process-template?RecalculationReason={Enum.GetName(typeof(MassRecalculationEnum), recalculationReason)}&Period={DateTime.Now.GetDateWhitMaxDate().ToString("yyyy-MM-dd")}", 
                    token,
                    stream,
                    fileName,
                    "Template")
                    ).ToString();
                return result;
            }
            catch (Exception ex)
            {
                var convertError = new ConvertJson<ErrorCalculate>();
                var error = convertError.ConverJsonToModel(result);
                throw new Exception(error?.Message);
            }
        }
    }
}
