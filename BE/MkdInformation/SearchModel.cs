using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.MkdInformation
{
    public class SearchModel
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string Building { get; set; }
    }
}
