using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DevexpressReport.Web.BL.Abstract;
using DevexpressReport.Web.Models.Auth;

namespace DevexpressReport.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IUserBL userBL;
        IReportBL reportBL;
        public HomeController(IUserBL _userBL, IReportBL _reportBL)
        {
            userBL = _userBL;
            reportBL = _reportBL;
        }
        
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Constant.AdministratorsRole))
            {
                var reportList = reportBL.GetAllReport();
                return View(reportList);
            }
            else
            {
                int reportCount = await userBL.GetReportsCountForUser(User);
                if (reportCount > 0)
                {
                    var reportList = await userBL.GetAvailableReportForSignedUser(User);
                    return View(reportList);
                }
                else
                    return RedirectToAction("NoReportsForUser");
            }
        }

        public IActionResult NoReportsForUser()
        {
            return View();
        }
    }
}
