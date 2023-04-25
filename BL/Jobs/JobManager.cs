using BE.JobManager;
using BL.Notification;
using DB.Query;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using WordGenerator;
using WordGenerator.Enums;

namespace BL.Jobs
{
    public interface IJobManager
    {
        void CheckDublicatePu();
        void CheckDublicatePers();
        void SendReceipt(string FullLic = "");
    }
    public class JobManager : IJobManager
    {
        private readonly INotificationMail _notificationMail;
        private readonly IPdfFactory _pdfFactory;
        public JobManager(INotificationMail notificationMail, IPdfFactory pdfFactory)
        {
            _notificationMail = notificationMail;
            _pdfFactory = pdfFactory;
        }
        public void CheckDublicatePu()
        {
            using (var db = new DbTPlus()) 
            {
                try
                {
                    var Result = db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.GVS1).ToList();
                    Result.AddRange(db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.GVS2).ToList());
                    Result.AddRange(db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.GVS3).ToList());
                    Result.AddRange(db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.GVS4).ToList());
                    Result.AddRange(db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.OTP1).ToList());
                    Result.AddRange(db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.OTP2).ToList());
                    Result.AddRange(db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.OTP3).ToList());
                    Result.AddRange(db.Database.SqlQuery<DuplicatePu>(QueryCheckDublicate.OTP4).ToList());
                    if (Result.Count() > 0)
                        _notificationMail.SendEmailAsyncDublicatePu(Result);
                }catch(Exception ex)
                {
                    _notificationMail.Error(ex);
                }
            }
        }
        public void CheckDublicatePers()
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var Result = db.Database.SqlQuery<DuplicatePers>(QueryPers.GtPersDublicate).ToList();
                    if (Result.Count() > 0)
                        _notificationMail.SendEmailAsyncDublicatePers(Result);
                }catch(Exception ex)
                {
                    _notificationMail.Error(ex);
                }
            }
        }
        public void SendReceipt(string FullLic = "")
        {
            using(var db = new ApplicationDbContext())
            {
                List<ReceiptNotSend> receiptNotSend = new List<ReceiptNotSend>();
                List<PersData> persData;
                if(string.IsNullOrEmpty(FullLic))
                { 
                    persData = db.PersData.Where(x => x.Main == true && x.SendingElectronicReceipt.Contains("Да")).ToList();
                }
                else
                {
                    persData = new List<PersData>();
                    var Lic = FullLic.Split(';');
                    for (int i = 0; i < Lic.Length; i++)
                        if (!string.IsNullOrEmpty(Lic[i].Trim()))
                        {
                            var lic = Lic[i].Trim();
                            persData.Add(db.PersData.FirstOrDefault(x => x.Main == true && x.Lic == lic));
                        }
                }
                var Recept = _pdfFactory.CreatePdf(PdfType.Personal);
                foreach (var Items in persData)
                {
                    try
                    {
                        Recept.Generate(Items.Lic, DateTime.Now.AddMonths(-1));
                        if (string.IsNullOrEmpty(Items.Email))
                            throw new Exception("Пустой Email");
                        _notificationMail.SendMailReceipt(Items.Lic, Items.Email);
                        receiptNotSend.Add(new ReceiptNotSend { FullLic = Items.Lic, Comment = "Отправлена квитанция" });
                    }
                    catch(Exception ex)
                    {
                        receiptNotSend.Add(new ReceiptNotSend { FullLic = Items.Lic, Comment = ex.Message });
                    }
                }
                if (receiptNotSend.Count() > 0)
                    _notificationMail.SendEmailReceiptNotSend(receiptNotSend);
            }
        }
    }
}
