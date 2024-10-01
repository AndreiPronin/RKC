using ExcelDna.Integration;
using System.Linq;
using System.IO;
using System;
using System.Collections.Generic;
using DB.DataBase;
using System.Text;

namespace Excels
{
    public class Formuls
    {
        [ExcelFunction(Description = "Поиск Проживающих по лицевому счету")]
        public static string GetFioByLic([ExcelArgument("Ввидеите лик")] string Lic)
        {
            StringBuilder result = new StringBuilder();
            using(var appDb = new ApplicationDbContext())
            {
                var Pers = appDb.PersData.Where(x=>x.Lic == Lic && x.IsDelete != true).ToList();
                foreach(var Item in Pers)
                {
                    result.AppendLine($"ФИО: {Item.LastName} {Item.LastName} {Item.MiddleName} \r\n");
                    result.Append($"Площадь: {Item.Square} \r\n");
                    result.Append($"Количество зарегестрированных: {Item.NumberOfPersons} \r\n");
                }
                var tt = result.ToString();
                return result.ToString();
            }
        }
      
    }
}
