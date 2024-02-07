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
using Quartz;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using WordGenerator.interfaces;
using BE.Actions;
using BL.Loggers;
using BE.Service;
using System.IO;
using DocumentFormat.OpenXml.Bibliography;

namespace BL.Jobs
{
    public interface IJobManager
    {
        void CheckDublicatePu();
        void  CheckDublicatePuNumber();
        void CheckDublicatePers();
        void SendReceipt(string FullLic = "");
        void SendReceiptDpu(string FullLic = "");
    }
    public class JobManager : IJobManager
    {
        private readonly INotificationMail _notificationMail;
        private readonly IPdfFactory _pdfFactory;
        public static object LockSendingPersonalReceipt = new object();
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
                    var Result = db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.GVS1).ToList();
                    Result.AddRange(db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.GVS2).ToList());
                    Result.AddRange(db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.GVS3).ToList());
                    Result.AddRange(db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.GVS4).ToList());
                    Result.AddRange(db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.OTP1).ToList());
                    Result.AddRange(db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.OTP2).ToList());
                    Result.AddRange(db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.OTP3).ToList());
                    Result.AddRange(db.Database.SqlQuery<Duplicate>(QueryCheckDublicate.OTP4).ToList());
                    if (Result.Count() > 0)
                        _notificationMail.SendEmailAsyncDublicatePu(Result);
                }catch(Exception ex)
                {
                    _notificationMail.Error(ex);
                }
            }
        }
        public void CheckDublicatePuNumber()
        {
            using (var db = new DbTPlus())
            {
                try
                {
                    var dublicate = new List<Duplicate>();
                    var result = db.IPU_COUNTERS.Where(x=>x.CLOSE_ != true).ToList();
                    var groupResult = result.GroupBy(x => x.FACTORY_NUMBER_PU).ToList();
                    foreach (var group in groupResult)
                    {
                        if(group.Count()>1)
                            foreach (var item in group)  
                                dublicate.Add(new Duplicate { FULL_LIC = item.FULL_LIC, TYPE_PU = item.TYPE_PU, FACTORY_NUMBER_PU = item.FACTORY_NUMBER_PU });
                    }
                    
                    if (dublicate.Count() > 0)
                        _notificationMail.SendEmailAsyncDublicatePu(dublicate);
                }
                catch (Exception ex)
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
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    //блокировка от двойного нажатия кнопки
                    var locks = db.Flags.FirstOrDefault(x => x.Id == (int)EnumFlags.SendingPersonalReceipt);
                    if (locks != null && locks.Flag != true)
                    {
                        locks.Flag = true;
                        db.SaveChanges();
                        List<ReceiptSend> receiptSend = new List<ReceiptSend>();
                        List<PersData> persData;
                        if (string.IsNullOrEmpty(FullLic))
                        {
                            persData = db.PersData.Where(x => x.Main == true && x.IsDelete != true && x.SendingElectronicReceipt.Contains("Да")).ToList();
                        }
                        else
                        {
                            persData = new List<PersData>();
                            var Lic = FullLic.Split(';');
                            for (int i = 0; i < Lic.Length; i++)
                                if (!string.IsNullOrEmpty(Lic[i].Trim()))
                                {
                                    var lic = Lic[i].Trim();
                                    persData.Add(db.PersData.FirstOrDefault(x => x.Main == true && x.Lic == lic && x.IsDelete != true && x.SendingElectronicReceipt.Contains("Да")));
                                }
                        }
                        var Recept = _pdfFactory.CreatePdf(PdfType.Personal);
                        foreach (var Items in persData)
                        {
                            try
                            {
                                if (Items != null)
                                {
                                    if (!File.Exists($@"{AppDomain.CurrentDomain.BaseDirectory}Template\Kvit\{DateTime.Now.AddMonths(-1):MMMM-yyyy}\Квитанция {FullLic} {DateTime.Now.AddMonths(-1).Month}.pdf"))
                                    {
                                        Recept.Generate(Items.Lic, DateTime.Now.AddMonths(-1));
                                        if (string.IsNullOrEmpty(Items.Email))
                                            throw new Exception("Пустой Email");
                                    }
                                    _notificationMail.SendMailReceipt(Items.Lic, Items.Email);
                                    receiptSend.Add(new ReceiptSend { FullLic = Items.Lic, Comment = "Отправлена квитанция", Email = Items.Email, DateTime = DateTime.Now, IsSend = true });
                                    try
                                    {
                                        saveReceiptSens(new List<ReceiptSend> { new ReceiptSend { FullLic = Items.Lic, Comment = "Отправлена квитанция", Email = Items.Email, DateTime = DateTime.Now, IsSend = true } }
                                        , (int)TypeReceipt.PersonalReceipt);
                                    }
                                    catch { }
                                }
                            }
                            catch (Exception ex)
                            {
                                if (Items != null)
                                {
                                    receiptSend.Add(new ReceiptSend { FullLic = Items.Lic, Comment = ex.Message, Email = Items.Email, DateTime = DateTime.Now, IsSend = false });
                                    try
                                    {
                                        saveReceiptSens(new List<ReceiptSend> { new ReceiptSend { FullLic = Items.Lic, Comment = ex.Message, Email = Items.Email, DateTime = DateTime.Now, IsSend = false } }
                                        , (int)TypeReceipt.PersonalReceipt);
                                    }
                                    catch { }
                                }
                            }
                            if (Items != null)
                                ShedulerLogger.WhriteToFile($"Идет работа сервиса. Отправка квитанции на лицевой счет {Items.Lic}");
                        }
                        if (receiptSend.Count() > 0)
                        {
                            _notificationMail.SendEmailReceiptNotSend(receiptSend);
                        }
                        //Разблокировка
                        locks = db.Flags.FirstOrDefault(x => x.Id == (int)EnumFlags.SendingPersonalReceipt);
                        locks.Flag = false;
                        db.SaveChanges();
                    }
                    else
                    {
                        List<ReceiptSend> receiptSend = new List<ReceiptSend>();
                        List<PersData> persData;
                        if (string.IsNullOrEmpty(FullLic))
                        {
                            persData = db.PersData.Where(x => x.Main == true && x.IsDelete != true && x.SendingElectronicReceipt.Contains("Да")).ToList();
                        }
                        else
                        {
                            persData = new List<PersData>();
                            var Lic = FullLic.Split(';');
                            for (int i = 0; i < Lic.Length; i++)
                                if (!string.IsNullOrEmpty(Lic[i].Trim()))
                                {
                                    var lic = Lic[i].Trim();
                                    persData.Add(db.PersData.FirstOrDefault(x => x.Main == true && x.Lic == lic && x.IsDelete != true && x.SendingElectronicReceipt.Contains("Да")));
                                }
                        }
                        addOrUpdateSendReceipt(persData, (int)TypeReceipt.PersonalReceipt);
                    }
                }
                catch(Exception ex)
                {
                    var locks = db.Flags.FirstOrDefault(x => x.Id == (int)EnumFlags.SendingPersonalReceipt);
                    locks.Flag = false;
                    db.SaveChanges();
                    _notificationMail.Error(ex);
                }
            }
        }

        public void SendReceiptDpu(string FullLic = "")
        {
            using (var db = new ApplicationDbContext())
            {
                List<ReceiptSend> receiptSend = new List<ReceiptSend>();
                List<DpuSendByEmailL> persData;
                if (string.IsNullOrEmpty(FullLic))
                {
#if DEBUG
                    persData = db.Database.SqlQuery<DpuSendByEmailL>($"select * from WEB_APP_Test.dbo.DpuSendByEmailLics('{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")}')").ToList();
#else
                    persData = db.Database.SqlQuery<DpuSendByEmailL>($"select * from WEB_APP.dbo.DpuSendByEmailLics('{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")}')").ToList();
#endif

                }
                else
                {
#if DEBUG
                    var allLic = db.Database.SqlQuery<DpuSendByEmailL>($"select * from WEB_APP_Test.dbo.DpuSendByEmailLics('{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")}')").ToList();
