using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    [Table(name:nameof(CourtWorkRequisites),Schema = "dbo")]
    public class CourtWorkRequisites
    {
        [Key]
        public int Id { get; set; }
        public int CourtGeneralInformId { get; set; }
        public DateTime Date { get; set; }
        public string Number { get; set; }
        public string Suma { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
