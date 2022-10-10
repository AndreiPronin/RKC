using BE.JobManager;
using BL.Excel;
using ClosedXML.Excel;
using DB.Model;
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BL.Notification
{
    public interface INotificationMail
    {
        void SendEmailAsyncDublicatePu(List<DuplicatePu> DuplicatePu);
        void SendEmailAsyncDublicatePers(List<DuplicatePers> DuplicatePu);
        void SendMailReceipt(string FullLic, string Mail);
        void SendEmailReceiptNotSend(List<ReceiptNotSend> ReceiptNotSend);
        void Error(Exception ex, string message = "");
    }
    public class NotificationMail : INotificationMail
    {
        public void SendEmailAsyncDublicatePu(List<DuplicatePu> DuplicatePu)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(NotificationExcel.CreateExcelDuplicatePu(DuplicatePu));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var writer = new StreamWriter(stream);
                        writer.Flush();
                    stream.Position = 0;
                    var File = new System.Net.Mail.Attachment(stream, "Дубликаты ПУ.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    SendMail(File);
                }
            }
            
        }
        public void SendMailReceipt(string FullLic,string Mail)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = ConfigurationManager.AppSettings["mail:host:T+"];
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mail:login:T+"], ConfigurationManager.AppSettings["mail:pass:T+"]);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["mail:from:T+"]);
            mailMessage.Attachments.Add(new System.Net.Mail.Attachment($@"{AppDomain.CurrentDomain.BaseDirectory}Template\Квитанция {FullLic} {DateTime.Now.Month-1}.pdf"));
            mailMessage.To.Add(new MailAddress(Mail));
            mailMessage.Subject = "Автоматическое отправление эл.квитанций";
            smtpClient.Send(mailMessage);
        }
        public void SendEmailAsyncDublicatePers(List<DuplicatePers> DuplicatePers)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(NotificationExcel.CreateExcelDuplicatePers(DuplicatePers));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var writer = new StreamWriter(stream);
                        writer.Flush();
                    stream.Position = 0;
                    var File = new System.Net.Mail.Attachment(stream, "Дубликаты Персы главные.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    SendMail(File);
                }
            }
        }
        public void SendEmailReceiptNotSend(List<ReceiptNotSend> ReceiptNotSend)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(NotificationExcel.CreateExcelReceiptNotSend(ReceiptNotSend));
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var writer = new StreamWriter(stream);
                    writer.Flush();
                    stream.Position = 0;
                    var File = new System.Net.Mail.Attachment(stream, "Не отправленная почта.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    SendMail(File);
                }
            }
        }
        public void Error(Exception ex,string message = "")
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = ConfigurationManager.AppSettings["mail:host:monitor"];
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mail:login:monitor"], ConfigurationManager.AppSettings["mail:pass:monitor"]);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["mail:from:monitor"]);
                mailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["mail:to:monitor"]));
                mailMessage.Subject = "Ошибка в работе приложения";
                if(ex == null)
                {
                    mailMessage.Body = message;
                }
                else
                {
                    mailMessage.Body = ex.Message;
                }
                smtpClient.Send(mailMessage);
            }
            catch (Exception e) { }
        }
        private void SendMail(Attachment File)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = ConfigurationManager.AppSettings["mail:host:monitor"];
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mail:login:monitor"], ConfigurationManager.AppSettings["mail:pass:monitor"]);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["mail:from:monitor"]);
                mailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["mail:to:monitor"]));
                mailMessage.Attachments.Add(File);
                mailMessage.Subject = "Результат работы Job";
                smtpClient.Send(mailMessage);
            }catch(Exception e) { }
        }
    }
}
