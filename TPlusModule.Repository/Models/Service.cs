using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPlusModule.Repository.Models
{
    /// <summary>
    /// Услуга
    /// </summary>
    [Table(name:(nameof(Service)), Schema = "dic")]
    public class Service
    {
        /// <summary>
        /// Идентификатор в словаре
        ///  <br/> <see cref="TPlusModule.Common.Enums.Services"/> содержит коды
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Наименование услуги
        /// <br/> Отопление - 2, Горячее водоснабжение (компонент ТЭ) - 3, Горячее водоснабжение (компонент ХВ) - 5
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public int Name { get; set; }
    }
}
