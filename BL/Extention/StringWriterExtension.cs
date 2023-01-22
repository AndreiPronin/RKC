using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Extention
{
    public static class StringWriterExtension
    {
        public static string ConvertLicToSubLic(this string s, string FullLic)
        {
            var SubLic = FullLic.Substring(3, 6);
            if (SubLic.StartsWith("0"))
            {
                SubLic = SubLic.Substring(1, 5);
                if (SubLic.StartsWith("0"))
                {
                    SubLic = SubLic.Substring(1, 4);
                    if (SubLic.StartsWith("0"))
                    {
                        SubLic = SubLic.Substring(1, 3);
                    }
                }
            }
            return SubLic;
        }
    }
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
