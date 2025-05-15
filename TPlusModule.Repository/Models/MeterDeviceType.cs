using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPlusModule.Repository.Models
{
    /// <summary>
    /// Тип прибора учёта
    /// </summary>
    [Table(name:nameof(MeterDeviceType), Schema = "dic")]
    public class MeterDeviceType
    {
        /// <summary>
        /// Идентификатор записи в словаре
        ///  <br/> <see cref="TPlusModule.Common.Enums.MeterDeviceTypes"/> содержит коды
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Наименование типа прибора учёта <br/>
        /// ГВС1 - 1, ГВС2 - 2, ГВС3 - 3, ГВС4 - 4, ОТП1 - 5, ОТП2 - 6
        /// </summary>
        [Column(TypeName = "varchar(10)")]
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор услуги <br/> 2 - Отопление, 3 - Горячее водоснабжение
        /// </summary>
        public int ServiceID { get; set; }
    }
}
