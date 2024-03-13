using BE.Counter;
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
    }
}
