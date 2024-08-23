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
        public string UnifiedAccountNumber { get; set; }
        public string IsClosed { get; set; }
        public decimal? TotalSquare { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Patronymic { get; set; }
    }
}
