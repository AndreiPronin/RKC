using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public class CourtGeneralInformation
    {
        public int Id { get; set; }
        public string IdClient { get; set; }
        public string IdВuty { get; set; }
        public string Lic { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Home { get; set; }
        public string Flat { get; set; }
        public string FioDuty { get; set; }
        public string Floor { get; set; }
        public string ShareOfOwnership { get; set; }
        public string DateBirthday { get; set; }
        public string PasportData { get; set; }
        public string Inn { get; set; }
        public string Snils { get; set; }
        public string Pensioner { get; set; }
        public string ExclusionMailing { get; set; }
        public string ReasonsExclusionMailing  { get; set; }
        public string ExclusionCourtWork { get; set; }
        public string ReasonsCourtWork { get; set; }
        public string Comment { get; set; }
        public virtual CourtWork CourtWork { get; set; }
        public ICollection<CourtDocumentScans> CourtDocumentScans { get; set; }
        //public ExecutionFSSP ExecutionFSSP { get; set; }
        public virtual CourtExecutionInPF CourtExecutionInPF { get; set; }
        public virtual CourtInstallmentPlan CourtInstallmentPlan { get; set; }
        public virtual CourtBankruptcy CourtBankruptcy { get; set; }
        public virtual CourtLitigationWork CourtLitigationWork { get; set; }
        public CourtStateDuty CourtStateDuty { get; set; }
        public CourtWriteOff CourtWriteOff { get; set; }
    }
}
