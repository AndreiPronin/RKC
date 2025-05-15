using System.ComponentModel.DataAnnotations.Schema;

namespace TPlusModule.Repository.Models
{
    /// <summary>
    /// Объёмы по лицевому счёту в разрезе периодов
    /// </summary>
    [Table(name:nameof(AccountVolume), Schema = "dic")]
    public class AccountVolume
    {
        public string AccountNumber { get; set; }
        public DateTime Period { get; set; }
        public int HeatingVolumeTypeId { get; set; }
        public decimal HeatingVolume { get; set; }
        public string? HeatingNote { get; set; }
        public int HotWaterVolumeTypeId { get; set; }
        public decimal HotWaterVolume { get; set; }
        public string? HotWaterNote { get; set; }
    }
}
