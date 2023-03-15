using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    public class CourtGeneralInformation
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// № л/с
        /// </summary>
        public string Lic { get; set; }
        /// <summary>
        /// регион
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// город/нас.пункт
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// улица
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// дом
        /// </summary>
        public string Home { get; set; }
        /// <summary>
        /// кв
        /// </summary>
        public string Flat { get; set; }
        /// <summary>
        /// ФИО должника
        /// </summary>
        public string FioDuty { get; set; }
        /// <summary>
        /// пол
        /// </summary>
        public string Floor { get; set; }
        /// <summary>
        /// Доля собственности
        /// </summary>
        public string ShareOfOwnership { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public string DateBirthday { get; set; }
        /// <summary>
        /// Паспортные дата
        /// </summary>
        public string PasportDate { get; set; }
        /// <summary>
        /// Паспортные номер
        /// </summary>
        public string PasportNumber { get; set; }
        /// <summary>
        /// Паспортные серия
        /// </summary>
        public string PasportSeria { get; set; }
        /// <summary>
        /// Паспортные кем выдан
        /// </summary>
        public string PasportIssue { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        public string Inn { get; set; }
        /// <summary>
        /// СНИЛС
        /// </summary>
        public string Snils { get; set; }
        /// <summary>
        /// Пенсионер
        /// </summary>
        public string Pensioner { get; set; }
        /// <summary>
        /// Исключение из рассылки
        /// </summary>
        public string ExclusionMailing { get; set; }
        /// <summary>
        /// Причины исключения из рассылки
        /// </summary>
        public string ReasonsExclusionMailing  { get; set; }
        /// <summary>
        /// Исключение из судебной работы
        /// </summary>
        public string ExclusionCourtWork { get; set; }
        /// <summary>
        /// Причины исключения из судебной работы
        /// </summary>
        public string ReasonsCourtWork { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string Comment { get; set; }
        public virtual CourtWork CourtWork { get; set; }
        public ICollection<CourtDocumentScans> CourtDocumentScans { get; set; }
        public CourtExecutionFSSP CourtExecutionFSSP { get; set; }
        public virtual CourtExecutionInPF CourtExecutionInPF { get; set; }
        public virtual CourtInstallmentPlan CourtInstallmentPlan { get; set; }
        public virtual CourtBankruptcy CourtBankruptcy { get; set; }
        public virtual CourtLitigationWork CourtLitigationWork { get; set; }
        public CourtStateDuty CourtStateDuty { get; set; }
        public CourtWriteOff CourtWriteOff { get; set; }
    }
}
