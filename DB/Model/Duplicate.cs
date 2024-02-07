using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Model
{
    public class Duplicate
    {
        public string FULL_LIC { get; set; }
        public string TYPE_PU { get; set; }
        public string FACTORY_NUMBER_PU { get; set; }
    }

    public class DuplicatePers
    {
        public string FULL_LIC { get; set; }
    }
}
