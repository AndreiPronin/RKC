using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelsReader.Services
{
    public class FilesServies
    {
        public List<string> GetAllExcelFile(string path)
        {
            var Files = new List<string>();
            var File = Directory.GetFiles(path);
            foreach (var Item in File)
            {
                if (Item.Contains(".xlsx"))
                {
                    Files.Add(Item);
                }
            }

            return Files;
        }
    }
}
