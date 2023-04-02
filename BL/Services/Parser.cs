using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace BL.Services
{
    public class Parser
    {
        public static string PdfParser()
        {
            using (PdfReader reader = new PdfReader("D:/Пензенская-область,-Пенза,-КАЛИНИНА,-97А,-24-58-29-3004007-1049-SOPP_2022-10-08_19-25-20.pdf"))
            {
                StringBuilder text = new StringBuilder();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }

                return text.ToString();
            }

        }
    }
}
