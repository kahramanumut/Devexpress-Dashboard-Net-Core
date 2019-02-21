using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevexpressReport.Web.Areas.Identity.Data;
using DevexpressReport.Web.Models.Auth;

namespace DevexpressReport.Web.Data
{
    public static class SeedData
    {
        public static async Task Seed(IServiceScope scope)
        {
            var serviceProvider = scope.ServiceProvider;
            ReportDbContext appContext = serviceProvider.GetRequiredService<ReportDbContext>();
            ApplicationDbContext identityContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            appContext.Database.Migrate();
            identityContext.Database.Migrate();

            if (!identityContext.Users.Any())
            {
                var sampleUser = await EnsureUser<ApplicationUser>(serviceProvider, "1244244", "umut.kahraman@umut.com", "Umut", "Kahraman");
                await EnsureRole(serviceProvider, sampleUser, Constant.AdministratorsRole);
            }

        }

        private static async Task<string> EnsureUser<T>(IServiceProvider serviceProvider, string testUserPw, string email, string name, string surname)
        {

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser { Email = email, Name = name, Surname = surname, UserName=email };
                user.EmailConfirmed = true;
                var r = await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            try
            {
                IdentityResult IR = null;
                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(role))
                {
                    IR = await roleManager.CreateAsync(new IdentityRole(role));
                }

                var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

                var user = await userManager.FindByIdAsync(uid);

                IR = await userManager.AddToRoleAsync(user, role);

                return IR;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
