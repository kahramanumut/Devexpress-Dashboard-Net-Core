using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevexpressReport.Web.BL.Abstract;
using DevexpressReport.Web.Models;
using DevexpressReport.Web.Models.Auth;

namespace DevexpressReport.Web.BL.Concrete
{
    public class UserBL : IUserBL
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IReportBL reportBL;

        public UserBL(UserManager<ApplicationUser> _userManager, IReportBL _reportBL)
        {
            userManager = _userManager;
            reportBL = _reportBL;
        }

        public async Task<bool> AddUser(ApplicationUser entity, string password, bool admin)
        {
            try
            {
                var result = await userManager.CreateAsync(entity, password);
                if (admin)
                {
                    var user = await userManager.FindByEmailAsync(entity.Email);
                    await userManager.AddToRoleAsync(user, Constant.AdministratorsRole);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                var user = await GetUserById(id);
                if (user != null)
                   await userManager.DeleteAsync(user);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ReportModel>> GetAvailableReportForSignedUser(ClaimsPrincipal claims)
        {
            var userId = userManager.GetUserId(claims);
            var user = await GetUserById(userId);
            var reportIdList = user.Reports.Split(";").ToList();
            List<ReportModel> reportList = new List<ReportModel>();
            foreach (var item in reportIdList)
            {
                reportList.Add(reportBL.GetReport(Convert.ToInt32(item)));
            }
            return reportList;
        }

        public async Task<int> GetReportsCountForUser(ClaimsPrincipal claims)
        {
            var user = await userManager.GetUserAsync(claims);
            return user.Reports.Length;
        }

        public async Task<ApplicationUser> GetUserById(string guid)
        {
            var user = await userManager.FindByIdAsync(guid);
            return user;
        }

        public async Task<string> GetUserRole(string userId)
        {
            var user = await GetUserById(userId);
            var role = await userManager.GetRolesAsync(user);
            if(role.Count>0)
                return role[0];
            return null;
        }

        public List<ApplicationUser> GetUsers()
        {
            return userManager.Users.ToList();
        }

        public bool ReportAuth(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUser(ApplicationUser entity, string password, bool admin)
        {
            try
            {
                if (entity != null)
                {
                    if (!String.IsNullOrEmpty(password))
                    {
                        await userManager.RemovePasswordAsync(entity);
                        await userManager.AddPasswordAsync(entity, password);
                    }
                    await userManager.UpdateAsync(entity);
                    if (admin)
                        await userManager.AddToRoleAsync(entity, Constant.AdministratorsRole);
                    else
                        await userManager.RemoveFromRoleAsync(entity, Constant.AdministratorsRole);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
