using BE.Counter;
using BE.Service;
using BL.Counters;
using BL.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Service
{
    public interface IReadFileBank
    {
        List<ReadFilesModel> Read(byte[] file, Banks Bank);
    }
    public class ReadFileBank : Counter, IReadFileBank
    {
        public string path { get { return AppDomain.CurrentDomain.BaseDirectory + "BankFile\\" + DateTime.Now.Date.ToString().Replace(" 0:00:00", ""); } }
       
        public ReadFileBank(Ilogger ilogger, IGeneratorDescriptons generatorDescriptons) : base(ilogger, generatorDescriptons)
        {

        }
        public List<ReadFilesModel> Read(byte[] file, Banks Bank)
        {
            switch (Bank)
            {
                case Banks.POCHTA:
                    return PochtaBank(file);
                case Banks.ConnectionLost:
                     ReadDBF(file);
                    break;
            }
            return new List<ReadFilesModel>();

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
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=dBASE IV"); // give your path directly 
            try
            {
                con.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("select * from kz_953_6315376946_KOM_010522.dbf", con); // update this query with your table name 
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                int i = ds.Tables[0].Rows.Count;
               // return true;
            }
            catch (Exception e)
            {
                var error = e.ToString();
                // check error details 
                //return false;
            }


        }
        private List<ReadFilesModel> PochtaBank(byte[] file)
        {
            List<ReadFilesModel> readFiles = new List<ReadFilesModel>();
            string[] str = Encoding.Default.GetString(file).Split('\r');
            for(int i=1;i<= str.Length-1; i++)
            {
                var Res = str[i].Split(',');
                if(Res.Length == 13)
                readFiles.Add(new ReadFilesModel { FullLic = Res[6], TypePU = Res[9], Indications = Res[12] });
                if (Res.Length == 17)
                {
                    readFiles.Add(new ReadFilesModel { FullLic = Res[6], TypePU = Res[9], Indications = Res[12] });
                    readFiles.Add(new ReadFilesModel { FullLic = Res[6], TypePU = Res[13], Indications = Res[16] });
                }
                if (Res.Length == 21)
                {
                    readFiles.Add(new ReadFilesModel { FullLic = Res[6], TypePU = Res[9], Indications = Res[12] });
                    readFiles.Add(new ReadFilesModel { FullLic = Res[6], TypePU = Res[13], Indications = Res[16] });
                    readFiles.Add(new ReadFilesModel { FullLic = Res[6], TypePU = Res[17], Indications = Res[20] });
                }
            }
            return readFiles;
        }
    }
}
