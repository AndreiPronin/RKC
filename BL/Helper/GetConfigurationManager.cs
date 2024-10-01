using DocumentFormat.OpenXml.Office2010.PowerPoint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Helper
{
    public class KeyConfigurationManager
    {
        public const string AppOpenId = "App:OpenId";
        public const string MailHostMonitor = "mail:host:monitor";
        public const string GeneralServiceKey = "General:Service:Key";
        public const string ReportServiceUrl = "Report:Service:Url";
        public const string RecalculationServiceUrl = "Recalculation:Service:Url";
        public const string ReportServiceUrlDbf = "Report:Service:Url:DBF";
        public const string CourtLogPath = "Court:Log:Path";
    }
    public class GetConfigurationManager
    {
        private string Value { get; set; }
        public GetConfigurationManager GetAppSettings(string Key)
        {
            var res = ConfigurationManager.AppSettings[Key];
            if (res != null)
            {
                Value = res;
                return this;
            }
            Value = string.Empty;
            return this;
        }
        public int GetInt()
        {
            var res = int.TryParse(Value, out var val);
            if(res) return val;
            return 0;
        }
        public double GetDouble()
        {
            var res = double.TryParse(Value, out var val);
            if (res) return val;
            return 0;
        }
        public decimal GetDecimal()
        {
            var res = decimal.TryParse(Value, out var val);
            if (res) return val;
            return 0;
        }
        public string GetString()
        {
            return Value;
        }
    }
}
