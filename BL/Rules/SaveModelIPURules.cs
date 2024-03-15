using BE.Counter;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Rules
{
    public static class SaveModelIPURules
    {
        private static Exception _exception;
        private static StringBuilder _exceptionString = new StringBuilder();
        public static void Validation(SaveModelIPU saveModelIPU)
        {
            _exceptionString.Clear();
            if (saveModelIPU.InterVerificationInterval.HasValue)
            {
                var error = saveModelIPU.InterVerificationInterval == 4 || saveModelIPU.InterVerificationInterval == 5 || saveModelIPU.InterVerificationInterval == 6;
                if (!error) {
                    _exceptionString.Append($"Не верно указан МПИ. МПИ должен иметь занчение 4 5 6");
                }
            }
            if (_exceptionString.ToString() != "") {
                _exception = new Exception(_exceptionString.ToString());
                throw _exception;
            }
        }
        public static void Validation(ModelAddPU modelAddPU)
        {
            if (modelAddPU.InterVerificationInterval.HasValue)
            {
                var error = modelAddPU.InterVerificationInterval == 4 || modelAddPU.InterVerificationInterval == 5 || modelAddPU.InterVerificationInterval == 6;
                if (!error)
                {
                    _exceptionString.Append($"Не верно указан МПИ. МПИ должен иметь занчение 4 5 6");
                }
            }
            if (modelAddPU.InterVerificationInterval.HasValue && modelAddPU.DATE_CHECK.HasValue && modelAddPU.DATE_CHECK_NEXT.HasValue)
            {
                var validDATE_CHECK = modelAddPU.DATE_CHECK.Value.AddYears(modelAddPU.InterVerificationInterval.Value);
                if (validDATE_CHECK != modelAddPU.DATE_CHECK_NEXT.Value)
                {
                    _exceptionString.Append($"Не верно указан МПИ {validDATE_CHECK} - {modelAddPU.DATE_CHECK_NEXT.Value}");
                }
            }
            if (_exceptionString.ToString() != "")
            {
                _exception = new Exception(_exceptionString.ToString());
                throw _exception;
            }
        }
        public static void ValidationExcelAddPu(ModelAddPU modelAddPU, List<BRAND> brands)
        {
            if (modelAddPU.InterVerificationInterval.HasValue)
            {
                var error = modelAddPU.InterVerificationInterval == 4 || modelAddPU.InterVerificationInterval == 5 || modelAddPU.InterVerificationInterval == 6;
                if (!error)
                {
                    _exceptionString.Append($"Не верно указан МПИ. МПИ должен иметь занчение 4 5 6");
                }
            }
            if (modelAddPU.InterVerificationInterval.HasValue && modelAddPU.DATE_CHECK.HasValue && modelAddPU.DATE_CHECK_NEXT.HasValue)
            {
                var validDATE_CHECK = modelAddPU.DATE_CHECK.Value.AddYears(modelAddPU.InterVerificationInterval.Value);
                if (validDATE_CHECK != modelAddPU.DATE_CHECK_NEXT.Value)
                {
                    _exceptionString.Append($"Не верно указан МПИ {validDATE_CHECK} - {modelAddPU.DATE_CHECK_NEXT.Value}");
                }
            }
           
            if ( string.IsNullOrEmpty(modelAddPU.FACTORY_NUMBER_PU))
            {
                _exceptionString.Append($"Не указан номер прибора"); 
            }
            if (modelAddPU.TYPE_PU == null)
            {
                _exceptionString.Append($"Не верно указан тип прибора");
            }
            if (brands != null)
            {
                var brand = new BRAND();
                if (string.IsNullOrEmpty(modelAddPU.BRAND_PU))
                {
                    _exceptionString.Append($"Не указан бренд прибора");
                }
                else
                {
                    brand =  brands.Where(x=>x.BRAND_NAME == modelAddPU.BRAND_PU).FirstOrDefault();
                    if(brand == null) {
                        _exceptionString.Append($"Название бренда не найдено в справочнике");
                    }
                }
                if (string.IsNullOrEmpty(modelAddPU.MODEL_PU))
                {
                    _exceptionString.Append($"Не указана модель прибора");
                }
                else
                {
                    var model = brand?.MODEL?.Where(x => x.MODEL_NAME == modelAddPU.MODEL_PU).FirstOrDefault();
                    if (model == null)
                    {
                        _exceptionString.Append($"Название модели не найдено в справочнике");
                    }
                }
            }
            if (modelAddPU.DIMENSION?.Id == 0)
            {
                _exceptionString.Append($"Не указан номер прибора");
            }
            if (modelAddPU.DATE_CHECK == null)
            {
                _exceptionString.Append($"Не указана дата поверки");
            }
            if (modelAddPU.DATE_CHECK_NEXT == null)
            {
                _exceptionString.Append($"Не указана дата следующей поверки");
            }
            if (modelAddPU.InitialReadings == null)
            {
                _exceptionString.Append($"Не указаны начальные показания прибора");
            }
            if (modelAddPU.EndReadings == 0)
            {
                _exceptionString.Append($"Не указаны конечные показания прибора");
            }
            if (_exceptionString.ToString() != "")
            {
                _exception = new Exception(_exceptionString.ToString());
                throw _exception;
            }
        }
    }
}
