using DB.Model.Court.DictiomaryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    /// <summary>
    /// Справочник 5 - Причины отзыва ИД с исполнения
    /// </summary>
    [Table(nameof(ReasonsRevokingIDExecution), Schema = "dic")]
    public class ReasonsRevokingIDExecution
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
