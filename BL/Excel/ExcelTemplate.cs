using AppCache;
using BE.Counter;
using BL.Counters;
using BL.Helper;
using ClosedXML.Excel;
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
    public class ExcelTemplate
    {
        public DataTable LoadExcelPUProperty()
        {
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[13] { new DataColumn("Лицевой счет"),
new DataColumn("Тип ПУ"),new DataColumn("Дата  акта ввода"),new DataColumn("Номер ПУ"), new DataColumn("Модель ПУ")
            , new DataColumn("Дата поверки"), new DataColumn("дата следующей поверки"), new DataColumn("Тип пломбы 1"), new DataColumn("Номер пломбы 1"), new DataColumn("Тип пломбы 2")
            , new DataColumn("Номер пломбы 2"), new DataColumn("Конт. Обход"), new DataColumn("Показания контролёра")});
            return dt;
        }
        public DataTable LoadExcelPersData()
        {
            DataTable dt = new DataTable("Counter");
            dt.Columns.AddRange(new DataColumn[13] { new DataColumn("Лицевой счет"),
new DataColumn("Тип ПУ"),new DataColumn("Дата  акта ввода"),new DataColumn("Номер ПУ"), new DataColumn("Модель ПУ")
            , new DataColumn("Дата поверки"), new DataColumn("дата следующей поверки"), new DataColumn("Тип пломбы 1"), new DataColumn("Номер пломбы 1"), new DataColumn("Тип пломбы 2")
            , new DataColumn("Номер пломбы 2"), new DataColumn("Конт. Обход"), new DataColumn("Показания контролёра")});
            return dt;
        }
    }
}
