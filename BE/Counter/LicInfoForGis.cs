using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Counter
{
    public class LicInfoForGis
    {
        public string AccountNumber { get; set; }
        public string Els {  get; set; }
        public string Igku { get; set; }
        public string AccountGUID { get; set; }
        public string UnifiedAccountNumber { get; set; }
        public string IsClosed { get; set; }
        public decimal? TotalSquare { get; set; }
        public decimal? NumberOfPersons { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Patronymic { get; set; }
    }
}
