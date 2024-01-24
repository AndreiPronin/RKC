﻿using BE.Counter;
using BL.Helper;
using BL.http;
using BL.Security;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.IO;
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
        Task<byte[]> GetNss(DateTime period, Stream stream, string fileName);
        Task<byte[]> GetSubagent(DateTime period);
        Task<byte[]> GetShortSaldo(DateTime period);
        Task<byte[]> GetFullSaldo(DateTime period);
        Task<byte[]> GetInvoices(DateTime period);
        Task<byte[]> GetConsumerData(DateTime period);
        Task<byte[]> GetNssErrors(DateTime period, Stream stream, string fileName);
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

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetInvoices?period={period.ToString("yyyy-MM-dd")}", token);
            return reult;
        }

        public async Task<byte[]> GetConsumerData(DateTime period)
        {
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
            var token = _tokenCreator.CreateTokenReportService();
            var Reuqests = new Reuqest<object>();

            var reult = await Reuqests.GetFileRequestWithTockenAsync($"{Url}/api/v1/ExcelReports/GetConsumerData?period={period.ToString("yyyy-MM-dd")}", token);
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
    }
}