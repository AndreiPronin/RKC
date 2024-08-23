using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Recalculation
{
    public class RecalculationReasons
    {
        /// <summary>
        /// причина перерасчёта
        /// </summary>
        public int RecalculationReason { get; set; }
        /// <summary>
        /// наименование перерасчёта
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// примечание к наименованию (для разделения перерасчётов с разными кодами в системе, но одинаковыми по данным Т+)
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// отображать поле для ввода количества человек
        /// </summary>
        public bool IsResidentsCountFieldEnabled { get; set; }
        /// <summary>
        /// отображать поле для выбора схемы применения перерасчёта
        /// </summary>
        public bool IsSummingTypeFieldEnabled { get; set; }
        /// <summary>
        /// отображать поле для выбора типа счётчика
        /// </summary>
        public bool IsCounterFieldEnabled { get; set; }
        /// <summary>
        /// отображать поле для ввода объема для перерасчёта
        /// </summary>
        public bool IsVolumeFieldEnabled { get; set; }
        /// <summary>
        /// скрывать поле для ввода начальной даты перерасчёта (в запросе /calculate – recalculationBeginningPeriod = recalculationEndingPeriod);
        /// </summary>
        public bool HideRecalculationBeginningPeriodField { get; set; }
    }
}
