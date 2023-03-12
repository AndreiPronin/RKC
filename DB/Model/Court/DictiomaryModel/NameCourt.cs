using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court.DictiomaryModel
{
    /// <summary>
    /// Справочник 1 - Наименование суда
    /// </summary>
    [Table(nameof(NameCourt), Schema = "dic")]
    public class NameCourt
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
