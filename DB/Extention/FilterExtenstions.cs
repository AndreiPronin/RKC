using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Extention
{
    public static class FilterExtenstions
    {
        public static IQueryable<IntegrationReadings> Filter(this DbSet<IntegrationReadings> integrationReadings)
        {
            var query = integrationReadings.Where(x => x.IsError == true);
            using (var context = new ApplicationDbContext())
            {
                var setting = context.Settings.Where(x => x.Key == "IntegrationReadings").FirstOrDefault();
                foreach(var Item in setting?.Value.Split(';'))
                {
                    var value = Item.Trim();
                    if (!string.IsNullOrEmpty(value))
                        query = query.Where(x=>!x.Description.Trim().Contains(value));
                }
                return query;
            }
        }
        public static IQueryable<NotSendReceipt> Filter(this DbSet<NotSendReceipt> notSendReceipts)
        {
            var query = notSendReceipts.Where(x => x.IsSend == false);
            using (var context = new ApplicationDbContext())
            {
                var setting = context.Settings.Where(x => x.Key == "NotSendReceipt").FirstOrDefault();
                foreach (var Item in setting.Value.Split(';'))
                {
                    if(!string.IsNullOrEmpty(Item))
                        query = query.Where(x => !x.ErrorDescription.Contains(Item));
                }
                return query;
            }
        }
    }
}
