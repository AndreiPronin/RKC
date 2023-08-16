using BE.Counter;
using BE.Court;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BL.Helper
{
    public interface Ilogger
    {
        void ActionUsers(int IdPU, string Description, string User);
        void ActionUsersPersData(int idPersData, string Description, string User);
        Task ActionUsersAsync(int IdPU, string Description, string User);
        void ActionUserCourt(string Lic, int CourtGeneralId, string Text);
        string GetActionUserCourt(string Lic, int CourtGeneralId);
    }
    public class Logger:Ilogger
    {
        public void ActionUsers(int IdPU, string Description, string User)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Log.Add(new Log { IdPU = IdPU, Description = Description, UserName = User, DateTime = DateTime.Now });
                    db.SaveChanges();
                }
            }
        }
        public async Task ActionUsersAsync(int IdPU, string Description, string User)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                using (var db = new ApplicationDbContext())
                {
                    db.Log.Add(new Log { IdPU = IdPU, Description = Description, UserName = User, DateTime = DateTime.Now });
                    await db.SaveChangesAsync();
                }
            }
        }
        public void ActionUsersPersData(int idPersData, string Description, string User)
        {
            if (!string.IsNullOrEmpty(Description))
            {
                using (var db = new ApplicationDbContext())
                {
                    db.LogsPersData.Add(new LogsPersData { idPersData = idPersData, Description = Description, UserName = User, DateTime = DateTime.Now });
                    db.SaveChanges();
                }
            }
        }
        public void ActionUserCourt(string Lic,int CourtGeneralId, string Text)
        {
            if(string.IsNullOrEmpty(Text))
                return;
            var FilePath = $@"\\10.10.10.17\doc_tplus_court\\{Lic}\\{CourtGeneralId}\\{CourtGeneralId}.log";
            if (!Directory.Exists($@"\\10.10.10.17\\doc_tplus_court\\{Lic}\\{CourtGeneralId}"))
            {
                Directory.CreateDirectory($@"\\10.10.10.17\\doc_tplus_court\\{Lic}\\{CourtGeneralId}");
            }
            if (File.Exists(FilePath))
            {
                var OldText = File.ReadAllText(FilePath);
                Text += Environment.NewLine;
                Text += OldText;
                File.WriteAllText(FilePath, Text);
            }
            else
            {
                File.Create(FilePath).Close();
                
                File.WriteAllText(FilePath, Text + Environment.NewLine);
            }
        }
        public string GetActionUserCourt(string Lic, int CourtGeneralId)
        {
            var FilePath = $@"\\10.10.10.17\doc_tplus_court\\{Lic}\\{CourtGeneralId}\\{CourtGeneralId}.log";
            if (File.Exists(FilePath))
            {
                var Text = File.ReadAllText(FilePath);
                return Text.Replace("0:00:00","");
            }
            else
            {
                return "";
            }
        }
    }
}
