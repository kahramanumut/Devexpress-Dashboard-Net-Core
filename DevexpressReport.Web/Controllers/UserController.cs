using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevexpressReport.Web.BL.Abstract;
using DevexpressReport.Web.Models.Auth;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevexpressReport.Web.Controllers
{
    [Authorize(Roles = Constant.AdministratorsRole)]
    public class UserController : Controller
    {
        private readonly IUserBL userBL;
        private readonly IReportBL reportBL;
        public UserController(IUserBL _userBL, IReportBL _reportBL)
        {
            userBL = _userBL;
            reportBL = _reportBL;
        }
        public IActionResult Index()
        {
            return View(userBL.GetUsers());
        }

        public IActionResult Add()
        {
            ViewBag.Raporlar = reportBL.GetAllReport().Select(c => new SelectListItem { Text= c.Caption , Value = c.Id.ToString() }).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string email, string name, string surname, string password, string passwordAgain, string admin , string [] raporlar)
        {
            try
            {
                if (password == passwordAgain && !String.IsNullOrEmpty(email))
                {
                    ApplicationUser user = new ApplicationUser { Email = email, Name = name, Surname = surname, UserName = email, EmailConfirmed = true, Reports = string.Join(";", raporlar) };
                    if (admin == null)
                        await userBL.AddUser(user, password, false);
                    else
                        await userBL.AddUser(user, password, true);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Update(string userId)

        {
            var user = await userBL.GetUserById(userId);
            var role = await userBL.GetUserRole(userId);
            ViewBag.Role = role;
            ViewBag.Raporlar = reportBL.GetAllReport().Select(c => new SelectListItem { Text = c.Caption, Value = c.Id.ToString() }).ToList();
            if (user != null)
                return View(user);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string userId, string email, string name, string surname, string password, string passwordAgain, string admin, string[] raporlar)
        {
            
            try
            {
                if (password == passwordAgain)
                {
                    var user = await userBL.GetUserById(userId);
                    user.Email = email;
                    user.Name = name;
                    user.Surname = surname;
                    user.UserName = email;
                    user.Reports = string.Join(";", raporlar);

                    if (admin == null)
                        await userBL.UpdateUser(user, password, false);
                    else
                        await userBL.UpdateUser(user, password, true);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string userId)
        {
            try
            {
                await userBL.DeleteUser(userId);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
