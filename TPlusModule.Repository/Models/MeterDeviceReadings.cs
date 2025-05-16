using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPlusModule.Repository.Models
{
    /// <summary>
    /// Показания прибора учёта в разрезе периодов
    /// </summary>
    [Table(name: nameof(MeterDeviceReadings), Schema = "dbo")]
    public class MeterDeviceReadings
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Номер лицевого счёта
        /// </summary>
        [Column(TypeName = "varchar(9)")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Период в котором снимается показание
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime Period { get; set; }

        /// <summary>
        /// Идентификатор типа прибора учёта
        /// <br/><see cref="Common.Enums.MeterDeviceTypes"/> содержит коды справочника
        /// </summary>
        public int MeterDeviceTypeId { get; set; }

        /// <summary>
        /// Идентификатор размерности
        /// <br/>1 - куб.м, 2 - ГКал, 3 - кВт/ч
        /// <br/><see cref="Common.Enums.Measures"/> содержит коды справочника
        /// </summary>
        public int MeasureId { get; set; }

        /// <summary>
        /// Начальные показания
        /// </summary>
        [Column(TypeName = "decimal(15,5)")]
        public decimal PreviousValue { get; set; }

        /// <summary>
        /// Конечные (текущие) показания
        /// </summary>
        [Column(TypeName = "decimal(15,5)")]
        public decimal CurrentValue { get; set; }

        /// <summary>
        /// Дата показания.<br/>Если null, то в этом периоде показаний не передали
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime? ReadingsDate { get; set; }

        /// <summary>
        /// Номер прибора учёта, по которому пришло показание
        /// </summary>
        public string? FactoryNumber { get; set; }

        /// <summary>
        /// Тип прибора учёта
        /// </summary>
        [ForeignKey(nameof(MeterDeviceType))]
        public MeterDeviceType MeterDeviceType { get; set; }

        /// <summary>
        /// Размерность прибора учёта
        /// </summary>
        [ForeignKey(nameof(MeasureId))]
        public Measure Measure { get; set; }
    }
}
