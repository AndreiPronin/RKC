using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordGenerator.Enums;
using WordGenerator.interfaces;

namespace WordGenerator
{
    public interface IPdfFactory
    {
        IPdfGenerate CreatePdf(PdfType PdfType);
    }
    public class ReceiptFactory : IPdfFactory
    {
        public IPdfGenerate CreatePdf(PdfType Type)
        {
            IPdfGenerate pdf = null;

            switch (Type)
            {
                case PdfType.Personal:
                    pdf = new ReceiptPersonal();
                    break;
                case PdfType.PersonalCabinter:
                    pdf = new ReceiptPersonalLk();
                    break;
                case PdfType.Dpu:
                    pdf = new ReceiptDPU();
                    break;
                case PdfType.NewDpu:
                    pdf = new ReceiptNewDPU();
                    break;
            }
            return pdf;
        }
    }
}
