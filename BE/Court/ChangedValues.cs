using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Court
{
    public class ChangedValues
    {
        public List<Values> Values { get; set; }
        public string User { get; set; }
        public DateTime? DateChanged { get; set; }
        public ChangedValues()
        {
            Values = new List<Values>();
        }
    }
    public class Values
    {
        public string NameValue { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

    }
}
