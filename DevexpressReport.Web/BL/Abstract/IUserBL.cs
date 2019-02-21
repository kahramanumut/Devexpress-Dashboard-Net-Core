using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevexpressReport.Web.Models;
using DevexpressReport.Web.Models.Auth;

namespace DevexpressReport.Web.BL.Abstract
{
    public interface IUserBL
    {
        List<ApplicationUser> GetUsers();
        Task<ApplicationUser> GetUserById(string guid);
        Task<string> GetUserRole(string userId);
        Task<bool> DeleteUser(string id);
        Task<bool> UpdateUser(ApplicationUser entity, string password, bool admin);
        Task<bool> AddUser(ApplicationUser entity, string password, bool admin);
        bool ReportAuth(ApplicationUser entity);
        Task<List<ReportModel>> GetAvailableReportForSignedUser(ClaimsPrincipal claims);
        Task<int> GetReportsCountForUser(ClaimsPrincipal claims);
    }
}
