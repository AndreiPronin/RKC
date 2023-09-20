using DB.DataBase;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;

namespace DB.Model.Court
{
    public class CourtExecutionFSSP
    {
        [Key]
        [ForeignKey("CourtGeneralInformation")]
        public int CourtGeneralInformationId { get; set; }
        /// <summary>
        /// ФИО сотрудника (направившего СП в ИО)
        /// </summary>
        public string FioSendSpIo { get; set; }
        /// <summary>
        /// Исполнительный орган (ФССП, ПФ, Банк)
        /// </summary>
        public string ExecutiveBody { get; set; }
        /// <summary>
        /// Адрес ИО
        /// </summary>
        public string AddressIO { get; set; }
        /// <summary>
        /// Дата отправки заявления в ФССП
        /// </summary>
        public DateTime? DateSendingApplicationFSSP { get; set; }
        /// <summary>
        /// Способ отправки заявления в ФССП
        /// </summary>
        public string SendApplicationFSSP { get; set; }
        /// <summary>
        /// Сумма по заявлению в ФССП - всего
        /// </summary>
        public double? SumApplicationAll { get; set; }
        /// <summary>
        /// Сумма по заявлению в ФССП - ОД
        /// </summary>
        public double? SumApplicationOd { get; set; }
        /// <summary>
        /// Сумма по заявлению в ФССП - пени
        /// </summary>
        public double? SumApplicationPeny { get; set; }
        /// <summary>
        /// Сумма по заявлению в ФССП - ГП
        /// </summary>
        public double? SumApplicationGp { get; set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - всего
        /// </summary>
        public double? SumApplicationPFAll { get; set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - ОД
        /// </summary>
        public double? SumApplicationPFOd { get; set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - пени
        /// </summary>
        public double? SumApplicationPFPeny { get; set; }
        /// <summary>
        /// Сумма по заявлению в ПФ - ГП
        /// </summary>
        public double? SumApplicationPFGp { get; set; }
        /// <summary>
        /// Номер ИП
        /// </summary>
        public string NumberIP { get; set; }
        /// <summary>
        /// Дата возбуждения ИП
        /// </summary>
        public DateTime? IPInitiationDate { get; set; }
        /// <summary>
        /// Сумма в постановлении о возбуждении ИП - всего
        /// </summary>
        public double? SumDecisionInitateIP { get; set; }
        /// <summary>
        /// Дата окончания ИП
        /// </summary>
        public DateTime? IPEndDate { get; set; }
        /// <summary>
        /// Основание окончания ИП
        /// </summary>
        public string GroundsEndingIP { get; set; }
        /// <summary>
        /// Дата отзыва ИД с исполнения
        /// </summary>
        public DateTime? IPExecutionDate { get; set; }
        /// <summary>
        /// Причина отзыва ИД с исполнения
        /// </summary>
        public string ReasonExecutionIP { get; set; }
        /// <summary>
        /// Дата поступления оригиналов ИД при окончании ИП
        /// </summary>
        public DateTime? DateReceiptOriginalIDEndIP { get; set; }
        /// <summary>
        /// Дата отказа в возбуждении ИП
        /// </summary>
        public DateTime? DateRefusalInitiateIP { get; set; }
        /// <summary>
        /// Основание отказа в возбуждении ИП
        /// </summary>
        public string GroundsRefusalInitiateIP { get; set; }
        /// <summary>
        /// Дата поступления оригинала ИД при отказе в возбуждении ИП
        /// </summary>
        public DateTime? DateReceiptOriginalIDcaseRefusalInitiateIP { get; set; }
        /// <summary>
        /// Номер ИП
        /// </summary>
        public string NumberIP2 { get; set; }
        /// <summary>
        /// Дата возбуждения ИП
        /// </summary>
        public DateTime? IPInitiationDate2 { get; set; }
        /// <summary>
        /// Сумма в постановлении о возбуждении ИП - всего
        /// </summary>
        public double? SumDecisionInitateIP2 { get; set; }
        /// <summary>
        /// Дата окончания ИП
        /// </summary>
        public DateTime? IPEndDate2 { get; set; }
        /// <summary>
        /// Основание окончания ИП
        /// </summary>
        public string GroundsEndingIP2 { get; set; }
        /// <summary>
        /// Дата отзыва ИД с исполнения
        /// </summary>
        public DateTime? IPExecutionDate2 { get; set; }
        /// <summary>
        /// Причина отзыва ИД с исполнения
        /// </summary>
        public string ReasonExecutionIP2 { get; set; }
        /// <summary>
        /// Дата поступления оригиналов ИД при окончании ИП
        /// </summary>
        public DateTime? DateReceiptOriginalIDEndIP2 { get; set; }
        /// <summary>
        /// Дата отказа в возбуждении ИП
        /// </summary>
        public DateTime? DateRefusalInitiateIP2 { get; set; }
        /// <summary>
        /// Основание отказа в возбуждении ИП
        /// </summary>
        public string GroundsRefusalInitiateIP2 { get; set; }
        /// <summary>
        /// Дата поступления оригинала ИД при отказе в возбуждении ИП
        /// </summary>
        public DateTime? DateReceiptOriginalIDcaseRefusalInitiateIP2 { get; set; }
        /// <summary>
        /// ФИО должника в ИП
        /// </summary>
        public string FullNameDebtorIP { get; set; }
        /// <summary>
        /// Дата рождения в ИП
        /// </summary>
        public DateTime? IPDateBirth { get; set; }
        /// <summary>
        /// СНИЛС в ИП
        /// </summary>
        public string SnilsIp { get; set; }
        /// <summary>
        /// ИНН в ИП
        /// </summary>
        public string InnIp { get; set; }
        /// <summary>
        /// Паспорт в ИП
        /// </summary>
        public string PasportIp { get; set; }
        /// <summary>
        /// Адрес в ИП
        /// </summary>
        public string AddressIp { get; set; }
        /// <summary>
        /// Сведения о счетах
        /// </summary>
        public string AccountInformation { get; set; }
        /// <summary>
        /// Дата принятой меры приставом по счетам
        /// </summary>
        public DateTime? DateActionTakenBailiffAccounts { get; set; }
        /// <summary>
        /// Принятая мера приставом по счетам
        /// </summary>
        public string ActionTakenBailiffAccounts { get; set; }
        /// <summary>
        /// Сведения о недвижимости
        /// </summary>
        public string InformationAboutRealRstate { get; set; }
        /// <summary>
        /// Дата принятой меры приставом по недвижимости
        /// </summary>
        public DateTime? DateActionTakenBailiff { get; set; }
        /// <summary>
        /// Принятая мера приставом по недвижимости
        /// </summary>
        public string ActionTakenBailiff { get; set; }
        /// <summary>
        /// Сведения о ТС
        /// </summary>
        public string InformationAboutVehicle { get; set; }
        /// <summary>
        /// Дата принятой меры приставом по ТС
        /// </summary>
        public DateTime? DateActionTakenBailiffVehicle { get; set; }
        /// <summary>
        /// Принятая мера приставом по ТС
        /// </summary>
        public string ActionTakenBailiffVehicle { get; set; }
        /// <summary>
        /// Номера телефонов должника
        /// </summary>
        public string PhoneNumbersDebtor { get; set; }
        /// <summary>
        /// Доход/пенсия
        /// </summary>
        public double? IncomePension { get; set; }
        /// <summary>
        /// Дата принятой меры приставом по доходам
        /// </summary>
        public DateTime? DateActionTakenBailiffIncome { get; set; }
        /// <summary>
        /// Принятая мера приставом по доходам
        /// </summary>
        public string ActionTakenBailiffIncome { get; set; }
        /// <summary>
        /// Смена ФИО
        /// </summary>
        public string NameChange { get; set; }
        /// <summary>
        /// Данные загс о смерти
        /// </summary>
        public string DeathRegistryOfficeData { get; set; }
        /// <summary>
        /// № наследственного дела
        /// </summary>
        public string NumberInheritanceCase { get; set; }
        /// <summary>
        /// ФИО нотариуса
        /// </summary>
        public string FullNameNotary { get; set; }
        /// <summary>
        /// Месяц проверки наследственного дела
        /// </summary>
        public DateTime? MonthCheckInheritance { get; set; }
        /// <summary>
        /// ФИО наследника
        /// </summary>
        public string FullNameHeir { get; set; }
        /// <summary>
        /// Дата прочих действий пристава
        /// </summary>
        public DateTime? DateActionsBailiff { get; set; }
        /// <summary>
        /// Прочие действия пристава
        /// </summary>
        public string ActionsBailiff { get; set; }
        /// <summary>
        /// Сумма оплаты всего от ФССП
        /// </summary>
        public double? SumPaymentAllFSSP { get; set; }
        /// <summary>
        /// Сумма оплаты ОД от ФССП
        /// </summary>
        public double? SumPaymentODFSSP { get; set; }
        /// <summary>
        /// Дата оплаты ОД от ФССП
        /// </summary>
        public DateTime? DatePaymentODFSSP { get; set; }
        /// <summary>
        /// Сумма оплаты пени от ФССП
        /// </summary>
        public double? SumPaymentPenyFSSP { get; set; }
        /// <summary>
        /// Дата оплаты пени от ФССП
        /// </summary>
        public DateTime? DatePaymentPenyFSSP { get; set; }
        /// <summary>
        /// Сумма оплаты ГП от ФССП
        /// </summary>
        public double? SumPaymentGpFSSP { get; set; }
        /// <summary>
        /// Дата оплаты ГП от ФССП
        /// </summary>
        public DateTime? DatePaymentGpFSSP { get; set; }
        /// <summary>
        /// Дата обращения
        /// </summary>
        public DateTime? DateApplication { get; set; }
        /// <summary>
        /// Краткая суть обращения
        /// </summary>
        public string BriefAppeal { get; set; }
        /// <summary>
        /// Дата подачи обращения
        /// </summary>
        public DateTime? DateApplicationSubmission { get; set; }
        /// <summary>
        /// Способ подачи обращения
        /// </summary>
        public string ApplicationSubmissionMethod { get; set; }
        /// <summary>
        /// Дата ответа на обращение (фактическая)
        /// </summary>
        public DateTime? DateReasonAppealActual { get; set; }
        /// <summary>
        /// Краткая суть ответа на обращение
        /// </summary>
        public string BriefSummaryResponseAppeal { get; set; }
        /// <summary>
        /// Дополнительные сведения
        /// </summary>
        public string AdditionalInformation { get; set; }
        /// <summary>
        /// Дата задания
        /// </summary>
        public DateTime? DateTask { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
