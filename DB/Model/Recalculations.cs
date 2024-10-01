using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name:nameof(Recalculations),Schema = "dbo")]
    public class Recalculations
    {
        [Key]
        public int Id { get; set; }
        public int Cadr { get; set; }
        public int ShortLic { get; set; }
        public string FullLic { get; set; }
        public int Idn { get; set; }
        public int RecalculationReasonId { get; set; }
        public int ServiceId { get; set; }
        public decimal RecalculationValue { get; set; }
        public DateTime RecalculationBeginPeriod { get; set; }
        public DateTime RecalculationEndPeriod { get; set; }
        public string RecalculationOwner { get; set; }
        public string Comment { get; set; }
        public DateTime Period { get; set; }
        public RecalculationReason RecalculationReason { get; set; }
        public Service Service { get; set; }
    }
    [Table(name: nameof(RecalculationReason), Schema = "dbo")]
    public class RecalculationReason
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }
    [Table(name: nameof(Service), Schema = "dbo")]
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
