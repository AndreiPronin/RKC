using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPlusModule.Repository.Models
{
    /// <summary>
    /// Объёмы по лицевому счёту в разрезе периодов
    /// </summary>
    [Table(name:nameof(AccountVolume), Schema = "dic")]
    public class AccountVolume
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
        /// Период
        /// </summary>
        [Column(TypeName = "date")]
        public DateTime Period { get; set; }

        /// <summary>
        /// Идентификатор типа расчёта объёма по отоплению
        /// <br/><see cref="Common.Enums.VolumeTypes"/> содержит коды справочника
        /// </summary>
        public int HeatingVolumeTypeId { get; set; }

        /// <summary>
        /// Объём отопление
        /// </summary>
        [Column(TypeName = "decimal(15,5)")]
        public decimal HeatingVolume { get; set; }

        /// <summary>
        /// Примечание к объёму по отоплению
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        public string? HeatingNote { get; set; }

        /// <summary>
        /// Идентификатор типа расчёта объёма по горячему водоснабжению
        /// <br/><see cref="Common.Enums.VolumeTypes"/> содержит коды справочника
        /// </summary>
        public int HotWaterVolumeTypeId { get; set; }

        /// <summary>
        /// Объём по горячему водоснабжению
        /// </summary>
        [Column(TypeName = "decimal(15,5)")]
        public decimal HotWaterVolume { get; set; }

        /// <summary>
        /// Примечание к объёму по горячему водоснабжению
        /// </summary>
        [Column(TypeName = "varchar(100)")]

        public string? HotWaterNote { get; set; }

        /// <summary>
        /// Тип расчёта объёма отопления
        /// </summary>
        [ForeignKey(nameof(HeatingVolumeTypeId))]
        public VolumeType HeatingVolumeType { get; set; }

        /// <summary>
        /// Тип расчёта объёма отопления
        /// </summary>
        [ForeignKey(nameof(HotWaterVolumeTypeId))]
        public VolumeType HotWaterVolumeType { get; set; }
    }
}
