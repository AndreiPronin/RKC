using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPlusModule.Repository.Models
{
    /// <summary>
    /// Размерность прибора учёта
    /// </summary>
    [Table(name:nameof(Measure), Schema = "dic")]
    public class Measure
    {
        /// <summary>
        /// Идентификатор в словаре
        ///  <br/> <see cref="TPlusModule.Common.Enums.Measures"/> содержит коды
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Наименование размерности
        /// <br/> Кубический метр - 1, Гигакалория - 2, Киловатт в час - 3
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public int Name { get; set; }
    }
}
