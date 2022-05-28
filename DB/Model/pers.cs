using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name:"Pers",Schema ="dbo")]
    public class Pers
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
