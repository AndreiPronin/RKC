using BE.PersData;
using BL.Notification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WordGenerator;
using WordGenerator.Enums;

namespace BL.ApiServices.PersonalData
{
    public interface IApiPersonalData
    {
        Task SendReceipt(string FullLic, string EmailTo, DateTime DateStart, DateTime DateEnd);
    }
    public class ApiPersonalData : IApiPersonalData
    {
        private readonly INotificationMail _notificationMail;
        private readonly IPdfFactory _pdfFactory;
        public ApiPersonalData(INotificationMail notificationMail, IPdfFactory pdfFactory)
        {
            _notificationMail = notificationMail;
            _pdfFactory = pdfFactory;
        }
        public async Task SendReceipt(string FullLic, string EmailTo ,DateTime DateStart, DateTime DateEnd)
        {
            var srcDate = DateStart;
            if (DateStart < DateEnd)
            {
                DateStart = DateEnd;
                DateEnd = srcDate;
            }


            List<PersDataDocumentLoad> persData = new List<PersDataDocumentLoad>();
            while (DateStart >= DateEnd)
            {
                try
                {
                    persData.Add(_pdfFactory.CreatePdf(PdfType.Personal).Generate(FullLic, DateEnd));
                }
                catch (Exception ex)
                {

                }
                DateEnd = DateEnd.AddMonths(1);
            }
            List<Attachment> attachments = new List<Attachment>();
            foreach(var item in persData)
            {
                if (item.FileBytes.Length > 0)
                {
                    Stream stream = new MemoryStream(item.FileBytes);
                    attachments.Add(new Attachment(stream, item.FileName, System.Net.Mime.MediaTypeNames.Application.Pdf));
                }
                    
            }
            _notificationMail.SendMailReceipt(attachments, EmailTo);
           
        }
    }
}
