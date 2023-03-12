using BE.Report;
using BL.Excel;
using DB.DataBase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IReport
    {
        byte[] GetSqlResult(int Id);
        Task SaveSqlQueryAsync(string SqlQuery, string SqlName);
        Task DeleteSqlResult(int Id);
        List<SqlQueryReports> GetSqlQueryReports();
        EditSqlScritpt GetEditSqlScript(int Id);
        Task RefreshSqlQuery(string SqlQuery, string SqlName,int Id);
    }
    internal class Report : IReport
    {
        
        public byte[] GetSqlResult(int id)
        {
            using (var dbApp = new ApplicationDbContext())
            {
                var Report = dbApp.Reports.FirstOrDefault(x => x.Id == id);


                List<List<object>> lists = new List<List<object>>();
                var ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(Report.SqlQuery, connection);
                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        bool trigger = true;
                        var listObg = new List<object>();
                        while (reader.Read())
                        {
                            if (trigger)
                            {
                                for (int i = 0; i <= reader.FieldCount-1; i++)
                                {
                                    listObg.Add(reader.GetName(i));
                                }
                                lists.Add(listObg);
                            }
                            listObg = new List<object>();
                            for (int i = 0; i <= reader.FieldCount-1; i++)
                            {
                                listObg.Add(reader[i]);
                            }
                            lists.Add(listObg);
                            trigger = false;
                        }
                        reader.Close();
                        return ExcelReport.Generate(lists);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                
                }
            }
            //return new List<object>();
        }

        public async Task SaveSqlQueryAsync(string SqlQuery, string SqlName)
        {
            CheckSqlQuert(SqlQuery);
            using (var AppDb = new ApplicationDbContext())
            {
                if (AppDb.Reports.FirstOrDefault(x => x.NameReport == SqlName) is null)
                {
                    AppDb.Reports.Add(new DB.Model.Reports { NameReport = SqlName, SqlQuery = SqlQuery });
                    await AppDb.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Название с таким отчетом уже существует");
                }
            }
        }

        public List<SqlQueryReports> GetSqlQueryReports()
        {
            using (var AppDb = new ApplicationDbContext())
            {
                var res = AppDb.Reports.ToList();
                return res.Select(x => new SqlQueryReports { Id = x.Id, Name = x.NameReport }).ToList();
            }
        }

        public async Task DeleteSqlResult(int Id)
        {
            using (var AppDb = new ApplicationDbContext())
            {
                var res = AppDb.Reports.FirstOrDefault(x=>x.Id == Id);
                AppDb.Reports.Remove(res);
                await AppDb.SaveChangesAsync();
                
            }
        }

        public EditSqlScritpt GetEditSqlScript(int Id)
        {
            using (var appDb = new ApplicationDbContext())
            {
                return appDb.Reports.Where(x => x.Id == Id).Select(x=> new EditSqlScritpt { SqlName = x.NameReport , SqlQuery = x.SqlQuery}).FirstOrDefault();
            }
        }

        public async Task RefreshSqlQuery(string SqlQuery, string SqlName, int Id)
        {
            CheckSqlQuert(SqlQuery);
            using (var appDb = new ApplicationDbContext())
            {
                var res = appDb.Reports.FirstOrDefault(x=>x.Id == Id);
                res.NameReport = SqlName;
                res.SqlQuery = SqlQuery;
                await appDb.SaveChangesAsync();
            }
        }
        private void CheckSqlQuert(string SqlQuery)
        {
            var sqlQuery = SqlQuery.ToLower();
            if (sqlQuery.Contains("update") || sqlQuery.Contains("delete") || sqlQuery.Contains("exec"))
                throw new Exception("Недопустимые слава в запросе");
        }
    }
}
