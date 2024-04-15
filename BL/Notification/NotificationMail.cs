using BE.Counter;
using BE.JobManager;
using BL.Excel;
using ClosedXML.Excel;
using DB.DataBase;
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
        void SendEmailAsyncDublicatePu(List<Duplicate> DuplicatePu);
        void SendEmailAsyncDublicatePers(List<DuplicatePers> DuplicatePu);
        void SendMailReceipt(string FullLic, string Mail);
        void SendMailReceiptDpu(string FullLic, string Mail);
        void SendEmailReceiptNotSend(List<ReceiptSend> ReceiptNotSend);
        void SendEmailReceiptNotSendDpu(List<ReceiptSend> ReceiptNotSend);
        void Error(Exception ex, string message = "");
        void SendEmailResultLoadCourt(System.Data.DataTable resultLoad, string FileName);
        void SendMailReceipt(List<Attachment> File, string To);
    }
    public class NotificationMail : INotificationMail
    {
        public void SendEmailAsyncDublicatePu(List<Duplicate> DuplicatePu)
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
            using (var db = new DbLIC())
            {
                var LicIsClose = db.ALL_LICS.Select(x=>new {x.ZAK, x.F4ENUMELS}).FirstOrDefault(x => x.F4ENUMELS == FullLic).ZAK;
                if (LicIsClose != null)
                {
                    throw new Exception("Лицевой счет закрыт, отправка квитанции не возможна");
                }
            }
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = ConfigurationManager.AppSettings["mail:host:T+"];
            smtpClient.EnableSsl = true;
            //smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 25;
            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mail:login:T+"], ConfigurationManager.AppSettings["mail:pass:T+"]);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["mail:from:T+"]);
            mailMessage.Attachments.Add(new System.Net.Mail.Attachment($@"{AppDomain.CurrentDomain.BaseDirectory}Template\Kvit\{DateTime.Now.AddMonths(-1):MMMM-yyyy}\Квитанция {FullLic} {DateTime.Now.AddMonths(-1).Month}.pdf"));
            mailMessage.To.Add(new MailAddress(Mail));
            mailMessage.Subject = "Автоматическое отправление эл.квитанций";
            smtpClient.Send(mailMessage);
        }
        public void SendMailReceiptDpu(string FullLic, string Mail)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = ConfigurationManager.AppSettings["mail:host:T+"];
            smtpClient.EnableSsl = true;
            //smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 25;
            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mail:login:T+"], ConfigurationManager.AppSettings["mail:pass:T+"]);
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["mail:from:T+"]);
            mailMessage.Attachments.Add(new System.Net.Mail.Attachment($@"{AppDomain.CurrentDomain.BaseDirectory}Template\KvitDPU\{DateTime.Now.AddMonths(-1):MMMM-yyyy}\Квитанция ДПУ {FullLic} {DateTime.Now.AddMonths(-1).Month}.pdf"));
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
        public void SendEmailReceiptNotSend(List<ReceiptSend> ReceiptNotSend)
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
                    var File = new System.Net.Mail.Attachment(stream, "Отчет отправки почты.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    SendMail(File);
                }
            }
        }
        public void SendEmailReceiptNotSendDpu(List<ReceiptSend> ReceiptNotSend)
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
                    var File = new System.Net.Mail.Attachment(stream, "Отчет отправки почты ДПУ.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
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
        public void SendMailReceipt(List<Attachment> File, string To)
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
                mailMessage.To.Add(new MailAddress(To));
                foreach (Attachment attachment in File)
                {
                    mailMessage.Attachments.Add(attachment);
                }
                if(File.Count() > 0)
                {
                    mailMessage.Subject = "Квитанция";
                    smtpClient.Send(mailMessage);
                }
                
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
        public void SendEmailResultLoadCourt(System.Data.DataTable resultLoad,string FileName)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(resultLoad);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    var writer = new StreamWriter(stream);
                    writer.Flush();
                    stream.Position = 0;
                    var File = new System.Net.Mail.Attachment(stream, FileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    SendMailCourt(File);
                }
            }
        }
        private void SendMailCourt(Attachment File)
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
                mailMessage.Subject = "Результат работы загрузки судебных дел";
                smtpClient.Send(mailMessage);
            }
            catch (Exception e) { }
        }
    }
}
