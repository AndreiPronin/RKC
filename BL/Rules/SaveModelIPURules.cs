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
            if (saveModelIPU.InterVerificationInterval.HasValue && saveModelIPU.DATE_CHECK.HasValue && saveModelIPU.DATE_CHECK_NEXT.HasValue) {
                var validDATE_CHECK = saveModelIPU.DATE_CHECK.Value.AddYears(saveModelIPU.InterVerificationInterval.Value);
                if(validDATE_CHECK.Year != saveModelIPU.DATE_CHECK_NEXT.Value.Year)
                {
                    _exceptionString.Append($"Не верно указан МПИ год поверки {validDATE_CHECK.Year} - {saveModelIPU.DATE_CHECK_NEXT.Value.Year}");
                }
            }
            if (_exceptionString.ToString() != "") {
                _exception = new Exception(_exceptionString.ToString());
                throw _exception;
            }
        }
        public static void Validation(ModelAddPU modelAddPU)
        {
            if (modelAddPU.InterVerificationInterval.HasValue && modelAddPU.DATE_CHECK.HasValue && modelAddPU.DATE_CHECK_NEXT.HasValue)
            {
                var validDATE_CHECK = modelAddPU.DATE_CHECK.Value.AddYears(modelAddPU.InterVerificationInterval.Value);
                if (validDATE_CHECK.Year != modelAddPU.DATE_CHECK_NEXT.Value.Year)
                {
                    _exceptionString.Append($"Не верно указан МПИ год поверки {validDATE_CHECK.Year} - {modelAddPU.DATE_CHECK_NEXT.Value.Year}");
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
