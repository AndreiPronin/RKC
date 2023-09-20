using BE.JobManager;
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
        public static DataTable CreateExcelReceiptNotSend(List<ReceiptNotSend> ReceiptNotSend)
        {

            DataTable dt = new DataTable("Receipt");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Лицевой счет"), new DataColumn("Коментарий"), new DataColumn("Электронная почта") });


            foreach (var Items in ReceiptNotSend)
            {
                dt.Rows.Add(Items.FullLic,Items.Comment,Items.Email);
            }
            return dt;
        }
    }
}
