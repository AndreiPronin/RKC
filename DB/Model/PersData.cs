using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "PersData", Schema = "dbo")]
    public class PersData
    {
        [Key]
        public int idPersData { get; set; }
        public string Lic { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; } = "";
        public string PassportSerial { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public string PassportIssued { get; set; } = "";
        public DateTime? PassportDate { get; set; }
        public string Tel1 { get; set; }
        public string Comment1 { get; set; } = "";
        public string Tel2 { get; set; }
        public string Comment2 { get; set; } = "";
        public string Email { get; set; } = "";
        public string Comment { get; set; } = "";
        public string UserName { get; set; } = "";
        public DateTime? DateAdd { get; set; }
        public string RoomType { get; set; } = "";
        public bool? Main { get; set; }
        public bool? IsDelete { get; set; }
        public string SnilsNumber { get; set; } = "";
        public string Inn { get; set; } = "";
        public int? NumberOfPersons { get; set; }
        public double? Square { get; set; }
        public string StateLic { get; set; } = "";
        public string SendingElectronicReceipt { get; set; }
        public DateTime? DateEdit { get; set; }
        public virtual ICollection<PersDataDocument> PersDataDocument { get; set; }

    }
    [Table(name: "PersDataDocument", Schema = "dbo")]
    public class PersDataDocument
    {
        public int id { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
        public int idPersData { get; set; }
        public PersData PersData { get; set; }
    }

    [Table(name: "LogsPersData", Schema ="dbo")]
    public class LogsPersData
    {
        public int Id { get; set; }
        public int? idPersData { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public DateTime? DateTime { get; set; }
    }


}
