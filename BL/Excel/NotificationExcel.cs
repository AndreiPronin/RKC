using BE.JobManager;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Excel
{
    public static class  NotificationExcel
    {
        public static DataTable CreateExcelDuplicatePu(List<DuplicatePu> duplicatePu)
        {

            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("Лицевой счет"),
                                        new DataColumn("Тип ПУ") });


            foreach (var Items in duplicatePu)
            {
                dt.Rows.Add(Items.FULL_LIC, Items.TYPE_PU);
            }
            return dt;
        }
        public static DataTable CreateExcelDuplicatePers(List<DuplicatePers> DuplicatePers)
        {

            DataTable dt = new DataTable("Pers");
            dt.Columns.AddRange(new DataColumn[1] { new DataColumn("Лицевой счет")});


            foreach (var Items in DuplicatePers)
            {
                dt.Rows.Add(Items.FULL_LIC);
            }
            return dt;
        }
        public static DataTable CreateExcelReceiptNotSend(List<ReceiptSend> ReceiptNotSend)
        {

            DataTable dt = new DataTable("Receipt");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Лицевой счет"), new DataColumn("Коментарий"), new DataColumn("Электронная почта") });


            foreach (var Items in ReceiptNotSend)
            {
                dt.Rows.Add(Items.FullLic,Items.Comment,Items.Email);
            }
            return dt;
        }
        public static DataTable CreateExcelReceiptNotSend()
        {
            using (var dbContext = new ApplicationDbContext())
            {
               var NotSendReceipts = dbContext.NotSendReceipts.ToList();
                DataTable dt = new DataTable("Receipt");
                dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Лицевой счет"), new DataColumn("Электронная почта"), new DataColumn("Месяц")
                , new DataColumn("Дата отправки"), new DataColumn("Количество попыток"), new DataColumn("Комментарий")});


                foreach (var Items in NotSendReceipts)
                {
                    dt.Rows.Add(Items.Lic, Items.Email, Items.Month, Items.DateTimeSend, Items.NumberAttempts, Items.ErrorDescription);
                }
                return dt;
            }
            
        }
    }
}
