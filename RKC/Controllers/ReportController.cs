using BE.Report;
using BE.Roles;
using BL.Services;
using ClosedXML.Excel;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace RKC.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReport _report;
        public ReportController(IReport report)
        {
            _report = report;
        }
        // GET: Report
        public ActionResult Index()
        {
            ViewBag.DropDownList = _report.GetSqlQueryReports();
            return View();
        }
        [Authorize(Roles = RolesEnums.Admin)]
        [HttpPost]
        public async Task<ActionResult> SaveSqlQuery(string SqlQuery, string SqlName)
        {
            try
            {
                await _report.SaveSqlQueryAsync(SqlQuery,SqlName);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = RolesEnums.Admin)]
        [HttpDelete]
        public async Task<ActionResult> DeleteSqlQuery(int Id)
        {
            try
            {
                await _report.DeleteSqlResult(Id);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public async Task<ActionResult> DownloadReport(int Id)
        {
            try
            {
                var Result = _report.GetSqlResult(Id);
                return File(Result, System.Net.Mime.MediaTypeNames.Application.Octet, "Othcet.xlsx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = RolesEnums.Admin)]
        [HttpGet]
        public JsonResult GetEditSqlScript(int Id)
        {
            return Json(_report.GetEditSqlScript(Id), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = RolesEnums.Admin)]
        [HttpPost]
        public async Task<ActionResult> RefreshSqlQuery(string SqlQuery, string SqlName,int Id)
        {
            try
            {
                await _report.RefreshSqlQuery(SqlQuery, SqlName,Id);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}