using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPlusModule.Repository.Models
{
    /// <summary>
    /// Тип результирующего объёма
    /// </summary>
    [Table(name:nameof(VolumeType), Schema = "dic")]
    public class VolumeType
    {
        /// <summary>
        /// Идентификатор в словаре
        ///  <br/> <see cref="TPlusModule.Common.Enums.VolumeTypes"/> содержит коды
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Наименование типа результирующего объёма<br/>
        /// По нормативу (нет ИПУ) - 0, По прибору - 1, По среднему - 2, По нормативу (есть ИПУ) - 3
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public int Name { get; set; }
    }
}