#else
                    var allLic = db.Database.SqlQuery<DpuSendByEmailL>($"select * from WEB_APP.dbo.DpuSendByEmailLics('{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01")}')").ToList();
#endif
                    persData = new List<DpuSendByEmailL>();
                    var Lic = FullLic.Split(';');
                    for (int i = 0; i < Lic.Length; i++)
                        if (!string.IsNullOrEmpty(Lic[i].Trim()))
                        {
                            var lic = Lic[i].Trim();
                            persData.Add(allLic.FirstOrDefault(x=>x.NewFullLic == lic));
                        }
                }
              
                foreach (var Items in persData)
                {
                    try
                    {
                        IPdfGenerate Recept = null;
                        if(Items.IsFirstReceipt == false) { 
                            Recept = _pdfFactory.CreatePdf(PdfType.Dpu);
                        }
                        else
                        {
                            Recept = _pdfFactory.CreatePdf(PdfType.NewDpu);
                        }
                        Recept.Generate(Items.NewFullLic, DateTime.Now.AddMonths(-1));
                        if (string.IsNullOrEmpty(Items.Email))
                            throw new Exception("Пустой Email");
                        _notificationMail.SendMailReceiptDpu(Items.NewFullLic, Items.Email);
                        receiptSend.Add(new ReceiptSend { FullLic = Items.NewFullLic, Comment = "Отправлена квитанция", Email = Items.Email });
                    }
                    catch (Exception ex)
                    {
                        receiptSend.Add(new ReceiptSend { FullLic = Items.NewFullLic, Comment = ex.Message, Email = Items.Email });
                    }
                }
                if (receiptSend.Count() > 0)
                    _notificationMail.SendEmailReceiptNotSendDpu(receiptSend);
            }
        }
        private void saveReceiptSens(List<ReceiptSend> receiptSend,int TypeReceipt)
        {
            using (var context = new ApplicationDbContext())
            {
                var month = DateTime.Now.Month;
                foreach (var Item in receiptSend)
                {
                    var receiptsends = context.NotSendReceipts.Where(x => x.Lic == Item.FullLic && x.Month == month).ToList();
                    if (receiptsends.Count() == 0)
                    {
                        context.NotSendReceipts.Add(new NotSendReceipt
                        {
                            Lic = Item.FullLic,
                            Email = Item.Email,
                            DateTimeSend = Item.DateTime,
                            IsSend = Item.IsSend,
                            ErrorDescription = Item.Comment,
                            Month = month,
                            NumberAttempts = 1,
                            TypeReceipt = TypeReceipt
                        });
                    }
                    else
                    {
                        foreach (var item in receiptsends)
                        {
                            item.ErrorDescription = Item.Comment;
                            item.Email = Item.Email;
                            item.IsSend = Item.IsSend;
                            item.NumberAttempts = item.NumberAttempts + 1;
                            item.DateTimeSend = Item.DateTime;
                        }
                        context.SaveChanges();
                    }

                    context.SaveChanges();
                }
            }
        }
        private void addOrUpdateSendReceipt(List<PersData> persDatas, int TypeReceipt)
        {
            if (persDatas.Count() > 0)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var month = DateTime.Now.Month;
                    foreach (var item in persDatas)
                    {
                        if (item == null) continue;

                        var receipt = dbContext.NotSendReceipts.Where(x => x.Lic == item.Lic).FirstOrDefault();
                        if (receipt != null)
                        {
                            receipt.IsSend = false;
                        }
                        else
                        {
                            dbContext.NotSendReceipts.Add(new NotSendReceipt
                            {
                                Lic = item.Lic,
                                Email = item.Email,
                                DateTimeSend = DateTime.Now,
                                IsSend = false,
                                ErrorDescription = "",
                                Month = month,
                                NumberAttempts = 1,
                                TypeReceipt = TypeReceipt
                            });
                        }
                        dbContext.SaveChanges();
                    }
                }
            }
        }
    }
}
