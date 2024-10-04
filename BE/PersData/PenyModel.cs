using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.PersData
{
    public class PenyModel
    {
        /// <summary>
        /// Период, в котором начислены пени
        /// </summary>
        public string AllLicPeriod { get; set; }

        /// <summary>
        /// Срок оплаты задолженности
        /// </summary>
        public string SrokOpl { get; set; } = string.Empty;

        /// <summary>
        /// Дата начисления пени
        /// </summary>
        public string SrokPo { get; set; } = string.Empty;

        /// <summary>
        /// Оплата
        /// </summary>
        public string Opl { get; set; } = string.Empty;

        /// <summary>
        /// Задолженность для расчёта пени
        /// </summary>
        public string Dolg { get; set; } = string.Empty;

        /// <summary>
        /// Количество дней просрочки платежа
        /// </summary>
        public string DnDolg { get; set; } = string.Empty;

        /// <summary>
        /// Период начисления пени начальный
        /// </summary>
        public string Tekd1 { get; set; } = string.Empty;

        /// <summary>
        /// Период начисления пени конечный
        /// </summary>
        public string Tekd2 { get; set; } = string.Empty;

        /// <summary>
        /// Количество дней для расчёта пени по 1/300 ставки рефинансирования
        /// </summary>
        public string Tek300 { get; set; } = string.Empty;

        /// <summary>
        /// Количество дней для расчёта пени по 1/130 ставки рефинансирования
        /// </summary>
        public string Tek130 { get; set; } = string.Empty;

        /// <summary>
        /// Сумма пени по 1/300 ставки рефинансирования
        /// </summary>
        public string Peny300 { get; set; } = string.Empty;

        /// <summary>
        /// Ставка рефинансирования
        /// </summary>
        public string RefinancePercent { get; set; } = string.Empty;

        /// <summary>
        /// Сумма пени по 1/130 ставки рефинансирования
        /// </summary>
        public string Peny130 { get; set; } = string.Empty;

        /// <summary>
        /// Итого начислено за период
        /// </summary>
        public string Itog { get; set; } = string.Empty;
    }
}
