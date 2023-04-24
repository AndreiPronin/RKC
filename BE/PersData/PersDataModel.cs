using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.PersData
{
    public class PersDataModel
    {
        public int idPersData { get; set; }
        public string Lic { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PassportSerial { get; set; }
        public string PassportNumber { get; set; }
        public string PassportIssued { get; set; }
        public DateTime? PassportDate { get; set; }
        public string Tel1 { get; set; }
        public string Comment1 { get; set; }
        public string Tel2 { get; set; }
        public string Comment2 { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime? DateAdd { get; set; }
        public string RoomType { get; set; }
        public bool? Main { get; set; }
        public bool? IsDelete { get; set; } = false;
        public string SnilsNumber { get; set; }
        public string Inn { get; set; }
        public int? NumberOfPersons { get; set; }
        public double? Square { get; set; }
        public string StateLic { get; set; }
        public string SendingElectronicReceipt { get; set; }
        public string FlatTypeId { get; set; }
        public string FlatType { get; set; }
    }
}
