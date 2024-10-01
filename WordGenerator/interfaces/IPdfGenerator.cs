using BE.PersData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGenerator.interfaces
{
    public interface IPdfGenerate
    {
        PersDataDocumentLoad Generate(string LIC, DateTime date);
        string GenerateHtml(string LIC, DateTime date);
    }
}
