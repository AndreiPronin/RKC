using BE.JobManager;
using BL.Notification;
using BL.Query;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using WordGenerator;

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
        public JobManager(INotificationMail notificationMail)
        {
            _notificationMail = notificationMail;
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
                    persData = db.PersData.Where(x => x.Main == true && x.SendingElectronicReceipt == "Да").ToList();
                }
                else
                {
                    persData = db.PersData.Where(x => x.Main == true && x.Lic == FullLic).ToList();
                }
                foreach(var Items in persData)
                {
                    try
                    {
                        GenerateFileHelpCalculation.Generate(Items.Lic, DateTime.Now.AddMonths(-1));
                        if (string.IsNullOrEmpty(Items.Email))
                            throw new Exception("Пустой Email");
                        _notificationMail.SendMailReceipt(Items.Lic, Items.Email);
                        receiptNotSend.Add(new ReceiptNotSend { FullLic = Items.Lic, Comment = "Отправлена квитанция" });
                    }
                    catch(Exception ex)
                    {
                        receiptNotSend.Add(new ReceiptNotSend { FullLic = Items.Lic, Comment = ex.Message });
                        //break;
                    }
                }
                if (receiptNotSend.Count() > 0)
                    _notificationMail.SendEmailReceiptNotSend(receiptNotSend);
            }
        }
    }
}
