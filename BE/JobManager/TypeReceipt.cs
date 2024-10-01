using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.JobManager
{
    /// <summary>
    /// Тип квитанций
    /// </summary>
    public enum TypeReceipt
    {
        /// <summary>
        /// отправка основных квитанций
        /// </summary>
        [Description("Отправка основных квитанций")]
        PersonalReceipt = 0,
    }
}
