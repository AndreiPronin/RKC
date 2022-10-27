using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    [Table(name: "KVIT",Schema = "dbo")]
    public class KVIT
    {
        public string ul { get; set; }
        public string dom { get; set; }
        public string kw { get; set; }
        public string lic { get; set; }
        public string fio { get; set; }
        public string sobs { get; set; }
        public string kl { get; set; }
        public string kopl { get; set; }
        public string sted2 { get; set; }
        public string koled2 { get; set; }
        public string sn2 { get; set; }
        public string sr2 { get; set; }
        public string it2 { get; set; }
        public string sted3 { get; set; }
        public string koled3 { get; set; }
        public string sn3 { get; set; }
        public string sr3 { get; set; }
        public string it3 { get; set; }
        public string koled4 { get; set; }
        public string sn4 { get; set; }
        public string sr4 { get; set; }
        public string it4 { get; set; }
        public string sted5 { get; set; }
        public string koled5 { get; set; }
        public string sn5 { get; set; }
        public string sr5 { get; set; }
        public string it5 { get; set; }
        public string koled6 { get; set; }
        public string sn6 { get; set; }
        public string sr6 { get; set; }
        public string it6 { get; set; }
        public string sr15 { get; set; }
        public string it15 { get; set; }
        public string u1nachzku { get; set; }
        public string u1oplzku { get; set; }
        public string u1dolgzku { get; set; }
        public string u1nachpeny { get; set; }
        public string u1oplpeny { get; set; }
        public string u1dolgpeny { get; set; }
        public string ipuxv1_1 { get; set; }
        public string ipuxv1_2 { get; set; }
        public string ipuxv2_1 { get; set; }
        public string ipuxv2_2 { get; set; }
        public string dpuotrasx { get; set; }
        public string dpuotsum { get; set; }
        public string dpuotnez { get; set; }
        public string dpuotnach { get; set; }
        public string dpugvsum { get; set; }
        public string dpugvnez { get; set; }
        public string dpugvnach { get; set; }
        public string dpuxvrasx { get; set; }
        public string dpuxvsum { get; set; }
        public string dpuxvnez { get; set; }
        public string dpuxvnach { get; set; }
        public string dpuelrasx { get; set; }
        public string dpuelnez { get; set; }
        public string dpukanach { get; set; }
        public string dpukasobs { get; set; }
        public string els { get; set; }
        public string igku { get; set; }
        public string ipuxv3_1 { get; set; }
        public string ipuxv3_2 { get; set; }
        public string ipuxv4_1 { get; set; }
        public string ipuxv4_2 { get; set; }
        public string ipuot1_1 { get; set; }
        public string ipuot1_2 { get; set; }
        public string ipuot2_1 { get; set; }
        public string ipuot2_2 { get; set; }
        public string ipuot3_1 { get; set; }
        public string ipuot3_2 { get; set; }
        public string s_oi { get; set; }
        public string s_notp { get; set; }
        public string ngvs1 { get; set; }
        public string ngvs2 { get; set; }
        public string ngvs3 { get; set; }
        public string ngvs4 { get; set; }
        public string notp1 { get; set; }
        public string notp2 { get; set; }
        public string notp3 { get; set; }
        public string dgvs1 { get; set; }
        public string dgvs2 { get; set; }
        public string dgvs3 { get; set; }
        public string dgvs4 { get; set; }
        public string dotp1 { get; set; }
        public string dotp2 { get; set; }
        public string dotp3 { get; set; }
        [Key]
        public DateTime? period { get; set; }
        public int id { get; set; }
    }


}
