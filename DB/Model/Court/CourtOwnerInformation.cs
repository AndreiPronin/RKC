using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    public class CourtOwnerInformation
    {
        [Key]
        [ForeignKey("CourtGeneralInformation")]
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// Имя должника
        /// </summary>
        public string OwnerFirstName { get; set; }
        /// <summary>
        /// Фамилия должника
        /// </summary>
        public string OwnerLastName { get; set; }
        /// <summary>
        /// Отчество должника
        /// </summary>
        public string OwnerSurname { get; set; }
        /// <summary>
        /// пол
        /// </summary>
        public string OwnerFloor { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public string OwnerDateBirthday { get; set; }
        /// <summary>
        /// Паспортные дата
        /// </summary>
        public string OwnerPasportDate { get; set; }
        /// <summary>
        /// Паспортные номер
        /// </summary>
        public string OwnerPasportNumber { get; set; }
        /// <summary>
        /// Паспортные серия
        /// </summary>
        public string OwnerPasportSeria { get; set; }
        /// <summary>
        /// Паспортные кем выдан
        /// </summary>
        public string OwnerPasportIssue { get; set; }
        /// <summary>
        /// ИНН
        /// </summary>
        public string OwnerInn { get; set; }
        /// <summary>
        /// СНИЛС
        /// </summary>
        public string OwnerSnils { get; set; }
        /// <summary>
        /// Адрес Регистрации
        /// </summary>
        public string OwnerAddressRegister { get; set; }
        /// <summary>
        /// Место рождения
        /// </summary>
        public string OwnerPlaceBirth { get; set; }
        /// <summary>
        /// Вид документа, удостоверяющего личность 
        /// </summary>
        public string OwnerTypeDocuments { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
