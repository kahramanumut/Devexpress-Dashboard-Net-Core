using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevexpressReport.Web.BL.Abstract;
using DevexpressReport.Web.Models;
using DevexpressReport.Web.Models.Auth;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevexpressReport.Web.Controllers
{
    [Authorize(Roles = Constant.AdministratorsRole)]
    public class ReportController : Controller
    {
        private readonly IReportBL reportBL;

        public ReportController(IReportBL _reportBL)
        {
            reportBL = _reportBL;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<ReportModel> reports = reportBL.GetAllReport();
            return View(reports);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ReportModel entity, string dashboard)
        {
            try
            {
                XDocument dashboardDoc = new XDocument();
                dashboardDoc = XDocument.Parse(dashboard);
                MemoryStream stream = new MemoryStream();
                dashboardDoc.Save(stream);

                entity.Dashboard = stream.ToArray();
                reportBL.AddReport(entity);
                return RedirectToAction("Index","Report");
            }
            catch (Exception)
            {
                return RedirectToAction();
            }
        }

        [HttpGet]
        public IActionResult Delete(int reportId)
        {
            reportBL.DeleteReport(reportId);
            return RedirectToAction("Index", "Report");
        }

        public IActionResult Update(int reportId)
        {
            ReportModel entity = reportBL.GetReport(reportId);
            return View(entity);
        }

        [HttpPost]
        public IActionResult Update(string caption, string dashboard, int reportId)
        {
            try
            {
                ReportModel entity = reportBL.GetReport(reportId);
                entity.Caption = caption;
                //XDocument dashboardDoc = new XDocument();
                //dashboardDoc = XDocument.Parse(dashboard);
                //MemoryStream stream = new MemoryStream();
                //dashboardDoc.Save(stream);
                // entity.Dashboard = dashboard.ToArray();
                entity.Dashboard = Encoding.UTF8.GetBytes(dashboard);
                reportBL.UpdateReport(entity);
                return RedirectToAction("Index", "Report");
            }
            catch (Exception ex)
            {
                return RedirectToAction();
            }
        }

        public IActionResult AddWithDesigner()
        {
            return View();
        }
    }
}
