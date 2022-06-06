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
        public bool? IsDelete { get; set; }
        public string SnilsNumber { get; set; }
        public string Inn { get; set; }
        public int? NumberOfPersons { get; set; }
        public double? Square { get; set; }
        public string StateLic { get; set; }
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

    [Table(name:"Pers",Schema ="dbo")]
    public class PersDatas
    {
        public int id { get; set; }
        public string lic { get; set; }
        public string nom { get; set; }
        public string udal { get; set; }
        public string fam { get; set; }
        public string im { get; set; }
        public string ot { get; set; }
        public string index_ { get; set; }
        public DateTime? data_rogd { get; set; }
        public string mesto_rogd { get; set; }
        public string pasp_ser { get; set; }
        public string pasp_nom { get; set; }
        public string pasp_kem { get; set; }
        public DateTime? pasp_date { get; set; }
        public string tel1 { get; set; }
        public string kom1 { get; set; }
        public string tel2 { get; set; }
        public string kom2 { get; set; }
        public string email { get; set; }
        public string komment { get; set; }
        public string podp { get; set; }
        public DateTime? date_edit { get; set; }
        public string sobstv { get; set; }
        public string nal_doc { get; set; }
        public DateTime? update_datetime { get; set; }
        public bool? main { get; set; }
        public int? id_nom { get; set; }
        public int? id_user { get; set; }
        public bool? delete { get; set; }
        public bool? doc_available { get; set; }
        public string SNILS_nom { get; set; }
        public string inn { get; set; }
        public int? kl { get; set; }
        public double? sobs { get; set; }
        public int? lic_k { get; set; }
        public string zak { get; set; }
    }


}
