using BE.Counter;
using BL.Counters;
using BL.Helper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Service
{
    public interface IReadFileBank
    {
        string Read(byte[] file, Banks Bank);
    }
    public class ReadFileBank : Counter, IReadFileBank
    {
        public string path { get { return AppDomain.CurrentDomain.BaseDirectory + "BankFile\\" + DateTime.Now.Date.ToString().Replace(" 0:00:00", ""); } }
       
        public ReadFileBank(Ilogger ilogger, IGeneratorDescriptons generatorDescriptons) : base(ilogger, generatorDescriptons)
        {

        }
        public string Read(byte[] file, Banks Bank)
        {
            ReadDBF(file);
            return "";

        }
        private void ReadDBF(byte[] file)
        {

            if (file != null)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes(path + "\\kz_953_6315376946_KOM_010522.dbf", file);
            }
            OleDbConnection myConn = new OleDbConnection();
            myConn.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + path + "\\kz_953_6315376946_KOM_010522.dbf"; ;
            OleDbConnection con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+ path + "\\kz_953_6315376946_KOM_010522.dbf";
            con.Open();
            OleDbCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select * from \\vdpr1701.dbf";
            OleDbDataReader reader = cmd.ExecuteReader();
            myConn.Open();

           

        }
    }
}
