using BE.Counter;
using BE.Court;
using DB.DataBase;
using DB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BL.Helper
{
    public interface Ilogger
    {
        /// <summary>
        /// Лог ПУ
        /// </summary>
        /// <param name="IdPU"></param>
        /// <param name="Description"></param>
        /// <param name="User"></param>
        void ActionUsers(int IdPU, string Description, string User);
        /// <summary>
        /// Лог персов
        /// </summary>
        /// <param name="idPersData"></param>
        /// <param name="Description"></param>
        /// <param name="User"></param>
        void ActionUsersPersData(int idPersData, string Description, string User);
        /// <summary>
        /// Лог ПУ асинхронный
        /// </summary>
        /// <param name="IdPU"></param>
        /// <param name="Description"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        Task ActionUsersAsync(int IdPU, string Description, string User);
        /// <summary>
        /// Лог судебные дела
        /// </summary>
        /// <param name="Lic"></param>
        /// <param name="CourtGeneralId"></param>
        /// <param name="Text"></param>
        void ActionUserCourt(string Lic, int CourtGeneralId, string Text);
        /// <summary>
        /// Получение инфы по логу судебных дел
        /// </summary>
        /// <param name="Lic"></param>
        /// <param name="CourtGeneralId"></param>
        /// <returns></returns>
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
                    for (int i = 0; i < 5; i++)
                    {
                        try
                        {
                            db.SaveChanges();
                            break; 
                        }
                        catch (Exception e)
                        {
                            if(i == 5) throw;
                            Thread.Sleep(100); 
                        }
                    }
                }
            }
        }
        public void ActionUserCourt(string Lic,int CourtGeneralId, string Text)
        {
            string _logPath = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.CourtLogPath).GetString();
            if(string.IsNullOrEmpty(Text))
                return;
            var FilePath = $@"\\10.10.10.17\{_logPath}\\{Lic}\\{CourtGeneralId}\\{CourtGeneralId}.log";
            if (!Directory.Exists($@"\\10.10.10.17\\{_logPath}\\{Lic}\\{CourtGeneralId}"))
            {
                Directory.CreateDirectory($@"\\10.10.10.17\\{_logPath}\\{Lic}\\{CourtGeneralId}");
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
            string _logPath = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.CourtLogPath).GetString();
            var FilePath = $@"\\10.10.10.17\{_logPath}\\{Lic}\\{CourtGeneralId}\\{CourtGeneralId}.log";
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
