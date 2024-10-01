using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court.DictiomaryModel
{
    /// <summary>
    /// Нименование справочника
    /// </summary>
    [Table(nameof(CourtNameDictionary), Schema = "dic")]
    public class CourtNameDictionary
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<CourtValueDictionary> CourtValueDictionaries { get; set; }
    }
    /// <summary>
    /// Значение справочника
    /// </summary>
    [Table(nameof(CourtValueDictionary), Schema = "dic")]
    public class CourtValueDictionary
    {
        [Key]
        public int Id { get; set; }
        public int CourtNameDictionaryId { get; set; }
        public string Name { get; set; }
        public CourtNameDictionary CourtNameDictionary { get; set; }
    }
}
