using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public interface IExcelCourt
    {
        Task<DataTable> ExcelsLoadCourt(XLWorkbook Excels);
    }
    public class ExcelCourt : IExcelCourt
    {
        public Task<DataTable> ExcelsLoadCourt(XLWorkbook Excels)
        {
            throw new Exception("Test");
        }
    }
}
