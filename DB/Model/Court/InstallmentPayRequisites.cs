using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model.Court
{
    [Table(name: nameof(InstallmentPayRequisites), Schema = "dbo")]
    public class InstallmentPayRequisites
    {
        [Key]
        public int Id { get; set; }
        public int CourtGeneralInformId { get; set; }
        public DateTime Date { get; set; }
        public string Suma { get; set; }
        public CourtGeneralInformation CourtGeneralInformation { get; set; }
    }
}
